using Newtonsoft.Json.Linq;
using Project_solution_test_task.Model.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Project_solution_test_task.Model
{
	public static class ManagerJWT
	{
		public static int LifeAccessToke => lifeAccessToke;
		public static int LifeRefreshToken => lifeRefreshToken;


		private static int
			lifeAccessToke = 10,	// minutes
			lifeRefreshToken = 30;	// days

		private static string secret = string.Empty;

		static ManagerJWT()
		{
			secret = CreateSecretPassword();

			/* beautiful console */
			{
				Console.WriteLine("\nsecret:");
				Console.BackgroundColor = ConsoleColor.Magenta;
				Console.ForegroundColor = ConsoleColor.White;
				Console.WriteLine($"\n{secret}");
				Console.ResetColor();
			}
		}

		public enum TypeToken
		{
			Access,
			Refresh
		}

		public static string GenerateToken(string email, TypeToken type)
		{
			DateTime lifeTime = NowAddTime(type);

			string header = Convert.ToBase64String
			(
				Encoding.UTF8.GetBytes
				(
					JsonSerializer.Serialize(new
					{
						alg = "HS256",
						typ = "JWT"
					})
				)
			);

			string payload = Convert.ToBase64String
			(
				Encoding.UTF8.GetBytes
				(
					JsonSerializer.Serialize(new
					{
						email,
						lifeTime
					})
				)
			);

			string signature = Convert.ToBase64String
			(
				HMACSHA256.HashData
				(
					Encoding.UTF8.GetBytes(secret),
					Encoding.UTF8.GetBytes(header + payload)
				)
			);

			return $"{header}.{payload}.{signature}";
		}

		public static User? ValidateToken(string token)
		{
			try
			{
				string[] parts = token.Split('.');

				if (parts.Length != 3)
				{
					Program.ConsoleColorError("No Validate Token: parts.Length != 3");
					return null;
				}

				string checkSignature = Convert.ToBase64String
				(
					HMACSHA256.HashData
					(
						Encoding.UTF8.GetBytes(secret),
						Encoding.UTF8.GetBytes(parts[0] + parts[1])
					)
				);

				if (checkSignature != parts[2]) return null;

				Dictionary<string, object>? payload = JsonSerializer.Deserialize<Dictionary<string, object>>
				(
					Encoding.UTF8.GetString(Convert.FromBase64String(parts[1]))
				);

				if (payload == null)
				{
					Program.ConsoleColorError("payload = null");
					Console.ResetColor();
					return null;
				}

				string[] strDate = JsonSerializer.Serialize(payload["lifeTime"]).Trim('"', '\'').Split('-', 'T', ':', 'Z', '.');
				DateTime ripTokenData = new DateTime // кастыль, я незнаю как это распарсить
				(
					int.Parse(strDate[0]), // year
					int.Parse(strDate[1]), // month
					int.Parse(strDate[2]), // day
					int.Parse(strDate[3]), // hour
					int.Parse(strDate[4]), // minute
					int.Parse(strDate[5])  // second
				);

				Console.WriteLine($"Json original: {strDate[0]}, {strDate[1]}, {strDate[2]}, {strDate[3]}, {strDate[4]}, {strDate[5]}");
				Console.WriteLine("ripTokenData: " + ripTokenData + "Ticks: " + ripTokenData.Ticks.ToString());
				Console.WriteLine("Utc Now:      " + DateTime.UtcNow.ToString() + "Ticks: " + DateTime.UtcNow.Ticks.ToString());

				if (DateTime.UtcNow.Ticks > ripTokenData.Ticks)
				{
					Program.ConsoleColorError("time over");
					return null;
				}

				Console.WriteLine($"\ndata\n{payload.ToString()}\nrip data\n{ripTokenData.ToString()}");

				string? email = payload["email"].ToString(); 

				if(email != null)
				{
					return DatabaseManager.Сontext.Users.FirstOrDefault(user => user.Email == email);
				}

				return null;
			}
			catch(Exception e)
			{
				Program.ConsoleColorError($"ManagerJWT: ValidateToken: error: \n{e.Message}");
				return null;
			}
		}

		public static string CreateSecretPassword()
		{
			Random rnd = new Random
			(
				DateTime.UtcNow.Day *
				DateTime.UtcNow.Month * 
				DateTime.UtcNow.Month
			);

			int linght = rnd.Next(16, 64);

			char[] chars = new char[linght];

			for (int i = 0; i < linght; i++)
			{
				chars[i] = (char)rnd.Next(0, 256);
			}

			return new string(chars);
		}

		private static DateTime NowAddTime(TypeToken type)
		{
			if (type == TypeToken.Access) return DateTime.UtcNow.AddMinutes(lifeAccessToke);
			else if (type == TypeToken.Refresh) return DateTime.UtcNow.AddDays(lifeRefreshToken);

			return default;
		}
	}
}
