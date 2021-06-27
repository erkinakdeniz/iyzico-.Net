
using System;
using System.Linq;
using Core.DataAccess.EntityFramework;
using Entities.Concrete;
using DataAccess.Concrete.EntityFramework.Contexts;
using DataAccess.Abstract;
namespace DataAccess.Concrete.EntityFramework
{
    public class BasketRepository : EfEntityRepositoryBase<Basket, ProjectDbContext>, IBasketRepository
    {
        public BasketRepository(ProjectDbContext context) : base(context)
        {
        }
    }
}
