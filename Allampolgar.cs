using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BevolkerungGUI
{
    internal class Allampolgar
    {
        public int Id { get; set; }
        public string Nem { get; set; }
        public int SzuletesiEv { get; set; }
        public int Suly { get; set; }
        public int Magassag { get; set; }
        public bool Dohanyzik { get; set; }
        public string DohanyzoString => Dohanyzik ? "igen" : "nem";
        public string Nemzetiseg { get; set; }
        public string Nepcsoport { get; set; }
        public string Tartomany { get; set; }
        public int NettoJovedelem { get; set; }
        public string IskolaiVegzettseg { get; set; }
        public string PolitikaiNezet { get; set; }
        public bool AktivSzavazo { get; set; }
        public string AktivSzavazoString => AktivSzavazo ? "igen" : "nem";
        public int SorFogyasztasEvente { get; set; }
        public string SorString { get; set; }
        public int KrumpliFogyasztasEvente { get; set; }
        public string KrumpliString { get; set; }
        public int HaviNettoBevetele => NettoJovedelem / 12;
        public int Eletkor => DateTime.Now.Year - SzuletesiEv;

        public Allampolgar(string row)
        {
            string[] adatok = row.Split(';');
            Id = int.Parse(adatok[0]);
            Nem = adatok[1];
            SzuletesiEv = int.Parse(adatok[2]);
            Suly = int.Parse(adatok[3]);
            Magassag = int.Parse(adatok[4]);
            Dohanyzik = adatok[5] == "igen";
            Nemzetiseg = adatok[6];
            if (Nemzetiseg == "német")
            {
                Nepcsoport = adatok[7];
            }
            else
            {
                Nepcsoport = null;
            }
            Tartomany = adatok[8];
            NettoJovedelem = int.Parse(adatok[9]);
            IskolaiVegzettseg = adatok[10];
            PolitikaiNezet = adatok[11];
            AktivSzavazo = adatok[12] == "igen";
            SorString = adatok[13];
            if (adatok[13] != "NA")
            {
                SorFogyasztasEvente = int.Parse(adatok[13]);
            }
            KrumpliString = adatok[14];
            if (adatok[14] != "NA")
            {
                KrumpliFogyasztasEvente = int.Parse(adatok[14]);
            }
        }
        public override string ToString()
        {
           return $"{Id} {Nem} {SzuletesiEv} {Suly}kg {Magassag}cm {(Dohanyzik? "dohányzik" : "nem dohányzik")} {Nemzetiseg} {Nepcsoport} {NettoJovedelem}EUR {IskolaiVegzettseg} {PolitikaiNezet} {(AktivSzavazo? "aktív szavazó" : "nem szavaz")} {SorFogyasztasEvente}l sör {KrumpliFogyasztasEvente}kg krumpli";
        }

        public string ToString(bool five) 
        {
            if (five)
            {
                return $"{Id}\t{Nem}\t{SzuletesiEv}\t{Suly}kg\t{Magassag}cm";
            }
            else
            {
                return $"{Id}\t{Nemzetiseg}\t{Nepcsoport}\t{Tartomany}\t{NettoJovedelem}€";
            }
        }

    }
   
}
