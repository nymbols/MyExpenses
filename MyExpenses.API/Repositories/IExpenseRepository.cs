using MyExpenses.API.Entities;
using MyExpenses.API.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyExpenses.API.Repositories
{
    public interface IExpenseRepository
    {
        void AddExpense(Expense expense);
        Task<IEnumerable<Expense>> GetExpenses();
        Task<IEnumerable<Expense>> GetExpensesByUserId(long userId);
        void AddUser(User user);
        bool UserExists(long userId);
        Task SaveAsync();
    }
}
