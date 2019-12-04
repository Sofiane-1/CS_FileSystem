using System;
using System.Collections.Generic;
using System.IO;

namespace GestionFichier
{
    public class IndexeurDeFichier
    {
        private ExplorateurDeRépertoire _explorateur;
        private int _nbCaractèresRépertoireRacine;
        public List<InformationFichierIndexé> FichierTraités { get; private set; }

        public IndexeurDeFichier(string répertoireAIndexer)
        {
            _explorateur = new ExplorateurDeRépertoire(répertoireAIndexer); 
            _nbCaractèresRépertoireRacine = répertoireAIndexer.Length + 1;
            FichierTraités = new List<InformationFichierIndexé>();
        }

        public  void MéthodeIndexationDeFichier(string unCheminDeFichier)
        {
            // FileInfo fi = new FileInfo(unCheminDeFichier);
            //Console.WriteLine(unCheminDeFichier);
            //string CheminRelatif = unCheminDeFichier.Substring(_nbCaractèresRépertoireRacine);

            //FileInfo fichier = new FileInfo(CheminRelatif);
            //long fichierpds = fichier.Length;
            //DateTime date = File.GetLastWriteTime(CheminRelatif);
            //FichierTraités.Add(new InformationFichierIndexé(CheminRelatif, fichierpds, date));

            InformationFichierIndexé info = new InformationFichierIndexé();
            FileInfo fi = new FileInfo(unCheminDeFichier);
            info.CheminRelatif = unCheminDeFichier.Substring(_nbCaractèresRépertoireRacine);
            info.Taille = fi.Length;
            info.DernièreModification = File.GetLastWriteTime(unCheminDeFichier);
            FichierTraités.Add(info);
        }

        public void EffectuerIndexationSynchrone()
        {
            _explorateur.ExplorationSynchrone(MéthodeIndexationDeFichier);
        }

    }
}
