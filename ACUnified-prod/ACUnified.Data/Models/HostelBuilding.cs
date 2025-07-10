using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACUnified.Data.Models
{
    public class HostelBuilding : BaseClass
    {
        [Column(TypeName = "VARCHAR(50)")]
        [StringLength(50)]
        public string Name { get; set; }

        public int TotalRooms { get; set; }
 
        [Column(TypeName = "VARCHAR(50)")]
        [StringLength(50)]
        public string Gender { get; set; }
    }
}