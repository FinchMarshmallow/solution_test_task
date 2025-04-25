using Microsoft.AspNetCore.Mvc;
using Main;
using System;
using System.Text;

namespace Presentation_Layer_Server.Controllers
{
	[ApiController]
	[Route("/")]
	public class ControllerView : ControllerBase
	{
		private readonly string
			muzzle = $"{Config.filePath}/Presentation_Layer (Server)/View/wwwroot/miezzle/";


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
