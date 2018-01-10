using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashDesk.Model
{
    public class Deposit : IDeposit
    {

        [NotMapped]
        IMembership IDeposit.Membership
        {
            get
            {
                return Membership;
            }
        }

        [Key]
        public int DepositId { get; set; }

        [Required]
        public Membership Membership { get; set; }

        [Required]
        public decimal Amount { get; set; }

    }
}