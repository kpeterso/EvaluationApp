using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace EvaluationApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StaticSurveyEvalSummary : Page
    {
        private MainPage rootPage;

        private string ID;
        private Evaluation evaluation;
        private ObservableCollection<Observation> oList;

        private Survey survey;
        ObservableCollection<surveyQuestion> questionList;

        public StaticSurveyEvalSummary()
        {
            this.InitializeComponent();
            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            rootPage = MainPage.Current;
            if (e.Parameter is string)
            {
                ID = e.Parameter.ToString();
                initSurvey();
                loadData();
            }
        }

        private async void initSurvey()
        {
            try
            {
                Windows.Data.Xml.Dom.XmlDocument doc = await Evaluation.LoadXmlFile("Surveys", "Survey1.xml");
                survey = new Survey(doc);
                questionList = new ObservableCollection<surveyQuestion>(survey.surveyQuestionList);
            }
            catch (Exception exp)
            {
                throw;
            }
        }

        private void listBox_surveyQuestions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedResponse=evaluation.surveyResponseList.ElementAt(listBox_surveyQuestions.SelectedIndex);
            textBlock_commentText.Text = selectedResponse.comment;
            textBlock_ratingText.Text = selectedResponse.rating.ToString();
        }

        private void loadData()
        {
            foreach (Evaluation eval in Evaluation.evaluationList)
            {
                if (eval.evalID == ID.ToString())
                {
                    evaluation = new Evaluation(eval);
                    break;
                }
            }
            oList = new ObservableCollection<Observation>(evaluation.observationList);
        }
    }
}
