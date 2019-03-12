using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace InsynsregistretScraper
{
    class Transaction
    {
        public string PubliceringsDatum { get; set; }
        public string Utgivare { get; set; }
        public string PersonILedandeStallning { get; set; }
        public string Befattning { get; set; }
        public string Narstaende { get; set; }
        public string Karaktar { get; set; }
        public string Instrumentnamn { get; set; }
        public string ISIN { get; set; }
        public string TransaktionsDatum { get; set; }
        public string Volym { get; set; }
        public string Volymsenhet { get; set; }
        public string Pris { get; set; }
        public string Valuta { get; set; }
        public string Handelsplats { get; set; }

        public string Description => $"{Befattning} vid namn {PersonILedandeStallning} " +
                                     $"har utfört {this.Karaktar} i {Instrumentnamn} " +
                                     $"för totalt {(decimal.Parse(Volym) * decimal.Parse(Pris)):C} " +
                                     $"{Volymsenhet} {Valuta} från {Handelsplats}";
    }
}

