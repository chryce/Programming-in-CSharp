﻿using System;

namespace 外观模式
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Stock1 gu1 = new Stock1();
            Stock2 gu2 = new Stock2();
            Stock3 gu3 = new Stock3();
            NationalDebt1 nd1 = new NationalDebt1();
            Realty1 rt1 = new Realty1();

            gu1.Buy();
            gu2.Buy();
            gu3.Buy();
            nd1.Buy();
            rt1.Buy();

            gu1.Sell();
            gu2.Sell();
            gu3.Sell();
            nd1.Sell();
            rt1.Sell();

            Console.Read();
        }
    }

    //股票1
    internal class Stock1
    {
        //卖股票
        public void Sell()
        {
            Console.WriteLine(" 股票1卖出");
        }

        //买股票
        public void Buy()
        {
            Console.WriteLine(" 股票1买入");
        }
    }

    //股票2
    internal class Stock2
    {
        //卖股票
        public void Sell()
        {
            Console.WriteLine(" 股票2卖出");
        }

        //买股票
        public void Buy()
        {
            Console.WriteLine(" 股票2买入");
        }
    }

    //股票3
    internal class Stock3
    {
        //卖股票
        public void Sell()
        {
            Console.WriteLine(" 股票3卖出");
        }

        //买股票
        public void Buy()
        {
            Console.WriteLine(" 股票3买入");
        }
    }

    //国债1
    internal class NationalDebt1
    {
        //卖国债
        public void Sell()
        {
            Console.WriteLine(" 国债1卖出");
        }

        //买国债
        public void Buy()
        {
            Console.WriteLine(" 国债1买入");
        }
    }

    //房地产1
    internal class Realty1
    {
        //卖房地产
        public void Sell()
        {
            Console.WriteLine(" 房产1卖出");
        }

        //买房地产
        public void Buy()
        {
            Console.WriteLine(" 房产1买入");
        }
    }
}