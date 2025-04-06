using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using Project_solution_test_task.Controllers.Interface;

namespace Project_solution_test_task.Controllers.Implementations
{
	internal class MyService : IMyService
	{
		string IMyService.MyResponce()
		{
			return "MyResponce";
		}
	}
}
