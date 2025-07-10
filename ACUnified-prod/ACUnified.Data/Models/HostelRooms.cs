using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACUnified.Data.Enum;

namespace ACUnified.Data.Models;

public class HostelRooms:BaseClass
{
    //public long Id { get; set; }
    public long HostelBuildingId { get; set; }
    public virtual HostelBuilding HostelBuilding { get; set; }

    public string RoomNumber { get; set; }
    public int Capacity { get; set; }
    public OccupancyStatus OccupancyStatus { get; set; }
}