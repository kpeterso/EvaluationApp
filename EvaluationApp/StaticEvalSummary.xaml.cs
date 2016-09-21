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
using System.Collections.ObjectModel;
using Windows.UI.Core;

namespace EvaluationApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StaticEvalSummary : Page
    {
        private MainPage rootPage;
        string ID;
        private Evaluation evaluation;
        private ObservableCollection<Observation> observationList = new ObservableCollection<Observation>();

        public StaticEvalSummary()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            rootPage = MainPage.Current;

            if (e.Parameter is string)
            {
                ID = e.Parameter.ToString();
                loadData();
            }
        }

        async void loadData()
        {
            var s = "descendant::evaluation[evalID = " + ID + "]";
            try
            {
                XmlDocument doc = await Evaluation.LoadXmlFile("EvaluationXml", "Evaluations.xml");
                var evaluations = doc.SelectSingleNode(s);
                evaluation = new Evaluation(evaluations);
                foreach (var item in evaluation.observationList)
                {
                    observationList.Add(item);
                    //observationList = new ObservableCollection<Observation>(evaluation.observationList);
                }
            }
            catch (Exception exp)
            {
                //rootPage.NotifyUser("Exception occured while loading xml file!", MainPage.NotifyType.ErrorMessage);
            }
        }
    }
}
