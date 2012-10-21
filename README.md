Continuous SEO
=======================================
Continuous SEO is a solution aimed at making search engine optimization a part of the development process by integrating it into your continuous integration workflow.

The Continuous SEO W3C Validator for HTML and CSS makes it easy to ensure your markup is standards-compliant by making reports available in either HTML or XML format that can be integrated into a continuous integration server such as TeamCity or CruiseControl.NET.

NOTE: The Continuous SEO W3C Markup and CSS validators use the open-source W3C validation API to do the validation. The contributors of Continuous SEO are not affilated with those of the Markup and CSS validation API service and we make no guarantees as to the reliablility of the service or whether that service will continue to be available in the future. It is recommended that if you use Continuous SEO to do W3C validation that you download and install the service on a local server where you will be in control of its availability. Instructions are available for [installing the service on your own server](http://validator.w3.org/docs/install.html).

W3C HTML and CSS Validator Usage
---------------------------------

There are 2 useful ways to integrate HTML or CSS validation into your project. First, you can integrate it into an MSBuild script that can be run from any computer with the .NET framework 3.5 installed. I won't go into much depth as to how to use MSBuild here, but this is the relevant XML to integrate HTML validation into an MSBuild script:

```xml

<Project DefaultTargets="W3cHtmlValidator" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
	<!-- These tags declare the MSBuild Tasks so they can be used -->
	<UsingTask AssemblyFile="bin\Debug\ContinuousSeo.MSBuild.dll"
		TaskName="ContinuousSeo.MSBuild.W3cHtmlValidator" />
	<UsingTask AssemblyFile="bin\Debug\ContinuousSeo.MSBuild.dll"
		TaskName="ContinuousSeo.MSBuild.W3cCssValidator" />


	<!-- This is a target that will run the W3C HTML Validation -->
	<!-- IMPORTANT: Watch the casing -->
	<Target Name="W3cHtmlValidator">
		
		<!-- Declare Target URLs, Target Sitemaps Files, or Target Project Files to run validation on -->
		<ItemGroup>
			<TargetUrls Include="http://www.google.com/"/>
			<TargetUrls Include="http://www.yahoo.com/"/>
			<TargetUrls Include="http://www.bing.com/"/>
		</ItemGroup>

		<!--<ItemGroup>
			<TargetSitemapsFiles Include="http://www.mysite.com/sitemaps.aspx"/>
		</ItemGroup>-->

		<!--<ItemGroup>
			<TargetProjectFiles Include="W3cHtmlValidator.project"/>
		</ItemGroup>-->

		<!-- This declaration will run the W3C HTML validator -->
		<W3cHtmlValidator
			TargetUrls="@(TargetUrls)"
			TargetSitemapsFiles="@(TargetSitemapsFiles)"
			TargetProjectFiles="@(TargetProjectFiles)"
			OutputPath="Temp\"
			OutputFormat="html">

			<Output TaskParameter="TotalErrors" PropertyName="HtmlValidatorErrors"/>
			<Output TaskParameter="TotalWarnings" PropertyName="HtmlValidatorWarnings"/>
		</W3cHtmlValidator>

		<!-- Break the build by throwing a conditional error. You can decide how many errors or warnings you will tolerate. -->
		<Error Condition="$(HtmlValidatorErrors) > 0" Text="W3C HTML Validator found $(HtmlValidatorErrors) error(s)."/>
		
	</Target>
</Project>

```
The above target will generate an HTML report that groups the results by domain name, and then sorts by the number of errors (highest to lowest). This makes it quick and easy to see where the problems lie in your markup. The report then links to detailed pages of what errors happened in each page and what markup caused the error.

The other way this package can be used is from within a .NET project directly. Add a reference to ContinuousSeo.W3cValidation.Core and use code similar to the following to run validation on any markup that is generated dynamically by your own components, like so:

```c#

	HtmlValidator validator = new HtmlValidator();
	HtmlValidatorSettings settings = new HtmlValidatorSettings();
	
	string fragment = @"<h2>Testing</h2>";
	settings.DocType = "HTML 4.01 Transitional";
	
	result = validator.Validate(fragment, InputFormat.Fragment, settings);
	
	Assert.IsTrue(result.IsValid);

```

The settings are simply passed through to the W3C API, and some documention on the available settings is available at http://validator.w3.org/docs/api.html#requestformat.

As you can see by the above code, this package can then be used in your unit tests to ensure that the markup your components generate is W3C compliant.

Note that the CSS validation API and MSBuild task is similar to the HTML validation, the only real difference is the report that you receive.

Roadmap
---------------------------------

Due to time constraints all that is available now is W3C HTML and CSS validation in .NET and MSBuild. I wish I had the time to make a console application and NAnt runner so everyone could take advantage of this. My hope is to someday make this package generate other reports similar to what Google Webmaster Tools. Some ideas I have are checking for redirect problems, reports on indexable vs blocked urls (and why they won't be indexed), detecting problems with robots.txt formatting, and detecting missing canonical tags (i.e. similar content on multiple urls). Reporting is the primary goal, just like other things in a continuous integration process.

While most of this information is available through Google Webmaster Tools, it comes way too late in the process - you get these reports weeks after your site has already been launched. My aim is to provide reports before your site makes it into production that help you curb these sorts of issues before they are a problem.

If you think any of these things need to be created and cannot wait for me to do it, or you have another idea in the same vein (SEO for CI) feel free to contribute by making a pull request. I only ask that you please keep it on the level - we only want white hat SEO practiced here. While things like a service to give you ranking reports by parsing Google may be useful, they are against Google's Terms Of Service and I ask that you please be mindful of the functionality you contribute. A safe bet is to make services that check for problems within the code under development rather than doing external requests.
