using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace GuessWS
{
    public class ToplistItem
    {
        public string Name { get; set; }
        public int Guesses { get; set; }
        public int TimeInSeconds { get; set; }
    }
}
