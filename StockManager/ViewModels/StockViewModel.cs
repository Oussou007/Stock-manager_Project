using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using StockManager.Models;
namespace StockManager.ViewModels
{
   public class StockViewModel : INotifyPropertyChanged
    {
        #region bindings de la page principale
        
        private Produit currentProduit;
        public Produit CurrentProduit { 
            get { 
                return currentProduit; 
            } 
            set {
                currentProduit = value;
                OnPropertyChanged("CurrentProduit");
               
                
            } 
        }

        private List<Produit> listProduit = new List<Produit>();
        private List<Produit> produits= new List<Produit>();
        public List<Produit> Produits
        {
            get
            {
                return produits;
            }
            set
            {
                produits = value;
                if (searchText.Length == 0)
                {
                    listProduit = produits;
                }
                OnPropertyChanged("Produits");
            }
        }

        private string searchText="";
        public string SearchText
        {
            get
            {
                return searchText;
            }
            set
            {
                searchText = value;
                Rechercher();
                OnPropertyChanged("SearchText");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
        
        //Constructeur du view model
        public StockViewModel()
        {
            Produit p = new Produit();
            p.Nom = "Produit 1";
            p.Stock = 8;
            p.Prix = 30;
            p.CodeBarre = "456fd654s6";
            p.Description = "Le premier produit";
            produits.Add(p);

            p = new Produit();
            p.Nom = "Produit 2";
            p.Stock = 132;
            p.Prix = 3;
            p.CodeBarre = "457856fd654s6";
            p.Description = "Le second produit";
            produits.Add(p);
            Produits = produits;
            if (produits.Count > 0)
            {
               CurrentProduit = Produits.First();
            }
        }


        public bool canExecuteCommandOnProduit(Object param)
        {
            return param != null;
        }

        #region Boutton plus
        //commande du bouton +
        private RelayCommand<Produit> commandPlusUnStock;
        public ICommand CommandPlusUn
        {
            get
            {
                if (commandPlusUnStock == null)
                {
                    commandPlusUnStock = new RelayCommand<Produit>(param => ActionCommandPlusUnStock(param), param => canExecuteCommandOnProduit(param));
                }
                return commandPlusUnStock;
            }
        }
        public void ActionCommandPlusUnStock(Produit param)
        {

            param.Stock++;
            OnPropertyChanged("CurrentProduit");
        }
        #endregion
        #region Boutton moins
        //Commande du bouton -
        private RelayCommand<Produit> commandMoinsUnStock;
        public ICommand CommandMoinsUn
        {
            get
            {
                if (commandMoinsUnStock == null)
                {
                    commandMoinsUnStock = new RelayCommand<Produit>(param => ActionCommandMoinsUnStock(param), param => canExecuteCommandOnProduit(param));
                }
                return commandMoinsUnStock;
            }
        }  
        public void ActionCommandMoinsUnStock(Produit param)
        {
            param.Stock--;
            OnPropertyChanged("CurrentProduit");
        }
        #endregion
        #region supprimmer produit
        //Commande de suppression d'un produit
        private RelayCommand<Produit> commandSupprimerProduit;
        public ICommand CommandSupprimerProduit
        {
            get
            {
                if (commandSupprimerProduit == null)
                {
                    commandSupprimerProduit = new RelayCommand<Produit>(param => ActionSupprimerProduit(param), param => canExecuteCommandOnProduit(param));
                }
                return commandSupprimerProduit;
            }
        }

        public void ActionSupprimerProduit(Produit param)
        {
            Produits.Remove(param);
            List<Produit> lp = new List<Produit>();
            lp.AddRange(produits);           
            Produits = lp;
            if (searchText.Length > 0) listProduit.Remove(param);
            OnPropertyChanged("Produits");
            CurrentProduit = null;
        }
        #endregion
        private enum Action { Modifier, Ajouter };
        private Action action;
        private Produit nouveauProduit;
        private ProduitForm pf;
        #region Modifier un produit
        //Commande de Modifier d'un produit 
        private RelayCommand<Produit> commandModifierProduit;
        public ICommand CommandModifierProduit
        {
            get
            {
                if (commandModifierProduit == null)
                {
                    commandModifierProduit = new RelayCommand<Produit>(param => ActionModifierProduit(param), param => canExecuteCommandOnProduit(param));
                }
                return commandModifierProduit;
            }
        }
        
        public Produit NouveauProduit
        {
            get
            {
                return nouveauProduit;
            }
            set
            {
                nouveauProduit = value;
                OnPropertyChanged("NouveauProduit");
            }
        }
        public void ActionModifierProduit(Produit param)
        {
            action = Action.Modifier;
             pf = new ProduitForm(this);
            NouveauProduit = param;
            pf.Show();            
        }
        #endregion

        #region Ajouter un produit
        //Commande d'ajout d'un produit
        private RelayCommand<Produit> commandAjouterProduit;
        public ICommand CommandAjouterProduit
        {
            get
            {
                if (commandAjouterProduit == null)
                {
                    commandAjouterProduit = new RelayCommand<Produit>(param => ActionAjouterProduit(param), param => { return true; });
                }
                return commandAjouterProduit;
            }
        }

        public void ActionAjouterProduit(Produit param)
        {
            action = Action.Ajouter;
            pf = new ProduitForm(this);
            NouveauProduit = new Produit();
            pf.Show();
            pf.Closing += Closing;
        }
        private bool saved = false;
        public void Closing(object sender, CancelEventArgs e)
        {
            
        }

        #endregion
        #region Enregistrer un nouveau produit
        //Commande enregistrer un produit
        private RelayCommand<Produit> commandEnregistrerProduit;
        public ICommand CommandEnregistrerProduit
        {
            get
            {
                if (commandEnregistrerProduit == null)
                {
                    commandEnregistrerProduit = new RelayCommand<Produit>(param => ActionEnregistrerProduit(param), param => { return NouveauProduit!=null; });
                }
                return commandEnregistrerProduit;
            }
        }

        public void ActionEnregistrerProduit(Produit param)
        {
            if(action==Action.Ajouter)saved = true;
            if (saved)
            {
                string codeBarrePattern = "[0-9]{13}";
                Regex matcher = new Regex(codeBarrePattern);
                if (matcher.IsMatch(NouveauProduit.CodeBarre)&&NouveauProduit.CodeBarre.Length!=0)
                {
                    Produits.Add(NouveauProduit);
                    List<Produit> lp = new List<Produit>();
                    lp.AddRange(produits);
                    Produits = lp;
                    if (searchText.Length > 0) listProduit.Add(NouveauProduit);
                    OnPropertyChanged("Produits");
                    NouveauProduit = null;
                    saved = false;
                    pf.Close();
                    return;
                }
                else
                {
                    MessageBox.Show("Veillez renseignez un code barre correct.\nContenant 13 chiffres.", "Erreur");
                    return;
                }
            }
            
        }
        #endregion

        #region Rechercher un nouveau produit
        //Commande enregistrer un produit
        private RelayCommand<Produit> commandRechercherProduit;
        public ICommand CommandRechercherProduit
        {
            get
            {
                if (commandRechercherProduit == null)
                {
                    commandRechercherProduit = new RelayCommand<Produit>(param => ActionRechercherProduit(param), param => { return (SearchText != null)&&(SearchText.Length>0); });
                }
                return commandRechercherProduit;
            }
        }

        public void ActionRechercherProduit(Produit param)
        {
            Rechercher();
        }

        public void Rechercher()
        {
            if (SearchText.Length > 0)
            {
                List<Produit> results = new List<Produit>();
                foreach (Produit p in listProduit)
                {
                    if (p.CodeBarre.Contains(SearchText) || p.Id.Contains(SearchText))
                    {
                        results.Add(p);
                    }
                }
                Produits = results;
            }
            else
            {
                Produits = listProduit;
            }
        }
        #endregion
    }


}

