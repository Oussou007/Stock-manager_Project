using StockManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace StockManager
{
    /// <summary>
    /// Logique d'interaction pour ProduitForm.xaml
    /// </summary>
    public partial class ProduitForm : Window
    {
        public ProduitForm(StockViewModel model)
        {
            InitializeComponent();
            DataContext = model;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
