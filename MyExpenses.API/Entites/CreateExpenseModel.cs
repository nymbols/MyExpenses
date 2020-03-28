using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyExpenses.API.Entities
{
	public class CreateExpenseModel
	{
		public string Title { get; set; }

		public string Reason { get; set; }

		public decimal Value { get; set; }
	}
}
