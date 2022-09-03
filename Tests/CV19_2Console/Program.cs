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
                yield return line;
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



        static void Main(string[] args)
        {
            //WebClient client = new WebClient();

            //foreach (var data_line in GetDataLines())
            //    Console.WriteLine(data_line);
            var dates = GetDates();
            Console.WriteLine(string.Join("\r\n", dates));

            Console.ReadLine();
        }
    }
}
