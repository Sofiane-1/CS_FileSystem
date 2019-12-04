using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace GestionFichier
{
    class Program
    {
        public static List<InformationFichierIndexé> FichierTraités { get; private set; }
        static void Main(string[] args)
        {

            foreach (var item in args)
            {

                if (item == "-aide")
                {
                    Console.WriteLine("Usage : GestionSystemeDeFichier [options] \n"+
                                          "     Options: \n" +
                                          "\t     -aide Afficher aide \n" +
                                          "\t     - index cheminRépertoireSource cheminRépertoireDestination \n"+
                                          "   Effectuer l’indexation du répertoire source et génère le \n"+
                                          "     fichier d’index à sa racine.\n"+
                                          " \t    - maj cheminRépertoireSource Effectuer une mise à jour du fichier\n"+
                                          "     d’index et synchronise le répertoire destination\n");

                }
                if (item == "-index")
                {
                    SynchroniseurDeRépertoire synchro = new SynchroniseurDeRépertoire(initialPath, destinationPath);
                    synchro.EffectuerIndexationSynchrone();
                    synchro.EffectuerMiseAJourSynchrone();

                }
                if (item == "-maj")
                {
                    Console.WriteLine("  Effectuer une mise à jour du fichier d’index et synchronise le répertoire destination");

                }
                if (item == "-index")
                {
                    Console.WriteLine("-index cheminRépertoireSource cheminRépertoireDestination Effectuer l’indexation du répertoire source et génère lefichier d’index à sa racine.");

                }

            }
            if (args.Length == 0)
                    {
                        Console.WriteLine("Usage : GestionSystemeDeFichier [options]",
                                          "     Options:",
                                          "     -aide Afficher aide",
                                          "     - index cheminRépertoireSource cheminRépertoireDestination",
                                          "     Effectuer l’indexation du répertoire source et génère le",
                                          "     fichier d’index à sa racine.",
                                          "     - maj cheminRépertoireSource Effectuer une mise à jour du fichier",
                                          "     d’index et synchronise le répertoire destination");
            }


            string initialPath = "C:\\Dossier"; // dossier origine
            string destinationPath = "C:\\Dossier"; // dossier destination
            FichierTraités = new List<InformationFichierIndexé>();
            ExplorateurDeRépertoire d = new ExplorateurDeRépertoire("C:\\Dossier");
            //d.ExplorationSynchrone(MonTraitementParticulierDeFichier);
            EnregistrerLeCatalogue(FichierTraités);
            
        }



        static public void EnregistrerLeCatalogue(List<InformationFichierIndexé> catalogue)
        {
            FileStream writeFileStream = null;
            if (File.Exists(@"C:\Dossier\catalogue.xml.txt"))
            {
                writeFileStream = new FileStream(@"C:\Dossier\catalogue.xml.txt", FileMode.Truncate);
            }
            else
            {
                writeFileStream = new FileStream(@"C:\Dossier\catalogue.xml.txt", FileMode.Create);
            }
            using (writeFileStream)
            {
                XmlSerializer s = new XmlSerializer(typeof(List<InformationFichierIndexé>));
                s.Serialize(XmlWriter.Create(writeFileStream), catalogue);
                writeFileStream.Flush();
            }
        }


        static public List<InformationFichierIndexé> ChargerLeCataloguePrécédent()
        {
            List<InformationFichierIndexé> catalogue = null;
            if (File.Exists(@"C:\Dossier\catalogue.xml.txt"))
            {
                FileStream fileStream = new FileStream(@"C:\Dossier\catalogue.xml.txt", FileMode.Open);
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    XmlSerializer s = new
                   XmlSerializer(typeof(List<InformationFichierIndexé>));
                    catalogue = (List<InformationFichierIndexé>)s.Deserialize(reader);
                }
            }
            return catalogue;
        }

    }
}

//public void EffectuerMiseAJourSynchrone()
//{
//    /*
//     * Exemple pour le using System.Linq
//     * IEnumerable<InformationFichierIndexé> tousLesFichiers = NouvelleListeDesFichiersIndexés;

//    int tailleMax = 2;

//    IEnumerable<InformationFichierIndexé> resultatWhere = tousLesFichiers.Where(FichierVide);
//  //   IEnumerable<InformationFichierIndexé> resultatWhere = tousLesFichiers.Where(infofichier => infofichier.Taille > tailleMax);

//    IEnumerable<string> cheminDesfichiers = resultatWhere.Select(unfichier => unfichier.CheminRelatif);

//    List<string> resul = cheminDesfichiers.ToList();
//   // string s = "azerty";

//    //var v =  MemoryExtensions.AsMemory(s);
//    // s.AsMemory();

//*/
//    //Mise à jour pour ajouter dans la synchro
//    foreach (InformationFichierIndexé infoFichier in NouvelleListeDesFichiersIndexés)
//    {
//        // InformationFichierIndexé fichierDéjàIndexé = _informationSynchronisation.FichiersIndexés.Where(i => i.CheminRelatif == infoFichier.CheminRelatif).FirstOrDefault();

//        foreach (InformationFichierIndexé infoSynchro in _informationSynchronisation.FichiersIndexés)
//        {
//            if (infoFichier.CheminRelatif == infoSynchro.CheminRelatif)
//            {
//                if (infoFichier.DernièreModification != infoSynchro.DernièreModification || infoFichier.Taille != infoSynchro.Taille)
//                {
//                    File.Copy(Path.Combine(_répertoirOrigine, infoFichier.CheminRelatif), Path.Combine(_répertoireDestination, infoSynchro.CheminRelatif), true);
//                }
//            }

//            //InformationFichierIndexé fichierDéjàIndexé2 = _informationSynchronisation.FichiersIndexés.Where(i => i.CheminRelatif != infoFichier.CheminRelatif).FirstOrDefault();

//            // if(fichierDéjàIndexé2 !=null)
//            else
//            {
//                if (!File.Exists(Path.Combine(_répertoireDestination, infoSynchro.CheminRelatif)))
//                {
//                    File.Copy(Path.Combine(_répertoirOrigine, infoFichier.CheminRelatif), Path.Combine(_répertoireDestination, infoSynchro.CheminRelatif));
//                }
//            }

//            //}
//        }
//    }

//    //Mise à jour pour supprimer dans la synchro
//    foreach (InformationFichierIndexé infoSynchro in _informationSynchronisation.FichiersIndexés)
//    {


//        /*
//         * 
//         * Essais Linq
//        InformationFichierComparer comparateurMêmeEmplacement = new InformationFichierComparer();
//        IEnumerable<InformationFichierIndexé> fichierSupprimés = _informationSynchronisation.FichiersIndexés.Except(NouvelleListeDesFichiersIndexés, comparateurMêmeEmplacement);
//       List<InformationFichierIndexé> listeFichiersSupprimés = fichierSupprimés.ToList();
//        foreach (InformationFichierIndexé infoFichier in listeFichiersSupprimés)
//        {
//            File.Delete(Path.Combine(_répertoireDestination, infoSynchro.CheminRelatif));
//        }
//        */


//        bool sup = true;
//        foreach (InformationFichierIndexé infoFichier in NouvelleListeDesFichiersIndexés)
//        {

//            if (infoSynchro.CheminRelatif == infoFichier.CheminRelatif)
//            {
//                sup = false;
//            }
//        }
//        if (sup)
//        {
//            File.Delete(Path.Combine(_répertoireDestination, infoSynchro.CheminRelatif));
//        }
//    }
//    _informationSynchronisation.FichiersIndexés = NouvelleListeDesFichiersIndexés;
//    EnregisterLesInformationsDeSynchronisation();

//}
