using Core.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repository
{
	public interface IRepositoryUser
	{
		public bool AddUser(string email, string password, Role role);
		public IUser? GetByEmail(string email);
		public IUser? GetByPassword(string email);
		public bool? PasswordCheck(string email, string password);
		public string HashPassword(string password, string email, int id);
	}
}
