using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Diagnostics;

namespace RegistraPW
{
    class Password
    {
        public string sito { get; set; }
        public string nomeUtente { get; set; }
        public string matrice { get; set; }
        public string pw { get; set; }

        public Password()
        {
            this.sito = "Non definito";
            this.nomeUtente = "Non definito";
            this.matrice = "Non definito";
            this.pw = "Non definito";
        }

        public Password(string _sito, string _nomeUtente, string _matrice, string _pw)
        {
            this.sito = _sito;
            this.nomeUtente = _nomeUtente ;
            this.matrice = _matrice;
            this.pw = _pw;
        }

        public static string Cripta(string _sito, string _nomeUtente, string _matrice)
        {
            string MD5sito;
            using (MD5 md5Hash = MD5.Create())
            {
                MD5sito = Password.GetMd5Hash(md5Hash, _sito);
                MD5sito = MD5sito.Substring(0, 4);
            }

            string MD5nomeutente;
            using (MD5 md5Hash = MD5.Create())
            {
                MD5nomeutente = Password.GetMd5Hash(md5Hash, _nomeUtente);
                MD5nomeutente = MD5nomeutente.Substring(0, 4);
            }

            string MD5matrice;
            using (MD5 md5Hash = MD5.Create())
            {
                MD5matrice = Password.GetMd5Hash(md5Hash, _matrice);
                MD5matrice = MD5matrice.Substring(0, 4);
            }

            string MD5completo;
            using (MD5 md5Hash = MD5.Create())
            {
                MD5completo = Password.GetMd5Hash(md5Hash, _sito + _nomeUtente + _matrice);
                MD5completo = MD5completo.Substring(0, 4);
            }


            string SommaMD5 = MD5sito + MD5nomeutente + MD5matrice + MD5completo;
            
            char[] lettere  = new char[] { 'q', 'w', 'r', 't', 'p', 's', 'd', 'f', 'g', 'h', 'j', 'k', 'l', 'z', 'x', 'c', 'v', 'b', 'n', 'm', 'a', 'e', 'i', 'o', 'u', 'y' };
            char[] numeri   = new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };
            char[] speciali = new char[] { '|', '!', '£', '$', '%', '&', '/', '(', ')', '=', '?', '^', '[', ']', '*', '+', '@', '#', ',', '.', '-', ';', ':', '_' };

            //  ----------QUA SAREBBE DA MESCOLARE I CARATTERI IN BASE ALLO SCHIFT DELLA MATRICE------
            string Mescolata = SommaMD5.Substring(0,1) + 
                               SommaMD5.Substring(5,1) +
                               SommaMD5.Substring(2,1) +
                               SommaMD5.Substring(15,1) +
                               
                               SommaMD5.Substring(4,1) +
                               SommaMD5.Substring(1,1) +
                               SommaMD5.Substring(14,1) +
                               SommaMD5.Substring(11,1) +
                               
                               SommaMD5.Substring(12,1) +
                               SommaMD5.Substring(9,1) +
                               SommaMD5.Substring(6,1) +
                               SommaMD5.Substring(3,1) +
                               
                               SommaMD5.Substring(8,1) +
                               SommaMD5.Substring(13,1) +
                               SommaMD5.Substring(10,1) +
                               SommaMD5.Substring(7,1); 

            List<char> lista = new List<char>();

            int cont = 0;
            foreach (char c in Mescolata)
            {
                // LETTERE MAIUSCOLE
                if ((cont == 0) || (cont == 4) || (cont == 8) || (cont == 12))
                {
                    
                    // Se è una lettera
                    if (lettere.Contains(Char.ToLower(c)))
                    {
                        lista.Add(char.ToUpper(c));
                    }
                    // Se è un numero
                    else if (numeri.Contains(Char.ToLower(c)))
                    {
                        // deve essere per forza una lettera maiuscola per cui converto
                        int indice = Array.IndexOf(numeri, Char.ToLower(c));
                        lista.Add(char.ToUpper(lettere[indice]));
                    }
                                        
                    ++cont;
                    continue;
                }
                
                // LETTERE MINUSCOLE
                else if ((cont == 1) || (cont == 5) || (cont == 9) || (cont == 13))
                {
                    // Se è una lettera
                    if (lettere.Contains(Char.ToLower(c)))
                    {
                        lista.Add(char.ToLower(c));
                    }
                    // Se è un numero
                    else if (numeri.Contains(Char.ToLower(c)))
                    {
                        // deve essere per forza una lettera maiuscola per cui converto
                        int indice = Array.IndexOf(numeri, Char.ToLower(c));
                        lista.Add(char.ToLower(lettere[indice+10]));
                    }

                    ++cont;
                    continue;
                }
                
                // NUMERI
                else if ((cont == 2) || (cont == 6) || (cont == 10) || (cont == 14))
                {

                    // Se è una lettera bisogna convertirla in numero
                    if (lettere.Contains(Char.ToLower(c)))
                    {
                        int indice = Array.IndexOf(lettere, Char.ToLower(c));
                        int car = 0;
                        for (int n = 0; n < indice ; ++n)
                        {
                            if (car + 1 > numeri.Length -1)
                                car = 0;
                            else
                                ++car;
                        }
                        lista.Add(numeri[car]);
                    }
                    // Se è un numero
                    else if (numeri.Contains(Char.ToLower(c)))
                    {
                        // se è già un numero basta copiarlo
                        lista.Add(c);
                    }

                    ++cont;
                    continue;
                }

                // CARATTERI SPECIALI
                else if ((cont == 3) || (cont == 7) || (cont == 11) || (cont == 15))
                {
                    // Se è una lettera bisogna convertirla in speciale
                    if (lettere.Contains(Char.ToLower(c)))
                    {
                        int indice = Array.IndexOf(lettere, Char.ToLower(c));
                        // se cade all'interno della lunghezza degli speciali
                        if (indice <= speciali.Length - 1)
                        {
                            lista.Add(speciali[indice]);
                        }
                        else
                        {
                            int car = 0;
                            for (int n = 0; n < indice; ++n)
                            {
                                if (car + 1 > speciali.Length -1)
                                    car = 0;
                                else
                                    ++car;
                            }
                            lista.Add(speciali[car]);
                        }
                    }
                    // Se è un numero bisogna convertirlo in speciale
                    else if (numeri.Contains(Char.ToLower(c)))
                    {
                        int indice = Array.IndexOf(numeri, Char.ToLower(c));
                        lista.Add(speciali[indice]);
                    }

                    ++cont;
                    continue;
                }
            }

            // VERIFICA CHE NON CI SIANO LETTERE DOPPIE E CORREGGI
            char[] lDoppi  = new char[16];
            List<char> temp = new List<char>();
            char ct;
            
            cont = 0;
            foreach (char c in lista)
            {
                ct = c;
                if (lDoppi.Contains(char.ToLower(ct)))
                {
                    // Caratteri da sostituire
                    // -----------------------
                    // LETTERE MAIUSCOLE
                    if ((cont == 0) || (cont == 4) || (cont == 8) || (cont == 12))
                    {
                        int x = 0;
                        do
                        {
                            if (x < lettere.Length)
                                ct = lettere[x];
                            x++;
                        } while (lDoppi.Contains(char.ToLower(ct)));
                        temp.Add(char.ToUpper (ct));
                    }
                    // LETTERE MINUSCOLE
                    else if ((cont == 1) || (cont == 5) || (cont == 9) || (cont == 13))
                    {
                        int x = 0;
                        do
                        {
                            if (x < lettere.Length)
                                ct = lettere[x];
                            x++;
                        } while (lDoppi.Contains(char.ToLower(ct)));
                        temp.Add(char.ToLower(ct));
                    }
                    // NUMERI
                    else if ((cont == 2) || (cont == 6) || (cont == 10) || (cont == 14))
                    {
                        int x = 0;
                        do
                        {
                            if (x < numeri.Length)
                                ct = numeri[x];
                            x++;
                        } while (lDoppi.Contains(ct));
                        temp.Add(ct);
                    }

                    // CARATTERI SPECIALI
                    else if ((cont == 3) || (cont == 7) || (cont == 11) || (cont == 15))
                    {
                        int x = 0;
                        do
                        {
                            if (x < speciali.Length)
                                ct = speciali[x];
                            x++;
                        } while (lDoppi.Contains(ct));
                        temp.Add(ct);
                    }
                    lDoppi[cont] = char.ToLower(ct);
                }
                else
                {
                    lDoppi[cont] = char.ToLower(ct);
                    temp.Add(ct); 
                }
                ++cont;
            }

            // SCRIVI IL RISULTATO
            string risultato = string.Join("", temp.ToList<char>() );
            return risultato;
        }
        
        public new string ToString()
        {
            return string.Format("|s: {0,30} | n: {1,24} |\n|m: {2,30} | p: {3,24} |\n|----------------------------------------------------------------|", sito, nomeUtente, matrice, pw);
        }

        private static string GetMd5Hash(MD5 md5Hash, string input)
        {
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
    }
}
