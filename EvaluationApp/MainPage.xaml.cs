using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class MainPage : Page
    {
        public static MainPage Current;

        List<Scenario> scenarios = new List<Scenario>
        {
            new Scenario() { Title = "Display List of Evaluations", ClassType = typeof(EvaluationApp.EvalDisplay) },
            new Scenario() { Title = "Create New Static Evaluation", ClassType = typeof(EvaluationApp.StaticEval) },
            new Scenario() { Title = "Create New Survey Evaluation", ClassType = typeof(EvaluationApp.StaticSurveyEval) },
            new Scenario() { Title = "Create Dynamic Survey", ClassType = typeof(EvaluationApp.DynamicEval) },
            // new Scenario() { Title = "XSLT Transformation", ClassType = typeof(EvaluationApp.XSLTTransform) }
        };

        public MainPage()
        {
            this.InitializeComponent();
            Current = this;
            //SampleTitle.Text = "Evaluation App";
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)

        {
            // Populate the scenario list from the SampleConfiguration.cs file
            ScenarioControl.ItemsSource = scenarios;
            ScenarioControl.SelectedIndex = -1;
        }

        /// <summary>
        /// Called whenever the user changes selection in the scenarios list.  This method will navigate to the respective
        /// sample scenario page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScenarioControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            // Clear the status block when navigating scenarios.
            //NotifyUser(String.Empty, NotifyType.StatusMessage);

            ListBox scenarioListBox = sender as ListBox;
            Scenario s = scenarioListBox.SelectedItem as Scenario;
            if (s != null)
            {
                rootFrame.Navigate(s.ClassType);
                /*if (Window.Current.Bounds.Width < 640)
                {
                    Splitter.IsPaneOpen = false;
                }*/
            }
        }

        public List<Scenario> Scenarios
        {
            get { return this.scenarios; }
        }

/*        /// <summary>
        /// Used to display messages to the user
        /// </summary>
        /// <param name="strMessage"></param>
        /// <param name="type"></param>
        public void NotifyUser(string strMessage, NotifyType type)
        {
            switch (type)
            {
                case NotifyType.StatusMessage:
                    StatusBorder.Background = new SolidColorBrush(Windows.UI.Colors.Green);
                    break;
                case NotifyType.ErrorMessage:
                    StatusBorder.Background = new SolidColorBrush(Windows.UI.Colors.Red);
                    break;
            }
            StatusBlock.Text = strMessage;

            // Collapse the StatusBlock if it has no text to conserve real estate.
            StatusBorder.Visibility = (StatusBlock.Text != String.Empty) ? Visibility.Visible : Visibility.Collapsed;
            if (StatusBlock.Text != String.Empty)
            {
                StatusBorder.Visibility = Visibility.Visible;
                StatusPanel.Visibility = Visibility.Visible;
            }
            else
            {
                StatusBorder.Visibility = Visibility.Collapsed;
                StatusPanel.Visibility = Visibility.Collapsed;
            }
        }
*/
        /*private void Button_Click(object sender, RoutedEventArgs e)
        {
            Splitter.IsPaneOpen = !Splitter.IsPaneOpen;
        }

        public enum NotifyType
        {
            StatusMessage,
            ErrorMessage
        };*/
    }

    public class ScenarioBindingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Scenario s = value as Scenario;
            return (MainPage.Current.Scenarios.IndexOf(s) + 1) + ") " + s.Title;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return true;
        }
    }
}
