using System.Collections.Generic;

namespace CashDesk.Model
{
    public class CashDesk
    {

        public List<Member> Members;

        public List<Membership> Membership;

        public List<Deposit> Deposit;

        public List<DepositStatistics> DepositStatistics;

        public CashDesk()
        {
            Members = new List<Member>();
            Membership = new List<Membership>();
            Deposit = new List<Deposit>();
            DepositStatistics = new List<DepositStatistics>();
        }

    }
}
