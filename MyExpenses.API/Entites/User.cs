using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyExpenses.API.Entities
{
	public class User
	{
		[Key]
		public long Id { get; set; }

		public ICollection<Expense> Expenses { get; set; }
		= new List<Expense>();
	}
}
