using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;

namespace EvaluationApp
{
    public class Evaluation
    {
        public static Windows.Data.Xml.Dom.XmlDocument evaluationDoc { get; set; }
        public static ObservableCollection<Evaluation> evaluationList = new ObservableCollection<Evaluation>();
        private static int maxID { get; set; }

        public string evalID { get; set; }
        public string driverName { get; set; }
        public string vehicleName { get; set; }
        public string evalType { get; set; }    //Name of Evaluation Template
        public string submitDate { get; set; }  //Date the evaluation was submitted
        public ObservableCollection<Observation> observationList;   //Stores list of Observations
        //public List<gpsTag> gpsRoute; //Store list of gps tags that made up route

        public Evaluation(IXmlNode evaluation)
        {
            //imports the XML data into the Evaluation object that calls it.
            this.evalID = evaluation.SelectSingleNode("descendant::evalID").InnerText;
            this.driverName = evaluation.SelectSingleNode("descendant::driverName").InnerText;
            this.vehicleName = evaluation.SelectSingleNode("descendant::vehicleName").InnerText;
            this.evalType = evaluation.SelectSingleNode("descendant::evalType").InnerText;
            this.submitDate = evaluation.SelectSingleNode("descendant::submitdate").InnerText;

            observationList = new ObservableCollection<Observation>();
            var obs = evaluation.SelectNodes("descendant::observations");
            foreach (var ob in obs)
            {
                string c = ob.SelectSingleNode("descendant::comment").InnerText;
                string ts = ob.SelectSingleNode("descendant::timestamp").InnerText;
                Observation observation = new Observation { comment = c, timestamp = ts };
                observationList.Add(observation);
            }

            int ID = Convert.ToInt32(this.evalID);
            if (ID > maxID)
            {
                maxID = ID;
            }
        }

        public Evaluation(string driver, string vehicle, string type, string timestamp, ObservableCollection<Observation> obsList)
        {
            this.driverName = driver;
            this.vehicleName = vehicle;
            this.evalType = type;
            this.submitDate = timestamp;
            this.evalID = maxID.ToString();
            maxID++;

            observationList = new ObservableCollection<Observation>(obsList);
        }

        public void addObservation(Observation obs)
        {
            this.observationList.Add(obs);
        }

        public static async Task<XmlDocument> LoadXmlFile(String folder, String file)
        {
            //opens an XML file and returns an XmlDocument object
            Windows.Storage.StorageFolder storageFolder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(folder);
            Windows.Storage.StorageFile storageFile = await storageFolder.GetFileAsync(file);
            XmlLoadSettings loadSettings = new Windows.Data.Xml.Dom.XmlLoadSettings();
            loadSettings.ProhibitDtd = false;
            loadSettings.ResolveExternals = false;
            return await XmlDocument.LoadFromFileAsync(storageFile, loadSettings);
        }

        public void AddEvaluation(Evaluation eval)
        {
            //Create the Evaluation element
            XmlElement newEval = evaluationDoc.CreateElement("Evaluation");

            //Add class attributes to Evaluation element
            XmlElement element = evaluationDoc.CreateElement("driverName");
            element.InnerText = eval.driverName;
            newEval.AppendChild(element);

            element = evaluationDoc.CreateElement("vehicleName");
            element.InnerText = eval.vehicleName;
            newEval.AppendChild(element);

            element = evaluationDoc.CreateElement("evalType");
            element.InnerText = eval.evalType;
            newEval.AppendChild(element);

            element = evaluationDoc.CreateElement("timestamp");
            element.InnerText = eval.submitDate;
            newEval.AppendChild(element);
        }

        public override string ToString()
        {
            return evalType;
        }
    }

    public class Observation
    {
        public string comment { get; set; }
        public string timestamp { get; set; }
        //public gpsTag geoTag; //Store location observation was made
        //public List<string> photoLinks;   //Store links to photo files in memory
        //public List<string> videoLinks;   //Store links to video files in memory

    }


}
