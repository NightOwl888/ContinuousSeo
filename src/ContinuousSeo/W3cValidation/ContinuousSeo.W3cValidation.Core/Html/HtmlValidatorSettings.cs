// -----------------------------------------------------------------------
// <copyright file="HtmlValidatorSettings.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.W3cValidation.Core.Html
{


    /// <summary>
    /// Advanced settings to be passed in to the HTML W3C validator API. These
    /// settings are documented at http://validator.w3.org/docs/api.html#requestformat.
    /// </summary>
    public class HtmlValidatorSettings
    {
        //public HtmlValidatorSettings()
        //{
        //    //Verbose = true;
        //    //Debug = true;
        //    //ShowSource = true;
        //    //Outline = true;

        //    Verbose = false;
        //}

        public string CharSet { get; set; }
        public string DocType { get; set; }
        public bool Verbose { get; set; }
        public bool Debug { get; set; }
        public bool ShowSource { get; set; }
        public bool Outline { get; set; }

    }
}
