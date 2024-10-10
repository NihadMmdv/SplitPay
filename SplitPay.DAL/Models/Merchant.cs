using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitPay.DAL.Models
{
    public class Merchant
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int TerminalNo { get; set; }
    }
}
