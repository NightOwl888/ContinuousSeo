// -----------------------------------------------------------------------
// <copyright file="W3cCssValidator.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.MSBuild
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;
    using ContinuousSeo.Core.Announcers;
    using ContinuousSeo.W3cValidation.Runner;
    using ContinuousSeo.W3cValidation.Runner.Initialization;
    using ContinuousSeo.W3cValidation.Runner.DI;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class W3cCssValidator : Task
    {

        #region Public Properties

        /// <summary>
        /// Input, list of project files containing urls to perform validation on.
        /// </summary>
        public ITaskItem[] TargetProjectFiles { get; set; }

        /// <summary>
        /// Input, list of target urls to perform validation on.
        /// </summary>
        public ITaskItem[] TargetUrls { get; set; }

        /// <summary>
        /// Input, list of arguments to use to replace strings within urls.
        /// For example, if you have the string "http://{0}/index.html" within
        /// the TargetProjectFiles or TargetUrls arguments, passing "www.mydomain.com"
        /// as the first argument here will yield the url "http://www.mydomain.com/index.html"
        /// to be validated. The number in {0} can be used to indicate the position of the 
        /// argument and it is zero based, so {1} can access the second argument, {2} the third, 
        /// and so on.
        /// </summary>
        public ITaskItem[] UrlReplacementArgs { get; set; }

        /// <summary>
        /// Input, the destination path to write the report. If OutputFormat is HTML, 
        /// this should correspond to a directory with no filename. If OutputFormat is XML,
        /// you can optionally include a filename. If no filename is provided, the default
        /// is "report.xml".
        /// </summary>
        [Required]
        public string OutputPath { get; set; }

        /// <summary>
        /// Input, can either be "html" or "xml". Casing is not important.
        /// </summary>
        public string OutputFormat { get; set; }

        /// <summary>
        /// Input, url to the W3C HTML Validator API. This can be used to 
        /// provide the path to a custom hosting location of the service. 
        /// If not provided, the default is "http://validator.w3.org/check".
        /// </summary>
        public string ValidatorUrl { get; set; }


        /// <summary>
        /// Input, if true indicates to use direct input mode of the W3C validator.
        /// This is useful if the webserver that contains the pages you are validating is
        /// behind a firewall, but it is recommended to host the W3C validation service 
        /// yourself if this is the case.
        /// </summary>
        public bool DirectInputMode { get; set; }

        public string UserMedium { get; set; }
        public string CssProfile { get; set; }
        public string Language { get; set; }
        public string WarningLevel { get; set; }
        

        [Output]
        public int TotalErrors { get; private set; }

        #endregion

        #region Execute

        public override bool Execute()
        {
            if (TargetProjectFiles == null && TargetUrls == null)
            {
                Log.LogError("You must specify at least 1 url in order to run the task. " +
                    "The urls can be provided through TargetUrls or TargetProjectFiles.");
                return false;
            }

            // Setup configuration of DI container
            Log.LogMessage(MessageImportance.Low, "Composing Application");
            var container = new StructureMap.Container();
            container.Configure(r => r.AddRegistry<CssValidatorRegistry>());

            // Add the current parameters to context
            Log.LogMessage(MessageImportance.Low, "Creating Context");

            var context = new CssValidatorRunnerContext(new ConsoleAnnouncer())
            {
                TargetUrls = this.TargetUrls.ToStringArray(),
                TargetProjectFiles = this.TargetProjectFiles.ToStringArray(),
                UrlReplacementArgs = this.UrlReplacementArgs.ToStringArray(),
                OutputPath = this.OutputPath,
                OutputFormat = this.OutputFormat,
                ValidatorUrl = this.ValidatorUrl,
                DirectInputMode = this.DirectInputMode,
                UserMedium = this.UserMedium,
                CssProfile = this.CssProfile,
                Language = this.Language,
                WarningLevel = this.WarningLevel
            };

            // Setup the container to use the context as a singleton so it is available everywhere.
            container.Configure(r => r.For<ICssValidatorRunnerContext>().Singleton().Use(x => context));
            container.Configure(r => r.For<IValidatorRunnerContext>().Singleton().Use(x => context));
            container.Configure(r => r.For<IRunnerContext>().Singleton().Use(x => context));

            var runner = container.GetInstance<IValidatorRunner>();

            Log.LogMessage(MessageImportance.Low, "Starting Runner");

            try
            {
                // get output information (errors, warnings)
                var report = runner.Execute();

                this.TotalErrors = report.TotalErrors;
            }
            catch (Exception ex)
            {
                Log.LogError("While executing validation the following error was encountered: {0}, {1}", ex.Message, ex.StackTrace);
                return false;
            }

            return true;
        }

        #endregion

    }
}
