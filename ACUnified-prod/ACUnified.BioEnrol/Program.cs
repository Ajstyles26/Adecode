using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.SignalR.Client;

class Program
{
    static async Task Main(string[] args)
    {
        // Connect to the SignalR hub on the server
        var connection = new HubConnectionBuilder()
            .WithUrl("https://yourserver/signalrhub")
            .Build();

        await connection.StartAsync();

        connection.On<string, string>("EnrollFingerprint", async (userId, requestId) =>
        {
            // Receive user ID and request ID from the server
            // Perform fingerprint enrollment logic, save image, etc.
            string result = await FingerprintDeviceManager.EnrollFingerprint(userId);

            // Notify the server about the enrollment result
            await connection.InvokeAsync("EnrollmentResult", requestId, result);
        });

        connection.On<string, string>("VerifyFingerprint", async (userId, requestId) =>
        {
            // Receive user ID and request ID from the server
            // Perform fingerprint verification logic, save image, etc.
            string result = await FingerprintDeviceManager.VerifyFingerprint(userId);

            // Notify the server about the verification result
            await connection.InvokeAsync("VerificationResult", requestId, result);
        });

        // Keep the console app running or implement additional logic as needed
        Console.ReadLine();
    }

}
