using Application.Persistence.Contracts;

namespace Application.Contracts.Persistence
{
    public interface IUnitOfWork : IDisposable
    {
        ILeaveAllocationRepository LeaveAllocationRepository { get; }

        ILeaveRequestRepository LeaveRequestRepository { get; }

        ILeaveTypeRepository LeaveTypeRepository { get; }

        Task Save();
    }
}