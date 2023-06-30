using Application.Constants;
using Application.Contracts.Persistence;
using Application.Persistence.Contracts;
using Microsoft.AspNetCore.Http;

namespace Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ILeaveAllocationRepository _leaveAllocationRepository;
        private ILeaveTypeRepository _leaveTypeRepository;
        private ILeaveRequestRepository _leaveRequestRepository;

        public ILeaveAllocationRepository LeaveAllocationRepository =>
            _leaveAllocationRepository ??= new LeaveAllocationRepository(_context);

        public ILeaveTypeRepository LeaveTypeRepository =>
            _leaveTypeRepository ??= new LeaveTypeRepository(_context);

        public ILeaveRequestRepository LeaveRequestRepository =>
            _leaveRequestRepository ??= new LeaveRequestRepository(_context);

        public UnitOfWork(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task Save()
        {
            var username = _httpContextAccessor.HttpContext.User.FindFirst(CustomClaimTypes.Uid)?.Value;

            await _context.SaveChangesAsync(username);
        }
    }
}