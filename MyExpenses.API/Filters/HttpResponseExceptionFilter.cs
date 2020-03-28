using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace MyExpenses.API.Data
{
	public class HttpResponseExceptionFilter : IActionFilter
	{
		public void OnActionExecuting(ActionExecutingContext context) { }

		public void OnActionExecuted(ActionExecutedContext context)
		{
			if (context.Exception != null)
			{
				//todo check detailed types of ex
				context.Result = new ObjectResult(context.Exception.Message)
				{
					StatusCode = (int)HttpStatusCode.BadRequest
				};

				context.ExceptionHandled = true;
			}
		}
	}
}
