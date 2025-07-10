using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using System;

namespace ACUnified.Portal.Hubs
{
    // Hubs/FingerprintHub.cs

    public class FingerprintHub : Hub
    {
        private readonly IConfiguration _configuration;
        private static readonly Dictionary<string, string> ConnectedClients = new Dictionary<string, string>();


        public FingerprintHub(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task JoinGroup(string groupName,string clientName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            Console.WriteLine($"Client {clientName} with connection id {Context.ConnectionId} joined group {groupName}");

            // Keep track of connected clients and their custom names
            ConnectedClients[Context.ConnectionId] = clientName;
        }

        public async Task RequestEnrollFingerprint(string userId,string groupName,string targetClientName)
        {
            // Generate a unique request ID
            string requestId = Guid.NewGuid().ToString();

            var targetConnectionId = ConnectedClients.FirstOrDefault(client => client.Value == targetClientName).Key;



            // Notify the console app to start enrolling fingerprint for the specified user
            try
            {
                var context = Clients.Client(targetConnectionId);
                if (targetConnectionId != null)
                {
                    // Client is connected
                    try
                    {
                        
                        
                        await context.SendAsync("EnrollFingerprint", userId, targetClientName);

                        await Console.Out.WriteLineAsync($"Request Sent to {targetClientName} ");
                    }
                    catch (Exception e)
                    {
                        if (e is HubException)
                        {
                            // Client not connected
                            Console.WriteLine($"Client {targetClientName} is not connected");
                        }
                        Console.WriteLine("Client is Not Connected");
                    }
                   
                }
                else
                {
                    // Client not connected
                    Console.WriteLine($"Client {targetClientName} not connected");
                }

                // await Clients.All.SendAsync("EnrollFingerprint", userId, requestId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending message to client {targetClientName}: {ex.Message}");
            }            

        }

        /*public async Task RequestVerifyFingerprint(string userId)
        {
            // Generate a unique request ID
            string requestId = Guid.NewGuid().ToString();

            // Notify the console app to start verifying fingerprint for the specified user
            await Clients.Client("consoleConnectionId").SendAsync("VerifyFingerprint", userId, requestId);
        }*/

        public override async Task OnConnectedAsync()
        {
            // Log or print connection information
            Console.WriteLine($"Server connected: {Context.ConnectionId}");
          

                    await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            // Log or print disconnection information
            Console.WriteLine($"Server disconnected: {Context.ConnectionId}");
            if (ConnectedClients.Select(x => x.Key == Context.ConnectionId).Count()>=1)
            {
                ConnectedClients.Remove(Context.ConnectionId);
            }
            
            await base.OnDisconnectedAsync(exception);
        }

        public async Task EnrollmentResult(string requestId, string result)
        {
            // Handle the enrollment result here
            // You can store the result in a database, notify other clients, etc.

            await Clients.All.SendAsync("EnrollmentAcknowledgment", requestId, result);
        }
    }

}
