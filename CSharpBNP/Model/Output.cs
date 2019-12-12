using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpBNP.Model
{
    public class Output
    {
        public string CorrelationID { get; set; }
        public int NumberOfTrades { get; set; }
        public StateEnum State { get; set; }
    }

}
