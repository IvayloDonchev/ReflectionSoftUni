using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReflectionSoftUni
{
    public class Cat : Animal
    {
        static Cat() // статичен конструктор. Изпълнява се само веднъж при достъпване на класа
        {
            Console.WriteLine("Static constructor execution");
        }
        private string somePrivateField;
        public Cat(string s)
        {
            this.Name = s;
            this.Age = 0;
            this.somePrivateField = "123";
        }
        public Cat() : this("Pesho") { }
        public Cat(string name, int age) : this(name)
        {
            this.Age = age;
        }
        public string Name { get; set; }
        public int Age { get; set; }
        public string getSomePrivateField() => this.somePrivateField;
        public void Hello() =>  Console.WriteLine("Hello form method");
        public string Hello(string s) => "Hello, " + this.Name + " and " + s;
    }
}
