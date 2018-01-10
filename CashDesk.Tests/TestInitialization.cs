using System;
using System.Threading.Tasks;
using Xunit;

namespace CashDesk.Tests
{
    public class TestInitialization
    {
        [Fact]
        public async Task MultipleInitializations()
        {
            using (var dal = new DataAccess())
            {
                 dal.InitializeDatabaseAsync();
                 Assert.ThrowsAsync<InvalidOperationException>(async () =>  dal.InitializeDatabaseAsync());
            }
        }

        [Fact]
        public void NoInitialization()
        {
            using (var dal = new DataAccess())
            {
                Assert.ThrowsAsync<InvalidOperationException>(async () =>  dal.AddMemberAsync("A", "B", DateTime.Today));
                Assert.ThrowsAsync<InvalidOperationException>(async () =>  dal.DeleteMemberAsync(0));
                Assert.ThrowsAsync<InvalidOperationException>(async () =>  dal.JoinMemberAsync(0));
                Assert.ThrowsAsync<InvalidOperationException>(async () =>  dal.CancelMembershipAsync(0));
                Assert.ThrowsAsync<InvalidOperationException>(async () =>  dal.DepositAsync(0, 1M));
                Assert.ThrowsAsync<InvalidOperationException>(async () =>  dal.GetDepositStatisticsAsync());
            }
        }
    }
}
