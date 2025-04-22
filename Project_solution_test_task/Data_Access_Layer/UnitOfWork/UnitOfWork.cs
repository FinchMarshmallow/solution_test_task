using _Main.Core.Interfaces.UnitOfWork;
using Data_Access_Layer.Repositories;
using Main.Core.Interfaces.Repository;
using Main.Data_Access_Layer.Repositories;
using Main.Migrations;

namespace Data_Access_Layer.UnitOfWork
{
	public class UnitOfWork : IUnitOfWork
	{
		private RepositoryComment _comment = new();
		private RepositoryProductCard _productCard = new();
		private RepositoryPurchase _purchase = new();
		private RepositoryUser _user = new();

		public IRepositoryComment Coments => _comment;
		public IRepositoryProductCard ProductCards => _productCard;
		public IRepositoryPurchase Purchases => _purchase;
		public IRepositoryUser Users => _user;

		public int SaveChanges()
		{
			return 0;
		}

		public void Init(string strOptions)
		{
			DatabaseManager.strOptions = strOptions;
		}

		public void Dispose()
		{
			// close data base
		}
	}
}
