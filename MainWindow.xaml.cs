using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BevolkerungGUI
{
    
public partial class MainWindow : Window
    {
        List<Allampolgar> allampolgar = new List<Allampolgar>();
        Binding mybinding = new Binding();
        public MainWindow()
        {
            InitializeComponent();

            using (StreamReader be = new StreamReader(path: @"../../../src/bevölkerung.txt", encoding: UTF8Encoding.UTF8))
            {
                _ = be.ReadLine();
                while (!be.EndOfStream)
                {
                    allampolgar.Add(new Allampolgar(be.ReadLine()));
                }
            }

            for (int i = 1; i <= 40; i++)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = i;
                Feladatok.Items.Add(item);
            }

            Feladatok.SelectionChanged += Feladatok_SelectionChanged;

           DataContext = allampolgar;
        }

        private void Feladatok_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MegoldasMondatos.Content = string.Empty;
            MegoldasLista.Items.Clear();
            MegoldasTeljes.ItemsSource = null;

            ComboBox comboBox = (ComboBox)sender;
            ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;
            string selectedValue = selectedItem.Content.ToString();

            string methodName = $"Feladat{selectedValue}";
            var method = typeof(MainWindow).GetMethod(methodName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            method?.Invoke(this, null);
        }
        #region Feladatok
        private void Feladat1()
        {
            int maxNettoJovedelem = allampolgar.Max(polgar => polgar.NettoJovedelem);
            MegoldasMondatos.Content = $"A legmagasabb nettó éves jövedelem: {maxNettoJovedelem} €";
        }


        private void Feladat2()
        {
            double atlagJovedelem = allampolgar.Average(polgar => polgar.NettoJovedelem);
            MegoldasMondatos.Content = $"Az állampolgárok átlagos nettó éves jövedelme: {atlagJovedelem:0.00} €";
        }

        private void Feladat3()
        {
            var groupedByTartomany = allampolgar.GroupBy(polgar => polgar.Tartomany);

            StringBuilder result = new StringBuilder();
            foreach (var group in groupedByTartomany)
            {
                string tartomany = group.Key;
                int count = group.Count();
                result.AppendLine($"Tartomány: {tartomany}, Állampolgárok száma: {count}");
            }

            MegoldasLista.Items.Clear();
            MegoldasLista.ItemsSource = result.ToString().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        }

        private void Feladat4()
        {
            MegoldasTeljes.ItemsSource = allampolgar.Where(polgar => polgar.Nemzetiseg == "angolai");
        }

        private void Feladat5()
        {
            MegoldasTeljes.ItemsSource = allampolgar.Where(polgar => polgar.SzuletesiEv == allampolgar.Min(p => p.SzuletesiEv));
        }

        private void Feladat6()
        {
            var nonSmokers = allampolgar.Where(polgar => !polgar.Dohanyzik).ToList();
            MegoldasLista.ItemsSource = nonSmokers.Select(polgar => $"Nem dohányzó állampolgár - Id: {polgar.Id}, Havi jövedelem: {polgar.HaviNettoBevetele} €").ToList();
        }

        private void Feladat7()
        {

        }

        private void Feladat8()
        {

        }

        private void Feladat9()
        {

        }

        private void Feladat10()
        {

        }

        private void Feladat11()
        {

        }

        private void Feladat12()
        {

        }

        private void Feladat13()
        {

        }

        private void Feladat14()
        {

        }

        private void Feladat15()
        {

        }

        private void Feladat16()
        {

        }

        private void Feladat17()
        {

        }

        private void Feladat18()
        {

        }

        private void Feladat19()
        {

        }

        private void Feladat20()
        {

        }

        private void Feladat21()
        {

        }

        private void Feladat22()
        {

        }

        private void Feladat23()
        {

        }

        private void Feladat24()
        {

        }

        private void Feladat25()
        {

        }

        private void Feladat26()
        {

        }

        private void Feladat27()
        {

        }

        private void Feladat28()
        {

        }

        private void Feladat29()
        {

        }

        private void Feladat30()
        {

        }

        private void Feladat31()
        {

        }

        private void Feladat32()
        {

        }

        private void Feladat33()
        {

        }

        private void Feladat34()
        {

        }

        private void Feladat35()
        {

        }

        private void Feladat36()
        {

        }

        private void Feladat37()
        {

        }

        private void Feladat38()
        {

        }

        private void Feladat39()
        {

        }

        private void Feladat40()
        {

        }
        #endregion
    }
}