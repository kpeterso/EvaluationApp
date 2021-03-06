﻿using EvaluationApp.AccessSQLService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Data.Xml.Dom;
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
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;
                rootFrame.Navigated += OnNavigated;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }
                // Ensure the current window is active
                Window.Current.Activate();
            }

            SystemNavigationManager.GetForCurrentView().BackRequested += App_BackRequested;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = rootFrame.CanGoBack ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;

            initEvalListFromSQLServer();
        }

        private async void initEvalListFromSQLServer()
        {
            IList<EvalQuery> articleList = null;
            //Load Evaluations and Surveys from SQL Server, load into static lists
            try
            {
                ServiceClient client = new ServiceClient();
                articleList = await client.QueryArticleAsync();
            }
            catch (Exception ex)
            {
                //NotifyUser(ex.Message);
            }

            foreach (EvalQuery E in articleList)
            {
                Evaluation eval = new Evaluation(E.EvalID, E.Vehicle, E.EvalType);
                Evaluation.evaluationList.Add(eval);
            }
            /*var evaluations = Evaluation.evaluationDoc.SelectNodes("descendant::evaluation");
            foreach (IXmlNode evaluation in evaluations)
            {
                Evaluation eval = new Evaluation(evaluation);
                Evaluation.evaluationList.Add(eval);
            }

            var surveys = Survey.surveyDoc.SelectNodes("descendant::survey");
            foreach (IXmlNode survey in surveys)
            {
                Survey s = new Survey(survey);
                Survey.surveyList.Add(s);
            }*/
        }


        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        private void App_BackRequested(object sender, BackRequestedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame == null)
                return;

            // If we can go back and the event has not already been handled, do so.
            if (rootFrame.CanGoBack && e.Handled == false)
            {
                e.Handled = true;
                rootFrame.GoBack();
            }
        }

        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            //Each time Navigation occurs, update Back button visibility
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = ((Frame)sender).CanGoBack ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
        }

        /*private async void initXml()
        {
            //Load Evaluations and Surveys from XML files, load into static lists
            try
            {
                XmlDocument doc = await Evaluation.LoadXmlFile("EvaluationXml", "Evaluations.xml");
                Evaluation.evaluationDoc = doc;

                doc = await Evaluation.LoadXmlFile("Surveys", "Survey1.xml");
                Survey.surveyDoc = doc;
            }
            catch (Exception exp)
            {
            }

            var evaluations = Evaluation.evaluationDoc.SelectNodes("descendant::evaluation");
            foreach (IXmlNode evaluation in evaluations)
            {
                Evaluation eval = new Evaluation(evaluation);
                Evaluation.evaluationList.Add(eval);
            }

            var surveys = Survey.surveyDoc.SelectNodes("descendant::survey");
            foreach (IXmlNode survey in surveys)
            {
                Survey s = new Survey(survey);
                Survey.surveyList.Add(s);
            }
        }*/

    }
}
