using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleeAndCatch
{
    public class Exception : System.Exception
    {
        private readonly int _id;

        public Exception(int pid, string pMessage) :base(pMessage)
        {
            _id = pid;
        }

        public int Id => _id;
    }
}
