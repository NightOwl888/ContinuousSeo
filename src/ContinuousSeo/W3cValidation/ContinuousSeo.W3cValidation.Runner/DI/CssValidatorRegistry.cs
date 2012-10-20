// -----------------------------------------------------------------------
// <copyright file="CssValidatorRegistry.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.W3cValidation.Runner.DI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    //using System.Text;
    using StructureMap.Configuration.DSL;
    using ContinuousSeo.W3cValidation.Core;
    using ContinuousSeo.W3cValidation.Core.Css;
    using ContinuousSeo.W3cValidation.Runner.Validators;
    using ContinuousSeo.W3cValidation.Runner.Xsl;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CssValidatorRegistry : ValidatorRegistry
    {
        public CssValidatorRegistry()
            : base()
        {
            this.For<IValidatorRunner>().Use(x => x.GetInstance<CssValidatorRunner>());

            // HtmlOutputUrlProcessor
            this.For<IValidatorWrapper>().Use(x => x.GetInstance<CssValidatorWrapper>());

            this.For<ResourceCopier>().Use(x => x.GetInstance<CssValidatorResourceCopier>()); // From validation.core

            this.For<IXslResourceProvider>().Use(x => x.GetInstance<CssXslResourceProvider>());

            // CssValidatorWrapper
            this.For<CssValidator>().Use(x => new CssValidator()); // from validation.core

            // ValidatorReportWriterFactory
            this.For<IValidatorSoap12ResponseParser>().Use(x => x.GetInstance<CssValidatorSoap12ResponseParser>());
        }
    }
}
