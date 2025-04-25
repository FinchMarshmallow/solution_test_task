using Microsoft.AspNetCore.Mvc;
using Core;
using System;
using System.Text;

namespace PresentationLayer.Controllers
{
	[ApiController]
	[Route("/")]
	public class ControllerView : ControllerBase
	{
		private readonly string
			muzzle = $"{Config.filePath}/Layer_Presentation/View/wwwroot/miezzle/";


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
