using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using WpfAppNepesseg.Model;

namespace WpfAppNepesseg
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Telepules> telepulesek = new List<Telepules>();

        public MainWindow()
        {
            InitializeComponent();

            //Adatok beolvasása fájlból
            StreamReader sr = new StreamReader("Datas\\kozerdeku_lakossag_2022.csv");
            sr.ReadLine();
            while (!sr.EndOfStream)
            {
                //3;32434;BAC;Kiskunhalas;város;13 134;14 213
                string[] tagok = sr.ReadLine().Split(';');
                Telepules ujTelepules = new Telepules(tagok[2],
                                                      tagok[3],
                                                      tagok[4],
                                                      int.Parse(tagok[5].Replace(" ", "")),
                                                      int.Parse(tagok[6].Replace(" ", ""))
                                                      );
                telepulesek.Add(ujTelepules);
            }
            sr.Close();


            //Az adatokból kiszedjük a lehetséges megyék kódjait
            
            Dictionary<string, int> megyek = new Dictionary<string, int>();
            foreach (var item in telepulesek)
            {
                if (!megyek.ContainsKey(item.Megye))
                {
                    megyek.Add(item.Megye, 1);
                }
            }

            foreach (var item in megyek.Keys)
            {
                cbMegyek.Items.Add(item);
            }
            
            //LINQ segítségével egyszerűbb lenne!
            //cbMegyek.ItemsSource =telepulesek.Select(obj => obj.Megye).Distinct().ToList();

            dgTelepulesek.ItemsSource = telepulesek;
            labSorokSzama.Content = telepulesek.Count;
        }

        //Helyette már ComboBox kezeli ezt az esetet!
        private void btnSzures_Click(object sender, RoutedEventArgs e)
        {
            //Szűrés LINQ segítségével
            //List<Telepules> szurtLista = telepulesek.Where(t => t.Megye == txtMegye.Text).ToList();

            //Szűrés hagyományo módon
            /*
            List<Telepules> szurtLista = new List<Telepules>();
            foreach (var item in telepulesek)
            {
                if (item.Megye == txtMegye.Text)
                {
                    szurtLista.Add(item);
                }
            }
            dgTelepulesek.ItemsSource = szurtLista;
            */
        }

        private void cbMegyek_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            string valasztottMegye = cbMegyek.SelectedItem.ToString();
            List<Telepules> szurtLista = telepulesek.Where(t => t.Megye == valasztottMegye).ToList();
            dgTelepulesek.ItemsSource = szurtLista;
            labSorokSzama.Content = szurtLista.Count;
        }
    }
}
