using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Project_solution_test_task.Model
{
	public static class DatabaseModel
	{
		private static Dictionary<string, string> users = new();
		private static string secret = "my_super_secret_qwer1234";

		static DatabaseModel()
		{
			Register("test@mail.com", "12345");
			Register("aser_Op@mail.com", "Opt128");
			Register("catFaiter@mail.com", "vatacy999");
			Register("aaytistca@mail.com", "228cring");

			secret = CreateSecretPassword(Program.port);
		}

		public static void Register(string email, string password)
		{
			users[email] = password;
		}

		public static string GetPassword(string email)
		{
			return users[email];
		}

		public static string? Login(string email, string password)
		{
			if (users.TryGetValue(email, out var storedPass) && storedPass == password)
			{
				return GenerateToken(email, daysValid: 30);
			}
			return null;
		}

		private static string GenerateToken(string email, int daysValid)
		{
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
						exp = DateTime.UtcNow.AddDays(daysValid).Ticks
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

		public static string CreateSecretPassword(int port)
		{
			int corn = DateTime.UtcNow.Day * DateTime.UtcNow.Month * DateTime.UtcNow.Month * port;

			Random rnd = new Random(corn);

			int linght = rnd.Next(16, 64);

			char[] chars = new char[linght];

			for (int i = 0; i < linght; i++)
			{
				chars[i] = (char)rnd.Next(0, 256);
			}

			return new string(chars);
		}

		public static string? ValidateToken(string token)
		{
			try
			{
				string[] parts = token.Split('.');

				if (parts.Length != 3) return null;

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

				if (payload == null ||
					DateTime.UtcNow.Ticks > (long)payload["exp"]) return null;

				return payload["email"].ToString();
			}
			catch
			{
				return null;
			}
		}
	}
}
