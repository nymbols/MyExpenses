using Microsoft.EntityFrameworkCore;
using MyExpenses.API.Entities;
using System;

namespace MyExpenses.API.Data
{
    public class ExpenseDbContext : DbContext
    {
        public ExpenseDbContext(DbContextOptions<ExpenseDbContext> options)
           : base(options)
        {
        }

        public DbSet<Expense> Expenses { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // seed the database with dummy data
            modelBuilder.Entity<User>().HasData(
                new User() { Id = 1 },
                new User() { Id = 2 }   
            );

            modelBuilder.Entity<Expense>().HasData(
               new Expense
               {
                   Id = 1,
                   UserId = 1,
                   Title = "Hair done",
                   Created = DateTime.UtcNow,
                   Reason = "Had to",
                   Value = 10750.00M,
                   VAT = 750.00M
               },
               new Expense
               {
                   Id = 2,
                   UserId = 1,
                   Title = "Make up done",
                   Created = DateTime.UtcNow,
                   Reason = "Had to",
                   Value = 1075.00M,
                   VAT = 75.00M
               },
               new Expense
               {
                   Id = 3,
                   UserId = 2,
                   Title = "Nails done",
                   Created = DateTime.UtcNow,
                   Reason = "Had to",
                   Value = 107.50M,
                   VAT = 7.50M
               }
            ) ;

            base.OnModelCreating(modelBuilder);
        }
    }
}
