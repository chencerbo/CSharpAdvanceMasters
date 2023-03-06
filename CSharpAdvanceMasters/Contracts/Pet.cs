using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public abstract class Pet: IPet
    {
        public abstract string Name { get; }
        public abstract int Age { get; }

        public abstract void Talk();
        public void Walk() 
        {
            Console.WriteLine("Walking...");
        }
        public void Run() 
        {
            Console.WriteLine("Running...");
        }
    }
}
