using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Concrete
{
   public class CartItem
    {
        public int id { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
