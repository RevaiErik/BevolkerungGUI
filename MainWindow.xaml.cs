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

            for (int i = 1; i <= 45; i++)
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
            MegoldasTeljes.ItemsSource= allampolgar.Where(polgar => polgar.Tartomany == "Bajorország" && polgar.NettoJovedelem > 30000).OrderBy(polgar => polgar.IskolaiVegzettseg).ToList();

        }

        private void Feladat8()
        {
            var maleCitizens = allampolgar.Where(polgar => polgar.Nem == "férfi").ToList();
            foreach (var item in maleCitizens)
            {
                MegoldasLista.Items.Add(item.ToString(true));
            }
        }

        private void Feladat9()
        {
            var womenInBavaria = allampolgar.Where(polgar => polgar.Nem == "nő" && polgar.Tartomany == "Bajorország").ToList();
            foreach (var item in womenInBavaria)
            {
                MegoldasLista.Items.Add(item.ToString(false));
            }
        }

        private void Feladat10()
        {
            var nonSmokers = allampolgar.Where(polgar => !polgar.Dohanyzik).OrderByDescending(polgar => polgar.NettoJovedelem).Take(10).ToList();
            MegoldasTeljes.ItemsSource = nonSmokers;
        }

        private void Feladat11()
        {
            var oldestCitizens = allampolgar.OrderBy(polgar => polgar.SzuletesiEv).Take(5).ToList();
            MegoldasTeljes.ItemsSource = oldestCitizens;
        }

        private void Feladat12()
        {
            var germanCitizensByNepcsoport = allampolgar.Where(polgar => polgar.Nemzetiseg == "német").GroupBy(polgar => polgar.Nepcsoport);

            foreach (var group in germanCitizensByNepcsoport)
            {
               MegoldasLista.Items.Add($"Népcsoport: {group.Key}");
                foreach (var polgar in group)
                {
                    MegoldasLista.Items.Add($"\t{(polgar.AktivSzavazo ? "aktív szavazó" : "nem aktív szavazó" )}\t {polgar.PolitikaiNezet}");
                }
            }

        }

        private void Feladat13()
        {
            double averageBeerConsumption = allampolgar.Where(polgar => polgar.Nem == "férfi").Average(polgar => polgar.SorFogyasztasEvente);
            MegoldasMondatos.Content = $"Az éves átlagos sörfogyasztás (férfi): {averageBeerConsumption:0.00} liter";
        }

        private void Feladat14()
        {
            MegoldasTeljes.ItemsSource = allampolgar.OrderBy(polgar => polgar.IskolaiVegzettseg);
        }

        private void Feladat15()
        {
            foreach (Allampolgar item in allampolgar.OrderByDescending(polgar => polgar.NettoJovedelem).Take(3))
            {
                MegoldasLista.Items.Add(item.ToString(false));
            }
            MegoldasLista.Items.Add("...");
            foreach (Allampolgar item in allampolgar.OrderBy(polgar => polgar.NettoJovedelem).Take(3))
            {
                MegoldasLista.Items.Add(item.ToString(false));
            }

        }

        private void Feladat16()
        {
            double activeVotersPercentage = (double)allampolgar.Count(polgar => polgar.AktivSzavazo) / allampolgar.Count * 100;
            MegoldasMondatos.Content = $"Az állampolgárok {activeVotersPercentage:0.00}%-a aktív szavazó";
        }

        private void Feladat17()
        {
            MegoldasTeljes.ItemsSource= allampolgar.Where(polgar => polgar.AktivSzavazo).OrderBy(polgar => polgar.Tartomany);

          
        }

        private void Feladat18()
        {
            MegoldasMondatos.Content = $"Az állampolgárok átlagos életkora: {(allampolgar.Average(polgar => polgar.Eletkor)):0.00} év";
        }

        private void Feladat19()
        {
            var legmagasabb = allampolgar
                .GroupBy(polgar => polgar.Tartomany)
                .OrderByDescending(group => group.Average(polgar => polgar.NettoJovedelem))
                .ThenByDescending(group => group.Count())
                .FirstOrDefault();

                MegoldasMondatos.Content = $"A legmagasabb átlagos éves nettó jövedelem: {legmagasabb.Key} (lakosok száma: {legmagasabb.Count()}) {legmagasabb.Average(polgar => polgar.NettoJovedelem)} €";
        }

        private void Feladat20()
        {
            double averageWeight = allampolgar.Average(polgar => polgar.Suly);

            var sortedWeights = allampolgar.Select(polgar => polgar.Suly).OrderBy(suly => suly).ToList();
            double medianWeight;
            if (sortedWeights.Count % 2 == 0)
            {
                int middleIndex = sortedWeights.Count / 2;
                medianWeight = (sortedWeights[middleIndex - 1] + sortedWeights[middleIndex]) / 2.0;
            }
            else
            {
                int middleIndex = sortedWeights.Count / 2;
                medianWeight = sortedWeights[middleIndex];
            }

            MegoldasMondatos.Content = $"Az állampolgárok átlagos súlya: {averageWeight:0.00} kg, medián súly: {medianWeight:0.00} kg";
        }


        private void Feladat21()
        {
            double szavazsor = allampolgar.Where(polgar => polgar.AktivSzavazo).Average(polgar => polgar.SorFogyasztasEvente);
            double nemszavazsor = allampolgar.Where(polgar => !polgar.AktivSzavazo).Average(polgar => polgar.SorFogyasztasEvente);

            MegoldasMondatos.Content = $"Az aktív szavazók átlagos sörfogyasztása évente: {szavazsor:0.00} liter, a nem szavazók átlagos sörfogyasztása évente: {nemszavazsor:0.00} liter";

            if (szavazsor > nemszavazsor)
            {
                MegoldasMondatos.Content += " (Az aktív szavazók fogyasztanak több sört.)";
            }
            else if (szavazsor < nemszavazsor)
            {
                MegoldasMondatos.Content += " (A nem szavazók fogyasztanak több sört.)";
            }
            else
            {
                MegoldasMondatos.Content += " (Az aktív szavazók és a nem szavazók átlagos sörfogyasztása megegyezik.)";
            }
        }


        private void Feladat22()
        {
            double averageHeightMales = allampolgar.Where(polgar => polgar.Nem == "férfi").Average(polgar => polgar.Magassag);
            double averageHeightFemales = allampolgar.Where(polgar => polgar.Nem == "nő").Average(polgar => polgar.Magassag);

            MegoldasMondatos.Content = $"Az átlagos magasság férfiaknál: {averageHeightMales:0.00} cm, nőknél: {averageHeightFemales:0.00} cm";
        }

        private void Feladat23()
        {
            var legtobbnepcsoport = allampolgar.Where(polgar => polgar.Nemzetiseg == "német").GroupBy(polgar => polgar.Nepcsoport).OrderByDescending(group => group.Count()).ThenByDescending(group => group.Average(polgar => polgar.Eletkor)).First();

            MegoldasMondatos.Content = $"A legtöbb német állampolgár: {legtobbnepcsoport.Key} népcsoportba tartozik, lakosság: {legtobbnepcsoport.Count()}";
        }

        private void Feladat24()
        {
            var dohanyzok = allampolgar.Where(polgar => polgar.Dohanyzik).Average(polgar => polgar.NettoJovedelem);
            var nemdohanyzok = allampolgar.Where(polgar => !polgar.Dohanyzik).Average(polgar => polgar.NettoJovedelem);

            MegoldasLista.Items.Add($"Dohányzók átlagos nettójövedelme: {(dohanyzok):0.00} €");
            MegoldasLista.Items.Add($"Nem dohányzók átlagos nettójövedelme: {(nemdohanyzok):0.00} €");

            if (dohanyzok > nemdohanyzok)
            {
                MegoldasLista.Items.Add($"A dohányzók nettó átlagkeresete több");
            }
            else if (nemdohanyzok > dohanyzok)
            {
                MegoldasLista.Items.Add($"A nem dohányzók nettó átlagkeresete több");
            }
            else
            {
                MegoldasLista.Items.Add($"Nem lehet eldönteni");
            }
        }

        private void Feladat25()
        {
            MegoldasMondatos.Content = $"Az átlag krumpli fogyasztás: {allampolgar.Average(polgar => polgar.KrumpliFogyasztasEvente):0.00} kg";
            MegoldasTeljes.ItemsSource = allampolgar.Where(polgar => polgar.KrumpliFogyasztasEvente > (allampolgar.Average(polgar => polgar.KrumpliFogyasztasEvente))).Take(15).ToList();
        }


       private void Feladat26()
    {
        foreach (IGrouping<string, Allampolgar> tartomany in allampolgar.GroupBy(t => t.Tartomany))
            MegoldasLista.Items.Add($"{tartomany.Key} - {tartomany.Average(k => k.Eletkor):.00} átlagév");
    }
    private void Feladat27()
    {
        MegoldasLista.Items.Add($"50 évnél idősebbek száma: {allampolgar.Count(k => k.Eletkor > 50)} fő\n");
        foreach (Allampolgar otvenes in allampolgar.Where(k => k.Eletkor > 50))
            MegoldasLista.Items.Add($"{otvenes.ToString(true)}");
    }
    private void Feladat28()
    {
        List<Allampolgar> dohanyzoNok = allampolgar.Where(x => x.Nem == "nő" && x.Dohanyzik).ToList();

        if (dohanyzoNok.Count > 0)
        {
            MegoldasLista.Items.Add($"Maximális jövedelem: {dohanyzoNok.OrderByDescending(n => n.NettoJovedelem).First().NettoJovedelem}\n");

            foreach (Allampolgar dohanyzoNo in allampolgar.Where(x => x.Nem == "nő" && x.Dohanyzik))
                MegoldasLista.Items.Add(dohanyzoNo.ToString(false));
        }
        else MegoldasLista.Items.Add("Nincs dohányzó nő");
    }
    private void Feladat29()
    {
        foreach (IGrouping<string, Allampolgar> tartomany in allampolgar.GroupBy(t => t.Tartomany))
        {
            Allampolgar legnagyobbAlkesz = tartomany
                .Where(a => a.SorString != "NA")
                .OrderByDescending(s => s.SorFogyasztasEvente).First();

            MegoldasLista.Items.Add($"{tartomany.Key} - {legnagyobbAlkesz.Id} ({legnagyobbAlkesz.SorFogyasztasEvente}l sör)");
        }
    }
    private void Feladat30()
    {
        MegoldasLista.Items.Add(allampolgar.Where(n => n.Nem == "nő").OrderByDescending(k => k.Eletkor).First().ToString(true));
        MegoldasLista.Items.Add(allampolgar.Where(n => n.Nem == "férfi").OrderByDescending(k => k.Eletkor).First().ToString(true));
    }
    private void Feladat31()
    {
        foreach (string nemzetiseg in allampolgar.Select(x => x.Nemzetiseg).Distinct().OrderDescending())
            MegoldasLista.Items.Add(nemzetiseg);
    }
    private void Feladat32()
    {
        foreach (IGrouping<string, Allampolgar> tartomany in allampolgar.GroupBy(t => t.Tartomany).OrderBy(x => x.Count()))
            MegoldasLista.Items.Add(tartomany.Key);
    }
    private void Feladat33()
    {
        foreach (Allampolgar topJovedelmu in allampolgar.OrderByDescending(j => j.NettoJovedelem).Take(3))
            MegoldasLista.Items.Add($"{topJovedelmu.Id}\t{topJovedelmu.NettoJovedelem}");
    }
    private void Feladat34()
    {
        MegoldasMondatos.Content = $"55 kg feletti krumplifogyasztók átlagos súlya: {allampolgar
            .Where(k => k.KrumpliFogyasztasEvente > 55 && k.Nem == "férfi")
            .Average(s => s.Suly):.00} kg";
    }
    private void Feladat35()
    {
        foreach (IGrouping<string, Allampolgar> tartomany in allampolgar.GroupBy(t => t.Tartomany))
            MegoldasLista.Items.Add($"{tartomany.Key} - {tartomany.OrderBy(k => k.Eletkor).First().Eletkor} éves");
    }
    private void Feladat36()
    {
        foreach (string nemzetiseg in allampolgar.Select(x => x.Nemzetiseg).Distinct())
            foreach (string tartomany in allampolgar.Where(x => x.Nemzetiseg == nemzetiseg).Select(x => x.Tartomany).Distinct())
                MegoldasLista.Items.Add($"{nemzetiseg} - {tartomany}");
    }
    private void Feladat37()
    {
        double atlag = allampolgar.Average(x => x.NettoJovedelem);

        MegoldasLista.Items.Add($"Átlagos jövedelem: {atlag}");
        MegoldasLista.Items.Add($"Átlagon felül keresők: {allampolgar.Count(x => x.NettoJovedelem > atlag)} db\n");

        foreach (Allampolgar atlagFolotti in allampolgar.Where(x => x.NettoJovedelem > atlag))
            MegoldasLista.Items.Add(atlagFolotti.ToString(false));
    }
    private void Feladat38()
    {
        MegoldasLista.Items.Add($"Nők száma: {allampolgar.Count(x => x.Nem == "nő")}");
        MegoldasLista.Items.Add($"Férfiak száma: {allampolgar.Count(x => x.Nem == "férfi")}");
    }
    private void Feladat39()
    {
        foreach (IGrouping<string, Allampolgar> tartomany in allampolgar.GroupBy(t => t.Tartomany).OrderBy(o => o.Max(m => m.NettoJovedelem)))
            MegoldasLista.Items.Add($"{tartomany.Key} - {tartomany.Max(m => m.NettoJovedelem)}");
    }
    private void Feladat40()
    {
        double nemetekHaviJovedelme = allampolgar.Where(x => x.Nemzetiseg == "német").Sum(s => s.HaviNettoBevetele);
        double nemNemetekHaviJovedelme = allampolgar.Where(x => x.Nemzetiseg != "német").Sum(s => s.HaviNettoBevetele);

        double abszKulonbseg = Math.Abs(nemetekHaviJovedelme - nemNemetekHaviJovedelme);
        double atlag = (nemetekHaviJovedelme + nemNemetekHaviJovedelme) / 2;

        double szazalekosKulonbseg = abszKulonbseg / atlag * 100;

        MegoldasLista.Items.Add($"Németek havi jövedelme: {nemetekHaviJovedelme}");
        MegoldasLista.Items.Add($"Nem németek havi jövedelme: {nemNemetekHaviJovedelme}\n");
        MegoldasLista.Items.Add($"Százalékos különbség: {szazalekosKulonbseg:.00}%");
    }

        private void Feladat41()
        {
            var random = new Random();
            MegoldasTeljes.ItemsSource = allampolgar.Where(polgar => polgar.Nemzetiseg == "török" && polgar.AktivSzavazo).OrderBy(x => Guid.NewGuid()).Take(random.Next(1,11));
        }
        private void Feladat42()
        {
            double averageBeerDrinking = allampolgar.Where(x => x.SorString != "NA").Average(x => x.SorFogyasztasEvente);
            List<Allampolgar> aboveAverageDrinkers = allampolgar.Where(x => x.SorFogyasztasEvente > averageBeerDrinking).OrderBy(x => Guid.NewGuid()).Take(5).ToList();

            MegoldasMondatos.Content = $"Az átlag sörfogyasztás értéke: {averageBeerDrinking:0.00} liter";
            foreach (var citizen in aboveAverageDrinkers)
            {
                MegoldasLista.Items.Add(citizen.ToString(true));
            }

        }
        private void Feladat43()
        {
            double atlagjovedelem = allampolgar.Average(x => x.NettoJovedelem);
            var tartomanyok = allampolgar.GroupBy(tartomany => tartomany.Tartomany)
                                         .Where(x => x.Min(m => m.NettoJovedelem) > atlagjovedelem)
                                         .OrderBy(g => Guid.NewGuid())
                                         .Take(2);

            MegoldasLista.Items.Add($"Átlagos nettó jövedelem: {atlagjovedelem}");
            foreach (var tartomany in tartomanyok)
            {
                string tartomanyNev = tartomany.Key;
                int legkisebbJovedelem = tartomany.Min(m => m.NettoJovedelem);
                MegoldasLista.Items.Add($"Tartomány neve: {tartomanyNev}, Legkisebb nettó jövedelem: {legkisebbJovedelem}");
            }

        }
        private void Feladat44()
        {
            var unknownEducationCitizens = allampolgar.Where(c => string.IsNullOrEmpty(c.IskolaiVegzettseg)).ToList();

            if (unknownEducationCitizens.Count >= 3)
            {
                var random = new Random();
                MegoldasTeljes.ItemsSource = unknownEducationCitizens.OrderBy(c => random.Next()).Take(3);
            }
        }
        private void Feladat45()
        {
           
        }
        #endregion
    }
}