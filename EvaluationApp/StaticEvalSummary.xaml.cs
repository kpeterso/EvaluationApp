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
        private ObservableCollection<Observation> oList;

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

        private void loadData()
        {
            foreach(Evaluation eval in Evaluation.evaluationList)
            {
                if (eval.evalID == ID.ToString())
                {
                    evaluation = new Evaluation(eval);
                    break;
                }
            }
            oList = new ObservableCollection<Observation>(evaluation.observationList);
        }
    }
}
