using Application.Contracts.Persistence;
using Application.DTOs.LeaveType.Validators;
using Application.Features.LeaveTypes.Requests.Commands;
using Application.Responses;
using AutoMapper;
using Domain;
using MediatR;

namespace Application.Features.LeaveTypes.Handlers.Commands
{
    public class CreateLeaveTypeCommandHandler : IRequestHandler<CreateLeaveTypeCommand, BaseCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateLeaveTypeCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseCommandResponse> Handle(CreateLeaveTypeCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var validator = new CreateLeaveTypeDtoValidator();
            var validationResult = await validator.ValidateAsync(request.LeaveTypeDto);

            if (!validationResult.IsValid)
            {
                response.Message = "Creation Failed";
                response.Errors = validationResult.Errors.Select(q => q.ErrorMessage).ToList();
            }
            else
            {
                var leaveType = _mapper.Map<LeaveType>(request.LeaveTypeDto);

                leaveType = await _unitOfWork.LeaveTypeRepository.Add(leaveType);
                await _unitOfWork.Save();

                response.IsSuccess = true;
                response.Message = "Creation Successful";
                response.Id = leaveType.Id;
            }

            return response;
        }
    }
}