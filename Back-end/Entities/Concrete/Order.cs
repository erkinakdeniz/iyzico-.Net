using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Concrete
{
  public  class Order:IEntity
    {
        public int OrderId { get; set; }
        public int OrderNumber { get; set; }
        public int Status { get; set; }
        public string Massage { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public string ShipCity { get; set; } // burası adresıd olarak değişicek

        public ICollection<Basket> Baskets { get; set; }
    }
}
