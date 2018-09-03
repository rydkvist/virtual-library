using System;
using System.IO;
using System.Collections.Generic;

namespace Bibliotek
{
    class Program
    {
        static List<Bok> Books = new List<Bok>();
        static List<Bok> borrowedBooks = new List<Bok>();
        static string space = "=================================";

        static void Main(string[] args)
        {
            Books.Clear();
            StreamReader fileRead = new StreamReader(@"böcker.txt");
            string text, temp_author, temp_titel;

            while ((text = fileRead.ReadLine()) != "=====|UTLANADE|=====")
            {
                string[] bokInfo = text.Split(',');
                for (int i = 0; i < bokInfo.Length; i++)
                {
                    temp_author = bokInfo[i];
                    temp_titel = bokInfo[++i];
                    Books.Add(new Bok(temp_author, temp_titel));
                }
            }
            while ((text = fileRead.ReadLine()) != null)
            {
                string[] bokInfo = text.Split(',');
                for (int i = 0; i < bokInfo.Length; i++)
                {
                    temp_author = bokInfo[i];
                    temp_titel = bokInfo[++i];
                    borrowedBooks.Add(new Bok(temp_author, temp_titel));
                }
            }
            fileRead.Close();
            Console.WriteLine("Välkommen till biblioteksprogrammet\n\n1. Tryck 'T' för att söka på Titel\n2. Tryck 'F' för att söka efter Författare\n3. Tryck 'L' för att låna en bok\n4. Tryck 'Å' för att återlämna en bok\n5. Tryck 'N' för att lägga in en ny bok\n6. Tryck 'B' för att ta bort en bok\n7. Tryck 'A' föra tt lista alla böcker\n8. Tryck 'S' för att avsluta programmet");
            Bibliotek();
        }
        static void Bibliotek()
        {
            char choice = ' ';
            Console.WriteLine("\nVad vill du göra? ");
            try
            {
                choice = Convert.ToChar(Console.ReadLine());
            } catch { Console.WriteLine("Ange en bokstav av de alternativ du kan välja!"); }

            switch (choice.ToString().ToLower())
            {
                case "t":
                    Console.WriteLine(space);
                    checkBook("titel", "med namnet");
                    Console.WriteLine(space);
                    break;
                case "f":
                    Console.WriteLine(space);
                    checkBook("författare", "skriven av");
                    Console.WriteLine(space);
                    break;
                case "l":
                    Console.WriteLine(space);
                    borrowBok();
                    Console.WriteLine(space);
                    break;
                case "å":
                    Console.WriteLine(space);
                    deleteBook(borrowedBooks, "lämna in?", "återlämnat");
                    Console.WriteLine(space);
                    break;
                case "n":
                    Console.WriteLine(space);
                    addBok();
                    Console.WriteLine(space);
                    break;
                case "b":
                    Console.WriteLine(space);
                    deleteBook(Books, "ta bort?", "tagits bort");
                    Console.WriteLine(space);
                    break;
                case "a":
                    Console.WriteLine(space);
                    showBooks(Books);
                    Console.WriteLine(space);
                    break;
                case "s":
                    Console.WriteLine(space);
                    Console.WriteLine("Välkommen åter!");
                    Environment.Exit(0);
                    break;
            }
            Bibliotek();
        }
        static void checkBook(string Message, string Result)
        {
            int antal_böcker = 0;
            Console.WriteLine("Vilken {0} vill du söka efter?", Message);
            string namn = Console.ReadLine();
            foreach(Bok txt in Books)
            {
                if(namn == txt.Författare && Message == "författare")
                {
                    antal_böcker++;
                }
                if (namn == txt.Titel && Message == "titel")
                {
                    antal_böcker++;
                }
            }
            if (antal_böcker == 0)
            {
                Console.WriteLine("Det hittades ingen bok med den {0}", Message);
            }
            else
            {
                Console.WriteLine("\nDet hittades {0} bok/böcker {1} '{2}'", antal_böcker, Result, namn);
                for (int i = 0; i < Books.Count; i++)
                {
                    if (namn == Books[i].Författare && Message == "författare")
                    {
                        Books[i].Info();
                    }
                    else if (namn == Books[i].Titel && Message == "titel")
                    {
                        Books[i].Info();
                    }
                }
            }
        }
        static void borrowBok()
        {
            Console.WriteLine("Böcker som går att låna ut just nu:\n");
            showBooks(Books);
            Console.WriteLine("\nAnge titeln på boken du vill låna: ");
            string temp_bok = Console.ReadLine();
            for(int i = 0; i < borrowedBooks.Count; i++)
            {
                if(temp_bok == borrowedBooks[i].Titel)
                {
                    Console.WriteLine("Boken är redan utlånad!");
                }
            }
            for (int i = 0; i < Books.Count; i++)
            {
                if (temp_bok == Books[i].Titel)
                {
                    borrowedBooks.Add(new Bok(Books[i].Författare, Books[i].Titel));
                    Books.RemoveAt(i);
                    writeBooks();
                    Console.WriteLine("Utlånad!");
                }
            }
        }
        static void addBok()
        {
            string new_author, new_titel;

            Console.Write("Författare på boken: ");
            new_author = Console.ReadLine();

            Console.Write("Titel på boken: ");
            new_titel = Console.ReadLine();

            Books.Add(new Bok(new_author, new_titel));
            Console.WriteLine("Boken '{0}' skriven av {1}, finns nu i 'böcker.txt'", new_titel, new_author);

            writeBooks();
        }
        static void deleteBook(List<Bok> Type, string Message, string Result)
        {
            if (Type.Count <= 0) { Console.WriteLine("Det finns inga böcker att {0}.", Result); }
            else
            {
                Console.WriteLine("Vilken bok ska du {0} Välj med nummer från 1 till {1}\n", Message, Type.Count);
                for (int i = 0; i < Type.Count; i++)
                {
                    Console.Write("#{0} ", (i + 1));
                    Type[i].Info();
                }
                int position = int.Parse(Console.ReadLine());

                if (Type == borrowedBooks)
                {
                    Books.Add(new Bok(borrowedBooks[position - 1].Författare, borrowedBooks[position - 1].Titel));
                    Type.RemoveAt(position - 1);
                }
                else { Books.RemoveAt(position - 1); }
                Console.WriteLine("Bok #{0} har {1}", position, Result);
                writeBooks();
            }
        }
        static void showBooks(List<Bok> Type)
        {
            for (int i = 0; i < Type.Count; i++)
            {
                Type[i].Info();
            }
        }
        static void writeBooks()
        {
            File.AppendAllText(@"böcker.txt", "");

            StreamWriter SW = new StreamWriter(@"böcker.txt");
            for (int i = 0; i < Books.Count; i++)
            {
                SW.WriteLine(Books[i].Författare + "," + Books[i].Titel);
            }
            SW.WriteLine("=====|UTLANADE|=====");
            for (int i = 0; i < borrowedBooks.Count; i++)
            {
                SW.WriteLine(borrowedBooks[i].Författare + "," + borrowedBooks[i].Titel);
            }
            SW.Close();
        }
    }
}