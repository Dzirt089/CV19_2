using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CV19_2.Infrastructures.Commands;
using CV19_2.ViewModels.Base;

namespace CV19_2.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
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

        /// <summary>
        /// Внутри конструктора создаем объкт команды. 
        /// </summary>
        public MainWindowViewModel()
        {
            #region Команды

            CloseApplicationCommand = new LambdaCommand (OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExecute);

            #endregion
        }
    }
}
