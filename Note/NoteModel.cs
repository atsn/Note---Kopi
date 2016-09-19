using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteMVVM
{
    class NoteModel
    {
        public string Emne { get; set; }
        public int Nummer { get; set; }
        public DateTime DateTime { get; set; }
        public string Tekst { get; set; }
        private static int count = 0;

       

        public NoteModel(string emne, string tekst)
        {
            Emne = emne;
            Nummer = count +1;
            Tekst = tekst;
            DateTime = DateTime.Now;

            count++;
        }

        public NoteModel()
        {
            Emne = null;
            Nummer = count+1;
            DateTime = DateTime.Now;
            Tekst = null;
            count++;
        }
       

        public override string ToString()
        {
            return $"Emne: {Emne}, Nummer: {Nummer}, DateTime: {DateTime}, Navn: {Tekst}";
        }


    }
}
