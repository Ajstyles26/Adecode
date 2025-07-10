using ACUnified.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACUnified.Data.DTOs
{
    public class HostelBuildingDto
    {
      
            public long Id { get; set; }
            public string Name { get; set; }
            public int TotalRooms { get; set; }

            public string Gender { get; set;}
    }
}