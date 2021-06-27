using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Concrete
{
   public class Basket: IEntity
    {
        public int BasketId { get; set; }

        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int? OrderId { get; set; }

    }
}
