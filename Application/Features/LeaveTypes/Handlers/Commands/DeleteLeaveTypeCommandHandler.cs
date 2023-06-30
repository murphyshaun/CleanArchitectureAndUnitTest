using Application.Contracts.Persistence;
using Application.Exceptions;
using Application.Features.LeaveTypes.Requests.Commands;
using AutoMapper;
using Domain;
using MediatR;

namespace Application.Features.LeaveTypes.Handlers.Commands
{
    internal class DeleteLeaveTypeCommandHandler : IRequestHandler<DeleteLeaveTypeCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DeleteLeaveTypeCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(DeleteLeaveTypeCommand request, CancellationToken cancellationToken)
        {
            var leaveType = await _unitOfWork.LeaveTypeRepository.Get(request.Id);

            if (leaveType == null)
                throw new NotFoundException(nameof(LeaveType), request.Id);

            await _unitOfWork.LeaveTypeRepository.Delete(leaveType);
            await _unitOfWork.Save();

            return Unit.Value;
        }
    }
}