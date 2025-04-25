using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
	public interface IServer
	{
		public bool StartServer(string url, string filePath, string passwordJWT);
	}
}
