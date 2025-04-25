using Core.Interfaces;
using Core;
using System.Threading.Tasks;
using LayerPresentation.Server;
using System.Net.Sockets;
using System.Net;

namespace Main
{
	internal class Program
	{
		static async Task Main(string[] args)
		{
			if (!Massage.QuestionUserBool("use automatic port ?"))
			{
				Config.port = Massage.GetInputInt("Enter port: ");
			}
			else
			{
				Config.port = GetFreePort();
			}

			Config.url = $"https://localhost:{Config.port}/";

			Config.filePath = FindFilePath();

			IServer server = new Server();

			server.StartServer(Config.url, Config.filePath, Config.passwordJWT);

			Massage.LogGood("Server -> ");
			Massage.Log(Config.url);

			await DontSleep();
		}


		public static async Task DontSleep() /* кастыль */ 
		{
			while (true)
			{
				await Task.Delay(100000);
				Massage.Log("Server Dont Sleep");
			}
		}

	
		private static string FindFilePath()
		{
			string fullPart = AppDomain.CurrentDomain.BaseDirectory;
			int binPos = fullPart.IndexOf("bin");

			fullPart = fullPart.Substring(0, binPos);

			Massage.Log("project position: \n" + fullPart);

			return fullPart;
		}


		private static int GetFreePort() /* кастыль */ 
		{
			var listener = new TcpListener(IPAddress.Loopback, 0);
			listener.Start();
			int port = ((IPEndPoint)listener.LocalEndpoint).Port;
			listener.Stop();
			return port;
		}
	}

}