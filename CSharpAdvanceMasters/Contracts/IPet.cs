using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IPet
    {
        public void Talk();
        public void Walk() { }
        public void Run() { }
    }
}
