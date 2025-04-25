using Core.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
	public interface IUnitOfWork : IDisposable
	{
		public IRepositoryComment Coments { get; }
		public IRepositoryProductCard ProductCards { get; }
		public IRepositoryPurchase Purchases { get; }
		public IRepositoryUser Users { get; }

		public int SaveChanges();

		public void Init(string strOptions);
	}
}
