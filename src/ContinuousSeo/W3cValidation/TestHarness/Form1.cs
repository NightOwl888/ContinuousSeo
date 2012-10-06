using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ContinuousSeo.W3cValidation.Core;
using ContinuousSeo.W3cValidation.Core.Html;
using ContinuousSeo.W3cValidation.Core.Css;

namespace TestHarness
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            HtmlValidator validator = new HtmlValidator();

            HtmlValidatorSettings settings = new HtmlValidatorSettings();

            //FileStream output = new FileStream(@"F:\TestW3C\ResponseNew.html", FileMode.Create);
            //try
            //{
            //    HtmlValidatorResult result = validator.Validate(output, OutputFormat.Soap12, "http://www.shuttercontractor.com", InputFormat.Uri, settings);
            //}
            //finally
            //{
            //    output.Flush();
            //    output.Dispose();
            //}


            HtmlValidatorResult result;


            //result = validator.Validate(@"F:\TestW3C\ResponseNew.xml", OutputFormat.Soap12, @"http://www.shuttercontractor.com/", InputFormat.Uri, settings);

            result = validator.Validate(@"F:\TestW3C-2\Response.html", @"http://www.shuttercontractor.com/");


            string fragment = @"<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Strict//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd""><html xmlns=""http://www.w3.org/1999/xhtml""><head><title>test</title></head><body><p>hello</p></body></html>";

            fragment = @"<h2>Testing</h2>";
            //fragment = @"<html><body><h2>Testing</h2></body></html>";
            //fragment = @"<!DOCTYPE html><html xmlns=""http://www.w3.org/1999/xhtml""><body><h2>Testing</h2></body></html>";
            //fragment = @"<!DOCTYPE html><html xmlns=""http://www.w3.org/1999/xhtml""><head><title></title></head><body><h2>Testing</h2></body></html>";

            //fragment = @"<!DOCTYPE html><html><head><title></title></head><body><h2>Testing</h2></body></html>";

            settings.DocType = "HTML 4.01 Transitional";

            //result = validator.Validate(@"F:\TestW3C\ResponseFragment.xml", OutputFormat.Soap12, fragment, InputFormat.Fragment, settings);

            result = validator.Validate(fragment, InputFormat.Fragment);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CssValidator validator = new CssValidator();

            CssValidatorSettings settings = new CssValidatorSettings();

            CssValidatorResult result = null;


            result = validator.Validate(@"F:\TestW3C-3\ResponseCss.html", OutputFormat.Html, "http://www.shuttercontractor.com/App_Themes/Theme3/styles.css", InputFormat.Uri, settings);

            //result = validator.Validate(@"F:\TestW3C\ResponseCss.xml", OutputFormat.Soap12, "http://www.shuttercontractor.com/App_Themes/Theme3/styles.css", InputFormat.Uri, settings);


            //string fragment = @"h1,h2,h3,h4,h5,h6 { padding:0; margin:0; border:0; outline:0; }";

            //StreamReader sr = File.OpenText(@"F:\TestW3C\Test.css");
            //fragment = sr.ReadToEnd();

            ////result = validator.Validate(@"F:\TestW3C\ResponseCssFragment.xml", OutputFormat.Soap12, fragment, InputFormat.Fragment, settings);

            //result = validator.Validate(fragment, InputFormat.Fragment, settings);


        }
    }
}
