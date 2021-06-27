
using System;
using System.Linq;
using Core.DataAccess.EntityFramework;
using Entities.Concrete;
using DataAccess.Concrete.EntityFramework.Contexts;
using DataAccess.Abstract;
namespace DataAccess.Concrete.EntityFramework
{
    public class ProductRepository : EfEntityRepositoryBase<Product, ProjectDbContext>, IProductRepository
    {
        public ProductRepository(ProjectDbContext context) : base(context)
        {
        }
    }
}
