using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace GuessWS
{
    public class GameStateService
    {
        public ConcurrentDictionary<Guid, Player> Clients { get; } = new ConcurrentDictionary<Guid, Player>();

        public List<ToplistItem> Toplist { get; set; } = new List<ToplistItem>();

        private int? currentValue;
        public int CurrentValue => (currentValue = (currentValue ?? Randomize())).Value;

        private Random Random { get; } = new Random();
        public int Randomize() => Random.Next(1, 100);

    }
}
