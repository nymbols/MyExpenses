using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyExpenses.API.Entities;
using MyExpenses.API.Repositories;
using Microsoft.Extensions.Configuration;

namespace MyExpenses.API.Controllers
{
	[Route("api/v1/[controller]")]
	[ApiController]
	public class ExpensesController : ControllerBase
	{
		private readonly IExpenseRepository _expensesRepository;
		private readonly IMapper _mapper;
		public IConfiguration Configuration { get; }

		public ExpensesController(
			IExpenseRepository expenseRepository,
			IMapper mapper,
			IConfiguration configuration )
		{
			_expensesRepository = expenseRepository ??
				throw new ArgumentNullException(nameof(expenseRepository));
			_mapper = mapper ??
				throw new ArgumentNullException(nameof(expenseRepository));
			Configuration = configuration;
		}

		[HttpGet]
		public async Task<ActionResult> GetExpensesAsync()
		{
			var allExpenses = await _expensesRepository.GetExpenses();
			var expenseModels = _mapper.Map<IEnumerable<ExpenseModel>>(allExpenses);

			return Ok(new
			{
				data = expenseModels
			});
		}

		[HttpGet("{userId:long}", Name = "GetExpensesForUser")]
		public async Task<ActionResult> GetExpensesForUser(long userId)
		{
			if (!_expensesRepository.UserExists(userId))
			{
				return NotFound();
			}

			var allExpensesForUser = await _expensesRepository.GetExpensesByUserId(userId);

			var expenseModels = allExpensesForUser.Select(ex => _mapper.Map<ExpenseModel>(ex));

			return Ok(new
			{
				data = expenseModels
			});
		}

		[HttpPost("{userId:long}", Name = "CreateExpenseForUser")]
		public async Task<ActionResult> CreateExpenseForUser(long userId, CreateExpenseModel expense)
        {
            if (!_expensesRepository.UserExists(userId))
            {
				return NotFound( new
				{
					error = new
					{
						message = "User does not exist."
					}
				});
			}

			var expenseEntity = _mapper.Map<Expense>(expense);
			expenseEntity.VAT = CalculateVATFromValue(expenseEntity.Value);
			expenseEntity.UserId = userId;
			expenseEntity.Created = DateTime.UtcNow;

			_expensesRepository.AddExpense(expenseEntity);
			await _expensesRepository.SaveAsync();

			var expenseModel = _mapper.Map<ExpenseModel>(expenseEntity);
			return CreatedAtRoute("CreateExpenseForUser",
                new { userId, expenseId = expenseModel.Id },
				expenseModel);
        }

		[HttpPost]
		public async Task<ActionResult<Expense>> CreateExpenseAsync(CreateExpenseModel expense)
		{
			var user = new User();
			_expensesRepository.AddUser(user);
			await _expensesRepository.SaveAsync();

			var expenseEntity = _mapper.Map<Expense>(expense);
			expenseEntity.UserId = user.Id;
			expenseEntity.VAT = CalculateVATFromValue(expenseEntity.Value);
			expenseEntity.Created = DateTime.UtcNow;

			_expensesRepository.AddExpense(expenseEntity);
			await _expensesRepository.SaveAsync();

			var expenseModel = _mapper.Map<ExpenseModel>(expenseEntity);
			return CreatedAtRoute("CreateExpense",
				new { user.Id, expenseId = expenseModel.Id },
				expenseModel);
		}

		private decimal CalculateVATFromValue(decimal totalValue)
		{
			var vatValue = 0M;
			if (totalValue < 0)
			{
				throw new ArgumentOutOfRangeException();
			}
			string vatPercentage = Configuration.GetSection("VAT").Value;

			if (decimal.TryParse(vatPercentage, out decimal vat))
			{
				vatValue = totalValue / 100 * vat;
			}
			return vatValue;
		}
	}
}
