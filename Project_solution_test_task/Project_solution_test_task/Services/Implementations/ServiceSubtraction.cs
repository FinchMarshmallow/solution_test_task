﻿using Project_solution_test_task.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_solution_test_task.Services.Implementations
{
	internal class ServiceSubtraction : IServiceSubtraction
	{
		string IServiceSubtraction.Operating(double a, double b)
		{
			return (a - b).ToString();
		}
	}
}
