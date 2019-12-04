using System;
using System.Collections.Generic;
using System.IO;

namespace GestionFichier
{
    public class InformationsSynchronisation
    {

        public DateTime DernièreSynchronisation { get; set; }

        public string RépertoireDestination { get; set; }

        public List<InformationFichierIndexé> FichiersIndexés{ get; set;}
        public InformationsSynchronisation()
        {
            RépertoireDestination = "";
            DernièreSynchronisation = DateTime.Now;
            FichiersIndexés = new List<InformationFichierIndexé>();
        }

        public InformationsSynchronisation(string répertoireDestination) : this()
        {
            RépertoireDestination = répertoireDestination;
        }
        


    }
}
