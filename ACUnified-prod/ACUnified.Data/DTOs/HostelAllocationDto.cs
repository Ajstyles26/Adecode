using ACUnified.Data.Enum;
using ACUnified.Data.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACUnified.Data.DTOs
{
    public class HostelAllocationDto
    {
        public long Id { get; set; }
        
        public long? StudentId { get; set; }
        public Student? Student { get; set; }
        
        public long? ApplicationFormId { get; set; }
        public ApplicationForm? ApplicationForm { get; set; }
        
        public long HostelRoomId { get; set; }
        public HostelRoomsDto? HostelRoom { get; set; } // Changed from HostelRooms to HostelRoom
        public DateTime DateAllocated { get; set; }
        public AllocationDuration HostelDuration { get; set; }
        public AllocationStatus CurrentStatus { get; set; }

        // Method to set either StudentId or ApplicationFormId
        public void SetAllocationIdentifier(long? studentId, long? applicationFormId)
        {
            if (studentId.HasValue)
            {
                StudentId = studentId;
                ApplicationFormId = null;
            }
            else if (applicationFormId.HasValue)
            {
                ApplicationFormId = applicationFormId;
                StudentId = null;
            }
            else
            {
                throw new ArgumentException("Either StudentId or ApplicationFormId must be provided.");
            }
        }
    }
}