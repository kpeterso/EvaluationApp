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
            //ItemCollection items = listBoxEval.Items;
            try
            {
                XmlDocument doc = await Evaluation.LoadXmlFile("EvaluationXml", "Evaluations.xml");
                var evaluations = doc.SelectNodes("descendant::evaluation");
                foreach (IXmlNode evaluation in evaluations)
                {
                    Evaluation eval = new EvaluationApp.Evaluation(evaluation);
                    Evaluations.Add(eval);

                    //var item = new TextBlock();
                    //item.DataContext = eval;
                    //item.Text = eval.evalID + " " + eval.driverName + " " + eval.vehicleName + " " + eval.evalType;
                    //items.Add(item);

                }
            }
            catch (Exception exp)
            {
                rootPage.NotifyUser("Exception occured while loading xml file!", MainPage.NotifyType.ErrorMessage);
            }
 
        }
    }
}
