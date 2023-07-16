using Application.Contracts.Persistence;
using Moq;

namespace UnitTests.Mocks
{
    public static class MockUnitOfWork
    {
        public static Mock<IUnitOfWork> GetUnitOfWork()
        {
            var mockUow = new Mock<IUnitOfWork>();
            var mockLeaveTypeRepo = MockLeaveTypeRepository.GetLeaveTypeRepository();

            mockUow.Setup(r => r.LeaveTypeRepository).Returns(mockLeaveTypeRepo.Object);

            return mockUow;
        }
    }
}