using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CSharpBNP.Model
{
    public class Trade
    {
        [XmlAttribute("CorrelationId")]
        public string CorrelationID { get; set; }

        [XmlAttribute("NumberOfTrades")]
        public int NumberOfTrades { get; set; }

        [XmlAttribute("Limit")]
        public int Limit { get; set; }

        [XmlText]
        public int Value { get; set; }

        [XmlAttribute("TradeID")]
        public string TradeID { get; set; }
    }
}
