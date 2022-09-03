using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CV19_2Console
{
    internal class Program
    {
        private const string data_url = @"https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_time_series/time_series_covid19_confirmed_global.csv";

        /// <summary>
        /// Создаем асинхронный метод, который вернет нам поток "Task<Stream>". Метод создает клиента внутри себя, получает ответ от сервера и 
        /// конфигурируем ответ таким образом, чтобы он не скачивал все содержимое ответа, мы уточняем что нам нужно знать пока только заголовки (HttpCompletionOption.ResponseHeadersRead)
        /// А остальное тело запроса пока остается в сетевой карте не тронутым. После этого мы возвращаем поток, который и обеспечит нам процесс чтения данных из сетевой карты
        /// Буквально: берем клиент (response), берем его контент (response.Content) и (response.Content.ReadAsStreamAsync()).
        /// В итоге, метод возвращает поток, из которого мы можем считать текстовые данные
        /// </summary>
        /// <returns></returns>
        private static async Task<Stream> GetDataStream()
        {
            var client = new HttpClient();
            var response = await client.GetAsync(data_url, HttpCompletionOption.ResponseHeadersRead);
            return await response.Content.ReadAsStreamAsync();
        }

        /// <summary>
        /// Идея следующая: в "using var data_stream = GetDataStream().Result;" этой строчке метода GetDataLines(), произойдет запрос к серверу. Сервер нам ответит в 
        /// методе GetDataStream(), скачает только заголовок ответа. Остальное тело ответа останется не принятым (он останется в буфере сетевой карты либо сервер приостановит передачу данных)
        /// После этого, метод GetDataStream() вернет нам поток, с которого мы сможем считать данные по байтно, и захватыем его в "using var data_stream".
        /// Создаем объект (using var data_reader = new StreamReader(data_stream);), который обеспечит чтение по строчкам и начнет читать по строчкам поток, байт за байтом while (!data_reader.EndOfStream)
        /// При этом считываем одну строчку и возвращаем её как результат "yield return line". И компилятор метод GetDataLines() преобразует в генератор, способом в Main...
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<string> GetDataLines()
        {
            using var data_stream = GetDataStream().Result;
            using var data_reader = new StreamReader(data_stream);

            while (!data_reader.EndOfStream)
            {
                var line = data_reader.ReadLine();
                if (string.IsNullOrWhiteSpace(line)) continue;
                yield return line.Replace("Korea,","Korea -");
            }
        }
        /// <summary>
        /// Метод, который получает все даты, по которым будут разбиты данные
        /// </summary>
        /// <returns>Массив дат</returns>
        private static DateTime[] GetDates() => GetDataLines()
            .First()
            .Split(',')
            .Skip(4)
            .Select(s => DateTime.Parse(s, CultureInfo.InvariantCulture))
            .ToArray();

        private static IEnumerable<(string Contry, string Province, int[] Counts)> GetData()
        {
            var lines = GetDataLines()
                .Skip(1)
                .Select(line => line.Split(','));

            foreach (var row in lines)   // Выделем сперва все данные в переменную, потом сгруппируем в кортеж и вернём его, чтобы было проще
            {
                var province = row[0].Trim();   //У каждой строки будем вызывать метод Trim(), который будет обрезать все лишнее в нашей строке (в плане пробелов, спец символов нечитаемых и т.д.)
                var country_name = row[1].Trim(' ', '"'); //А вот для contry_name надо будет указать что конкретно мы хотим обрезать (пробелы и ковычки). ЗАпятаю не получится обрезать, это разделитель колонок и будут проблемы
                var i = 0;
                if (!int.TryParse(row[4], out int res))
                    i = 1;
                var counts = row.Skip(4 + i).Select(int.Parse).ToArray();
                //var counts = row.Skip(4).Select(s => int.Parse(s)).ToArray(); //Так как 2 и 3 столбцом идут широта и долгота, мы пропускаем их. Остальное - это кол-во зараженных на дату
                //Мы считали в каждую переменную данные по строчно. После чего, каждый из элементов мы превращаем в целое число


                yield return (province, country_name, counts); //С помощью yield return возвращаем данные в виде кортежа. 
            }
            //foreach (var row in lines)
            //{
            //    var province = row[0].Trim();
            //    var contry_name = row[1].Trim(' ', '"');
            //    var count = row.Skip(4).Select(int.Parse).ToArray();
            //    yield return (contry_name, province, count);
            //}
        }

        static void Main(string[] args)
        {
            //WebClient client = new WebClient();

            //foreach (var data_line in GetDataLines())
            //    Console.WriteLine(data_line);
            //var dates = GetDates();
            //Console.WriteLine(string.Join("\r\n", dates));

            var russia = GetData().FirstOrDefault(v=>v.Contry.Equals("Russia", StringComparison.OrdinalIgnoreCase));

            Console.WriteLine(string.Join("\r\n", GetDates().Zip(russia.Counts, (date, count) => $"{date} - {count}")));
        }
    }
}
