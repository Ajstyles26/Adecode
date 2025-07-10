using ACUnified.Data.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace ACUnified.Data.DTOs
{
	public class ApplicationProgramDto
	{
        public long Id { get; set; }
        [Required(ErrorMessage="Please Provide Application Program Name")]
        public string Name { get; set; }
        public string Description { get; set; }
        //institution provider
        public virtual long? InstitutionProviderId { get; set; }

    }
}

