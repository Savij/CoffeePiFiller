using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeePiFiller
{
   public interface IShell
   {
       bool IsDebugMode { get; set; }
       void Process();
   }
}
