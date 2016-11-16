using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.SpeechRecognition;
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

        private SpeechRecognizer speechRecognizer;
        private ObservableCollection<Observation> oList;
        private string surveyName;

        ObservableCollection<surveyQuestion> questionList;

        public StaticSurveyEval()
        {
            this.InitializeComponent();
            oList = new ObservableCollection<Observation>();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            rootPage = MainPage.Current;

            initSurvey();

            //dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;
            speechRecognizer = new SpeechRecognizer();

            //Compile the dictation grammar by default
            await speechRecognizer.CompileConstraintsAsync();
        }

        private void initSurvey()
        {
            surveyName = "Test Survey";
            Survey survey = Survey.findSurvey(surveyName);
            questionList = new ObservableCollection<surveyQuestion>(survey.surveyQuestionList);
        }


        private void listBox_surveyQuestions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedResponse = evaluation.surveyResponseList.ElementAt(listBox_surveyQuestions.SelectedIndex);
            textBox_comment.Text = selectedResponse.comment;
            //textBlock_ratingText.Text = selectedResponse.rating.ToString();
        }

        private void SubmitObservationButton_Click(object sender, RoutedEventArgs e)
        {
            var comment = textBox_comment.Text;
            string timestamp = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.ff");
            Observation obs = new EvaluationApp.Observation() { comment = comment, timestamp = timestamp };
            oList.Add(obs);
            textBox_comment.Text = "";
        }

        private void button_EndEval_Click(object sender, RoutedEventArgs e)
        {
            string driver = textBox_name.Text;
            string vehicle = textBlock_MakeText.Text + " " + textBlock_ModelText.Text;
            string type = "Static Evaluation";
            string ts = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.ff");

            evaluation = new Evaluation(driver, vehicle, type, null, ts, oList);
            Evaluation.evaluationList.Add(evaluation);

            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.GoBack();
        }

        private async void StartVoiceRecognition_Click(object sender, RoutedEventArgs e)
        {
            string textBoxText = textBox_comment.Text;

            //Start Recognition
            Windows.Media.SpeechRecognition.SpeechRecognitionResult speechRecognitionResult = await speechRecognizer.RecognizeWithUIAsync();

            //Put recognition result in commentTextBlock
            textBox_comment.Text = textBoxText + " " + speechRecognitionResult.Text;
        }
    }
}
