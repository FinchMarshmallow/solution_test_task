using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Project_solution_test_task.Model.Services.Interface;

namespace Project_solution_test_task.Controller
{
	[ApiController]
	[Route("calculator")]
	public class ControllerCalculator : ControllerBase
	{
		private readonly IServiceAddition addition;
		private readonly IServiceDivision division;
		private readonly IServiceMultiplication multiplication;
		private readonly IServiceSubtraction subtraction;

		public ControllerCalculator
		(
			IServiceAddition addition,
			IServiceDivision division, 
			IServiceMultiplication multiplication, 
			IServiceSubtraction subtraction
		){
			this.addition = addition;
			this.division = division;
			this.multiplication = multiplication;
			this.subtraction = subtraction;
		}

		[HttpGet("addition/{a}/{b}")]
		public IActionResult Addition(double a, double b)
		{
			Console.WriteLine($"Addition: {a}, {b}");
			return Ok(new { Result = addition.Operating(a, b) });
		}

		[HttpGet("division/{a}/{b}")]
		public IActionResult Division(double a, double b) => Ok(new { Result = division.Operating(a, b) });

		[HttpGet("multiplication/{a}/{b}")]
		public IActionResult Multiplication(double a, double b) => Ok(new { Result = multiplication.Operating(a, b) });

		[HttpGet("subtraction/{a}/{b}")]
		public IActionResult Subtraction(double a, double b) => Ok(new { Result = subtraction.Operating(a, b) });
	}
}
