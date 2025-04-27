using Microsoft.AspNetCore.Mvc;
using Core;
using System;
using System.Text;
using LayerPresentation.Server;
using System.IO;

namespace PresentationLayer.Controllers
{
	[ApiController]
	[Route("/")]
	public class ControllerView : ControllerBase
	{
		private static string

			pos = $"{Server.FindFilePath()}Presentation Layer\\View\\wwwroot\\",

			muzzle = $"{pos}miezzle\\";


		[HttpGet()]
		public IActionResult Muzzle()
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