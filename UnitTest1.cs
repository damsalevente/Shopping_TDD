using System;
using System.Collections.Generic;
using Xunit;

namespace Shopping_TDD
{

    public class AmountDiscount
    {
        public double Ratio { get; set; }
        public double Limit { get; set; }

        public AmountDiscount(int N, double M)
        {
            Limit = N;
            Ratio = M;
        }
    }

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
        private Dictionary<char, AmountDiscount> amountdiscounts = new Dictionary<char, AmountDiscount>();
        internal void RegisterAmountDiscount(char v1, int v2, double v3)
        {
            amountdiscounts.Add(v1, new AmountDiscount(v2, v3));
        }
        public void RegisterProduct(char name, double price)
        {
            products.Add(name, price);
        }
        public void RegisterCountDiscount(char name, int topay, int number)
        {
            countdiscounts[name] = new CountDiscount(topay, number);
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
                if (amountdiscounts.ContainsKey(shopitem.Key))
                {
                    if (shopitem.Value >= amountdiscounts[shopitem.Key].Limit)
                    {
                        sum += shopitem.Value * amountdiscounts[shopitem.Key].Ratio * products[shopitem.Key];
                    }
                }
                else
                {
                    sum += shopitem.Value * products[shopitem.Key];
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
        [Fact]
        public void Set_Amount_Discount()
        {
            /* create shoppingcart */
            ShoppingCart Shop = new ShoppingCart();
            Shop.RegisterProduct('A', 10);
            Shop.RegisterProduct('B', 20);
            Shop.RegisterProduct('C', 30);
            Shop.RegisterAmountDiscount('A', 5, 0.9);
            var price = Shop.GetPrice("AAAABCA"); // 5 * 0.9 + 20 + 30 
            Assert.Equal(95, price);
        }
        [Fact]
        public void Set_Combo_Discount()
        {
            /* this unit test has to be reworked,
             * since the club_membership will give him 10% discount */

            /* create shoppingcart */
            ShoppingCart Shop = new ShoppingCart();
            Shop.RegisterProduct('A', 10);
            Shop.RegisterProduct('B', 20);
            Shop.RegisterProduct('C', 30);
            Shop.RegisterComboDiscount("ABC", 30);
            var price = Shop.GetPrice("ABCAAAA"); // (30 + 40) * .90
            Assert.Equal(63, price);
        }
    }
}
