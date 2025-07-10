using ACUnified.Data.Enum;
using ACUnified.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACUnified.Data.DTOs
{
    public class HostelRoomsDto
    {
        public long Id { get; set; }
        public long HostelBuildingId { get; set; }
        public virtual HostelBuilding HostelBuilding { get; set; }
        public string RoomNumber { get; set; }
        public int Capacity { get; set; }
        public OccupancyStatus OccupancyStatus { get; set; }
        public List<ApplicationFormDto> Occupants { get; set; }
    }
}