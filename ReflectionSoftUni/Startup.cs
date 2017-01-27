using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ReflectionSoftUni
{
    public class Startup
    {
        public static void Main(string[] args)
        {
            // 1. Типът Type
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

            var typesInCurrentAssembly = Assembly.GetEntryAssembly().GetTypes();  // асемблито, в което е извикан Main
            // всички типове в текущото асембли
            Console.WriteLine();
            foreach (var type in typesInCurrentAssembly)
            {
                Console.WriteLine(type.Name);
            }

            // от всички типове отделяме само интерфейсите
            var interfacesInCurrentAssembly = Assembly.GetEntryAssembly().GetTypes().Where(t => t.IsInterface);
            Console.WriteLine();
            foreach (var type in interfacesInCurrentAssembly)
            {
                Console.WriteLine(type.Name);
            }
            // 2. Инстантиране на типове
            Type sbType = Type.GetType("System.Text.StringBuilder");
            StringBuilder bsInstance = (StringBuilder) Activator.CreateInstance(sbType);

            // трябва да има default constructor
            var list = new List<Cat>();
            var watch = Stopwatch.StartNew();
            for (int i = 0; i < 10000; i++)
            {
                var cat = (Cat) Activator.CreateInstance(typeOfCat, "Gosho"); // вместо var cat = new Cat();
                //var cat = Activator.CreateInstance<Cat>(); // има generic версия
                list.Add(cat);
            }
            watch.Stop();
            Console.WriteLine($"Time: {watch.Elapsed}");
            Console.WriteLine($"List of {list.Count} elements\n");
            list = new List<Cat>();
            watch = Stopwatch.StartNew();
            for (int i = 0; i < 10000; i++)
            {
                var cat = new Cat("Pesho");
                list.Add(cat);
            }
            watch.Stop();
            Console.WriteLine($"Time: {watch.Elapsed}");
            Console.WriteLine($"List of {list.Count} elements\n");
            // cat.Name = "Pesho";


            // 3. Информация за елементи (полетата, properties) на класа
            Console.WriteLine();
            var tom = new Cat();
            var fields = typeOfCat.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (var f in fields)
            {
                Console.WriteLine(f.FieldType.Name + " " + f.Name);
                if (f.Name.Contains("Name"))
                    f.SetValue(tom, "Pesho");
            }
            // как можем да променим private field извън класа!
            var ff = typeof(Cat).GetField("somePrivateField", BindingFlags.Instance | BindingFlags.NonPublic);
            Console.WriteLine(ff.Name);
            ff.SetValue(tom, "Hello");
            Console.WriteLine(tom.getSomePrivateField());

            Console.WriteLine();
            Cat gcat = new Cat { Name = "Gosho", Age = 25 };
            typeOfCat = gcat.GetType();
            var nameProperty = typeOfCat.GetProperty("Name");
            Console.WriteLine(nameProperty.GetValue(gcat));
            nameProperty.SetValue(gcat, "Ivan");
            Console.WriteLine(nameProperty.GetValue(gcat));
            PropertyInfo[] properties = typeOfCat.GetProperties();  // може и с var
            foreach(var prop in properties)
                Console.WriteLine(prop.Name);


            // https://softuni.bg/trainings/resources/video/9841/video-screen-25-july-2016-ivailo-kenov-csharp-oop-advanced-july-2016
            // до 1 ч. 10 мин.
            Console.ReadKey();

        }
        private static string GetTypeName<T>() =>typeof(T).Name;
       
    }
}
