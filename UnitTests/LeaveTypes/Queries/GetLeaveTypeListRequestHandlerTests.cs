using Application.Contracts.Persistence;
using Application.DTOs.LeaveType;
using Application.Features.LeaveTypes.Handlers.Commands;
using Application.Features.LeaveTypes.Handlers.Queries;
using Application.Features.LeaveTypes.Requests.Queries;
using Application.Persistence.Contracts;
using Application.Profiles;
using AutoMapper;
using Moq;
using Shouldly;
using System.Reflection.Metadata;
using UnitTests.Mocks;
using Xunit;

namespace UnitTests.LeaveTypes.Queries
{
    public class GetLeaveTypeListRequestHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IUnitOfWork> _mockRepo;

        public GetLeaveTypeListRequestHandlerTests()
        {
            _mockRepo = MockUnitOfWork.GetUnitOfWork();

            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });

            _mapper = mapperConfig.CreateMapper();
        }

        [Fact]
        public async Task GetLeaveTypeListTest()
        {
            var handler = new GetLeaveTypeListRequestHandler(_mockRepo.Object, _mapper);

            var result = await handler.Handle(new GetLeaveTypeListRequest(), CancellationToken.None);

            result.ShouldBeOfType<List<LeaveTypeDto>>();

            result.Count.ShouldBe(3);
        }
    }
}