using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyExpenses.API.Entities
{
	public class ExpenseModel
	{
		public long Id { get; set; }

		public string Title { get; set; }

		public string Reason { get; set; }

		public decimal Value { get; set; }

		public decimal VAT { get; set; }

		public DateTime Created { get; set; }

		public long UserId { get; set; }
	}

}
