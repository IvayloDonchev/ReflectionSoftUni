using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReflectionSoftUni
{
    public abstract class Animal : IAnimal, IMoveable, ICanEat
    {
    }

    internal interface ICanEat
    {
    }

    internal interface IMoveable
    {
    }

    public interface IAnimal
    {
    }
}
