using System;

namespace Bibliotek
{
    class Bok
    {
        private string författare;
        private string titel;

        public Bok(string f, string t)
        {
            författare = f;
            titel = t;
        }
        public string Författare
        {
            get { return författare; }
            set { författare = value; }
        }
        public string Titel
        {
            get { return titel; }
            set { titel = value; }
        }
        public void Info()
        {
            Console.WriteLine(titel + " <|> skriven av " + författare);
        }
    }
}
