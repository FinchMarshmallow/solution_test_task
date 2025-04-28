using Core.Interfaces.Models;
using Core.Interfaces.Repository;
using Core;
using LayerDataAccess.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.DataContracts;
using System.Text;
using System.Threading.Tasks;
using System;
using LayerDataAccess.Model;
using LayerDataAccess;
using System.Security.Cryptography;
using System.Net.Sockets;

namespace LayerDataAccess.Repositories
{
	public class RepositoryUser : IRepositoryUser
	{
		public bool AddUser(string email, string password, Role role)
		{
			if (GetByEmail(email) != null)
			{
				Massage.LogError("are not allowed two users one email !!! " + email);
				return false;
			}

			DatabaseManager.Сontext.Users.Add(new User()
			{
				Email = email,
				Password = password,
				Role = role
			});

			Massage.LogGood("add user");
			Massage.Log($"email: {email} \npassword: {password} \nrole: {role.ToString()}");

			DatabaseManager.Сontext.SaveChanges();

			return true;
		}

		public IUser? GetByEmail(string email)
		{
			User? user = DatabaseManager.Сontext.Users.FirstOrDefault(user => user.Email == email);

			if (user == null) Massage.Log("user find by email: " + email);
			else
				Massage.Log("user NOT find by email: " + email);

			return user;
		}

		public IUser? GetByPassword(string password)
		{
			User? user = DatabaseManager.Сontext.Users.FirstOrDefault(user => user.Password == password);

			if (user == null) Massage.Log("user find by password: " + password);
			else
				Massage.Log("user NOT find by password: " + password);

			return user;
		}
		
		public bool? PasswordCheck(string email, string password)
		{
			IUser? user = GetByEmail(email);

			if (user == null) return null;

			if (user.Password != PasswordHash(password, user.Email, user.Id)) return false;

			return true;
		}

		public string HashPassword(string password, string email, int id) => PasswordHash(password, email, id);

		public static string PasswordHash(string password, string email, int id)
		{
			int randomGrain = id;

			for (int i = 0; i < password.Length; i++)
				randomGrain += (char)password[i];

			for (int i = 0; i < Config.passwordHash.Length; i++)
				randomGrain += (char)Config.passwordHash[i];

			Random random = new Random(randomGrain);

			byte[]
				saltId = Encoding.UTF8.GetBytes(random.Next(100000, 999999).ToString()),
				saltEmail = new HMACSHA256(Encoding.UTF8.GetBytes(email)).ComputeHash(Encoding.UTF8.GetBytes(Config.passwordHash)),
				saltPassword = new HMACSHA256(Encoding.UTF8.GetBytes(password)).ComputeHash(Encoding.UTF8.GetBytes(Config.passwordHash)),
				saltPasswordHash = new HMACSHA256(Encoding.UTF8.GetBytes(Config.passwordHash)).ComputeHash(Encoding.UTF8.GetBytes(Config.passwordHash)),
				
				resyldHash = new byte[saltId.Length + saltEmail.Length + saltPassword.Length];

			int[] mixingOrder = new int[resyldHash.Length];

			for (int i = 0; i < mixingOrder.Length; i++)
				mixingOrder[i] = random.Next(0, 2);

			int
				counterId = 0,
				counterEmail = 0,
				counterPassword = 0;

			for (int i = 0; i < mixingOrder.Length; i++)
			{
				switch (mixingOrder[i])
				{
					case 0:
						if (counterId < saltId.Length)
						{
							resyldHash[i] = saltId[counterId];
							counterId++;
						}
						else
							goto case 1;
						break;
					case 1:
						if (counterEmail < saltEmail.Length)
						{
							resyldHash[i] = saltEmail[counterEmail];
							counterEmail++;
						}
						else
							goto case 2;
						break;
					case 2:
						if (counterPassword < saltPassword.Length)
						{
							resyldHash[i] = saltPassword[counterPassword];
							counterPassword++;
						}
						else
							goto case 0;
						break;

					default:
						Massage.LogError("Error hash password");
						break;
				}
			}

			return Convert.ToBase64String(new HMACSHA256(Encoding.UTF8.GetBytes(password)).ComputeHash(Encoding.UTF8.GetBytes(Config.passwordHash)));
		}
	}
}
