using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockManager.Models
{
   public class Produit
    {
        static int count = 0;
        public string Id { get; set; }
        public string Nom { get; set; }
        public string CodeBarre { get; set; } = "";
        public int Stock { get; set; }
        public float Prix { get; set; }
        public string Description { get; set; }

        public Produit()
        {
            Produit.count++;
            Id = Produit.count.ToString();
        }
    }
}
