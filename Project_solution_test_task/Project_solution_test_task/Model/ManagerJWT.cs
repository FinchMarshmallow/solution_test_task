using Newtonsoft.Json.Linq;
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
		private static int
			lifeAccessToke,		// minutes
			lifeRefreshToken;	// days

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
			AccessToke,
			RefreshToken
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

		public static bool ValidateToken(string token)
		{
			try
			{
				string[] parts = token.Split('.');

				if (parts.Length != 3) return false;

				string checkSignature = Convert.ToBase64String
				(
					HMACSHA256.HashData
					(
						Encoding.UTF8.GetBytes(secret),
						Encoding.UTF8.GetBytes(parts[0] + parts[1])
					)
				);

				if (checkSignature != parts[2]) return false;

				Dictionary<string, object>? payload = JsonSerializer.Deserialize<Dictionary<string, object>>
				(
					Encoding.UTF8.GetString(Convert.FromBase64String(parts[1]))
				);

				if (payload == null || DateTime.UtcNow.Ticks > (long)payload["exp"]) return false;

				return true;
			}
			catch
			{
				return false;
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
			if (type == TypeToken.AccessToke) return DateTime.UtcNow.AddMinutes(lifeAccessToke);
			else if (type == TypeToken.RefreshToken) return DateTime.UtcNow.AddDays(lifeRefreshToken);

			return default;
		}
	}
}
