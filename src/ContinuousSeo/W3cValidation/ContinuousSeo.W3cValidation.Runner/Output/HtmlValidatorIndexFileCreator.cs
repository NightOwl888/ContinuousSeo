// -----------------------------------------------------------------------
// <copyright file="HtmlIndexFileCreator.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.W3cValidation.Runner.Output
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;
    using System.Xml;
    using ContinuousSeo.W3cValidation.Runner.Processors;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class HtmlValidatorIndexFileCreator
    {
        public void CreateIndexFile(Stream output, IEnumerable<IValidatorReportItem> reports)
        {
            // TODO: check validity of all documents in file
            bool isValid = true;
            int errors = 0;
            int warnings = 5;
            int documents = reports.Count();
            int domains = 1;


            using (XmlTextWriter writer = new XmlTextWriter(output, Encoding.UTF8))
            {
                writer.Formatting = Formatting.Indented;


                writer.WriteRaw(@"<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Strict//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\"">");

                // <html>
                writer.WriteStartElement("html", "http://www.w3.org/1999/xhtml");

                writer.WriteStartAttribute("xml", "lang", string.Empty);
                writer.WriteValue("en");
                writer.WriteEndAttribute();

                writer.WriteStartAttribute("lang");
                writer.WriteValue("en");
                writer.WriteEndAttribute();

                WriteHead(writer, isValid);

                WriteBody(writer, isValid, errors, warnings, documents, domains);
                

                // </html>
                writer.WriteEndElement();

                writer.Flush();
            }
            
        }

        private void WriteHead(XmlTextWriter writer, bool isValid)
        {
            //<head>
            writer.WriteStartElement("head");

            // <meta>
            writer.WriteStartElement("meta");

            writer.WriteStartAttribute("http-equiv");
            writer.WriteValue("Content-Type");
            writer.WriteEndAttribute();

            writer.WriteStartAttribute("content");
            writer.WriteValue("text/html;charset=utf-8");
            writer.WriteEndAttribute();

            //</meta>
            writer.WriteEndElement();

            // <title>
            writer.WriteStartElement("title");
            writer.WriteValue("Markup Validation Summary - W3C Markup Validator");
            writer.WriteEndElement();

            // <link>
            writer.WriteStartElement("link");

            writer.WriteStartAttribute("rel");
            writer.WriteValue("icon");
            writer.WriteEndAttribute();

            // TODO: Make this image dynamic. This is the red one for failure, need to also add a green one and make it conditional.

            if (isValid)
            {
                writer.WriteStartAttribute("href");
                writer.WriteValue("data:image/png,%89PNG%0D%0A%1A%0A%00%00%00%0DIHDR%00%00%00%10%00%00%00%10%08%02%00%00%00%90%91h6%00%00%00%19IDAT(%91c%0C%DD%10%C5%40%0A%60%22I%F5%A8%86Q%0DCJ%03%00dy%01%7F%0C%9F0%7D%00%00%00%00IEND%AEB%60%82");
                writer.WriteEndAttribute();
            }
            else
            {
                writer.WriteStartAttribute("href");
                writer.WriteValue("data:image/png,%89PNG%0D%0A%1A%0A%00%00%00%0DIHDR%00%00%00%10%00%00%00%10%08%02%00%00%00%90%91h6%00%00%00%19IDAT(%91c%BCd%AB%C2%40%0A%60%22I%F5%A8%86Q%0DCJ%03%00%DE%B5%01S%07%88%8FG%00%00%00%00IEND%AEB%60%82");
                writer.WriteEndAttribute();
            }

            writer.WriteStartAttribute("type");
            writer.WriteValue("image/png");
            writer.WriteEndAttribute();

            // </link>
            writer.WriteEndElement();

            // <link>
            writer.WriteStartElement("link");

            writer.WriteStartAttribute("rev");
            writer.WriteValue("made");
            writer.WriteEndAttribute();

            writer.WriteStartAttribute("href");
            writer.WriteValue("mailto:ww-validator@w3.org");
            writer.WriteEndAttribute();

            // </link>
            writer.WriteEndElement();

            // <link>
            writer.WriteStartElement("link");

            writer.WriteStartAttribute("rev");
            writer.WriteValue("start");
            writer.WriteEndAttribute();

            writer.WriteStartAttribute("href");
            writer.WriteValue("./");
            writer.WriteEndAttribute();

            writer.WriteStartAttribute("title");
            writer.WriteValue("Home Page");
            writer.WriteEndAttribute();

            // </link>
            writer.WriteEndElement();

            // <style>
            writer.WriteStartElement("style");

            writer.WriteStartAttribute("type");
            writer.WriteValue("text/css");
            writer.WriteEndAttribute();

            writer.WriteStartAttribute("media");
            writer.WriteValue("all");
            writer.WriteEndAttribute();

            writer.WriteValue(@"@import ""./style/base"";" + Environment.NewLine + @"@import ""./style/results"";");

            // </style>
            writer.WriteEndElement();


            //</head>
            writer.WriteEndElement();
        }

        private void WriteBody(XmlTextWriter writer, bool isValid, int errors, int warnings, int documents, int domains)
        {
            // <body>
            writer.WriteStartElement("body");

            WriteBanner(writer);
            WriteResults(writer, isValid, errors, warnings, documents, domains);

            writer.WriteEndElement();
        }

        private void WriteBanner(XmlTextWriter writer)
        {
            // <div>
            writer.WriteStartElement("div");

            writer.WriteStartAttribute("id");
            writer.WriteValue("banner");
            writer.WriteEndAttribute();

            // <h1>
            writer.WriteStartElement("h1");

            writer.WriteStartAttribute("id");
            writer.WriteValue("title");
            writer.WriteEndAttribute();


            // <a>
            writer.WriteStartElement("a");

            writer.WriteStartAttribute("href");
            writer.WriteValue("http://www.w3.org/");
            writer.WriteEndAttribute();


            // <img>
            writer.WriteStartElement("img");

            writer.WriteStartAttribute("alt");
            writer.WriteValue("W3C");
            writer.WriteEndAttribute();

            writer.WriteStartAttribute("width");
            writer.WriteValue("110");
            writer.WriteEndAttribute();

            writer.WriteStartAttribute("height");
            writer.WriteValue("61");
            writer.WriteEndAttribute();

            writer.WriteStartAttribute("id");
            writer.WriteValue("logo");
            writer.WriteEndAttribute();

            writer.WriteStartAttribute("src");
            writer.WriteValue("./images/w3c.png");
            writer.WriteEndAttribute();

            //</img>
            writer.WriteEndElement();

            //</a>
            writer.WriteEndElement();


            // <a>
            writer.WriteStartElement("a");

            writer.WriteStartAttribute("href");
            writer.WriteValue("./");
            writer.WriteEndAttribute();

            // <span>
            writer.WriteStartElement("span");

            writer.WriteValue("Markup Validation Service");

            //</span>
            writer.WriteEndElement();

            //</a>
            writer.WriteEndElement();

            //</h1>
            writer.WriteEndElement();


            // <p>
            writer.WriteStartElement("p");

            writer.WriteStartAttribute("id");
            writer.WriteValue("tagline");
            writer.WriteEndAttribute();

            writer.WriteValue("Check the markup (HTML, XHTML, …) of Web documents");

            // </p>
            writer.WriteEndElement();

            
            // </div>
            writer.WriteEndElement();
        }

        private void WriteResults(XmlTextWriter writer, bool isValid, int errors, int warnings, int documents, int domains)
        {
            // <div>
            writer.WriteStartElement("div");

            writer.WriteStartAttribute("id");
            writer.WriteValue("results_container");
            writer.WriteEndAttribute();

            WriteResultsJumpTo(writer);
            WriteResultsSummary(writer, isValid, errors, warnings, documents, domains);

            // </div>
            writer.WriteEndElement();
        }

        private void WriteResultsJumpTo(XmlTextWriter writer)
        {
            // TODO: Figure out how to make the jump points in the document
            // There will need to be a jump point for each unique domain


            // <ul>
            writer.WriteStartElement("ul");

            writer.WriteStartAttribute("class");
            writer.WriteValue("navbar");
            writer.WriteEndAttribute();

            writer.WriteStartAttribute("id");
            writer.WriteValue("jumpbar");
            writer.WriteEndAttribute();


            // <li>
            writer.WriteStartElement("li");

            // <strong>
            writer.WriteStartElement("strong");

            writer.WriteValue("Jump To:");

            // </strong>
            writer.WriteEndElement();

            // </li>
            writer.WriteEndElement();


            WriteResultsJumpToPoint(writer, "Test Point", "#result");
            // TODO: Make jump points dynamic and add here

            // </ul>
            writer.WriteEndElement();
        }

        private void WriteResultsJumpToPoint(XmlTextWriter writer, string title, string href)
        {
            // <li>
            writer.WriteStartElement("li");

            // <a>
            writer.WriteStartElement("a");

            writer.WriteStartAttribute("title");
            writer.WriteValue(title);
            writer.WriteEndAttribute();

            writer.WriteStartAttribute("href");
            writer.WriteValue(href);
            writer.WriteEndAttribute();

            writer.WriteValue(title);

            // </a>
            writer.WriteEndElement();

            // </li>
            writer.WriteEndElement();
        }


        private void WriteResultsSummary(XmlTextWriter writer, bool isValid, int errors, int warnings, int documents, int domains)
        {

            // <h2>
            writer.WriteStartElement("h2");

            writer.WriteStartAttribute("id");
            writer.WriteValue("results");
            writer.WriteEndAttribute();

            if (isValid)
            {
                writer.WriteStartAttribute("class");
                writer.WriteValue("valid");
                writer.WriteEndAttribute();

                writer.WriteValue("All documents were successfully checked!");
            }
            else
            {
                writer.WriteStartAttribute("class");
                writer.WriteValue("invalid");
                writer.WriteEndAttribute();

                writer.WriteValue("Errors found while checking these documents!");
            }

            // </h2>
            writer.WriteEndElement();

            // <table>
            writer.WriteStartElement("table");

            writer.WriteStartAttribute("class");
            writer.WriteValue("header");
            writer.WriteEndAttribute();

            // <tr>
            writer.WriteStartElement("tr");

            // <th>
            writer.WriteStartElement("th");

            writer.WriteValue("Result:");

            // </th>
            writer.WriteEndElement();

            // <td>
            writer.WriteStartElement("td");

            writer.WriteStartAttribute("colspan");
            writer.WriteValue("2");
            writer.WriteEndAttribute();

            writer.WriteStartAttribute("style");
            writer.WriteValue("width:70%;");
            writer.WriteEndAttribute();

            if (isValid)
            {
                writer.WriteStartAttribute("class");
                writer.WriteValue("valid");
                writer.WriteEndAttribute();

                writer.WriteValue("Passed");
            }
            else
            {
                writer.WriteStartAttribute("class");
                writer.WriteValue("invalid");
                writer.WriteEndAttribute();

                writer.WriteValue("Failed");
            }

            // </td>
            writer.WriteEndElement();

            // </tr>
            writer.WriteEndElement();

            WriteResultsSummaryRow(writer, "Total Errors", errors.ToString());
            WriteResultsSummaryRow(writer, "Total Warnings", warnings.ToString());
            WriteResultsSummaryRow(writer, "Number of Documents", documents.ToString());
            WriteResultsSummaryRow(writer, "Number of Domains", domains.ToString());


            // </table>
            writer.WriteEndElement();
        }

        private void WriteResultsSummaryRow(XmlTextWriter writer, string title, string value)
        {
            // <tr>
            writer.WriteStartElement("tr");

            // <th>
            writer.WriteStartElement("th");

            writer.WriteValue(title + ":");

            // </th>
            writer.WriteEndElement();

            // <td>
            writer.WriteStartElement("td");

            writer.WriteStartAttribute("colspan");
            writer.WriteValue("2");
            writer.WriteEndAttribute();

            writer.WriteValue(value);

            // </td>
            writer.WriteEndElement();

            // </tr>
            writer.WriteEndElement();

        }

    }
}
