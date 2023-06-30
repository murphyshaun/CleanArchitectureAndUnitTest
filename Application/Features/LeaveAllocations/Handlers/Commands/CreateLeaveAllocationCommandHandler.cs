using Application.Contracts.Identity;
using Application.Contracts.Persistence;
using Application.DTOs.LeaveAllocation.Validators;
using Application.Features.LeaveAllocations.Requests.Commands;
using Application.Responses;
using AutoMapper;
using Domain;
using MediatR;

namespace Application.Features.LeaveAllocations.Handlers.Commands
{
    public class CreateLeaveAllocationCommandHandler : IRequestHandler<CreateLeaveAllocationCommand, BaseCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public CreateLeaveAllocationCommandHandler(
            IUnitOfWork unitOfWork,
            IUserService userService, 
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<BaseCommandResponse> Handle(CreateLeaveAllocationCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var validator = new CreateLeaveAllocationDtoValidator(_unitOfWork.LeaveTypeRepository);
            var validationResult = await validator.ValidateAsync(request.LeaveAllocationDto);

            if (!validationResult.IsValid)
            {
                response.Message = "Allocations Failed";
                response.Errors = validationResult.Errors.Select(q => q.ErrorMessage).ToList();
            }
            else
            {
                var leaveType = await _unitOfWork.LeaveTypeRepository.Get(request.LeaveAllocationDto.LeaveTypeId);
                var employees = await _userService.GetEmployees();
                var period = DateTime.Now.Year;
                var allocations = new List<LeaveAllocation>();

                foreach (var emp in employees)
                {
                    if (await _unitOfWork.LeaveAllocationRepository.AllocationExists(emp.Id, leaveType.Id, period))
                        continue;
                    allocations.Add(new LeaveAllocation
                    {
                        EmployeeId = emp.Id,
                        LeaveTypeId = leaveType.Id,
                        NumberOfDays = leaveType.DefaultDays,
                        Period = period
                    });
                }

                await _unitOfWork.LeaveAllocationRepository.AddAllocations(allocations);
                await _unitOfWork.Save();
                response.IsSuccess = true;
                response.Message = "Allocations Successful";
            }


            return response;

        }
    }
}