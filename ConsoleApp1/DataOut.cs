using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class DataOut
    {
        public int T_o { get; set; }
        public int T_zm { get; set; }
        public string timestamp { get; set; }
    }

    public class DataIn
    {
        public int speed { get; set; }
        public string symTime { get; set; }
    }
}
