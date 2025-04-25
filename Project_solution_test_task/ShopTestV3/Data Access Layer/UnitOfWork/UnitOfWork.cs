using Core.Interfaces.Repository;
using Core.Interfaces;
using LayerDataAccess.Data_Access_Layer.Repositories;
using LayerDataAccess.Migrations;
using LayerDataAccess.Repositories;

namespace LayerDataAccess.UnitOfWork
{
	public class UnitOfWork : IUnitOfWork
	{
		private RepositoryComment _comment = new();
		private RepositoryProductCard _productCard = new();
		private RepositoryPurchase _purchase = new();
		private RepositoryUser _user = new();

		public IRepositoryComment Coments => (IRepositoryComment)_comment;
		public IRepositoryProductCard ProductCards => (IRepositoryProductCard)_productCard;
		public IRepositoryPurchase Purchases => (IRepositoryPurchase)_purchase;
		public IRepositoryUser Users => (IRepositoryUser)_user;

		public int SaveChanges()
		{
			return DatabaseManager.Сontext.SaveChanges();
		}

		public void Init(string strOptions)
		{
			DatabaseManager.strOptions = strOptions;
		}

		public void Dispose()
		{
			DatabaseManager.Сontext.Dispose();
		}
	}
}
