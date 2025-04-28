using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
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

			strOptions  = string.Empty,
			filePath    = string.Empty,
			url         = string.Empty,
			server      = string.Empty;

		public const string passwordJWT = "Ax>M*=5(p[r1;kZB_9+234!@#$(Ad^(9yh9jQwE880-769cd68$vd./m?";

		public const string passwordHash = "[rF@p80-7jv!@#$m?(AGB./_($uo8$vd#69d^(9yh9j[po-)jQwE8Jcd6";




		public static int port;
	}
}
