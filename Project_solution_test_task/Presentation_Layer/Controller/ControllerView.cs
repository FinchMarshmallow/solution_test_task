using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Presentation_Layer.Controller
{
	[ApiController]
	[Route("/")]
	public class ControllerView : ControllerBase
	{
		private readonly string
			muzzle = $"{Program.filePath}/Presentation Layer (UI)/View/wwwroot/miezzle/";


		[HttpGet("/")]
		public ContentResult Muzzle()
		{
			return new ContentResult
			{
				ContentType = "text/html",
				Content = File.ReadAllText(muzzle + "miezzle.html"),
				StatusCode = 200
			};
		}
	}
}
