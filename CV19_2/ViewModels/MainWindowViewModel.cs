using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CV19_2.Infrastructures.Commands;
using CV19_2.Models;
using CV19_2.Models.Decanat;
using CV19_2.ViewModels.Base;

namespace CV19_2.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {

        /*---------------------------------------------------------------------------------------------------------------------------------*/

        public ObservableCollection<Group> Groups { get; set; }

        public object[] CompositeCollection { get; }

        #region SelectedCompositeValue : object - Выбранный непонятный элемент
        /// <summary>
        /// Выбранный непонятный элемент
        /// </summary>
        private object _SelectedCompositeValue;
        /// <summary>
        /// Выбранный непонятный элемент
        /// </summary>
        public object SelectedCompositeValue
        {
            get => _SelectedCompositeValue;
            set => Set(ref _SelectedCompositeValue, value);
        }
        #endregion

        #region SelectGroup : Group - выбранная группа

        private Group _SelectGroup;
        public Group SelectGroup
        {
            get => _SelectGroup;
            set => Set(ref _SelectGroup, value);
        }

        #endregion

        #region TestDataPoints : IEnumerable<DataPoint> - Тестовый набор данных для визуализации графиков
        //НАм понадобиться сво-во для перечесления точек данных, которые мы будем строить на графике
        /// <summary>
        /// Тестовый набор данных для визуализации графиков
        /// </summary>
        private IEnumerable<DataPoint> _TestDataPoints;
        /// <summary>
        /// Тестовый набор данных для визуализации графиков
        /// </summary>
        public IEnumerable<DataPoint> TestDataPoints
        {
            get => _TestDataPoints;
            set => Set(ref _TestDataPoints, value);
        }

        #endregion

        #region Заголовок окна
        private string _Title = "Анализ статистики CV19";
        /// <summary>
        /// Заголовок окна
        /// </summary>
        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
            //set
            //{
            //    //Этот код должен быть в каждом свойстве. 
            //    //if (Equals(_Title, value)) return;
            //    //_Title = value;
            //    //OnPropertyChanged();
            //    //Так как этот код мы вписали в метод Set, то мы делаем так:
                
            //    Set (ref _Title, value);
            //}
        }
        #endregion

        #region Status : string - Статус программы
        /// <summary>
        /// Статус Программы
        /// </summary>
        private string _Status = "Готов!";
        /// <summary>
        /// Статус Программы
        /// </summary>
        public string Status
        {
            get => _Status;
            set => Set(ref _Status, value);
        }
        #endregion

        /*---------------------------------------------------------------------------------------------------------------------------------*/

        #region Команды

        #region CloseApplicationCommand
        public ICommand CloseApplicationCommand { get; }
        /// <summary>
        /// Метод выполняется, когда команда выполняется
        /// </summary>
        /// <param name="p"></param>
        private void OnCloseApplicationCommandExecuted(object p) 
        {
            Application.Current.Shutdown();
        }
        /// <summary>
        /// В нашем случае, команда будет доступна для выполнения всегда. Поэтому метод возвращает true
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private bool CanCloseApplicationCommandExecute(object p) => true;
        #endregion
        #endregion

        /*---------------------------------------------------------------------------------------------------------------------------------*/

        /// <summary>
        /// Внутри конструктора создаем объект команды. 
        /// </summary>
        public MainWindowViewModel()
        {
            #region Команды

            CloseApplicationCommand = new LambdaCommand (OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExecute);

            #endregion

            var data_points = new List<DataPoint>((int)(360 / 0.1));
            for (var x = 0d; x <= 360; x += 0.1)
            {
                const double to_rad = Math.PI / 180;
                var y = Math.Sin(x * to_rad);

                data_points.Add(new DataPoint { XValue = x, YValue = y });
            }
            TestDataPoints = data_points;

            var student_index = 1;

            var students = Enumerable.Range(1, 10).Select(i => new Student
            {
                Name = $"Name {student_index}",
                Surname = $"Surname {student_index}",
                Patronymic = $"Patronymic {student_index++}",
                Birthday = DateTime.Now,
                Rating = 0
            });

            var groups = Enumerable.Range(1, 20).Select(i => new Group 
            { 
                Name = $"Группа {i}",
                Students = new ObservableCollection<Student>(students)
            });
            Groups = new ObservableCollection<Group>(groups);

            var data_list = new List<object>();
            data_list.Add("Hello world");
            data_list.Add(42);
            var group = Groups[1];
            data_list.Add(group);
            data_list.Add(group.Students[0]);


            CompositeCollection = data_list.ToArray();
        }

        /*---------------------------------------------------------------------------------------------------------------------------------*/


    }
}
