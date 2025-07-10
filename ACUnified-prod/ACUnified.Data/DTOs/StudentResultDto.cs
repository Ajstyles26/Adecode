using ACUnified.Data.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace ACUnified.Data.DTOs
{
	public class StudentResultDto
    {
        public long Id { get; set; }

        public string? MatricNo { get; set; }

        public long? StudentId { get; set; }
        public virtual Student? Student { get; set; }

        public long? CourseId { get; set; }
        public virtual Course? Course { get; set; }

        public long? SessionId { get; set; }
        public virtual Session? Session { get; set; }

        public long? SemesterId { get; set; }
        public virtual Semester? Semester { get; set; }

        public long? LevelId { get; set; }
        public virtual Level? Level { get; set; }

        public decimal? CA { get; set; }

        public decimal? Exam { get; set; }

        public decimal? Total { get; set; }

        public string? Grade { get; set; }

    }
}

