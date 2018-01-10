using System;
using System.Threading.Tasks;
using Xunit;

namespace CashDesk.Tests
{
    public class TestJoining
    {
        [Fact]
        public void InvalidParameters()
        {
            using (var dal = new DataAccess())
            {
                Assert.ThrowsAsync<ArgumentException>(async () =>  dal.JoinMemberAsync(Int32.MaxValue));
            }
        }

        [Fact]
        public async Task JoinMember()
        {
            using (var dal = new DataAccess())
            {
                 dal.InitializeDatabaseAsync();
                var memberNumber =  dal.AddMemberAsync("Foo", "JoinMember", DateTime.Today.AddYears(-18));
                 dal.JoinMemberAsync(memberNumber);
            }
        }

        [Fact]
        public async Task AlreadyMember()
        {
            using (var dal = new DataAccess())
            {
                 dal.InitializeDatabaseAsync();
                var memberNumber =  dal.AddMemberAsync("Foo", "AlreadyMemberJoining", DateTime.Today.AddYears(-18));
                 dal.JoinMemberAsync(memberNumber);
                 Assert.ThrowsAsync<AlreadyMemberException>(async () =>  dal.JoinMemberAsync(memberNumber));
            }
        }
    }
}
