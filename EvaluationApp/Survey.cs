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
        public static XmlDocument surveyDoc { get; set; }
        public static ObservableCollection<Survey> surveyList = new ObservableCollection<Survey>();

        public string surveyName;
        public ObservableCollection<surveyQuestion> surveyQuestionList;

        public Survey(IXmlNode s)
        {
            this.surveyName = s.SelectSingleNode("descendant::surveyname").InnerText;

            surveyQuestionList = new ObservableCollection<surveyQuestion>();
            var questions = s.SelectSingleNode("descendant::questions").SelectNodes("descendant::question");
            foreach (var q in questions)
            {
                string t = q.SelectSingleNode("descendant::text").InnerText;
                string id = q.SelectSingleNode("descendant::id").InnerText;
                surveyQuestion a = new surveyQuestion { questionID = Convert.ToInt16(id), questionText = t };
                surveyQuestionList.Add(a);
            }
        }

        public static Survey findSurvey(string name)
        {
            foreach(Survey survey in surveyList)
            {
                if(survey.surveyName == name)
                {
                    return survey;
                }
            }
            return null;
        }
    }

    public class surveyQuestion
    {
        public string questionText { get; set; }
        public int questionID { get; set; }

        public override string ToString() { return questionText; }
    }
}