// -----------------------------------------------------------------------
// <copyright file="W3cHtmlValidator.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.MSBuild
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;
    using ContinuousSeo.W3cValidation.Runner;
    using ContinuousSeo.W3cValidation.Runner.Initialization;
    using ContinuousSeo.W3cValidation.Runner.DI;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class W3cHtmlValidator : Task
    {

        #region Public Properties

        /// <summary>
        /// Input, list of sitemap files containing urls to perform validation on. The validator will process all urls in the sitmaps files.
        /// </summary>
        public ITaskItem[] TargetSitemapsFiles { get; set; }

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

        /// <summary>
        /// Input, the character set of the documents you are validating. If not included,
        /// the validator will attempt to get this information from the pages themselves.
        /// Acceptable values are:
        /// 
        /// Value -- Description
        /// ================================================
        /// utf-8 -- utf-8 (Unicode, worldwide)
        /// utf-16 -- utf-16 (Unicode, worldwide)
        /// iso-8859-1 -- iso-8859-1 (Western Europe)
        /// iso-8859-2 -- iso-8859-2 (Central Europe)
        /// iso-8859-3 -- iso-8859-3 (Southern Europe)
        /// iso-8859-4 -- iso-8859-4 (North European)
        /// iso-8859-5 -- iso-8859-5 (Cyrillic)
        /// iso-8859-6-i -- iso-8859-6-i (Arabic)
        /// iso-8859-7 -- iso-8859-7 (Greek)
        /// iso-8859-8 -- iso-8859-8 (Hebrew, visual)
        /// iso-8859-8-i -- iso-8859-8-i (Hebrew, logical)
        /// iso-8859-9 -- iso-8859-9 (Turkish)
        /// iso-8859-10 -- iso-8859-10 (Latin 6)
        /// iso-8859-11 -- iso-8859-11 (Latin/Thai)
        /// iso-8859-13 -- iso-8859-13 (Latin 7, Baltic Rim)
        /// iso-8859-14 -- iso-8859-14 (Latin 8, Celtic)
        /// iso-8859-15 -- iso-8859-15 (Latin 9)
        /// iso-8859-16 -- iso-8859-16 (Latin 10)
        /// us-ascii -- us-ascii (basic English)
        /// euc-jp -- euc-jp (Japanese, Unix)
        /// shift_jis -- shift_jis (Japanese, Win/Mac)
        /// iso-2022-jp -- iso-2022-jp (Japanese, email)
        /// euc-kr -- euc-kr (Korean)
        /// gb2312 -- gb2312 (Chinese, simplified)
        /// gb18030 -- gb18030 (Chinese, simplified)
        /// big5 -- big5 (Chinese, traditional)
        /// big5-HKSCS -- Big5-HKSCS (Chinese, Hong Kong)
        /// tis-620 -- tis-620 (Thai)
        /// koi8-r -- koi8-r (Russian)
        /// koi8-u -- koi8-u (Ukrainian)
        /// iso-ir-111 -- iso-ir-111 (Cyrillic KOI-8)  
        /// macintosh -- macintosh (MacRoman)
        /// windows-1250 -- windows-1250 (Central Europe)
        /// windows-1251 -- windows-1251 (Cyrillic)
        /// windows-1252 -- windows-1252 (Western Europe)
        /// windows-1253 -- windows-1253 (Greek)
        /// windows-1254 -- windows-1254 (Turkish)
        /// windows-1255 -- windows-1255 (Hebrew)
        /// windows-1256 -- windows-1256 (Arabic)
        /// windows-1257 -- windows-1257 (Baltic Rim)
        /// </summary>
        public string CharSet { get; set; }


        /// <summary>
        /// Input, the character set of the documents you are validating. If not included,
        /// the validator will attempt to get this information from the pages themselves.
        /// Acceptable values are:
        /// 
        /// Value -- Description
        /// ================================================
        /// Inline -- (detect automatically)
        /// HTML5 -- HTML5 (experimental)
        /// XHTML 1.0 Strict -- XHTML 1.0 Strict
        /// XHTML 1.0 Transitional -- XHTML 1.0 Transitional
        /// XHTML 1.0 Frameset -- XHTML 1.0 Frameset
        /// HTML 4.01 Strict -- HTML 4.01 Strict
        /// HTML 4.01 Transitional -- HTML 4.01 Transitional
        /// HTML 4.01 Frameset -- HTML 4.01 Frameset
        /// HTML 4.01 + RDFa 1.1 -- HTML 4.01 + RDFa 1.1
        /// HTML 3.2 -- HTML 3.2
        /// HTML 2.0 -- HTML 2.0
        /// ISO/IEC 15445:2000 ("ISO HTML") -- ISO/IEC 15445:2000 ("ISO HTML")
        /// XHTML 1.1 -- XHTML 1.1
        /// XHTML + RDFa -- XHTML + RDFa
        /// XHTML Basic 1.0 -- XHTML Basic 1.0
        /// XHTML Basic 1.1 -- XHTML Basic 1.1
        /// XHTML Mobile Profile 1.2 -- XHTML Mobile Profile 1.2
        /// XHTML-Print 1.0 -- XHTML-Print 1.0
        /// XHTML 1.1 plus MathML 2.0 -- XHTML 1.1 plus MathML 2.0 
        /// XHTML 1.1 plus MathML 2.0 plus SVG 1.1 -- XHTML 1.1 plus MathML 2.0 plus SVG 1.1 
        /// MathML 2.0 -- MathML 2.0
        /// SVG 1.0 -- SVG 1.0
        /// SVG 1.1 -- SVG 1.1
        /// SVG 1.1 Tiny -- SVG 1.1 Tiny
        /// SVG 1.1 Basic -- SVG 1.1 Basic
        /// SMIL 1.0 -- SMIL 1.0
        /// SMIL 2.0 -- SMIL 2.0
        /// </summary>
        public string DocType { get; set; }

        public bool Verbose { get; set; }

        public bool Debug { get; set; }

        public bool ShowSource { get; set; }

        public bool Outline { get; set; }

        public bool GroupErrors { get; set; }

        public bool UseHtmlTidy { get; set; }

        [Output]
        public int TotalErrors { get; private set; }

        [Output]
        public int TotalWarnings { get; private set; }

        #endregion

        #region Execute

        public override bool Execute()
        {
            if (TargetSitemapsFiles == null && TargetProjectFiles == null && TargetUrls == null)
            {
                Log.LogError("You must specify at least 1 url in order to run the task. " + 
                    "The urls can be provided through TargetUrls, TargetProjectFiles, or TargetSitemapFiles.");
                return false;
            }

            // Setup configuration of DI container
            Log.LogMessage("Composing Application");
            var container = new StructureMap.Container();
            container.Configure(r => r.AddRegistry<HtmlValidatorRegistry>());

            // Add the current parameters to context
            Log.LogMessage("Creating Context");

            var context = new HtmlValidatorRunnerContext()
            {
                TargetSitemapsFiles = this.TargetSitemapsFiles.ToStringArray(),
                TargetUrls = this.TargetUrls.ToStringArray(),
                TargetProjectFiles = this.TargetProjectFiles.ToStringArray(),
                UrlReplacementArgs = this.UrlReplacementArgs.ToStringArray(),
                OutputPath = this.OutputPath,
                OutputFormat = this.OutputFormat,
                ValidatorUrl = this.ValidatorUrl,
                DirectInputMode = this.DirectInputMode,
                CharSet = this.CharSet,
                DocType = this.DocType,
                Verbose = this.Verbose,
                Debug = this.Debug,
                ShowSource = this.ShowSource,
                Outline = this.Outline,
                GroupErrors = this.GroupErrors,
                UseHtmlTidy = this.UseHtmlTidy
            };

            // Setup the container to use the context as a singleton so it is available everywhere.
            container.Configure(r => r.For<HtmlValidatorRunnerContext>().Singleton().Use(x => context));

            var runner = container.GetInstance<IValidatorRunner>();

            Log.LogMessage("Starting Validation");

            try
            {
                // get output information (errors, warnings)
                var report = runner.Execute();

                this.TotalErrors = report.TotalErrors;
                this.TotalWarnings = report.TotalWarnings;

                Log.LogMessage("Validation completed with {0} error(s) and {1} warning(s).", report.TotalErrors, report.TotalWarnings);
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
