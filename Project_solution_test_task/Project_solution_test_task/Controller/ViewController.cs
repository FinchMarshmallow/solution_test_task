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
	public class ViewController : ControllerBase
	{
		private readonly string
			muzzle = $"{Program.filePath}/View/wwwroot/miezzle/";


		[HttpGet("/")]
		public ContentResult Muzzle()
		{
			return new ContentResult
			{
				ContentType = "text/html",
				Content = System.IO.File.ReadAllText(muzzle + "miezzle.html"),
				StatusCode = 200
			};
		}
	}
}
