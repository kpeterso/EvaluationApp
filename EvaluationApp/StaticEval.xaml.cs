using System;
using System.Collections.Generic;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace EvaluationApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StaticEval : Page
    {
        public StaticEval()
        {
            this.InitializeComponent();
        }

        private async void StartVoiceRecognition_Click(object sender, RoutedEventArgs e)
        {
            //Create instance of SpeechRecognizer
            var speechRecognizer = new Windows.Media.SpeechRecognition.SpeechRecognizer();

            //Compile the dictation grammar by default
            await speechRecognizer.CompileConstraintsAsync();

            //Start Recognition
            Windows.Media.SpeechRecognition.SpeechRecognitionResult speechRecognitionResult = await speechRecognizer.RecognizeWithUIAsync();

            //Put recognition result in commentTextBlock
            textBox_comment.Text = speechRecognitionResult.Text;
        }
    }
}
