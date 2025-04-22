using Microsoft.AspNetCore;
using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.Runtime.Intrinsics.X86;
using Microsoft.AspNetCore.Hosting.Server.Features;
using System.Net.Sockets;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Npgsql;
using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Intrinsics.Arm;
using System.Security.Policy;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text;
using _Main;


namespace Main
{
	static class Program
	{
		private static void Main(string[] args)
		{
			//DataBaseInit();

			bool isUseAutomaticPort =
				//true;
				Massage.QuestionUserBool("use automatic port ?");

			if (!isUseAutomaticPort)
			{
				Config.port = Massage.GetInputInt("Enter port: ");
			}
			else
			{
				Config.port = GetFreePort();
			}

			Config.url = 
				//"https://localhost:51785/";
				$"https://localhost:{Config.port}/";

			RaiseServer(Config.url);

			Massage.LogGood("Server -> ");
			Massage.Log(Config.url);

			Config.filePath = FindFilePath();



			while (true) 
			{ 
				Task.Delay(100); // кастыль, чтоб сервак не закрылся
			}
		}



		private static string FindFilePath()
		{
			string fullPart = AppDomain.CurrentDomain.BaseDirectory;
			int binPos = fullPart.IndexOf("bin");
			return fullPart.Substring(0, binPos);
		}

		private static async void RaiseServer(string url)
		{
			IWebHost host = new WebHostBuilder()
			.UseKestrel(options =>
			{
				options.ConfigureHttpsDefaults(httpsOptions =>
				{
					httpsOptions.SslProtocols = System.Security.Authentication.SslProtocols.Tls12 | System.Security.Authentication.SslProtocols.Tls13;
				});
			})
			.UseUrls(url)
			.UseWebRoot("wwwroot")
			.ConfigureServices(services =>
			{
				services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
				.AddCookie(options =>
				{
					options.Cookie.HttpOnly = true;
					options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
					options.Cookie.SameSite = SameSiteMode.Strict;
				});

				services.AddControllers();
			})
			.Configure(app =>
			{
				app.UseHttpsRedirection();
				app.UseStaticFiles(new StaticFileOptions
				{
					OnPrepareResponse = file =>
					{
						Massage.Log($"user Get static files: \n\n{file.File.PhysicalPath}\n");
					},

					FileProvider = new PhysicalFileProvider
					(
						Path.Combine(Directory.GetCurrentDirectory(), Config.filePath)
					),
					RequestPath = Config.filePath
				});
				app.UseRouting();
				app.UseEndpoints(endpoints =>
				{
					endpoints.MapControllers();
				});

			})
			.SuppressStatusMessages(true)
			.Build();

			await host.RunAsync();
		}



		private static int GetFreePort() // кастыль
		{
			var listener = new TcpListener(IPAddress.Loopback, 0);
			listener.Start();
			int port = ((IPEndPoint)listener.LocalEndpoint).Port;
			listener.Stop();
			return port;
		}

		public static string ConvertToHash(string str)
		{
			return Convert.ToBase64String(Encoding.UTF8.GetBytes(str));
		}


		/*
		private static void DataBaseInit()
		{
			strOptions =
				$"Host=localhost;" +
				$"Port=5432;" +
				$"Database={nameDb};" +
				$"User Id={userId};" +
				$"Password={passwordDb};";


			ConsoleColorWarning("Users:");

			if (QuestionUserBool("create test users ?"))
			{
				Random ran = new Random((int)DateTime.Now.Ticks);

				GenerationUsers(ran.Next(26, 84), Role.Default, ran);
				GenerationUsers(ran.Next(4, 16), Role.Admin, ran);

				DatabaseManager.Сontext.SaveChanges();
			}
			else
			{
				foreach (User user in DatabaseManager.Сontext.Users)
				{
					Console.WriteLine($"email: {user.Email},{new string(' ', (int)Math.Clamp(34 - user.Email.Length, 4, 34))}password: {user.Password}");
				}
			}
		}

		//warning: cringe code
		private static void GenerationUsers(int countUser, Role role, Random ran)
		{
			string[] syllables = new string[] {"ing"
,"er"
,"a"
,"ly"
,"ed"
,"i"
,"es"
,"re"
,"tion"
,"in"
,"e"
,"con"
,"y"
,"ter"
,"ex"
,"al"
,"de"
,"com"
,"o"
,"di"
,"en"
,"an"
,"ty"
,"ry"
,"u"
,"ti"
,"ri"
,"be"
,"per"
,"to"
,"pro"
,"ac"
,"ad"
,"ar"
,"ers"
,"ment"
,"or"
,"tions"
,"ble"
,"der"
,"ma"
,"na"
,"si"
,"un"
,"at"
,"dis"
,"ca"
,"cal"
,"man"
,"ap"
,"po"
,"sion"
,"vi"
,"el"
,"est"
,"la"
,"lar"
,"pa"
,"ture"
,"for"
,"is"
,"mer"
,"pe"
,"ra"
,"so"
,"ta"
,"as"
,"col"
,"fi"
,"ful"
,"ger"
,"low"
,"ni"
,"par"
,"son"
,"tle"
,"day"
,"ny"
,"pen"
,"pre"
,"tive"
,"car"
,"ci"
,"mo"
,"on"
,"ous"
,"pi"
,"se"
,"ten"
,"tor"
,"ver"
,"ber"
,"can"
,"dy"
,"et"
,"it"
,"mu"
,"no"
,"ple"
,"cu"
,"fac"
,"fer"
,"gen"
,"ic"
,"land"
,"light"
,"ob"
,"of"
,"pos"
,"tain"
,"den"
,"ings"
,"mag"
,"ments"
,"set"
,"some"
,"sub"
,"sur"
,"ters"
,"tu"
,"af"
,"au"
,"cy"
,"fa"
,"im"
,"li"
,"lo"
,"men"
,"min"
,"mon"
,"op"
,"out"
,"rec"
,"ro"
,"sen"
,"side"
,"tal"
,"tic"
,"ties"
,"ward"
,"age"
,"ba"
,"but"
,"cit"
,"cle"
,"co"
,"cov"
,"da"
,"dif"
,"ence"
,"ern"
,"eve"
,"hap"
,"ies"
,"ket"
,"lec"
,"main"
,"mar"
,"mis"
,"my"
,"nal"
,"ness"
,"ning"
,"n't"
,"nu"
,"oc"
,"pres"
,"sup"
,"te"
,"ted"
,"tem"
,"tin"
,"tri"
,"tro"
,"up"
,"va"
,"ven"
,"vis"
,"am"
,"bor"
,"by"
,"cat"
,"cent"
,"ev"
,"gan"
,"gle"
,"head"
,"high"
,"il"
,"lu"
,"me"
,"nore"
,"part"
,"por"
,"read"
,"rep"
,"su"
,"tend"
,"ther"
,"ton"
,"try"
,"um"
,"uer"
,"way"
,"ate"
,"bet"
,"bles"
,"bod"
,"cap"
,"cial"
,"cir"
,"cor"
,"coun"
,"cus"
,"dan"
,"dle"
,"ef"
,"end"
,"ent"
,"ered"
,"fin"
,"form"
,"go"
,"har"
,"ish"
,"lands"
,"let"
,"long"
,"mat"
,"meas"
,"mem"
,"mul"
,"ner"
,"play"
,"ples"
,"ply"
,"port"
,"press"
,"sat"
,"sec"
,"ser"
,"south"
,"sun"
,"the"
,"ting"
,"tra"
,"tures"
,"val"
,"var"
,"vid"
,"wil"
,"win"
,"won"
,"work"
,"act"
,"ag"
,"air"
,"als"
,"bat"
,"bi"
,"cate"
,"cen"
,"char"
,"come"
,"cul"
,"ders"
,"east"
,"fect"
,"fish"
,"fix"
,"gi"
,"grand"
,"great"
,"heav"
,"ho"
,"hunt"
,"ion"
,"its"
,"jo"
,"lat"
,"lead"
,"lect"
,"lent"
,"less"
,"lin"
,"mal"
,"mi"
,"mil"
,"moth"
,"near"
,"nel"
,"net"
,"new"
,"one"
,"point"
,"prac"
,"ral"
,"rect"
,"ried"
,"round"
,"row"
,"sa"
,"sand"
,"self"
,"sent"
,"ship"
,"sim"
,"sions"
,"sis"
,"sons"
,"stand"
,"sug"
,"tel"
,"tom"
,"tors"
,"tract"
,"tray"
,"us"
,"vel"
,"west"
,"where"
,"writ"
				};

			string[] paswordTokens = new string[]
				{
					"!",
					"@",
					"$",
					"%",
					"^",
					"&",
					"*",
					"(",
					")",
					"-",
					"+",
					"=",
					"_",
					"{",
					"}",
					"<",
					">",
					":",
					";",
					"~",
					"1",
					"2",
					"3",
					"4",
					"5",
					"6",
					"7",
					"8",
					"9",
					"0"
				};

			int
				lengthEmail,
				lengthNums;

			List<string> listEmail = new();
			string
				emailByffer = string.Empty,
					passwordBuffer = string.Empty;

			for (int i = 0; i < countUser; i++)
			{
				lengthEmail = ran.Next(2, 6);

				for (int j = 0; j < lengthEmail; j++)
				{
					emailByffer += syllables[ran.Next(0, syllables.Length)];

					if (ran.Next(0, 6) == 6)
					{
						lengthNums = ran.Next(1, 6);

						for (int k = 0; k < lengthNums; k++)
						{
							emailByffer += ran.Next(0, 9);
						}
					}
				}

				emailByffer += "@";

				lengthEmail = ran.Next(1, 3);

				for (int j = 0; j < lengthEmail; j++)
				{
					emailByffer += syllables[ran.Next(0, syllables.Length)];
				}

				emailByffer += "." + syllables[ran.Next(0, syllables.Length)];

				listEmail.Add(emailByffer);
				emailByffer = string.Empty;
			}

			foreach (string email in listEmail)
			{
				lengthNums = ran.Next(4, 16);
				for (int i = 0; i < lengthNums; i++)
				{
					passwordBuffer += paswordTokens[ran.Next(0, paswordTokens.Length)];
				}


				User user = new User
				{
					Email = email,
					Role = role,
					Password = ConvertToHash(passwordBuffer),
					Money = ran.Next(0, 9999)
				};

				DatabaseManager.Сontext.Users.Add(user);

				Console.WriteLine($"email: {user.Email},{new string(' ', (int)Math.Clamp(34 - user.Email.Length, 4, 34))}password: {passwordBuffer}");

				passwordBuffer = string.Empty;
			}
		}
		*/
	}
}
