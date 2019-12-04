using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace GestionFichier
{
    public class SynchroniseurDeRépertoire
    {
        #region Variables

        private string _emplacementDuFichierIndex;
        private IndexeurDeFichier _indexeur;
        public InformationsSynchronisation _informationSynchronisation;
        private string _répertoirOrigine;
        private string _répertoireDestination;

        #endregion

        public List<InformationFichierIndexé> NouvelleListeDesFichiersIndexés { get => _indexeur.FichierTraités; }




        public SynchroniseurDeRépertoire(string répertoireOrigine, string répertoireDestination)
        {
            _répertoirOrigine = répertoireOrigine;
            _répertoireDestination = répertoireDestination;
            _indexeur = new IndexeurDeFichier(répertoireOrigine);
            _emplacementDuFichierIndex = Path.Combine(répertoireOrigine.ToString(), "Synchro.index");

            if (!ChargerLesInformationsDeSynchronisation())
            {
                _informationSynchronisation = new InformationsSynchronisation(répertoireDestination);
            }

        }


        public void EffectuerIndexationSynchrone()
        {
            _indexeur.EffectuerIndexationSynchrone();
        }

        public void EnregistrerLesInformationsDeSynchronisation()
        {
            FileStream writeFileStream = null;
            if (File.Exists(_emplacementDuFichierIndex))
            {
                writeFileStream = new FileStream(_emplacementDuFichierIndex, FileMode.Truncate);
            }
            else
            {
                writeFileStream = new FileStream(_emplacementDuFichierIndex, FileMode.Create);
            }
            using (writeFileStream)
            {
                XmlSerializer s = new XmlSerializer(typeof(InformationsSynchronisation));
                s.Serialize(XmlWriter.Create(writeFileStream), _informationSynchronisation);
                writeFileStream.Flush();
            }
            FileAttributes attributs = File.GetAttributes(_emplacementDuFichierIndex);
            attributs = attributs | FileAttributes.Hidden;
            File.SetAttributes(_emplacementDuFichierIndex, attributs);
        }

        public bool ChargerLesInformationsDeSynchronisation()
        {

            if (File.Exists(_emplacementDuFichierIndex))
            {

                FileStream fileStream = new FileStream(_emplacementDuFichierIndex, FileMode.Open);
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    XmlSerializer s = new XmlSerializer(typeof(List<InformationFichierIndexé>));
                    _informationSynchronisation = (InformationsSynchronisation)s.Deserialize(reader);
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        //public void EffectuerMiseAJourSynchrone()
        //{ Méthode à revoir

        //    foreach (var item in NouvelleListeDesFichiersIndexés)
        //    {
        //        bool present = false;
        //        InformationFichierIndexé save = null;
        //        foreach (var comp in _informationSynchronisation.FichiersIndexés)
        //        {
        //            if (comp.CheminRelatif.Equals(item.CheminRelatif))
        //            {
        //                present = true;
        //                save = comp;
        //            }
        //        }

        //    }


        //}

        public void EffectuerMiseAJourSynchrone()
        {

            foreach (InformationFichierIndexé infoFichier in  NouvelleListeDesFichiersIndexés)
            {
                foreach (InformationFichierIndexé infoSynchro in  _informationSynchronisation.FichiersIndexés)
                {
                    if (infoFichier.CheminRelatif == infoSynchro.CheminRelatif)
                    {
                        if (infoFichier.DernièreModification != infoSynchro.DernièreModification || infoFichier.Taille != infoSynchro.Taille)
                        {
                            File.Copy(Path.Combine(_répertoirOrigine, infoFichier.CheminRelatif), Path.Combine(_répertoireDestination, infoSynchro.CheminRelatif), true);
                        }
                    }
                    else
                    {
                        if (!File.Exists(Path.Combine(_répertoireDestination, infoSynchro.CheminRelatif)))
                        {
                            File.Copy(Path.Combine(_répertoirOrigine, infoFichier.CheminRelatif), Path.Combine(_répertoireDestination, infoSynchro.CheminRelatif));
                        }
                    }
                }
            }


            foreach (InformationFichierIndexé infoSynchro in _informationSynchronisation.FichiersIndexés)
            {
                bool test = true;
                foreach (InformationFichierIndexé infoFichier in NouvelleListeDesFichiersIndexés)
                {

                    if (infoSynchro.CheminRelatif == infoFichier.CheminRelatif)
                    {
                        test = false;
                    }
                }
                if (test)
                {
                    File.Delete(Path.Combine(_répertoireDestination, infoSynchro.CheminRelatif));
                }
            }
            _informationSynchronisation.FichiersIndexés = NouvelleListeDesFichiersIndexés;
            EnregistrerLesInformationsDeSynchronisation();

        }


    }
}
