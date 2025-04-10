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

		public static bool SignUp(UserData data)
		{

			if (users.GetValueOrDefault(data.email) == null)
			{
				Program.ConsoleColorBeautiful("new user:");
				Console.WriteLine($"\nemail:\t\t {data.email}\npassword:\t {data.password}\nrole:\t\t {data.role.ToString()}");

				users[data.email] = data;
				return true;
			}
			else
			{
				Program.ConsoleColorError("this user already exists");
				return false;
			}
		}

		public static bool TrySignIn(string email, string password)
		{
			return users.TryGetValue(email, out UserData? data) && data.password == password;
		}

		public static UserData? GetUserData(string email)
		{
			return users.GetValueOrDefault(email);
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
