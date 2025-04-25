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

			return user as IUser;
		}

		public IUser? GetByPassword(string password)
		{
			User? user = DatabaseManager.Сontext.Users.FirstOrDefault(user => user.Password == password);

			if (user == null) Massage.Log("user find by password: " + password);
			else
				Massage.Log("user NOT find by password: " + password);

			return user as IUser;
		}
	}
}
