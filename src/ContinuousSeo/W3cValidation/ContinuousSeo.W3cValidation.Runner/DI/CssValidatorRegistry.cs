#region Copyright
// -----------------------------------------------------------------------
//
// Copyright (c) 2012, Shad Storhaug <shad@shadstorhaug.com>
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// -----------------------------------------------------------------------
#endregion

namespace ContinuousSeo.W3cValidation.Runner.DI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
