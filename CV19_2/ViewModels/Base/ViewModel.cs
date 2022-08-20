﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CV19_2.ViewModels.Base
{
    internal abstract class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        ///OnPropertyChanged метод можем вызывать не указывая ему имя свойсва в (). 
        ///CallerMemberName-атрибут для компилятора, компилятор автоматически подставит имя метода
        ///Из которого вызывается наша процедура OnPropertyChanged
        /// </summary>
        /// <param name="PropertyName">Имя свойства</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
        /// <summary>
        /// Метод Set - его задача обновлять значения свойства,для которого определено поле field
        /// Задача этого метода - разрешить колцевые изменения свойств, которые могут возникать когда одно изменеие свойства порождает второе и т.д.
        /// Что может привести к переполнению стека. Мы сравниваем значение нашего св-ва с новым, и если они равны - то возвращаем ложь.
        /// Если действительно значение изменилось, то присваиваем новое значение св-ву и генерируем событие OnPropertyChanged(PropertyName);
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field">Ссылка на поле свойства</param>
        /// <param name="value">Новое значение, которое мы хотим установить</param>
        /// <param name="PropertyName">Параметр самостоятельно определяется компилятором свойство, которое передадим в OnPropertyChanged </param>
        /// <returns></returns>
        protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string PropertyName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(PropertyName);
            return true;
        }
    }
}
