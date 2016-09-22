using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Resources.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.SpeechRecognition;
using Windows.UI.Core;
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
    public sealed partial class StaticEval : Page
    {
        //Variables used for continuous dictation, not needed for short bursts
        //private CoreDispatcher dispatcher;
        //private ResourceContext speechContext;
        //private ResourceMap speechResourceMap;

        //Define page vars
        private SpeechRecognizer speechRecognizer;
        private ObservableCollection<Observation> oList;
        private Evaluation evaluation;
        private MainPage rootPage;

        public StaticEval()
        {
            this.InitializeComponent();
            oList = new ObservableCollection<Observation>();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            rootPage = MainPage.Current;

            //dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;
            speechRecognizer = new SpeechRecognizer();

            //Compile the dictation grammar by default
            await speechRecognizer.CompileConstraintsAsync();

            //SpeechRecognitionCompilationResult result = await speechRecognizer.CompileConstraintsAsync();
            //speechRecognizer.ContinuousRecognitionSession.ResultGenerated += ContinuousRecognitionSession_ResultGenerated;
            //speechRecognizer.ContinuousRecognitionSession.Completed += ContinuousRecognitionSession_Completed;
        }

        private async void StartVoiceRecognition_Click(object sender, RoutedEventArgs e)
        {
            string textBoxText = textBox_comment.Text;

            //Start Recognition
            Windows.Media.SpeechRecognition.SpeechRecognitionResult speechRecognitionResult = await speechRecognizer.RecognizeWithUIAsync();

            //Put recognition result in commentTextBlock
            textBox_comment.Text = textBoxText + " " + speechRecognitionResult.Text;
        }

        private void SubmitObservationButton_Click(object sender, RoutedEventArgs e)
        {
            var comment = textBox_comment.Text;
            string timestamp = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.ff");
            Observation obs = new EvaluationApp.Observation() { comment=comment, timestamp=timestamp };
            oList.Add(obs);
            textBox_comment.Text = "";
        }

        private void button_EndEval_Click(object sender, RoutedEventArgs e)
        {
            string driver = textBox_name.Text;
            string vehicle = textBlock_MakeText.Text + " " + textBlock_ModelText.Text;
            string type = "Static Evaluation";
            string ts = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.ff");

            evaluation = new Evaluation(driver, vehicle, type, ts, oList);
            Evaluation.evaluationList.Add(evaluation);

            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.GoBack();

        }

        /*These functions are for Continuous Dictation speech recognition, not needed for short sentances        
                private async void ContinuousRecognitionSession_ResultGenerated(SpeechContinuousRecognitionSession sender, SpeechContinuousRecognitionResultGeneratedEventArgs e)
                {
                    if(e.Result.Confidence == SpeechRecognitionConfidence.Medium || e.Result.Confidence == SpeechRecognitionConfidence.High)
                    {
                        dictationTextBuilder.Append(e.Result.Text + " ");

                        await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            dictationTextBox.Text = dictationTextBuilder.ToString();
                            btnClearText.IsEnabled = true;
                        });
                    }
                    else
                    {
                        await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            dictationTextBox.Text = dictatedTextBuilder.ToString();
                        });
                    }
                }

                private async void ContinuousRecognitionSession_Completed(SpeechContinuousRecognitionSession sender, SpeechContinuousRecognitionCompletedEventArgs e)
                {
                    if(e.Status != SpeechRecognitionResultStatus.Success)
                    {
                        if(e.Status == SpeechRecognitionResultStatus.TimeoutExceeded)
                        {
                            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                            {
                                DictationButtonText.Text = "Continuous Recognition"
                                dictationTextBox.Text = dictationTextBuilder.ToString();
                            });
                        }
                        else
                        {
                            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                            {
                                DictationButtonText.Text = "Continuous Recognition";
                            });
                        }
                    }
                }

                private async void SpeechRecognizer_HypothesisGenerated(SpeechRecognizer sender, SpeechRecognitionHypothesisGeneratedEventArgs e)
                {
                    string hypothesis = e.Hypothesis.Text;
                    string textboxContent = dictatedTextBuilder.ToString() + " " + hypothesis + " ...";

                    await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        dictationTextBox.Text = textboxContent;
                        btnClearText.IsEnabled = true;
                    });
                }*/
    }
}
