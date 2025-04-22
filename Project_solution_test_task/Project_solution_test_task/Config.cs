using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main
{
	public static class Config
	{
		public static string

			// data base
			nameDb      = "test_shop",
			userId      = "postgres",
			passwordDb  = "Ql^73#91Lop@4",
			strDatabaseOptions =
				$"Host=localhost;" +
				$"Port=5432;" +
				$"Database={nameDb};" +
				$"User Id={userId};" +
				$"Password={passwordDb};",
			
			// file
			filePath    = string.Empty,
			url         = string.Empty,
			strOptions  = string.Empty;

		public static int port;
	}
}
