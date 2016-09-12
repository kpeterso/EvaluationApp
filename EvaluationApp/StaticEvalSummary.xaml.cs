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
using Windows.Data.Xml.Dom;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace EvaluationApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StaticEvalSummary : Page
    {
        private MainPage rootPage;
        string ID;
        Evaluation evaluation;

        public StaticEvalSummary()
        {
            this.InitializeComponent();

            loadData();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if(e.Parameter is string)
            {
                rootPage = MainPage.Current;
                ID = e.Parameter.ToString();
            }
        }

        async void loadData()
        {
            try
            {
                XmlDocument doc = await Evaluation.LoadXmlFile("EvaluationXml", "Evaluations.xml");
                var evaluations = doc.SelectNodes("descendant::evaluation[evalID = " + ID);
                evaluation = new Evaluation(evaluations[0]);
            }
            catch (Exception exp)
            {
                rootPage.NotifyUser("Exception occured while loading xml file!", MainPage.NotifyType.ErrorMessage);
            }
        }
    }
}
