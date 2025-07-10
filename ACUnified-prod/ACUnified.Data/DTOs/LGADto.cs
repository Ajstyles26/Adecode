using System;
using ACUnified.Data.Models;
namespace ACUnified.Data.DTOs
{
    public class LGADto
    {
        public long Id { get; set; }
        public long StateID { get; set; }
    public virtual State State { get; set; }
    //institution provider
        public string Name { get; set; }
        public string Description { get; set; }
    }
}