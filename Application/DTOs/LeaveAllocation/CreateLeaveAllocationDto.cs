using Application.DTOs.Common;

namespace Application.DTOs.LeaveAllocation
{
    public class CreateLeaveAllocationDto : BaseDto
    {
        public int LeaveTypeId { get; set; }
    }
}