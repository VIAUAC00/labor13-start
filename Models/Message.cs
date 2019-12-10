using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GuessWS
{
    public class Message
    {
        public string Action { get; set; }

        public string Name { get; set; }

        public int Guess { get; set; }
    }
}
