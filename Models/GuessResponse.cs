using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GuessWS
{
    public class GuessResponse
    {
        public string Name { get; set; }
        public int Guess { get; set; }
        public string Value { get; set; }
        public int TimeInSeconds { get; set; }
    }
}
