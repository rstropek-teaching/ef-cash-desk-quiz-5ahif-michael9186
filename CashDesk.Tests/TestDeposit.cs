using System;
using System.Threading.Tasks;
using Xunit;

namespace CashDesk.Tests
{
    public class TestDeposit
    {
        [Fact]
        public void InvalidParameters()
        {
            using (var dal = new DataAccess())
            {
                Assert.ThrowsAsync<ArgumentException>(async () =>  dal.DepositAsync(Int32.MaxValue, 100M));
                Assert.ThrowsAsync<ArgumentException>(async () =>  dal.DepositAsync(1, -1M));
            }
        }

        [Fact]
        public async Task Deposit()
        {
            using (var dal = new DataAccess())
            {
                 dal.InitializeDatabaseAsync();
                var memberNumber =  dal.AddMemberAsync("Foo", "Deposit", DateTime.Today.AddYears(-18));
                 dal.JoinMemberAsync(memberNumber);
                 dal.DepositAsync(memberNumber, 100M);
            }
        }

        [Fact]
        public async Task NoMember()
        {
            using (var dal = new DataAccess())
            {
                 dal.InitializeDatabaseAsync();
                var memberNumber =  dal.AddMemberAsync("Foo", "NoMemberDeposit", DateTime.Today.AddYears(-18));
                 Assert.ThrowsAsync<NoMemberException>(async () =>  dal.DepositAsync(memberNumber, 100M));
            }
        }
    }
}
