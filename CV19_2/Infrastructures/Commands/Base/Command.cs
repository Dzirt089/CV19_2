using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CV19_2.Infrastructures.Commands.Base
{
    internal abstract class Command : ICommand
    {
        /// <summary>
        /// Передаем управление событием "event EventHandler CanExecuteChanged" системе WPF. Для этого реализуем события явно. Таким образом, что
        /// передаем управление событием классу CommandManager и WPF автоматичиски генерирует это событие у всех команд, когда что-то происходит
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value; 
            remove => CommandManager.RequerySuggested -= value;
        }
        /// <summary>
        /// CanExecute - это функция, которая возвращает либо истину, либо ложь  
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public abstract bool CanExecute(object parameter);
        /// <summary>
        /// Метод Execute, это то - что должно быть выполнено самой коммандой. Это основная логика команды, которая она должна выполнять
        /// </summary>
        /// <param name="parameter"></param>
        /// <exception cref="NotImplementedException"></exception>
        public abstract void Execute(object parameter);
}
