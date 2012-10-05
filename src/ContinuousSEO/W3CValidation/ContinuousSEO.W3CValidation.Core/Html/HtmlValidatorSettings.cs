// -----------------------------------------------------------------------
// <copyright file="HtmlValidatorSettings.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSEO.W3CValidation.Core.Html
{


    /// <summary>
    /// The settings to be passed in to the HTML W3C validator API.
    /// </summary>
    public class HtmlValidatorSettings
    {
        public HtmlValidatorSettings()
        {
            //Verbose = true;
            //Debug = true;
            //ShowSource = true;
            //Outline = true;
            //OutputFormat = OutputFormat.Html;
            //InputFormat = InputFormat.Uri;

            Verbose = false;
        }

        //public InputFormat InputFormat { get; set; }
        //public OutputFormat OutputFormat { get; set; }
        public string CharSet { get; set; }
        public string DocType { get; set; }
        public bool Verbose { get; set; }
        public bool Debug { get; set; }
        public bool ShowSource { get; set; }
        public bool Outline { get; set; }

    }
}
