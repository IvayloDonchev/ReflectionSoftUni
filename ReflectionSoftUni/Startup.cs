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


            // Получаване на конструктори
            Console.WriteLine();
            var constructors = typeof(Cat).GetConstructors();
            foreach(var constructor in constructors)
            {
                var parameters = constructor.GetParameters();
                foreach(var parameter in parameters)
                    Console.WriteLine(parameter.Name + " : " + parameter.ParameterType.Name);
                Console.WriteLine("-------------");
            }
            var ctr = typeof(Cat).GetConstructor(Type.EmptyTypes); // взема конструктора без параметри
            var cat2 = (Cat) ctr.Invoke(new object[0]);
            Console.WriteLine(cat2.Name);

            var ctr2 = typeof(Cat).GetConstructor(new[] { typeof(string) });
            var cat3 = ctr2.Invoke(new[] { "Ivan" });


            // работа с методите
            Console.WriteLine();
            //var methods = typeof(Cat).GetMethods(); // всички методи, включително и наследените
            var methods = typeof(Cat).GetMethods(BindingFlags.Instance | BindingFlags.Public); // само декларираните в класа
            foreach (var method in methods)
                Console.WriteLine(method.Name);
            var met = typeof(Cat).GetMethod("Hello",Type.EmptyTypes);
            met.Invoke(new Cat(), new object[0]); // втория аргумент е масива с параметри на метода - в случая - празен

            var met2 = typeof(Cat).GetMethod("Hello", new[] { typeof(string) }); // другия метод Hello с един параметър string
            string s = (string) met2.Invoke(cat3, new[] { "Vankata" });
            Console.WriteLine(s);
            if(met2.ReturnType == typeof(string))
                Console.WriteLine("Test");
            if(met2.ReturnType.Name == "String")
                Console.WriteLine("Test");
            Console.WriteLine();
            var prms = met2.GetParameters();
            foreach (var prm in prms)
                Console.WriteLine(prm.Name + " " + prm.ParameterType.Name );

            //Атрибути
            Console.WriteLine();
            var aname = typeof(Cat).GetProperty("Name").GetCustomAttribute<AuthorAttribute>()?.Name;
            Console.WriteLine(aname);

            Console.WriteLine();
            typeof(Cat)
                .GetProperties()
                .Select(pr => new
                {
                    Name = pr.Name,
                    Attrs = pr.GetCustomAttributes()
                })
                .ToList()
                .ForEach(pr => Console.WriteLine(pr.Name + ": " + string.Join(", ",pr.Attrs.Select(a => a.GetType().Name.Replace("Attribute",string.Empty))))) ;
            Console.ReadKey();

            // https://softuni.bg/trainings/resources/video/9841/video-screen-25-july-2016-ivailo-kenov-csharp-oop-advanced-july-2016
            // 1:40 часа
        }
        private static string GetTypeName<T>() =>typeof(T).Name;
       
    }
}
