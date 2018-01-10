using System;
using System.Threading.Tasks;
using Xunit;

namespace CashDesk.Tests
{
    public class TestDeleteMember
    {
        [Fact]
        public void InvalidParameters()
        {
            using (var dal = new DataAccess())
            {
                Assert.ThrowsAsync<ArgumentException>(async () =>  dal.DeleteMemberAsync(Int32.MaxValue));
            }
        }

        [Fact]
        public async Task DeleteMember()
        {
            using (var dal = new DataAccess())
            {
                 dal.InitializeDatabaseAsync();
                var memberNumber =  dal.AddMemberAsync("Foo", "DeleteMember", DateTime.Today.AddYears(-18));
                 dal.DeleteMemberAsync(memberNumber);
            }
        }

        [Fact]
        public async Task CascadeDeleteMember()
        {
            using (var dal = new DataAccess())
            {
                 dal.InitializeDatabaseAsync();
                var memberNumber =  dal.AddMemberAsync("Foo", "CascadeDeleteMember", DateTime.Today.AddYears(-18));
                 dal.JoinMemberAsync(memberNumber);
                 dal.DepositAsync(memberNumber, 100);
                 dal.DeleteMemberAsync(memberNumber);
            }
        }
    }
}
