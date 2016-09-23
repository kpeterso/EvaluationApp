using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;

namespace EvaluationApp
{
    class Survey
    {
        public ObservableCollection<surveyQuestion> surveyQuestionList;
        public string surveyName;

        public Survey()
        {   
        }

        public async void initSurvey(string folderName, string fileName)
        {
            XmlDocument doc= await LoadXmlFile(folderName, fileName);

            this.surveyName = doc.SelectSingleNode("descendant::surveyname").InnerText;

            surveyQuestionList = new ObservableCollection<surveyQuestion>();
            var questions = doc.SelectSingleNode("descendant::questions").SelectNodes("descendant::question");
            foreach (var q in questions)
            {
                string t = q.SelectSingleNode("descendant::text").InnerText;
                string id = q.SelectSingleNode("descendant::id").InnerText;
                surveyQuestion a = new surveyQuestion { questionID = Convert.ToInt16(id), questionText = t };
                surveyQuestionList.Add(a);
            }
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
    }

    public class surveyQuestion
    {
        public string questionText { get; set; }
        public int questionID { get; set; }
    }