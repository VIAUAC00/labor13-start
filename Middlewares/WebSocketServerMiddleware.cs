using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GuessWS
{
    public class WebSocketServerMiddleware
    {
        public WebSocketServerMiddleware(RequestDelegate next, GameStateService gameState)
        {
            Next = next;
            GameState = gameState;
        }

        public RequestDelegate Next { get; }
        public GameStateService GameState { get; }

        public async Task Invoke(HttpContext context)
        {
            if (!context.WebSockets.IsWebSocketRequest)
            {
                await Next(context);
                return;
            }

            var socketId = Guid.NewGuid();
            var socket = await context.WebSockets.AcceptWebSocketAsync();
            var player = new Player { Socket = socket };
            GameState.Clients.TryAdd(socketId, player);

            var buffer = new byte[1024 * 4];
            var timer = new Stopwatch();
            while (socket.CloseStatus == null)
            {
                var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                var message = JsonConvert.DeserializeObject<Message>(Encoding.UTF8.GetString(buffer, 0, result.Count));
                switch (message.Action)
                {
                    case "startGame":
                        GameState.Randomize();
                        timer.Restart();
                        break;
                    case "setName":
                        player.Name = message.Name;
                        break;
                    case "guess":
                        if (!timer.IsRunning)
                            break;
                        player.CurrentGuesses++;
                        var val = GameState.CurrentValue;
                        var respMessage = new GuessResponse 
                        { 
                            Name = player.Name,
                            Guess = message.Guess,
                            Value = val < message.Guess 
                                ? "greater" 
                                : val > message.Guess 
                                    ? "less" 
                                    : "correct",
                            TimeInSeconds = (int)timer.Elapsed.TotalSeconds
                        };

                        if (respMessage.Value == "correct")
                        {
                            timer.Stop();
                            GameState.Toplist.Add(new ToplistItem 
                            {
                                Name = player.Name,
                                TimeInSeconds = (int)timer.Elapsed.TotalSeconds,
                                Guesses = player.CurrentGuesses
                            });
                            GameState.Toplist = GameState.Toplist.OrderBy(e => e.Guesses).ThenBy(e => e.TimeInSeconds).ToList();
                            player.CurrentGuesses = 0;
                        }

                        var response = new ArraySegment<byte>(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(respMessage)));
                        foreach(var client in GameState.Clients.ToList())
                        {
                            await client.Value.Socket.SendAsync(response, WebSocketMessageType.Text, true, CancellationToken.None);
                        }
                        break;
                    case "toplist":
                        await socket.SendAsync(
                            new ArraySegment<byte>(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(GameState.Toplist))),
                            WebSocketMessageType.Text,
                            true,
                            CancellationToken.None);
                        break;
                }
            }
            GameState.Clients.TryRemove(socketId, out var _);
        }
    }
}
