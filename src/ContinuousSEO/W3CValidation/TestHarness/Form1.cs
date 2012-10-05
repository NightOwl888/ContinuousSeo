using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ContinuousSEO.W3CValidation.Core;
using ContinuousSEO.W3CValidation.Core.Html;

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


            string fragment = @"<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Strict//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd""><html xmlns=""http://www.w3.org/1999/xhtml""><head><title>test</title></head><body><p>hello</p></body></html>";

            fragment = @"<h2>Testing</h2>";
            //fragment = @"<html><body><h2>Testing</h2></body></html>";
            //fragment = @"<!DOCTYPE html><html xmlns=""http://www.w3.org/1999/xhtml""><body><h2>Testing</h2></body></html>";
            //fragment = @"<!DOCTYPE html><html xmlns=""http://www.w3.org/1999/xhtml""><head><title></title></head><body><h2>Testing</h2></body></html>";

            //fragment = @"<!DOCTYPE html><html><head><title></title></head><body><h2>Testing</h2></body></html>";

            settings.DocType = "HTML 4.01 Transitional";

            result = validator.Validate(@"F:\TestW3C\ResponseFragment.xml", OutputFormat.Soap12, fragment, InputFormat.Fragment, settings);
        }
    }
}
