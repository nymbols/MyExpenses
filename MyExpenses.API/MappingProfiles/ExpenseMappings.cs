using AutoMapper;
using MyExpenses.API.Entities;

namespace MyExpenses.API.MappingProfiles
{
	public class ExpenseMappings : Profile
    {
        public ExpenseMappings()
        {
            CreateMap<Expense, ExpenseModel>().ReverseMap();
            CreateMap<Expense, CreateExpenseModel>().ReverseMap();
        }
    }
}