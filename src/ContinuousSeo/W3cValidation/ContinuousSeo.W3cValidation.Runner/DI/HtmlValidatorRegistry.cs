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
    using System.Text;
    using StructureMap.Configuration.DSL;
    using ContinuousSeo.Core;
    using ContinuousSeo.Core.IO;
    using ContinuousSeo.Core.Net;
    using ContinuousSeo.Core.Announcers;
    using ContinuousSeo.W3cValidation.Core;
    using ContinuousSeo.W3cValidation.Core.Html;
    using ContinuousSeo.W3cValidation.Runner.Processors;
    using ContinuousSeo.W3cValidation.Runner.Parsers;
    using ContinuousSeo.W3cValidation.Runner.Output;
    using ContinuousSeo.W3cValidation.Runner.Initialization;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class HtmlValidatorRegistry : Registry
    {
        public HtmlValidatorRegistry()
        {

            this.Scan(x =>
                {
                    x.LookForRegistries();
                    x.AssemblyContainingType<ContinuousSeo.Core.DefaultGuidProvider>();
                    x.AssemblyContainingType<ContinuousSeo.W3cValidation.Core.ResourceCopier>();
                });

            // HtmlValidationRunnerContext
            this.For<IAnnouncer>().Use(x => x.GetInstance<ConsoleAnnouncer>());

            this.For<IValidatorRunner>().Use(x => x.GetInstance<HtmlValidatorRunner>());

            // HtmlValidationRunner
            //this.For<IHtmlValidatorRunnerContext>().Use(x => x.GetInstance<HtmlValidatorRunnerContext>());
            this.For<IUrlProcessor>().Use(x => x.GetInstance<HtmlValidatorUrlProcessor>());

            // HtmlValidationUrlProcessor
            this.For<IValidatorWrapper>().Use(x => x.GetInstance<HtmlValidatorWrapper>());
            //this.For<IHtmlValidatorRunnerContext>().Use(x => x.GetInstance<HtmlValidatorRunnerContext>()); // TODO: make singleton scope
            this.For<IUrlAggregator>().Use(x => x.GetInstance<UrlAggregator>());
            this.For<IFileNameGenerator>().Use(x => x.GetInstance<FileNameGenerator>());
            this.For<ResourceCopier>().Use(x => x.GetInstance<HtmlValidatorResourceCopier>()); // From validation.core
            this.For<IValidatorReportWriterFactory>().Use(x => x.GetInstance<ValidatorReportWriterFactory>());
            this.For<IStreamFactory>().Use(x => x.GetInstance<StreamFactory>()); // from core
            this.For<IXslTransformer>().Use(x => x.GetInstance<XslTransformer>());

            // HtmlValidationWrapper
            //this.For<HtmlValidator>().Use(x => x.GetInstance<HtmlValidator>()); // from validation.core
            this.For<HtmlValidator>().Use(x => new HtmlValidator());

            // UrlAggregator
            this.For<ISitemapsParser>().Use(x => x.GetInstance<SitemapsParser>());
            this.For<IProjectFileParser>().Use(x => x.GetInstance<ProjectFileParser>());

            // FileNameGenerator
            //this.For<GuidProvider>().Use(x => x.GetInstance<GuidProvider>());
            //this.For<GuidProvider>().Use(x => GuidProvider.Current);

            // ValidatorReportWriterFactory
            this.For<IValidatorSoap12ResponseParser>().Use(x => x.GetInstance<HtmlValidatorSoap12ResponseParser>());

            // Sitemaps Parser
            this.For<IHttpClient>().Use(x => x.GetInstance<HttpClient>());

        }
    }
}
