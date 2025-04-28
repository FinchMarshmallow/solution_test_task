using Core.Interfaces.Repository;
using Core.Interfaces;
using LayerDataAccess.Data_Access_Layer.Repositories;
using LayerDataAccess.Migrations;
using LayerDataAccess.Repositories;
using Core;

namespace LayerDataAccess.UnitOfWork
{
	public static class UnitOfWork
	{
		private static RepositoryComment _comment = new();
		private static RepositoryProductCard _productCard = new();
		private static RepositoryPurchase _purchase = new();
		private static RepositoryUser _user = new();

		public static IRepositoryComment Coments => (IRepositoryComment)_comment;
		public static IRepositoryProductCard ProductCards => (IRepositoryProductCard)_productCard;
		public static IRepositoryPurchase Purchases => (IRepositoryPurchase)_purchase;
		public static IRepositoryUser Users => (IRepositoryUser)_user;

		static UnitOfWork()
		{
			
		}

		public static int SaveChanges()
		{
			return DatabaseManager.context.SaveChanges();
		}

		public static void Dispose()
		{
			DatabaseManager.context.Dispose();
		}
	}
}
