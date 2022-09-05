using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CV19_2.Models.Decanat
{
    internal class Student
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public DateTime Birthday { get; set; }
        public double Rating { get; set; }

        public string Description { get; set; }
    }

    internal class Group
    {
        public string Name { get; set; }

        /// <summary>
        /// Свойство группы задаем ICollection<>, чтобы получить свободу выбора - какую коллекцию сюда добавлять. Это может быть Список, Массив, и все чо угодно, что может хранить студентов 
        /// </summary>
        public IList<Student> Students { get; set; }
        public string Description { get; set; }
    }

}
