using Application.DTOs.Common;
using Application.DTOs.LeaveType;
using Application.Models.Identity;

namespace Application.DTOs.LeaveAllocation
{
    public class LeaveAllocationDto : BaseDto
    {
        public int NumberOfDays { get; set; }

        public LeaveTypeDto LeaveType { get; set; }

        public Employee Employee { get; set; }

        public string EmployeeId { get; set; }

        public int LeaveTypeId { get; set; }

        public int Period { get; set; }
    }
}