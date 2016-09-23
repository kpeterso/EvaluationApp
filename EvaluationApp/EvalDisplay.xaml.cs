using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Data.Xml.Dom;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Core;
using System.Collections.ObjectModel;

namespace EvaluationApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EvalDisplay : Page
    {
        private MainPage rootPage;
        private ObservableCollection<Evaluation> Evaluations = new ObservableCollection<Evaluation>();
        

        public EvalDisplay()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Disabled;

            EvalDisplayInit();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            rootPage = MainPage.Current;
        }

        void EvalDisplayInit()
        {
            Evaluations = new ObservableCollection<Evaluation>(Evaluation.evaluationList);
        }

        private void button_openSummary_Click(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            //textBlock.Text = listBoxEval.SelectedItem.ToString();
            var item = ((Evaluation)listBoxEval.SelectedItem).evalID;
            switch (listBoxEval.SelectedItem.ToString())
            {
                case "Static Evaluation":
                    rootFrame.Navigate(typeof(StaticEvalSummary), item);
                    break;
                case "Static Survey Evaluation":
                    rootFrame.Navigate(typeof(StaticSurveyEvalSummary), item);
                    break;
                case "Route Evaluation":
                    break;
            }
        }
    }
}
