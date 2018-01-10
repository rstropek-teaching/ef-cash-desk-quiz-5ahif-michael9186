using CashDesk.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CashDesk
{
    /// <inheritdoc />
    public class DataAccess : IDataAccess
    {

        private DataContext db;

        /// <inheritdoc />
        public void InitializeDatabaseAsync()
        {
            if (db == null)
            {
                db = new DataContext();
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        /// <inheritdoc />
        public int AddMemberAsync(string firstName, string lastName, DateTime birthday)
        {
            checkDbInit();
            if (firstName == null || lastName == null)
            {
                throw new ArgumentException();
            }
            else
            {
                var duplicateLastName = db.Members.Where(p => p.LastName.ToUpper().Equals(lastName.ToUpper())).ToArray();

                if (duplicateLastName.Count() == 0)
                {
                    db.Members.Add(new Member { FirstName = firstName, LastName = lastName, Birthday = birthday });
                    db.SaveChanges();
                    var newmember = db.Members.Where(p => p.LastName.ToLower().Equals(lastName.ToLower())).ToArray().First();
                    return newmember.MemberNumber;
                }
                else
                {
                    throw new DuplicateNameException();
                }
            }
        }

        /// <inheritdoc />
        public void DeleteMemberAsync(int memberNumber)
        {
            checkDbInit();
            if (memberNumber < 0)
            {
                throw new ArgumentException();
            }
            else
            {
                var member = db.Members.Where(p => p.MemberNumber.Equals(memberNumber)).First();
                db.Members.Remove(member);
            }

        }

        /// <inheritdoc />
        public IMembership JoinMemberAsync(int memberNumber)
        {
            checkDbInit();
            if (memberNumber < 0)
            {
                throw new ArgumentException();
            }
            else
            {
                var member = db.Members.Where(p => p.MemberNumber.Equals(memberNumber)).First();

                if (db.Memberships.Where(p => p.End == null && p.Member.Equals(member)).Count() > 0)
                {
                    throw new AlreadyMemberException();
                }
                else
                {
                    var membership = db.Memberships.Add(new Membership { Member = member, Begin = DateTime.Now });
                    db.SaveChanges();

                    return membership.Entity;
                }
            }
        }

        /// <inheritdoc />
        public async Task<IMembership> CancelMembershipAsync(int memberNumber)
        {
            checkDbInit();
            if (memberNumber < 0)
            {
                throw new ArgumentException();
            }
            else
            {

                var member = await db.Members.Where(p => p.MemberNumber.Equals(memberNumber)).FirstAsync();

                if (db.Memberships.Where(p => p.Member.Equals(member)).Count() == 0)
                {
                    throw new NoMemberException();
                }

                var membership = await db.Memberships.Where(p => p.Member.Equals(member)).FirstAsync();
                membership.End = DateTime.Now;
                await db.SaveChangesAsync();

                return membership;
            }
        }

        /// <inheritdoc />
        public async Task DepositAsync(int memberNumber, decimal amount)
        {
            checkDbInit();
            if (memberNumber < 0 || amount < 0)
            {
                throw new ArgumentException();
            }

            var member = await db.Members.Where(p => p.MemberNumber.Equals(memberNumber)).FirstAsync();
            var membership = await db.Memberships.Where(p => p.Member.Equals(member)).FirstOrDefaultAsync();

            if (membership == null)
            {
                throw new NoMemberException();
            }

            var deposit = db.Deposits.Add(new Deposit { Membership = membership, Amount = amount });
            db.SaveChanges();

        }

        /// <inheritdoc />
        public async Task<IEnumerable<IDepositStatistics>> GetDepositStatisticsAsync()
        {
            checkDbInit();

            var deposit = await db.Deposits.GroupBy(p => new { Year = p.Membership.Begin.Year, Member = p.Membership.Member }).Select(p => new DepositStatistics { Member = p.Key.Member, Year = p.Key.Year, TotalAmount = p.Sum(t => t.Amount) }).ToListAsync();
            return deposit;
        }

        /// <inheritdoc />
        public void Dispose()
        {

        }

        public void checkDbInit()
        {
            if (db == null)
            {
                throw new InvalidOperationException();
            }
        }

    }
}