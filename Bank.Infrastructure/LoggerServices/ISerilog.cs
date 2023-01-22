using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace Bank.Infrastructure.LoggerServices
{
    public interface ISerilog
    {
        void Information(string message);
        void Error(string message);
        void Warning(string message);
    }
}
