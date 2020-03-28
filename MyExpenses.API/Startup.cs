using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;

using MyExpenses.API.Data;
using Microsoft.AspNetCore.Internal;
using Microsoft.AspNetCore.Rewrite;
using MyExpenses.API.Repositories;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Http;
using MyExpenses.API.MappingProfiles;

namespace MyExpenses.API
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }
		readonly string AllowSpecificOrigins = "_allowSpecificOrigins";

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			var webURL = Configuration.GetSection("Web:URL").Value;
			services.AddCors(options =>
			{
				options.AddPolicy(AllowSpecificOrigins,
				builder =>
				{
					builder.WithOrigins(webURL)
						.SetIsOriginAllowedToAllowWildcardSubdomains()
						.AllowAnyMethod()
						.AllowAnyHeader()
						.AllowCredentials();
				});
			});

			services.AddOptions();
			//services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
			services.AddAutoMapper(typeof(ExpenseMappings));

			services.AddScoped<IExpenseRepository, ExpenseRepository>();

			var connectionString = Configuration.GetSection("ConnectionStrings:DefaultConnection").Value;
			services.AddDbContext<ExpenseDbContext>(options =>
			{
				options.UseSqlServer(connectionString);
			});

			//services.AddSwaggerGen();
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "My Expenses API", Version = "v1" });
			});
			services.AddRouting(options => options.LowercaseUrls = true);
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ExpenseDbContext expenseContext)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}
			app.UseCors(AllowSpecificOrigins);

			app.UseHttpsRedirection();
			app.Use(async (context, next) =>
			{
				context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
				context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
				await next();
			});

			// Enable middleware to serve generated Swagger as a JSON endpoint.
			app.UseSwagger();

			// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
			// specifying the Swagger JSON endpoint.
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
			});

			//var option = new RewriteOptions();
			//option.AddRedirect("^$", "swagger");
			//app.UseRewriter(option);

			app.UseMvc();
			expenseContext.Database.EnsureCreated();
		}
	}
}
