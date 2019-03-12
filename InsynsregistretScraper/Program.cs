using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using HtmlAgilityPack;

namespace InsynsregistretScraper
{
    class Program
    {
        static void Main(string[] args)
        {
            var Scraper = new MyScraper();

            while (true)
            {
                Scraper.LoadDocument();
                Scraper.CheckForNew();
                Thread.Sleep(1000 * 10);
            }

            //Console.ReadLine();
        }

    }

    public class MyScraper
    {
        public DateTime LastChecked { get; set; }
        private List<Transaction> Transactions { get; set; } = new List<Transaction>();
        private Transaction LastTransaction { get; set; }
        public HtmlWeb HtmlWeb { get; private set; } = new HtmlWeb();
        public HtmlDocument HtmlDocument { get; set; }
        string url = "https://marknadssok.fi.se/publiceringsklient";

        public MyScraper()
        {
            LastChecked = DateTime.Now;
            LoadDocument();
            GetTransactions();
            Console.Beep();
            Console.WriteLine("Go morron, go morron! Senaste transaktionen:");
            LastTransaction = Transactions[0];
            Console.WriteLine(LastTransaction.Description);
            Console.Title = "-";
        }

        public void CheckForNew()
        {

            LastTransaction = Transactions[0];
            GetTransactions();

            for (int i = 0; i < Transactions.Count; i++)
            {
                var t = Transactions[i];
                if (t.ISIN == LastTransaction.ISIN)
                {
                    if (i == 0)
                    {
                        Console.Write(".");
                        return;
                    }

                    ExecuteSignal();
                    Console.WriteLine($"{i} new Transactions!");
                    return;
                }
                Console.WriteLine(DateTime.Now.TimeOfDay);
                Console.WriteLine(t.Description);
            }
            Console.WriteLine("A shit-ton of new transactions");
        }

        private static void ExecuteSignal()
        {
            Console.Beep();
            Thread.Sleep(1000);
            Console.Beep();
            Thread.Sleep(1000);
            Console.Beep();
            Thread.Sleep(1000);
            Console.Beep();
            Thread.Sleep(1000);
            Console.Beep();
            Console.Title = DateTime.Now.Hour + ":" + DateTime.Now.Minute;
        }

        private void GetTransactions()
        {
            Transactions.Clear();

            var rowNodes = HtmlDocument.DocumentNode.SelectNodes("//tbody/tr");

            foreach (var raow in rowNodes)
            {
                var cellNodes = raow.SelectNodes("./td[position()<last()-1]").Select(td => WebUtility.HtmlDecode(td.InnerHtml.ToString())).ToList();
                Transactions.Add(new Transaction
                {
                    Befattning = cellNodes[3],
                    Handelsplats = cellNodes[1],
                    ISIN = cellNodes[7],
                    Instrumentnamn = cellNodes[6],
                    Karaktar = cellNodes[5],
                    Narstaende = cellNodes[4],
                    PersonILedandeStallning = cellNodes[2],
                    Pris = cellNodes[11],
                    PubliceringsDatum = cellNodes[0],
                    TransaktionsDatum = cellNodes[8],
                    Utgivare = cellNodes[1],
                    Valuta = cellNodes[12],
                    Volym = cellNodes[9],
                    Volymsenhet = cellNodes[10]
                });
            }

            foreach (var row in Transactions)
            {
                {
                    //Console.Write(row.ISIN);
                }

            }
        }


        public void LoadDocument()
        {
            HtmlDocument = HtmlWeb.Load(url);
        }
    }
}
