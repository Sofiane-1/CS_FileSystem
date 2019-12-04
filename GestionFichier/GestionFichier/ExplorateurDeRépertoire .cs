using System;
using System.IO;

namespace GestionFichier
{
    public class ExplorateurDeRépertoire
    {

        private string répertoireInitial;


        public ExplorateurDeRépertoire(string répertoireInital)
        {
            répertoireInitial = répertoireInital;
            
        }

        private void ExplorerRépertoire(string répertoireCourant, TraitementDeFichier traitementDeFichier)
        {
            long pds = 0;
            foreach (string d in Directory.GetDirectories(répertoireCourant))
            {
                FileAttributes attributes = File.GetAttributes(d);
                if ((attributes & FileAttributes.Hidden) != FileAttributes.Hidden && (attributes & FileAttributes.System) != FileAttributes.System) { 
                    ExplorerRépertoire(d, traitementDeFichier);
                }
            }

            foreach (string f in Directory.GetFiles(répertoireCourant))
                {
                
                    FileAttributes attributes = File.GetAttributes(f);
                    if ((attributes & FileAttributes.Hidden) != FileAttributes.Hidden && (attributes & FileAttributes.System) != FileAttributes.System)
                     {
                        FileInfo fi = new FileInfo(f);
                        pds += fi.Length;
                    traitementDeFichier(f);

                    }

                }
            Console.WriteLine(répertoireCourant + " " + pds);
        }

        public void ExplorationSynchrone(TraitementDeFichier traitementDeFichier)
        {
            ExplorerRépertoire(répertoireInitial, traitementDeFichier);
        }



    }

    public delegate void TraitementDeFichier(string fileEntry);
}
