using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyExpenses.API.Entities
{
	public class Expense
	{
		[Key]
		public long Id { get; set; }

		[Required]
		[MaxLength(100)]
		public string Title { get; set; }

		[Required]
		[MaxLength(250)]
		public string Reason { get; set; }

		[Column(TypeName = "decimal(18,2)")]
		[Required]
		public decimal Value { get; set; }

		[Column(TypeName = "decimal(18,2)")]
		public decimal VAT { get; set; }

		public DateTime Created { get; set; }

		[ForeignKey("UserId")]
		public User User { get; set; }

		public long UserId { get; set; }
	}
}
