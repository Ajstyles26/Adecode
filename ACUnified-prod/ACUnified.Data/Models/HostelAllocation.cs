using ACUnified.Data.Enum;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACUnified.Data.Models
{

public class HostelAllocation:BaseClass
{

    public long? StudentId { get; set; }
    public Student? Student{get; set;}
    public long? ApplicationFormId {get; set;}
    public ApplicationForm? ApplicationForm{get; set;}
    public long HostelRoomId { get; set; }
    public HostelRooms? HostelRoom{get; set;}
    public DateTime DateAllocated { get; set; }
    public AllocationDuration  HostelDuration { get; set; }
    public AllocationStatus CurrentStatus { get; set; }
}
}

    