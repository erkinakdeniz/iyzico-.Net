using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Concrete
{
   public class Iyzico
    {
        public string CardHolderName { get; set; }
        public string CardNumber { get; set; }
        public string ExpireMonth { get; set; }
        public string ExpireYear { get; set; }
        public string Cvc { get; set; }
        public int RegisterCard { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string IdentityNumber { get; set; } //angular da tc kimlik no alanı olacak
        public string RegistrationAddress { get; set; } //tek bi adrese gidecek
        public string City { get; set; }
        public string Country { get; set; }
        public string ContactName { get; set; } //faturayı alacak kişi
        public string Price { get; set; }
        public List<CartItem> cartItems { get; set; }
    }
}
