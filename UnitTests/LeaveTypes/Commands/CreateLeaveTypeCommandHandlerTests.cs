using Application.Contracts.Persistence;
using Application.DTOs.LeaveType;
using Application.Features.LeaveTypes.Handlers.Commands;
using Application.Features.LeaveTypes.Requests.Commands;
using Application.Profiles;
using Application.Responses;
using AutoMapper;
using Moq;
using Shouldly;
using UnitTests.Mocks;
using Xunit;

namespace UnitTests.LeaveTypes.Commands
{
    public class CreateLeaveTypeCommandHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IUnitOfWork> _mockUow;
        private readonly CreateLeaveTypeDto _leaveTypeDto;
        private readonly CreateLeaveTypeCommandHandler _handler;

        public CreateLeaveTypeCommandHandlerTests()
        {
            _mockUow = MockUnitOfWork.GetUnitOfWork();

            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });

            _mapper = mapperConfig.CreateMapper();
            _handler = new CreateLeaveTypeCommandHandler(_mockUow.Object, _mapper);

            _leaveTypeDto = new CreateLeaveTypeDto
            {
                DefaultDays = 12,
                Name = "Test DTO"
            };
        }

        [Fact]
        public async Task ValidLeaveTypeAdded()
        {

            var result = await _handler.Handle(new CreateLeaveTypeCommand() { LeaveTypeDto = _leaveTypeDto }, CancellationToken.None);

            var leaveTypes = await _mockUow.Object.LeaveTypeRepository.GetAll();

            leaveTypes.Count.ShouldBe(4);

            result.ShouldBeOfType<BaseCommandResponse>();
        }

        [Fact]
        public async Task InValidLeaveTypeAdded()
        {
            _leaveTypeDto.DefaultDays = -1;

            var result = await _handler.Handle(new CreateLeaveTypeCommand() { LeaveTypeDto = _leaveTypeDto }, CancellationToken.None);

            var leaveTypes = await _mockUow.Object.LeaveTypeRepository.GetAll();

            leaveTypes.Count.ShouldBe(3);

            result.ShouldBeOfType<BaseCommandResponse>();
        }
    }
}