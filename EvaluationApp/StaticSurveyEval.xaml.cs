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
    public sealed partial class StaticSurveyEval : Page
    {
        private MainPage rootPage;

        private string ID;
        private Evaluation evaluation;
        private ObservableCollection<Observation> oList;
        private string surveyName;

        ObservableCollection<surveyQuestion> questionList;

        public StaticSurveyEval()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            rootPage = MainPage.Current;
            if (e.Parameter is string)
            {
                ID = e.Parameter.ToString();

                loadData();
                initSurvey();
            }
        }

        private void initSurvey()
        {
            Survey survey = Survey.findSurvey(surveyName);
            questionList = new ObservableCollection<surveyQuestion>(survey.surveyQuestionList);
        }


        private void listBox_surveyQuestions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedResponse = evaluation.surveyResponseList.ElementAt(listBox_surveyQuestions.SelectedIndex);
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
            surveyName = evaluation.surveyName;
        }
    }
}
