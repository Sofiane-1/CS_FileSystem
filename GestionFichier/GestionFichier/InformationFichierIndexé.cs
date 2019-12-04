using System;
using System.IO;

namespace GestionFichier
{
    public class InformationFichierIndexé
    {
        public string CheminRelatif { get; set; }
        public long Taille { get; set; }
        public DateTime DernièreModification { get; set; }

        public InformationFichierIndexé() { }
        public InformationFichierIndexé(string Chemin, long taille, DateTime dernière)
        {
            CheminRelatif = Chemin;
            Taille = taille;
            DernièreModification = dernière;
        }

    }
}
