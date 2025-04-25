using Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LayerDataAccess.Migrations
{
	public static class DatabaseManager
	{
		public static string strOptions = string.Empty;

		private static AppDbContext? context = null;
		public static AppDbContext Сontext
		{
			get
			{
				while (context == null ) InitializeDatabase(strOptions);
				return context;
			}
		}

		public static void InitializeDatabase(string strOptions)
		{
			DbContextOptionsBuilder<AppDbContext> options = new();
			options.UseNpgsql(strOptions);
			context = new(options.Options);

			try
			{
				if (!context.Database.CanConnect())
				{
					Massage.Log("Creating database and tables...");

					context.Database.EnsureCreated();

					Massage.Log("Database created successfully!");
				}
				else
				{
					Massage.LogGood("Database already exists.");
				}
			}
			catch (Exception ex)
			{
				Massage.LogError("Error:");
				Massage.Log(ex.Message);
			}
		}
	}
}
