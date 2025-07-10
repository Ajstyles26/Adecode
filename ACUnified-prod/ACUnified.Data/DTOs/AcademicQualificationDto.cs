using ACUnified.Data.Enum;
using ACUnified.Data.Models;
using System;
namespace ACUnified.Data.DTOs
{
	public class AcademicQualificationDto
    {
        public long Id { get; set; }
        public string ExamType { get; set; }
        public string ExamNumber { get; set; }
         public string? ExamType1 { get; set; }
        public string? ExamNumber1 { get; set;}
        public int UTMEScore { get; set; }
        public string Subject1 { get; set; }
        public string Grade1 { get; set; }
        public string Subject2 { get; set; }
        public string Grade2 { get; set; }
        public string Subject3 { get; set; }
        public string Grade3 { get; set; }
        public string Subject4 { get; set; }
        public string Grade4 { get; set; }
        public string Subject5 { get; set; }
        public string Grade5 { get; set; }
         public string? Secondsubject1 { get; set; }
        public string? Secondgrade1 { get; set; }
        public string? Secondsubject2 { get; set; }
        public string? Secondgrade2 { get; set; }
        public string?Secondsubject3 { get; set; }
        public string? Secondgrade3 { get; set; }
        public string? Secondsubject4 { get; set; }
        public string? Secondgrade4 { get; set; }
        public string? Secondsubject5 { get; set; }
        public string? Secondgrade5 { get; set; }
         public string  UTMESubject1{get; set;}
        public int UTMESubscore1 {get; set;}
        public string  UTMESubject2{get; set;}
        public int UTMESubscore2 {get; set;}
        public string  UTMESubject3{get; set;}
        public int UTMESubscore3 {get; set;}
        public string  UTMESubject4{get; set;}
        public int UTMESubscore4 {get; set;}
        public string UTMERegNo{get;set;}
        public EntryMode Modeofentry { get; set; } = EntryMode.UTME;
        public string Choice1 { get; set; }
        public string Choice2  { get; set; }
        public string UserId { get; set; }
    }
}

