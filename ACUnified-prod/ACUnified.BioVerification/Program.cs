using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACUnified.BioVerification
{
    public class Program
    {
        static async Task Main(string[] args)
        {


            await Console.Out.WriteLineAsync("BioVerification Server Started and Ready");
            string appurl = System.Configuration.ConfigurationSettings.AppSettings["appUrl"];

            //Data injection and other internal data plumbings.
           
            //End internal plumbings
            // Connect to the SignalR hub on the server
            //var connection = new HubConnectionBuilder()
            //    //.WithUrl("https://localhost:44305/FingerprintHub")
            //    //.WithUrl("http://localhost:5242/FingerprintHub")
            //    .WithUrl(appurl)
            //    .Build();


            string customClientName = System.Configuration.ConfigurationSettings.AppSettings["customClient"];
            string Group = System.Configuration.ConfigurationSettings.AppSettings["group"];

            // Handle connection events
            //connection.Closed += async (exception) =>
            //{
            //    await Console.Out.WriteLineAsync($"Connection closed: {exception?.Message}");

            //    // Optionally, attempt to restart the connection only if explicitly closed by the server or due to a network issue
            //    if (exception != null && !(exception is InvalidOperationException))
            //    {
            //        await connection.StartAsync();
            //    }
            //};





            //connection.On<string, string>("EnrollFingerprint", async (userId, requestId) =>
            //{
            //    await Console.Out.WriteLineAsync("Enroll Finger Called");
            //    // Receive user ID and request ID from the server
            //    // Perform fingerprint enrollment logic, save image, etc.
            //    string result = await FingerprintDeviceManager.EnrollFingerprint(userId);

            //    // Notify the server about the enrollment result
            //    await connection.InvokeAsync("EnrollmentResult", requestId, result);
            //});

            //connection.On<string, string>("VerifyFingerprint", async (userId, requestId) =>
            //{
            //    // Receive user ID and request ID from the server
            //    // Perform fingerprint verification logic, save image, etc.
            //    string result = await FingerprintDeviceManager.VerifyFingerprint(userId);

            //    // Notify the server about the verification result
            //    await connection.InvokeAsync("VerificationResult", requestId, result);
            //});
            //try
            //{
            //    await connection.StartAsync();
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.ToString());
            //}

            //string connectionId = connection.ConnectionId;
            //await Console.Out.WriteLineAsync("Connected with " + connection.ConnectionId + "and client id " + customClientName);
            //await connection.SendAsync("JoinGroup", Group, customClientName);
            // await connection.InvokeAsync("RequestEnrollFingerprint", "123",Group, customClientName);

            //Validate the number of fingerprints available
            //LastNumberUpdate.ReviewFiles("C:\\Users\\1010\\Documents\\Prints\\final");
            //End validate the number of fingerpints available

            //start fingerprint matcher
            FingerprintMatcher f = new FingerprintMatcher();
            f.MatchFingerPrint("C:\\Users\\1010\\Documents\\Prints\\final\\000a6dc9-4ebc-4fc6-bddf-07cae3478f01_FingerprintImage_874.jpg", "C:\\Users\\1010\\Documents\\Prints\\final\\000a6dc9-4ebc-4fc6-bddf-07cae3478f01_FingerprintImage_874.jpg");

            // Keep the console app running or implement additional logic as needed
            Console.ReadLine();
        }
    }
}
