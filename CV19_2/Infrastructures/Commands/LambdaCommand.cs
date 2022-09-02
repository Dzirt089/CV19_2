using CV19_2.Infrastructures.Commands.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CV19_2.Infrastructures.Commands
{
    internal class LambdaCommand : Command
    {
        private readonly Action<object> _Execute;
        private readonly Func<object,bool> _CanExecute;

        /// <summary>
        /// В конструкторе надо получить два делегата. Один, который будет выполняться методом CanExecute. Второй - выполняться методом Execute
        /// т.е. указываем 2 действия, которые команда может выполнять. И сохраняем данные (которые пришли из конструктора) в приватные поля
        /// </summary>
        /// <param name="Execute">У нас команды из разметки могут получать параметры. Тип их данных может быть любым. 
        /// Поэтому делегат "Action<>" получает параметр класса "object". В дальнейшем - его преобразуем к нужному нам виду. (Или получим Null, если ничего не было передано)</param>
        /// <param name="CanExecute">Делегату Func, передаем параметры <object,bool>, что означает преобразование из "object" в "bool". CanExecute = null прописываем
        /// для того, чтобы можно было не передвать второй параметр (по умолч. будет Null, если его не передавать)</param>
        public LambdaCommand(Action<object> Execute, Func<object,bool> CanExecute = null) 
        {
            _Execute = Execute ?? throw new ArgumentNullException(nameof(Execute)); //Ругаемся, если не передали параметр в Execute.
            _CanExecute = CanExecute;
        }
        /// <summary>
        /// Вызываем метод CanExecute, подрузумевая, что там может быть пустая ссылка. Проводим проверку, и если нет этого делегата, то считаем что команду можно выполнить в любом случае. Передав true
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public override bool CanExecute(object parameter) => _CanExecute?.Invoke(parameter) ?? true;
        /// <summary>
        /// Вызываем метод Execute и передаем в него параметр
        /// </summary>
        /// <param name="parameter"></param>
        public override void Execute(object parameter) => _Execute(parameter); 
    }
}
