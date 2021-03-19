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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace KonyvtariNyilvantarto
{
    class Konyvek
    {
        public int KonyvID { get; set; }
        public string KonyvSzerzo { get; set; }
        public string KonyvCim { get; set; }
        public string KonyvKiadasEve { get; set; }
        public string KonyvKiado { get; set; }
        public bool KonyvKolcsonozheto { get; set; }
        public Konyvek(string sor)
        {
            string[] resz = sor.Split(';');
            KonyvID = Convert.ToInt32(resz[0]);
            KonyvSzerzo = resz[1];
            KonyvCim = resz[2];
            KonyvKiadasEve = resz[3];
            KonyvKiado = resz[4];
            if (resz[5] == "True")
            {
                KonyvKolcsonozheto = true;
            }
            else
            {
                KonyvKolcsonozheto = false;
            }
        }
        public Konyvek()
        { }
    }
    class Tagok
    {
        public int TagID { get; set; }
        public string TagNev { get; set; }
        public string TagSzuletesDatum { get; set; }
        public string TagIranyitoSzam { get; set; }
        public string TagTelepules { get; set; }
        public string TagUtcaHazszam { get; set; }
        public Tagok(string sor)
        {
            string[] resz = sor.Split(';');
            TagID = Convert.ToInt32(resz[0]);
            TagNev = resz[1];
            TagSzuletesDatum = resz[2];
            TagIranyitoSzam = resz[3];
            TagTelepules = resz[4];
            TagUtcaHazszam = resz[5];
        }
        public Tagok()
        { }
    }
    class Kolcsonzesek
    {
        public int KolcsonzesID { get; set; }
        public int OlvasoID { get; set; }
        public int KonyvID { get; set; }
        public string KolcsonzesDatuma { get; set; }
        public string VisszavetelDatuma { get; set; }
        public Kolcsonzesek(string sor)
        {
            string[] resz = sor.Split(';');
            KolcsonzesID = Convert.ToInt32(resz[0]);
            OlvasoID = Convert.ToInt32(resz[1]);
            KonyvID = Convert.ToInt32(resz[2]);
            KolcsonzesDatuma = resz[3];
            VisszavetelDatuma = resz[4];
        }
        public Kolcsonzesek()
        { }
    }
    public partial class MainWindow : Window
    {
        List<Konyvek> konyvek = new List<Konyvek>();
        List<Tagok> kolcsonzok = new List<Tagok>();
        List<Kolcsonzesek> kolcsonzesek = new List<Kolcsonzesek>();
        List<int> konyvIDk = new List<int>();
        List<int> kolcsonzoIDk = new List<int>();
        bool kolcsonzoIrSzamOK = false;
        bool kolcsonzesKolcsIDOK = false;
        bool kolcsonzesKonyvIDOK = false;
        bool kezdetDatumOK = false;
        bool vegDatumOK = true;
        public MainWindow()
        {
            InitializeComponent();
            foreach (var item in File.ReadAllLines("konyvek.txt"))
            {
                konyvek.Add(new Konyvek(item));
            }
            foreach (var item in File.ReadAllLines("tagok.txt"))
            {
                kolcsonzok.Add(new Tagok(item));
            }
            foreach (var item in File.ReadAllLines("kolcsonzesek.txt"))
            {
                kolcsonzesek.Add(new Kolcsonzesek(item));
            }
            Konyvek.DataContext = konyvek;
            Kolcsonzok.DataContext = kolcsonzok;
            Kolcsonzesek.DataContext = kolcsonzesek;
            foreach (var item in konyvek)
            {
                konyvIDk.Add(item.KonyvID);
            }
            foreach (var item in kolcsonzok)
            {
                kolcsonzoIDk.Add(item.TagID);
            }
        }
        private void Konyvek_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            konyvek[Konyvek.SelectedIndex] = (Konyvek)Konyvek.SelectedItem;
            KonyvekMenteseButton.IsEnabled = true;
            konyvIDk.Clear();
            foreach (var item in konyvek)
            {
                konyvIDk.Add(item.KonyvID);
            }
        }
        private void Konyvek_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Konyvek.SelectedItems.Count != 0)
            {
                KonyvTorleseGomb.IsEnabled = true;
            }
            else
            {
                KonyvTorleseGomb.IsEnabled = false;
            }
        }
        private void KonyvSzerzoBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            KonyvHozzaadasaEngedely();
        }
        private void KonyvCimBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            KonyvHozzaadasaEngedely();
        }
        private void KonyvKiadasEveBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            KonyvHozzaadasaEngedely();
        }
        private void KonyvKiadoBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            KonyvHozzaadasaEngedely();
        }
        public void KonyvHozzaadasaEngedely()
        {
            if (!(KonyvSzerzoBox.Text == "" && KonyvCimBox.Text == "" && KonyvKiadasEveBox.Text == "" && KonyvKiadoBox.Text == ""))
            {
                KonyvHozzaadasaButton.IsEnabled = true;
                KonyvHozzaadasaMegseGomb.IsEnabled = true;
            }
            else
            {
                KonyvHozzaadasaButton.IsEnabled = false;
                KonyvHozzaadasaMegseGomb.IsEnabled = false;
            }
        }
        private void KonyvHozzaadasaButton_Click(object sender, RoutedEventArgs e)
        {
            Konyvek.DataContext = null;
            konyvek.Add(new Konyvek() { KonyvID = konyvek[konyvek.Count - 1].KonyvID + 1, KonyvSzerzo = KonyvSzerzoBox.Text, KonyvCim = KonyvCimBox.Text, KonyvKiadasEve = KonyvKiadasEveBox.Text, KonyvKiado = KonyvKiadoBox.Text, KonyvKolcsonozheto = (bool)KonyvKolcsonozhetoCheck.IsChecked });
            Konyvek.DataContext = konyvek;
            KonyvSzerzoBox.Text = "";
            KonyvCimBox.Text = "";
            KonyvKiadasEveBox.Text = "";
            KonyvKiadoBox.Text = "";
            KonyvKolcsonozhetoCheck.IsChecked = true;
            KonyvHozzaadasaButton.IsEnabled = false;
            KonyvHozzaadasaMegseGomb.IsEnabled = false;
            KonyvekMenteseButton.IsEnabled = true;
            konyvIDk.Clear();
            foreach (var item in konyvek)
            {
                konyvIDk.Add(item.KonyvID);
            }
        }
        private void KonyvHozzaadasaMegseGomb_Click(object sender, RoutedEventArgs e)
        {
            KonyvSzerzoBox.Text = "";
            KonyvCimBox.Text = "";
            KonyvKiadasEveBox.Text = "";
            KonyvKiadoBox.Text = "";
            KonyvKolcsonozhetoCheck.IsChecked = true;
            KonyvHozzaadasaButton.IsEnabled = false;
            KonyvHozzaadasaMegseGomb.IsEnabled = false;
        }
        private void KonyvTorleseGomb_Click(object sender, RoutedEventArgs e)
        {
                foreach (var item in Konyvek.SelectedItems)
                {
                    konyvek.Remove((Konyvek)item);
                }
                Konyvek.DataContext = null;
                Konyvek.DataContext = konyvek;
                KonyvekMenteseButton.IsEnabled = true;
                konyvIDk.Clear();
                foreach (var item in konyvek)
                {
                    konyvIDk.Add(item.KonyvID);
                }
        }
        private void KonyvekMenteseButton_Click(object sender, RoutedEventArgs e)
        {
            KonyvekMentese();
        }
        public void KonyvekMentese()
        {
            StreamWriter sw = new StreamWriter("./konyvek.txt");
            foreach (var item in konyvek)
            {
                sw.WriteLine(item.KonyvID + ";" + item.KonyvSzerzo + ";" + item.KonyvCim + ";" + item.KonyvKiadasEve + ";" + item.KonyvKiado + ";" + item.KonyvKolcsonozheto);
            }
            sw.Close();
            sw.Dispose();
            KonyvekMenteseButton.IsEnabled = false;
        }
        private void Kolcsonzok_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            kolcsonzok[Kolcsonzok.SelectedIndex] = (Tagok)Kolcsonzok.SelectedItem;
            KolcsonzokMenteseButton.IsEnabled = true;
            kolcsonzoIDk.Clear();
            foreach (var item in kolcsonzok)
            {
                kolcsonzoIDk.Add(item.TagID);
            }
        }
        private void Kolcsonzok_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Kolcsonzok.SelectedItems.Count != 0)
            {
                KolcsonzoTorleseButton.IsEnabled = true;
            }
            else
            {
                KolcsonzoTorleseButton.IsEnabled = false;
            }
        }
        private void KolcsonzoNevBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            KolcsonzoHozzaadasaEngedely();
        }
        private void KolcsonzoSzuletesBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            KolcsonzoHozzaadasaEngedely();
        }
        private void KolcsonzoIranyitoszamBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (KolcsonzoIranyitoszamBox.Text != "")
            {
                if (int.TryParse(KolcsonzoIranyitoszamBox.Text, out _))
                {
                    kolcsonzoIrSzamOK = true;
                }
                else
                {
                    kolcsonzoIrSzamOK = false;
                }
            }
            KolcsonzoHozzaadasaEngedely();
        }
        private void KolcsonzoTelepulesBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            KolcsonzoHozzaadasaEngedely();
        }
        private void KolcsonzoUtcaBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            KolcsonzoHozzaadasaEngedely();
        }
        public void KolcsonzoHozzaadasaEngedely()
        {
            if (KolcsonzoNevBox.Text != "" && KolcsonzoSzuletesBox.Text != "" && KolcsonzoIranyitoszamBox.Text != "" && kolcsonzoIrSzamOK && KolcsonzoTelepulesBox.Text != "" && KolcsonzoUtcaBox.Text != "")
            {
                KolcsonzoFelvetelButton.IsEnabled = true;
            }
            else
            {
                KolcsonzoFelvetelButton.IsEnabled = false;
            }
            if (!(KolcsonzoNevBox.Text == "" && KolcsonzoSzuletesBox.Text == "" && KolcsonzoIranyitoszamBox.Text == "" && KolcsonzoTelepulesBox.Text == "" && KolcsonzoUtcaBox.Text == ""))
            {
                KolcsonzoFelvetelMegseButton.IsEnabled = true;
            }
            else
            {
                KolcsonzoFelvetelMegseButton.IsEnabled = false;
            }
        }
        private void KolcsonzoFelvetelButton_Click(object sender, RoutedEventArgs e)
        {
            Kolcsonzok.DataContext = null;
            kolcsonzok.Add(new Tagok() { TagID = kolcsonzok[kolcsonzok.Count - 1].TagID + 1, TagNev = KolcsonzoNevBox.Text, TagSzuletesDatum = KolcsonzoSzuletesBox.Text, TagIranyitoSzam = KolcsonzoIranyitoszamBox.Text, TagTelepules = KolcsonzoTelepulesBox.Text, TagUtcaHazszam = KolcsonzoUtcaBox.Text });
            Kolcsonzok.DataContext = kolcsonzok;
            KolcsonzoNevBox.Text = "";
            KolcsonzoSzuletesBox.Text = "";
            KolcsonzoIranyitoszamBox.Text = "";
            KolcsonzoTelepulesBox.Text = "";
            KolcsonzoUtcaBox.Text = "";
            KolcsonzoFelvetelButton.IsEnabled = false;
            KolcsonzoFelvetelMegseButton.IsEnabled = false;
            KolcsonzokMenteseButton.IsEnabled = true;
            kolcsonzoIDk.Clear();
            foreach (var item in kolcsonzok)
            {
                kolcsonzoIDk.Add(item.TagID);
            }
        }
        private void KolcsonzoFelvetelMegseButton_Click(object sender, RoutedEventArgs e)
        {
            KolcsonzoNevBox.Text = "";
            KolcsonzoSzuletesBox.Text = "";
            KolcsonzoIranyitoszamBox.Text = "";
            KolcsonzoTelepulesBox.Text = "";
            KolcsonzoUtcaBox.Text = ""; 
            KolcsonzoFelvetelButton.IsEnabled = false;
            KolcsonzoFelvetelMegseButton.IsEnabled = false;
        }
        private void KolcsonzoTorleseButton_Click(object sender, RoutedEventArgs e)
        {
                foreach (var item in Kolcsonzok.SelectedItems)
                {
                    kolcsonzok.Remove((Tagok)item);
                }
                Kolcsonzok.DataContext = null;
                Kolcsonzok.DataContext = kolcsonzok;
                KolcsonzokMenteseButton.IsEnabled = true;
                kolcsonzoIDk.Clear();
                foreach (var item in kolcsonzok)
                {
                    kolcsonzoIDk.Add(item.TagID);
                }
        }
        private void KolcsonzokMenteseButton_Click(object sender, RoutedEventArgs e)
        {
            KolcsonzokMentese();
        }
        public void KolcsonzokMentese()
        {
            StreamWriter sw = new StreamWriter("./tagok.txt");
            foreach (var item in kolcsonzok)
            {
                sw.WriteLine(item.TagID + ";" + item.TagNev + ";" + item.TagSzuletesDatum + ";" + item.TagIranyitoSzam + ";" + item.TagTelepules + ";" + item.TagUtcaHazszam);
            }
            sw.Close();
            sw.Dispose();
            KolcsonzokMenteseButton.IsEnabled = false;
        }
        private void Kolcsonzesek_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            kolcsonzesek[Kolcsonzesek.SelectedIndex] = (Kolcsonzesek)Kolcsonzesek.SelectedItem;
            KolcsonzesekMenteseButton.IsEnabled = true;
        }
        private void Kolcsonzesek_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Kolcsonzesek.SelectedItems.Count != 0)
            {
                KolcsonzesTorleseButton.IsEnabled = true;
            }
            else
            {
                KolcsonzesTorleseButton.IsEnabled = false;
            }
        }
        private void KolcsonzesKonyvIDBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (konyvIDk.Contains(Convert.ToInt32(KolcsonzesKonyvIDBox.Text)) && KolcsonzesKonyvIDBox.Text != "")
                {
                    kolcsonzesKonyvIDOK = true;
                }
                else
                {
                    kolcsonzesKonyvIDOK = false;
                }
            }
            catch
            {
                kolcsonzesKonyvIDOK = false;
            }
            KolcsonzesHozzaadasaEngedely();
        }
        private void KolcsonzesKolcsonzoIDBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (kolcsonzoIDk.Contains(Convert.ToInt32(KolcsonzesKolcsonzoIDBox.Text)) && KolcsonzesKolcsonzoIDBox.Text != "")
                {
                    kolcsonzesKolcsIDOK = true;
                }
                else
                {
                    kolcsonzesKolcsIDOK = false;
                }
            }
            catch
            {
                kolcsonzesKolcsIDOK = false;
            }
            KolcsonzesHozzaadasaEngedely();
        }
        private void KolcsonzesKezdeteBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (DateTime.TryParse(KolcsonzesKezdeteBox.Text, out _))
            {
                kezdetDatumOK = true;
                KolcsonzesVegeBox.IsEnabled = true;
            }
            else
            {
                kezdetDatumOK = false;
                KolcsonzesVegeBox.IsEnabled = false;
            }
            KolcsonzesHozzaadasaEngedely();
        }
        private void KolcsonzesVegeBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (DateTime.TryParse(KolcsonzesVegeBox.Text, out _) || KolcsonzesVegeBox.Text == "")
            {
                vegDatumOK = true;
            }
            else
            {
                vegDatumOK = false;
            }
            KolcsonzesHozzaadasaEngedely();
        }
        public void KolcsonzesHozzaadasaEngedely()
        {
            if (kolcsonzesKolcsIDOK && kolcsonzesKonyvIDOK && kezdetDatumOK && vegDatumOK)
            {
                KolcsonzesFelveteleButton.IsEnabled = true;
            }
            else
            {
                KolcsonzesFelveteleButton.IsEnabled = false;
            }
            if (KolcsonzesKolcsonzoIDBox.Text != "" || KolcsonzesKonyvIDBox.Text != "" || KolcsonzesVegeBox.Text != "" || KolcsonzesKezdeteBox.Text != "")
            {
                KolcsonzesFelveteleMegseButton.IsEnabled = true;
            }
            else
            {
                KolcsonzesFelveteleMegseButton.IsEnabled = false;
            }
        }
        private void KolcsonzesFelveteleButton_Click(object sender, RoutedEventArgs e)
        {
            Kolcsonzesek.DataContext = null;
            if (KolcsonzesVegeBox.Text == "")
            {
                kolcsonzesek.Add(new Kolcsonzesek() { KolcsonzesID = kolcsonzesek[kolcsonzesek.Count - 1].KolcsonzesID + 1, OlvasoID = Convert.ToInt32(KolcsonzesKolcsonzoIDBox.Text), KonyvID = Convert.ToInt32(KolcsonzesKonyvIDBox.Text), KolcsonzesDatuma = KolcsonzesKezdeteBox.Text, VisszavetelDatuma = "" });
            }
            else
            {
                kolcsonzesek.Add(new Kolcsonzesek() { KolcsonzesID = kolcsonzesek[kolcsonzesek.Count - 1].KolcsonzesID + 1, OlvasoID = Convert.ToInt32(KolcsonzesKolcsonzoIDBox.Text), KonyvID = Convert.ToInt32(KolcsonzesKonyvIDBox.Text), KolcsonzesDatuma = KolcsonzesKezdeteBox.Text, VisszavetelDatuma = KolcsonzesVegeBox.Text });
            }
            Kolcsonzesek.DataContext = kolcsonzesek;
            KolcsonzesKonyvIDBox.Text = "";
            KolcsonzesKolcsonzoIDBox.Text = "";
            KolcsonzesVegeBox.Text = "";
            KolcsonzesKezdeteBox.Text = "";
            KolcsonzesFelveteleButton.IsEnabled = false;
            KolcsonzesFelveteleMegseButton.IsEnabled = false;
            KolcsonzesekMenteseButton.IsEnabled = true;
        }
        private void KolcsonzesFelveteleMegseButton_Click(object sender, RoutedEventArgs e)
        {
            KolcsonzesKonyvIDBox.Text = "";
            KolcsonzesKolcsonzoIDBox.Text = "";
            KolcsonzesVegeBox.Text = "";
            KolcsonzesKezdeteBox.Text = "";
            KolcsonzesFelveteleButton.IsEnabled = false;
            KolcsonzesFelveteleMegseButton.IsEnabled = false;
        }
        private void KolcsonzesTorleseButton_Click(object sender, RoutedEventArgs e)
        {
                foreach (var item in Kolcsonzesek.SelectedItems)
                {
                    kolcsonzesek.Remove((Kolcsonzesek)item);
                }
                Kolcsonzesek.DataContext = null;
                Kolcsonzesek.DataContext = kolcsonzesek;
                KolcsonzesekMenteseButton.IsEnabled = true;
        }
        private void KolcsonzesekMenteseButton_Click(object sender, RoutedEventArgs e)
        {
            KolcsonzesekMentese();
        }
        public void KolcsonzesekMentese()
        {
            StreamWriter sw = new StreamWriter("./kolcsonzesek.txt");
            foreach (var item in kolcsonzesek)
            {
                sw.WriteLine(item.KolcsonzesID + ";" + item.OlvasoID + ";" + item.KonyvID + ";" + item.KolcsonzesDatuma + ";" + item.VisszavetelDatuma);
            }
            sw.Close();
            sw.Dispose();
            KolcsonzesekMenteseButton.IsEnabled = false;
        }
    }
}