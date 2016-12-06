using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Devices.Geolocation;
using Windows.Media.SpeechRecognition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
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
    public sealed partial class DynamicEval : Page
    {
        private MainPage rootPage;
        private SpeechRecognizer speechRecognizer;

        public DynamicEval()
        {
            this.InitializeComponent();
            MyMap.Loaded += MyMap_Loaded;
            //myMap.MapTapped += MyMap_MapTapped;
        }


        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            rootPage = MainPage.Current;

            //create and initialize new SpeechRecognizer
            speechRecognizer = new SpeechRecognizer();
            await speechRecognizer.CompileConstraintsAsync();
        }

        private async void StartVoiceRecognition_Click(object sender, RoutedEventArgs e)
        {
            string textBoxText = textBox_comment.Text;

            //Start Recognition
            Windows.Media.SpeechRecognition.SpeechRecognitionResult speechRecognitionResult = await speechRecognizer.RecognizeWithUIAsync();

            //Put recognition result in commentTextBlock
            textBox_comment.Text = textBoxText + " " + speechRecognitionResult.Text;
        }

        private void MyMap_Loaded(object sender, RoutedEventArgs e)
        {
            MyMap.Center =
                new Geopoint(new BasicGeoposition()
                {
                    //Geopoint for NTCNA test track
                    Latitude = 32.9492892,
                    Longitude = -111.9956382
                });
            MyMap.ZoomLevel = 15;
        }

    }
}
