using ACUnified.BioVerification.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ACUnified.BioVerification
{
    public  class LastNumberUpdate
    {
        public static int GetLastNumber(string fileName)
        {
            // Extract the last number using regular expression
            Match match = Regex.Match(fileName, @"(\d+).jpg$");

            if (match.Success)
            {
                 if (int.TryParse(match.Groups[1].Value, out int extractedNumber))
                {
                    return extractedNumber;
                }
            }

            // Return a default value or throw an exception based on your requirements
            return -1;
        }

        public static void ReviewFiles(string folderPath)
        {
            // Replace "YourFolderPath" with the actual path to your folder containing the files
            //string folderPath = "YourFolderPath";

            // Replace "YourDatabaseConnectionString" with the actual connection string to your database
            //string databaseConnectionString = "YourDatabaseConnectionString";

            // Get all files in the folder
            string[] files = Directory.GetFiles(folderPath);
            Dictionary<string, int> RepoUpdate=new Dictionary<string, int>();
            
            foreach (string filePath in files)
            {
                // Extract the last number from the file name
                int lastNumber = GetLastNumber(filePath);
                if(lastNumber > 0)
                {
                    // Save the information to the database
                    RepoUpdate.Add(filePath, lastNumber);
                    //fetch data using the lastNumber
                    ACUDbContext dbc = new ACUDbContext();
                    var x2 = dbc.Student.Where(xx => xx.Id == lastNumber).FirstOrDefault();
                    string fileName = Path.GetFileName(filePath);
                    x2.FingerUrl = fileName;
                    //update fingerurl using the lastnumer
                    dbc.Student.AddOrUpdate<Student>(x2);
                    dbc.SaveChanges();
                    Console.WriteLine($"Updated {fileName}");
                }
               
            }
           

            Console.WriteLine("done with the files");
            Console.ReadKey();
    }
}
}
