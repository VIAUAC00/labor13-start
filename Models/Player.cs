using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace GuessWS
{
    public class Player
    {
        public WebSocket Socket { get; set; }

        public string Name { get; set; }

        public int CurrentGuesses { get; set; }
    }
}
