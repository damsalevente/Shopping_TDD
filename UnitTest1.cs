using System;
using System.Collections.Generic;
using Xunit;

namespace Shopping_TDD
{
    public class CountDiscount
    {
        public double Ratio { get; set; }
        public CountDiscount(int N, int M)
        {
            Ratio = (double)N / M;
        }
    }

    public class ShoppingCart
    {
        private Dictionary<char, CountDiscount> countdiscounts = new Dictionary<char, CountDiscount>();

        private Dictionary<char, double> products = new Dictionary<char, double>();

        private Dictionary<char, int> shoppinglist = new Dictionary<char, int>();

        public void RegisterProduct(char name, double price)
        {
            products.Add(name, price);
        }

        public double GetPrice(string s)
        {
            double sum = 0;

            foreach (char c in s)
            {
                if (shoppinglist.ContainsKey(c))
                {
                    shoppinglist[c] += 1;
                }
                else
                {
                    shoppinglist[c] = 1;
                }
            }

            /* apply countdiscount */
            foreach (KeyValuePair<char, CountDiscount> am_discount in countdiscounts)
            {
                if (shoppinglist.ContainsKey(am_discount.Key))
                {
                    shoppinglist[am_discount.Key] = (int)Math.Ceiling(shoppinglist[am_discount.Key] * am_discount.Value.Ratio);
                }
            }

            foreach (KeyValuePair<char, int> shopitem in shoppinglist)
            {
                sum += shopitem.Value * products[shopitem.Key];
            }

            return sum;
        }


        
        public void RegisterCountDiscount(char name, int topay, int number)
        {
            countdiscounts[name] = new CountDiscount(topay, number);
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
