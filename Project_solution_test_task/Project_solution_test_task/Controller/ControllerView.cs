using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Project_solution_test_task.Controller
{
	[ApiController]
	[Route("/")]
	public class ControllerView : ControllerBase
	{
		private readonly string
			muzzle = $"{Program.filePath}/wwwroot/index.html";


		[HttpGet("/")] // главная страница
		public ContentResult Muzzle()
		{
			return new ContentResult
			{
				ContentType = "text/html",
				Content = System.IO.File.ReadAllText(muzzle),
				StatusCode = 200
			};
		}
	}
}
