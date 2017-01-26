using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReflectionSoftUni
{
    public class Startup
    {
        public static void Main(string[] args)
        {
            var obj = new object();
            Type typeOfobj = obj.GetType();
            var startupType = typeof(Startup);   // Type startupType = typeof(Startup);
            var typeOfCat = new Cat().GetType(); // = typeof(Cat);
            Console.WriteLine(typeOfCat.Name);
            Console.WriteLine(typeOfCat.FullName);
            Console.WriteLine(typeOfCat.Assembly.FullName);
            foreach(var prop in typeOfCat.GetProperties())
            {
                Console.WriteLine(prop.Name + " " + prop.PropertyType.Name);
            }
            Console.WriteLine(GetTypeName<Cat>());
            Console.WriteLine(typeOfCat.BaseType.Name);  //Animal

            var typeOfAnimal = typeof(Cat).BaseType;
            var allInterfces = typeOfAnimal.GetInterfaces();
            foreach (var interf in allInterfces)
                Console.WriteLine(interf.Name);

            var baseType = typeof(Cat);
            do
            {
                Console.WriteLine(baseType.Name);
                baseType = baseType.BaseType;
            } while (baseType != typeof(Object));
            Console.ReadKey();

        }
        private static string GetTypeName<T>() =>typeof(T).Name;
       
    }
}
