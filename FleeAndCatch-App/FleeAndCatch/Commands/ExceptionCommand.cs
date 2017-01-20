using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleeAndCatch.Commands.Models;

namespace FleeAndCatch.Commands
{
    public class ExceptionCommand : Command
    {
        public ExceptionCommand(string pId, string pType, ClientIdentification pIdentification) : base(pId, pType, pIdentification)
        {
        }

        public override string GetCommand()
        {
            throw new NotImplementedException();
        }
    }

    public enum ExceptionType
    {
        
    }
}
