using System.Net.WebSockets;
using System.Net;
using System.Text;
using System.Collections.Concurrent;

namespace Moodle.API.WebSocketServer
    
{
    public class WebSocketServer
    {
        
        private readonly string _ipAddress;
        private readonly int _port;

        public WebSocketServer(string ipAddress, int port)
        {
            _ipAddress = ipAddress;
            _port = port;
        }

        public async Task Run()
        {
            var host = new WebHostBuilder()
                .UseKestrel(options =>
                {
                    options.Listen(IPAddress.Parse(_ipAddress), _port);
                })
                .Configure(app =>
                {
                    app.UseWebSockets();
                    app.Use(async (HttpContext context, Func<Task> next) =>
                    {
                        if (context.WebSockets.IsWebSocketRequest)
                        {
                            WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                            await HandleWebSocket(webSocket);
                        }
                        else
                        {
                            context.Response.StatusCode = 400;
                        }
                    });
                })
                .Build();

            await host.RunAsync();

            Console.WriteLine($"WebSocket server is up and running on {_ipAddress}:{_port}");
        }



        private async Task ReceiveMessagesAsync(WebSocket webSocket, CancellationToken cancellationToken)
        {
            byte[] buffer = new byte[1024 * 4];

            while (webSocket.State == WebSocketState.Open)
            {
                WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);

                if (result.MessageType == WebSocketMessageType.Text)
                {
                    // Extract the message from the buffer
                    string message = Encoding.UTF8.GetString(buffer, 0, result.Count);

                    // Split the message into username and message content
                    string[] parts = message.Split(new char[] { ':' }, 2);
                    if (parts.Length == 2)
                    {
                        string username = parts[0].Trim();
                        string messageContent = parts[1].Trim();

                        // Log the received message with the username
                        Console.WriteLine($"Received from {username}: {messageContent}");

                        // Broadcast the message to all connected clients
                        foreach (var client in _clients.Values)
                        {
                            if (client.State == WebSocketState.Open)
                            {
                                await client.SendAsync(Encoding.UTF8.GetBytes($"{username}: {messageContent}"), WebSocketMessageType.Text, true, CancellationToken.None);
                                Console.WriteLine("Broadcast message to client");
                            }
                        }
                    }
                    else
                    {
                        // Handle messages without a username properly
                        Console.WriteLine("Received message without username.");
                    }
                }
                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    // If the client sends a close message, break out of the loop
                    break;
                }
            }
        }


        private readonly ConcurrentDictionary<Guid, WebSocket> _clients = new ConcurrentDictionary<Guid, WebSocket>();

        private async Task HandleWebSocket(WebSocket webSocket)
        {
            try
            {
                // Create a cancellation token source for handling client disconnections
                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

                // Generate a new Guid for the client
                Guid clientId = Guid.NewGuid();

                // Add the WebSocket to the clients dictionary
                _clients[clientId] = webSocket;

                // Log client connection
                Console.WriteLine($"Client connected: {clientId}");

                // Start a new task to handle messages from this WebSocket connection
                Task receiveTask = ReceiveMessagesAsync(webSocket, cancellationTokenSource.Token);

                // Wait for the receive task to complete or for the client to disconnect
                await receiveTask;

                // Remove the WebSocket from the clients dictionary
                _clients.TryRemove(clientId, out _);

                // Close the WebSocket connection
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "WebSocket connection closed", CancellationToken.None);
                Console.WriteLine($"Client disconnected: {clientId}");
            }
            catch (WebSocketException ex)
            {
                // Handle WebSocket exceptions
                Console.WriteLine($"WebSocket exception: {ex.Message}");
            }
        }
    }
}