using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using ACUnified.Data.DTOs;
using ACUnified.Business.IServices;
using ACUnified.Business.Services.IServices;
using ACUnified.Business.Services;
using System.Text;

namespace ACUnified.Business.Services
{
    public class CsvExportServices : ICsvExportServices
    {
        public byte[] ExportToCsvs(IEnumerable<ApplicationFormDto> data)
        {
            using (var memoryStream = new MemoryStream())
            using (var streamWriter = new StreamWriter(memoryStream))
            {
                // Write header
                streamWriter.WriteLine("Name,Form Reference,Age,Exam Type,Mode of Entry,UTMERegNo,Choice1,Choice2,ApprovedCourse,UTME Score,Subject1,Subject2,Subject3,Subject4,Subject5,Secondsubject1,Secondsubject2,Secondsubject3,Secondsubject4,Secondsubject5,UTME Subject1,UTME Subject2,UTME Subject3,UTME Subject4");

                foreach (var item in data)
                {
                    int age = CalculateAge(item.BioData.DOB);
                    var line = new StringBuilder();

                    AppendIfNotNull(line, $"{item.BioData.FirstName} {item.BioData.LastName}");
                    AppendIfNotNull(line, item.FormRefNo);
                    AppendIfNotNull(line, age.ToString());
                    AppendIfNotNull(line, item.AcademicQualification.ExamType);
                    AppendIfNotNull(line, item.AcademicQualification.Modeofentry);
                    AppendIfNotNull(line, item.AcademicQualification.UTMERegNo);
                    AppendIfNotNull(line, item.AcademicQualification.Choice1);
                    AppendIfNotNull(line, item.AcademicQualification.Choice2);
                    AppendIfNotNull(line, item.ApprovedCourse);
                    line.Append($" {item.AcademicQualification.UTMEScore},");   
                    AppendIfNotNull(line, $"{item.AcademicQualification.Subject1} {item.AcademicQualification.Grade1}");
                    AppendIfNotNull(line, $"{item.AcademicQualification.Subject2} {item.AcademicQualification.Grade2}");
                    AppendIfNotNull(line, $"{item.AcademicQualification.Subject3} {item.AcademicQualification.Grade3}");
                    AppendIfNotNull(line, $"{item.AcademicQualification.Subject4} {item.AcademicQualification.Grade4}");
                    AppendIfNotNull(line, $"{item.AcademicQualification.Subject5} {item.AcademicQualification.Grade5}");
                    AppendIfNotNull(line, $"{item.AcademicQualification.Secondsubject1} {item.AcademicQualification.Secondgrade1}");
                     AppendIfNotNull(line, $"{item.AcademicQualification.Secondsubject2} {item.AcademicQualification.Secondgrade2}");
                      AppendIfNotNull(line, $"{item.AcademicQualification.Secondsubject3} {item.AcademicQualification.Secondgrade3}");
                       AppendIfNotNull(line, $"{item.AcademicQualification.Secondsubject4} {item.AcademicQualification.Secondgrade4}");
                        AppendIfNotNull(line, $"{item.AcademicQualification.Secondsubject5} {item.AcademicQualification.Secondgrade5}");
                    AppendIfNotNull(line, $"{item.AcademicQualification.UTMESubject1} {item.AcademicQualification.UTMESubscore1}");
                    AppendIfNotNull(line, $"{item.AcademicQualification.UTMESubject2} {item.AcademicQualification.UTMESubscore2}");
                    AppendIfNotNull(line, $"{item.AcademicQualification.UTMESubject3} {item.AcademicQualification.UTMESubscore3}");
                    AppendIfNotNull(line, $"{item.AcademicQualification.UTMESubject4} {item.AcademicQualification.UTMESubscore4}");

                    // Remove the last comma if it exists
                    if (line.Length > 0 && line[line.Length - 1] == ',')
                    {
                        line.Length--;
                    }

                    streamWriter.WriteLine(line.ToString());
                }

                streamWriter.Flush();
                return memoryStream.ToArray();
            }
        }

        private void AppendIfNotNull(StringBuilder sb, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                sb.Append(value);
            }
            sb.Append(',');
        }

        private int CalculateAge(DateTime dateOfBirth)
        {
            var today = DateTime.Today;
            var age = today.Year - dateOfBirth.Year;
            if (dateOfBirth.Date > today.AddYears(-age)) age--;
            return age;
        }
    }
}