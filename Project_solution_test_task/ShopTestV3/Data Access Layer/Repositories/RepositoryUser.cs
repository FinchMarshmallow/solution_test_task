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

using LayerDataAccess.Model;
using LayerDataAccess;
using System.Security.Cryptography;

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

			DatabaseManager.context.Users.Add(new User()
			{
				Email = email,
				Password = password,
				Role = role
			});

			Massage.LogGood("add user");
			Massage.Log($"email: {email} \npassword: {password} \nrole: {role.ToString()}");

			DatabaseManager.context.SaveChanges();

			return true;
		}

		public IUser? GetByEmail(string email)
		{
			User? user = DatabaseManager.context.Users.FirstOrDefault(user => user.Email == email);

			if (user == null) Massage.Log("user find by email: " + email);
			else
				Massage.Log("user NOT find by email: " + email);

			return user;
		}

		public IUser? GetByPassword(string password)
		{
			User? user = DatabaseManager.context.Users.FirstOrDefault(user => user.Password == password);

			if (user == null) Massage.Log("user find by password: " + password);
			else
				Massage.Log("user NOT find by password: " + password);

			return user;
		}
		
		public bool? PasswordCheck(string email, string password)
		{
			IUser? user = GetByEmail(email);

			if (user == null) return null;

			if (user.Password != HashPassword(password, user.Email, user.Id)) return false;

			return true;
		}

		public static string HashPassword(string password, string email, int id)
		{
			int randomGrain = id;

			for (int i = 0; i < password.Length; i++)
				randomGrain += (char)password[i];

			for (int i = 0; i < Config.passwordHash.Length; i++)
				randomGrain += (char)password[i];

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
				Massage.Log(Convert.ToBase64String(resyldHash));

				switch (mixingOrder[i])
				{
					case 0:

						if (counterId < saltId.Length)
						{
							counterId++;
							saltPasswordHash[i] = saltId[counterId];
						}
						else
							goto case 1;
						break;
					case 1:

						if (counterEmail < saltEmail.Length)
						{
							counterEmail++;
							saltPasswordHash[i] = saltEmail[counterEmail];
						}
						else
							goto case 2;
						break;
					case 2:
						if (counterPassword < saltPassword.Length)
						{
							counterPassword++;
							saltPasswordHash[i] = saltPassword[counterPassword];
						}
						else
							goto case 0;
						break;

					default:
						Massage.LogError("Error hash password");
						break;
				}
			}

			return Convert.ToBase64String(new HMACSHA256(resyldHash).ComputeHash(saltId));
		}
	}
}
