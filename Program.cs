using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Diagnostics;

namespace RegistraPW
{
    class Program
    {
        static string CorreggiSito(string sito)
        {
            try
            {
                // tolgo gli spazi vuoti iniziali e finali
                sito = sito.Trim();

                // trasformo l'indirizzo in minuscolo
                sito = sito.ToLower();

                // controllo che non sia stato inserito www.
                int indiceWWW = sito.IndexOf("www.");
                if (indiceWWW != -1)
                    sito = sito.Substring(indiceWWW + 4, sito.Length - indiceWWW - 4);

                // controllo che non sia stato inserito http://
                int indiceHttp = sito.IndexOf("http://");
                if (indiceHttp != -1)
                    sito = sito.Substring(indiceHttp + 7, sito.Length - indiceHttp - 7);

                // controllo che non sia stato inserito https://
                int indiceHttps = sito.IndexOf("https://");
                if (indiceHttps != -1)
                    sito = sito.Substring(indiceHttps + 8, sito.Length - indiceHttps - 8);

                return sito;
            }
            catch (Exception e)
            {
                Console.WriteLine("Errore nella zona sistema nome sito {0} Exception caught.", e);
                return "Errore";
            }
        }

        [STAThread]
        static void Main(string[] args)
        {
            string nomefile = "lista.xml";
            
            Lpassword lista = new Lpassword();
            
            // MENU

            ConsoleKeyInfo risposta;
            do
            {
                if (File.Exists(nomefile))
                {
                    lista = Lpassword.LeggiXML(nomefile);
                }
                else
                {
                    lista.ScriviXML(nomefile);
                }
                Console.Clear();
                Console.WriteLine("|----------------------------------------------------------------|");
                Console.WriteLine("|                        REGISTRA PASSWORD                       |");
                Console.WriteLine("|----------------------------------------------------------------|");
                Console.WriteLine("|                        Studio Archistico                       |");
                Console.WriteLine("|----------------------------------------------------------------|\n");
                Console.WriteLine(" Scegliere un'opzione dal menu (digita il numero desiderato):\n");
                Console.WriteLine(" 1) Inserire nuova password\n");
                Console.WriteLine(" 2) Visualizzare le password registrate\n");
                Console.WriteLine(" 3) Cerca tra le password registrate suddivise per sito\n");
                Console.WriteLine(" 4) Cerca tra le password registrate suddivise per nome utente\n");
                Console.WriteLine(" 5) Cancella una password\n");
                Console.WriteLine(" 6) Uscire oppure premere ESC");
                Console.Write("\n ");
                risposta = Console.ReadKey();

                // INSERISCI UNA PASSWORD
                if (risposta.Key == ConsoleKey.D1 || risposta.Key == ConsoleKey.NumPad1)
                {
                    Console.Clear();
                    Console.WriteLine("|----------------------------------------------------------------|");
                    Console.WriteLine("|                       CREA NUOVA PASSWORD                      |");
                    Console.WriteLine("|----------------------------------------------------------------|\n");
                    Console.Write("Inserire il sito web (senza 'www.' o 'http://'): ");
                    string _sito = Console.ReadLine();
                    if (_sito.Length < 5)
                    {
                        do
                        {
                            Console.WriteLine("ATTENZIONE - Il sito web deve essere almeno di 5 caratteri");
                            Console.Write("Inserire il sito web (senza 'www.' o 'http://'): ");
                            _sito = Console.ReadLine();
                        } while (_sito.Length < 5);
                    }

                    // Correzione sito web
                    _sito = CorreggiSito(_sito);

                    // Carico il nome utente
                    string _nomeUtente;
                    do
                    {
                        Console.Write("Inserire il nome utente (obbligatorio): ");
                        _nomeUtente = Console.ReadLine();
                    } while (_nomeUtente.Length == 0);

                    // Carico la matrice
                    string _matrice;
                    do
                    {
                        Console.Write("Inserire la matrice (obbligatorio): ");
                        _matrice = Console.ReadLine();
                    } while (_matrice.Length == 0);

                    // PASSWORD
                    Console.Write("La password creata è: ");
                    string _pw = Password.Cripta(_sito, _nomeUtente, _matrice);
                    Console.WriteLine(_pw);

                    // ----COPIA NELLA CLIPBOARD---
                    Clipboard.SetText(_pw);

                    Console.WriteLine("|----------------------------------------------------------------|");
                    Console.WriteLine("|                Password copiata negli appunti                  |");
                    Console.WriteLine("|----------------------------------------------------------------|");

                    ConsoleKeyInfo rispRegistro;
                    do
                    {
                        Console.Write("\nRegistro la password? (S/N) ");
                        rispRegistro = Console.ReadKey();
                    } while ((rispRegistro.Key != ConsoleKey.S) && (rispRegistro.Key  != ConsoleKey.N));

                    if (rispRegistro.Key == ConsoleKey.S)
                    {
                        if (Lpassword.VerificaNuovo(new Password { sito = _sito, nomeUtente = _nomeUtente, matrice = _matrice, pw = _pw }, nomefile))
                        { 
                            lista.Add(new Password { sito = _sito, nomeUtente = _nomeUtente, matrice = _matrice, pw = _pw });
                            lista.ScriviXML(nomefile);
                            Console.WriteLine("");
                            Console.WriteLine("|----------------------------------------------------------------|");
                            Console.WriteLine("|                   REGISTRAZIONE EFFETTUATA                     |");
                            Console.WriteLine("|----------------------------------------------------------------|");
                            Console.WriteLine("\nPremere un tasto qualsiasi per tornare al menu");
                            Console.ReadLine();
                        }
                        else
                        {
                            Console.WriteLine("");
                            Console.WriteLine("|----------------------------------------------------------------|");
                            Console.WriteLine("|                   PASSWORD GIA' PRESENTE                       |");
                            Console.WriteLine("|----------------------------------------------------------------|");
                            Console.WriteLine("\nPremere un tasto qualsiasi per tornare al menu");
                            Console.ReadLine();
                        }
                    }
                }
                
                // VISUALIZZA TUTTE LE PASSWORD
                if (risposta.Key == ConsoleKey.D2 || risposta.Key == ConsoleKey.NumPad2)
                {
                    Console.Clear();
                    Console.WriteLine("|----------------------------------------------------------------|");
                    Console.WriteLine("|             Lista di tutte le password registrate              |");
                    Console.WriteLine("|----------------------------------------------------------------|");
                    lista.Leggi();
                    ConsoleKeyInfo rispVisualizzazione;
                    do
                    {
                        Console.Write("\nVisualizzo le password sul browser? (S/N) ");
                        rispVisualizzazione = Console.ReadKey();
                    } while ((rispVisualizzazione.Key != ConsoleKey.S) && (rispVisualizzazione.Key  != ConsoleKey.N));

                    if (rispVisualizzazione.Key == ConsoleKey.S)
                    {
                        System.Diagnostics.Process.Start(Environment.CurrentDirectory + "\\lista.html");
                    }
                    Console.WriteLine("\n\nPremere un tasto qualsiasi per tornare al menu");
                    Console.ReadLine();
                }

                // CERCA UNA PASSWORD PER SITO
                if (risposta.Key == ConsoleKey.D3 || risposta.Key == ConsoleKey.NumPad3)
                {
                    Console.Clear();
                    Console.WriteLine("|----------------------------------------------------------------|");
                    Console.WriteLine("|           Cerca tra le password suddivise per sito             |");
                    Console.WriteLine("|----------------------------------------------------------------|\n");
                    Console.Write("Inserire il sito web relativo: ");
                    string _sito = Console.ReadLine();
                    Lpassword risultato = new Lpassword();
                    risultato = lista.CercaSito(_sito);
                    Console.WriteLine("");
                    Console.WriteLine("|----------------------------------------------------------------|");
                    risultato.Leggi();
                    Console.WriteLine("\nPremere un tasto qualsiasi per tornare al menu");
                    Console.ReadLine();
                }

                // CERCA UNA PASSWORD PER NOME UTENTE
                if (risposta.Key == ConsoleKey.D4 || risposta.Key == ConsoleKey.NumPad4)
                {
                    Console.Clear();
                    Console.WriteLine("|----------------------------------------------------------------|");
                    Console.WriteLine("|           Cerca tra le password suddivise per utente           |");
                    Console.WriteLine("|----------------------------------------------------------------|\n");
                    Console.Write("Inserire il nome utente da ricercare: ");
                    string _nomeutente = Console.ReadLine();
                    Lpassword risultato = new Lpassword();
                    risultato = lista.CercaNomeUtente(_nomeutente);
                    Console.WriteLine("");
                    Console.WriteLine("|----------------------------------------------------------------|");
                    risultato.Leggi();
                    Console.WriteLine("\nPremere un tasto qualsiasi per tornare al menu");
                    Console.ReadLine();
                }

                // CANCELLA UNA PASSWORD
                if (risposta.Key == ConsoleKey.D5 || risposta.Key == ConsoleKey.NumPad5)
                {
                    Console.Clear();
                    Console.WriteLine("Cancella una password registrata\n");
                    Console.WriteLine("|-------------------------------------------|");
                    Console.WriteLine("|      FUNZIONE ANCORA NON SUPPORTATA       |");
                    Console.WriteLine("|-------------------------------------------|");
                    Console.WriteLine("\nPremere un tasto qualsiasi per tornare al menu");
                    Console.ReadLine();
                }
            } while ((risposta.Key != ConsoleKey.Escape) && (risposta.Key != ConsoleKey.D6) && (risposta.Key != ConsoleKey.NumPad6));
            Console.Clear();
        }
    }
}
