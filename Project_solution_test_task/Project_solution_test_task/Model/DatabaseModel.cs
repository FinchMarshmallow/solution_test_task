using Microsoft.AspNetCore.DataProtection;
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
		private static Dictionary<string, UserData> users = new();

		static DatabaseModel()
		{
			SignUp(new UserData("test@mail.com" , "12345", UserRole.Default));
			SignUp(new UserData("aser_Op@mail.com", "Opt128", UserRole.Default));
			SignUp(new UserData("catFaiter@mail.com", "vatacy999", UserRole.Default));
			SignUp(new UserData("aaytistca@mail.com", "228cring", UserRole.Default));
		}

		public static void SignUp(UserData data)
		{
			/* beautiful console */
			{
				Console.BackgroundColor = ConsoleColor.Magenta;
				Console.ForegroundColor = ConsoleColor.White;
				Console.WriteLine($"new user: \n{data.email}\n{data.password}\n{data.role.ToString()}");
				Console.ResetColor();
			}

			users[data.email] = data;
		}

		public static bool TrySignIn(string email, string password)
		{
			return users.TryGetValue(email, out UserData? data) && data.password == password;
		}

		public enum UserRole
		{
			Observer,
			Default,
			Admin,
		}

		public class UserData
		{
			public string email;
			public string password;
			public UserRole role;

			public UserData(string email, string password, UserRole role)
			{
				this.email = email;
				this.password = password;
				this.role = role;
			}
		}
	}
}
