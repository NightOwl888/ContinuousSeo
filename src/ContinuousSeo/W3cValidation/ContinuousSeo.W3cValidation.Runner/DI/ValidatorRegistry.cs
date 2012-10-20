// -----------------------------------------------------------------------
// <copyright file="ValidatorRegistry.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.W3cValidation.Runner.DI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using StructureMap.Configuration.DSL;
    using ContinuousSeo.Core.Announcers;
    using ContinuousSeo.Core.IO;
    using ContinuousSeo.Core.Net;
    using ContinuousSeo.W3cValidation.Runner.Output;
    using ContinuousSeo.W3cValidation.Runner.Parsers;
    using ContinuousSeo.W3cValidation.Runner.Xsl;
    using ContinuousSeo.W3cValidation.Runner.UrlAggregators;
    using ContinuousSeo.W3cValidation.Runner.UrlProcessors;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public abstract class ValidatorRegistry : Registry
    {
        public ValidatorRegistry()
        {
            this.Scan(x =>
            {
                x.AssemblyContainingType<ContinuousSeo.Core.DefaultTimeProvider>();
                x.AssemblyContainingType<ContinuousSeo.W3cValidation.Core.ResourceCopier>();
            });

            this.For<IAnnouncer>().Use(x => x.GetInstance<ConsoleAnnouncer>());
            this.For<IUrlAggregator>().Use(x => x.GetInstance<AnnouncingUrlAggregator>());
            this.For<IFileNameGenerator>().Use(x => x.GetInstance<FileNameGenerator>());
            this.For<IValidatorReportWriterFactory>().Use(x => x.GetInstance<ValidatorReportWriterFactory>());
            this.For<IStreamFactory>().Use(x => x.GetInstance<StreamFactory>()); // from core
            this.For<IXslTransformer>().Use(x => x.GetInstance<XslTransformer>());
            this.For<IUrlProcessorFactory>().Use<UrlProcessorFactory>()
                .Ctor<UrlProcessor>("xmlProcessor").Is<AnnouncingXmlOutputUrlProcessor>()
                .Ctor<UrlProcessor>("htmlProcessor").Is<AnnouncingHtmlOutputUrlProcessor>();

            // UrlAggregator
            this.For<ISitemapsParser>().Use(x => x.GetInstance<SitemapsParser>());
            this.For<IProjectFileParser>().Use(x => x.GetInstance<ProjectFileParser>());

            // Sitemaps Parser
            this.For<IHttpClient>().Use(x => x.GetInstance<HttpClient>());

            this.For<IHtmlIndexFileWriter>().Use(x => x.GetInstance<HtmlIndexFileWriter>());
        }
    }
}
