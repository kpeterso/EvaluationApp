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
        private ObservableCollection<SurveyResponse> surveyResponseList;
        private string surveyName;
        private int currentQuestion=0;

        ObservableCollection<surveyQuestion> questionList;

        public StaticSurveyEval()
        {
            this.InitializeComponent();
            surveyResponseList = new ObservableCollection<SurveyResponse>();
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
            //TODO: Add survey selection screen, access survey name as parameter
            surveyName = "Test Survey";
            Survey survey = Survey.findSurvey(surveyName);
            questionList = new ObservableCollection<surveyQuestion>(survey.surveyQuestionList);
            for (int i = 0; i < questionList.Count; i++)
            {
                SurveyResponse s = new SurveyResponse();
                s.questionNumber = i;
                s.comment = "";
                s.rating = 0;
                surveyResponseList.Add(s);
            }

            updateScreen();
        }

        private void listBox_surveyQuestions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            updateCurrentResponse();
            currentQuestion = listBox_surveyQuestions.SelectedIndex;
            updateScreen();
            
            //textBlock_ratingText.Text = selectedResponse.rating.ToString();
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            updateCurrentResponse();

            currentQuestion++;
            if (currentQuestion == questionList.Count)
            {
                currentQuestion = 0;
            }

            updateScreen();
            listBox_surveyQuestions.SelectedIndex = currentQuestion;
        }

        private void button_EndEval_Click(object sender, RoutedEventArgs e)
        {
            string driver = textBox_name.Text;
            string vehicle = textBlock_MakeText.Text + " " + textBlock_ModelText.Text;
            string type = "Static Survey Evaluation";
            string ts = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.ff");

            Evaluation evaluation = new Evaluation(driver, vehicle, type, "Test Survey", ts, surveyResponseList);
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

        private void updateCurrentResponse()
        {
            //Save text in textBox_comment into surveyResponse array
            string timestamp = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.ff");
            surveyResponseList.ElementAt(currentQuestion).comment = textBox_comment.Text;
            //surveyResponseList.ElementAt(currentQuestion).rating = Pivot_Thumbs.SelectedIndex;
            surveyResponseList.ElementAt(currentQuestion).timestamp = timestamp;
        }

        private void updateScreen()
        {
            //updateScreen screen elements to reflect current question
            SurveyQuestion_TextBlock.Text = questionList.ElementAt(currentQuestion).questionText;
            textBox_comment.Text = surveyResponseList.ElementAt(currentQuestion).comment;
            Pivot_Thumbs.SelectedIndex = surveyResponseList.ElementAt(currentQuestion).rating;
        }

        private void Pivot_Thumbs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            surveyResponseList.ElementAt(currentQuestion).rating = Pivot_Thumbs.SelectedIndex;
        }
    }
}
