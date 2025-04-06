using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Project_solution_test_task.Controllers.Interface;

namespace Project_solution_test_task.Controllers.Implementations
{
	internal class MyService1 : IMyService1
	{
		string IMyService1.MyResponce1()
		{
			return "MyResponce 1";
		}
	}
}
