using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;
using System.Xml;
using System.Diagnostics;
using System.Xml.Xsl;

namespace RegistraPW
{
    class Lpassword : List<Password>
    {
        public static Lpassword LeggiXML(string nomefile)
        {
            Lpassword lista = new Lpassword();

            var element = XElement.Load(nomefile);
            var query = from p in element.Descendants("Password")
                        select new Password(p.Element("sito").Value,
                                            p.Element("nomeUtente").Value,
                                            p.Element("matrice").Value,
                                            p.Element("pw").Value
                                            );

            foreach (Password i in query)
            {
                lista.Add(i);
            }

            return lista;
        }

        public static bool VerificaNuovo(Password password, string nomefile)
        {
            
            var element = XElement.Load(nomefile);
            var query = from p in element.Descendants("Password")
                        select new Password(p.Element("sito").Value,
                                            p.Element("nomeUtente").Value,
                                            p.Element("matrice").Value,
                                            p.Element("pw").Value
                                            );
            bool nuovo = true;

            foreach (Password i in query)
            {
                if ((i.sito == password.sito) && (i.nomeUtente == password.nomeUtente) && (i.matrice == password.matrice))
                    nuovo = false;
            }

            return nuovo;
        }

        public void ScriviXML(string nomefile)
        {
            XElement xmlPassword = new XElement("ListaPassword", 
                                    from p in this
                                    select new XElement("Password",
                                           new XElement("sito", p.sito),
                                           new XElement("nomeUtente", p.nomeUtente),
                                           new XElement("matrice", p.matrice),
                                           new XElement("pw", p.pw)
                                           ));
            
            var doc = new XDocument(new XProcessingInstruction("xml-stylesheet", "type='text/xsl' href='lista.xsl'"), xmlPassword);
            doc.Save(nomefile);
        }

        public Lpassword CercaSito(string sitoweb)
        {
            Lpassword lista = new Lpassword();

            var query = from p in this
                        where p.sito == sitoweb
                        select new Password { sito = p.sito, nomeUtente = p.nomeUtente, matrice = p.matrice, pw = p.pw };

            foreach (Password i in query)
            {
                lista.Add(i);
            }

            return lista;
        }

        public Lpassword CercaNomeUtente(string nomeutente)
        {
            Lpassword lista = new Lpassword();

            var query = from p in this
                        where p.nomeUtente == nomeutente
                        select new Password { sito = p.sito, nomeUtente = p.nomeUtente, matrice = p.matrice, pw = p.pw };

            foreach (Password i in query)
            {
                lista.Add(i);
            }

            return lista;
        }

        public void Leggi()
        {
            foreach (Password p in this)
            {
                Console.WriteLine(p.ToString());
            }
        }
    }
}
