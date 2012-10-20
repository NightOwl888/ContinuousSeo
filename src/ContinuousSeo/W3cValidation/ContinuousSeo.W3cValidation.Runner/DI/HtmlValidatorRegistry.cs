// -----------------------------------------------------------------------
// <copyright file="HtmlValidatorRegistry.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.W3cValidation.Runner.DI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using StructureMap.Configuration.DSL;
    using ContinuousSeo.W3cValidation.Core;
    using ContinuousSeo.W3cValidation.Core.Html;
    using ContinuousSeo.W3cValidation.Runner.Validators;
    using ContinuousSeo.W3cValidation.Runner.Xsl;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class HtmlValidatorRegistry : ValidatorRegistry
    {
        public HtmlValidatorRegistry()
            : base()
        {
            this.For<IValidatorRunner>().Use(x => x.GetInstance<HtmlValidatorRunner>());

            // HtmlOutputUrlProcessor
            this.For<IValidatorWrapper>().Use(x => x.GetInstance<HtmlValidatorWrapper>());

            this.For<ResourceCopier>().Use(x => x.GetInstance<HtmlValidatorResourceCopier>()); // From validation.core

            this.For<IXslResourceProvider>().Use(x => x.GetInstance<HtmlXslResourceProvider>());

            // HtmlValidatorWrapper
            this.For<HtmlValidator>().Use(x => new HtmlValidator());

            // ValidatorReportWriterFactory
            this.For<IValidatorSoap12ResponseParser>().Use(x => x.GetInstance<HtmlValidatorSoap12ResponseParser>());
        }
    }
}
