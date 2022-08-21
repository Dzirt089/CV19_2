using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
