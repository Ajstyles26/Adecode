// FingerprintDeviceManager.cs

using System.Threading.Tasks;
using System;
using System.Runtime.Remoting.Contexts;
using ACUnified.BioVerify.BioCapture;
using System.Threading;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;

public  class FingerprintDeviceManager
{
    // Singleton instance
    private static readonly FingerprintDeviceManager _instance = new FingerprintDeviceManager();


    public static FPScanner _fpScanner = new FPScannerZF1(Dermalog.Imaging.Capturing.DeviceIdentity.FG_ZF1, FPScanner.GetAttachedDevices(Dermalog.Imaging.Capturing.DeviceIdentity.FG_ZF1)[0].index);

    public static FingerprintDeviceManager Instance => _instance;
    // Callbacks for notifying the server about enrollment and verification results
    public Action<string, string> EnrollFingerprintCallback { get; set; }
    public Action<string, string> VerifyFingerprintCallback { get; set; }

    public void Start()
    {
        // Initialization logic for starting the communication with the fingerprint device
        
    }

    public void Stop()
    {
        // Cleanup and stop communication with the fingerprint device
        
    }

    public static async Task<string> EnrollFingerprint(string userId)
    {
        // Use the fingerprint device SDK to capture and enroll the fingerprint
        // Save the fingerprint image to the server and get the result
        try
        {
            _fpScanner.StartCapturing();
            _fpScanner.OnScannerImage += HandleScannerImage;
           _fpScanner.OnFingerprintsDetected +=(ofd)=> HandleFingerprintDetected(ofd, userId);
            //_fpScanner.OnFingerprintsDetected += HandleFingerprintDetected;

            await Console.Out.WriteLineAsync("Dermalog Called");
            Thread.Sleep(20000);
            FingerprintDeviceManager._fpScanner.StopCapturing();

        }
        catch (Exception e)
        {

            await Console.Out.WriteLineAsync($"Dermalog Called {e}");
        }
      
        // Mock result for demonstration purposes
        string result = "Enrollment successful";
        await Console.Out.WriteLineAsync("Enrollment Successful");

        // Notify the server about the result
        var fingerprintManager = new FingerprintDeviceManager();
        fingerprintManager.EnrollFingerprintCallback?.Invoke(userId, result);
        
        return result;
    }

    private static void HandleFingerprintDetected(List<Fingerprint> fingerprints,string userid)
    {
        _fpScanner.StopCapturing();
       var currentImage= fingerprints.FirstOrDefault().Image;
        string binDirectory = AppDomain.CurrentDomain.BaseDirectory;

        // Generate a unique file name (you might want to use user-specific information)
        string fileName = $"{Guid.NewGuid()}_FingerprintImage_{userid}.jpg";

        // Combine the bin directory and file name
        string filePath = Path.Combine(binDirectory, fileName);

        try
        {
            // Save the image to the specified file path
            currentImage.Save(filePath, ImageFormat.Jpeg);

            Console.WriteLine($"Fingerprint Image saved to: {filePath}");

            // Additional logic if needed
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving fingerprint image: {ex.Message}");
            // Handle the exception (log, notify user, etc.)
        }
    }

    private static void HandleScannerImage(System.Drawing.Image image)
    {
        Console.WriteLine("Image Received");
        //string binDirectory = AppDomain.CurrentDomain.BaseDirectory;

        //// Generate a unique file name (you might want to use user-specific information)
        //string fileName = $"{Guid.NewGuid()}_FingerprintImage.jpg";

        //// Combine the bin directory and file name
        //string filePath = Path.Combine(binDirectory, fileName);

        //try
        //{
        //    // Save the image to the specified file path
        //    image.Save(filePath, ImageFormat.Jpeg);

        //    Console.WriteLine($"Fingerprint Image saved to: {filePath}");

        //    // Additional logic if needed
        //}
        //catch (Exception ex)
        //{
        //    Console.WriteLine($"Error saving fingerprint image: {ex.Message}");
        //    // Handle the exception (log, notify user, etc.)
        //}
    }

    public static async Task<string> VerifyFingerprint(string userId)
    {
        // Use the fingerprint device SDK to capture the fingerprint for verification
        // Compare the captured fingerprint with the enrolled fingerprint data on the server

        // Mock result for demonstration purposes
        string result = "Verification successful";

        // Notify the server about the result
        var fingerprintManager = new FingerprintDeviceManager();
        fingerprintManager.VerifyFingerprintCallback?.Invoke(userId, result);

        return result;
    }
}
