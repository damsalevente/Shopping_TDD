using System;
using System.Collections.Generic;
using Xunit;

namespace Shopping_TDD
{
    public class ShoppingCart
    {
     
        private Dictionary<char, double> products = new Dictionary<char, double>();
    
        public void RegisterProduct(char name, double price)
        {
            products.Add(name, price);
        }
        public double GetPrice(string s)
        {
            double sum = 0;
            foreach(char  c in s)
            {
                if(products.ContainsKey(c))
                {
                    sum += products[c];
                }
            }
            return sum;
        }

    }
    public class UnitTest1
    {
        [Fact]
        public void Call_RegisterProduct()
        {
            /* create shoppingcart */
            ShoppingCart Shop = new ShoppingCart();
            Shop.RegisterProduct('A', 10);
            Shop.RegisterProduct('B', 20);
            Shop.RegisterProduct('C', 30);
            var price = Shop.GetPrice("AC");
            Assert.Equal(40, price);
        }
        [Fact]
        public void Set_CountDiscount()
        {
            /* create shoppingcart */
            ShoppingCart Shop = new ShoppingCart();
            Shop.RegisterProduct('A', 10);
            Shop.RegisterProduct('B', 20);
            Shop.RegisterProduct('C', 30);
            Shop.RegisterCountDiscount('A', 3, 4);
            var price = Shop.GetPrice("AAAAAAAC");
            Assert.Equal(90, price);
        }
    }
}
