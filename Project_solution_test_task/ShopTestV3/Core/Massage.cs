using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Core
{
	public static class Massage
	{
		// здесь будет система логирования

		public static void Log(string massage)
		{
			Console.Write(DateTime.Now.ToString() + ": ");
			Console.WriteLine(massage);
		}

		public static void LogError(string massage)
		{
			Console.Write(DateTime.Now.ToString() + ": ");

			Console.BackgroundColor = ConsoleColor.Red;
			Console.ForegroundColor = ConsoleColor.White;

			Console.WriteLine(massage);
			Console.ResetColor();
		}

		public static void LogWarning(string massage)
		{
			Console.Write(DateTime.Now.ToString() + ": ");

			Console.BackgroundColor = ConsoleColor.DarkYellow;
			Console.ForegroundColor = ConsoleColor.White;

			Console.WriteLine(massage);
			Console.ResetColor();
		}

		public static void LogGood(string massage)
		{
			Console.Write(DateTime.Now.ToString() + ": ");

			Console.BackgroundColor = ConsoleColor.Green;
			Console.ForegroundColor = ConsoleColor.White;

			Console.WriteLine(massage);
			Console.ResetColor();
		}

		public static void LogBeautiful(string massage)
		{
			Console.Write(DateTime.Now.ToString() + ": ");

			Console.BackgroundColor = ConsoleColor.DarkMagenta;
			Console.ForegroundColor = ConsoleColor.White;

			Console.WriteLine(massage);
			Console.ResetColor();
		}

		public static bool QuestionUserBool(string question = "what is the meaning of life ?")
		{
			Console.WriteLine(question);

			Console.ResetColor();
			Console.Write(" ");


			Console.BackgroundColor = ConsoleColor.Red;
			Console.ForegroundColor = ConsoleColor.White;
			Console.Write("N");
			Console.ResetColor();

			Console.ResetColor();
			Console.Write(" / ");


			Console.BackgroundColor = ConsoleColor.Green;
			Console.ForegroundColor = ConsoleColor.White;
			Console.Write("Y");
			Console.ResetColor();

			Console.ResetColor();
			Console.Write(" ");

			while (true)
			{
				ConsoleKey buffer = Console.ReadKey(true).Key;

				if (buffer == ConsoleKey.Y)
				{
					Log(question + " : true, Y");
					return true;
				}
				else if (buffer == ConsoleKey.N)
				{
					Log(question + " : false, N");
					return false;
				}
			}
		}

		public static int GetInputInt(string header = "Enter int: ")
		{
			string? input = null;
			string
				exception = "",
				nullReference = "you not entered text";

			while (true)
			{
				

				if (exception.Length > 0)
				{
					Console.BackgroundColor = ConsoleColor.Yellow;
					Console.ForegroundColor = ConsoleColor.White;
					Console.Write($"{exception}\n");
					Console.ResetColor();
				}
				if (header.Length > 0)
				{
					Console.WriteLine(header);
				}

				try
				{
					input = Console.ReadLine();

					if (input == null)
					{
						throw new ArgumentNullException(nullReference);
					}
					else
					{
						int buffer = int.Parse(input);
						Log(header + " Enter: " + input);
						return buffer;
					}

				}
				catch (Exception e)
				{
					exception = e.Message;
				}

				Console.SetCursorPosition(0, Console.CursorTop - 1);
				Console.Write("\r" + new string(' ', Console.BufferWidth) + "\r");
			}
		}
	}
}
