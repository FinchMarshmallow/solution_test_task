using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_solution_test_task.Model.Db
{
	public static class DatabaseManager
	{
		private static AppDbContext? context = null;
		public static AppDbContext Сontext
		{
			get
			{
				while (context == null)
				{
					InitializeDatabase(Program.strOptions);
				}

				return context;
			}
		}

		public static void InitializeDatabase(string strConnection)
		{
			DbContextOptionsBuilder<AppDbContext> options = new();
			options.UseNpgsql(strConnection);
			context = new(options.Options);

			try
			{
				if (!context.Database.CanConnect())
				{
					Console.WriteLine("Creating database and tables...");

					context.Database.EnsureCreated();

					Console.WriteLine("Database created successfully!");
				}
				else
				{
					Program.ConsoleColorGood("Database already exists.");
				}
			}
			catch (Exception ex)
			{
				Program.ConsoleColorError("Error:");
				Console.WriteLine(ex.Message);
			}
		}




		// Gets

		//public static User? GetUserById(int id) => Сontext.Users.FirstOrDefault(user => user.Id == id);
		//public static User? GetUserByEmail(string email) => Сontext.Users.FirstOrDefault(user => user.Email == email);
		//public static User? GetUserByPassword(string password) => Сontext.Users.FirstOrDefault(user => user.PasswordHash == password);


		//public static Purchase? GetPurchaseById(int id) => Сontext.Purchases.FirstOrDefault(purchase => purchase.Id == id);
	}
}
