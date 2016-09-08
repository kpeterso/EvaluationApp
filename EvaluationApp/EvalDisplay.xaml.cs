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
    public sealed partial class EvalDisplay : Page
    {
        private MainPage rootPage;
        private List<Evaluation> Evaluations = new List<Evaluation>();

        public EvalDisplay()
        {
            this.InitializeComponent();

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

        async void EvalDisplayInit()
        {
            
            try
            {
                Windows.Data.Xml.Dom.XmlDocument doc = await Scenario.LoadXmlFile("EvaluationXml", "Evaluations.xml");
                var evaluations = doc.SelectNodes("descendant::evaluation");
                foreach (var evaluation in evaluations)
                {
                    string id = evaluation.SelectSingleNode("descendant::evalID").InnerText;
                    string driverName = evaluation.SelectSingleNode("descendant::driverName").InnerText;
                    string vehicleName = evaluation.SelectSingleNode("descendant::vehicleName").InnerText;
                    string evalType = evaluation.SelectSingleNode("descendant::evalType").InnerText;

                    Evaluation eval = new EvaluationApp.Evaluation(id, driverName, vehicleName, evalType);
                    Evaluations.Add(eval);

                    

                }
            }
            catch (Exception exp)
            {
                rootPage.NotifyUser("Exception occured while loading xml file!", MainPage.NotifyType.ErrorMessage);
            }

            listBoxEval.ItemsSource = Evaluations;
        }

        void EvalDisplay_LoadCompleted(object sender, Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            ItemCollection items = listBoxEval.Items;
            var item = new TextBlock();
            foreach (Evaluation eval in Evaluations)
            {
                item.DataContext = eval;
                item.Text = eval.evalID + eval.driverName + eval.vehicleName + eval.evalType;
                items.Add(item);
            }
            
        }
    }
}
