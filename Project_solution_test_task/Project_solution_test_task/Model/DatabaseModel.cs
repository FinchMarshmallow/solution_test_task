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

		static DatabaseModel()
		{
			Register("test@mail.com", "12345");
			Register("aser_Op@mail.com", "Opt128");
			Register("catFaiter@mail.com", "vatacy999");
			Register("aaytistca@mail.com", "228cring");
		}

		public static void Register(string email, string password)
		{
			users[email] = password;
		}

		public static bool TryLogin(string email, string password)
		{
			return users.TryGetValue(email, out string? storedPass) && storedPass == password;
		}
	}
}
