using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReflectionSoftUni
{
    public class Cat : Animal
    {
        private string somePrivateField;
        public Cat(string s)
        {
            this.Name = s;
            this.Age = 0;
            this.somePrivateField = "123";
        }
        public Cat() : this("") { }
        public string Name { get; set; }
        public int Age { get; set; }
        public string getSomePrivateField() => this.somePrivateField;
    }
}
