using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACUnified.Data.Models
{
	public class BaseClass
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
		public long Id { get; set; }
		public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime UpdatedDate { get; set; }= DateTime.Now;
    }
}

