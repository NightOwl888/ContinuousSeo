<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE xsl:stylesheet [
	<!ENTITY hellip "&#8230;">
	<!ENTITY copy "&#169;">
	<!ENTITY reg "&#174;">
	<!ENTITY uarr "&#8593;">
	<!ENTITY nbsp "&#160;">
]>

<!--
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
-->


<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:key name="validationResult-by-domain" match="validationResult" use="@domainName" />
	<xsl:output method="xml" encoding="utf-8" indent="yes" omit-xml-declaration="yes"/>
	
	<xsl:template match="w3cValidatorResults">
		<xsl:variable name="batchErrors">
			<xsl:value-of select="sum(//w3cValidatorResults/validationResult/@errors)" />
		</xsl:variable>
		<xsl:text disable-output-escaping="yes">&lt;!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN"
    "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd"&gt;</xsl:text>
		<html>
		<head>
			<meta http-equiv="Content-Type" content="text/html;charset=utf-8" />
			
			<xsl:choose>
				<xsl:when test="$batchErrors = 0">
					<title>[Valid] Markup Validation Summary - W3C Markup Validator</title>
					<link rel="icon" href="data:image/png,%89PNG%0D%0A%1A%0A%00%00%00%0DIHDR%00%00%00%10%00%00%00%10%08%02%00%00%00%90%91h6%00%00%00%19IDAT(%91c%0C%DD%10%C5%40%0A%60%22I%F5%A8%86Q%0DCJ%03%00dy%01%7F%0C%9F0%7D%00%00%00%00IEND%AEB%60%82" type="image/png" />
				</xsl:when>
				<xsl:otherwise>
					<title>[Invalid] Markup Validation Summary - W3C Markup Validator</title>
					<link rel="icon" href="data:image/png,%89PNG%0D%0A%1A%0A%00%00%00%0DIHDR%00%00%00%10%00%00%00%10%08%02%00%00%00%90%91h6%00%00%00%19IDAT(%91c%BCd%AB%C2%40%0A%60%22I%F5%A8%86Q%0DCJ%03%00%DE%B5%01S%07%88%8FG%00%00%00%00IEND%AEB%60%82" type="image/png" />
				</xsl:otherwise>
			</xsl:choose>
			<style type="text/css" media="all">
				@import "./style/base"; 
				@import "./style/results";
				<!-- styles for current document -->
				.details table.header th { text-align: left; }
				span.err_type img {
					height: 1.5em;
					padding-bottom: 0.2em;
					vertical-align: middle;
					width: 1.5em;
				}
			</style>
		</head>
		<body>
			<div id="banner">
				<h1 id="title">
					<a href="http://www.w3.org/">
						<img alt="W3C" width="110" height="61" id="logo" src="./images/w3c.png" />
					</a>
					<a href="./">
						<span>Markup Validation Service</span>
					</a>
				</h1>
				<p id="tagline">Check the markup (HTML, XHTML, &hellip;) of Web documents</p>
			</div>
			<div id="results_container">
	
				<!-- Jump Bar -->
				<ul class="navbar" id="jumpbar">
					<li>
						<strong>Jump To:</strong>
					</li>
					<xsl:apply-templates mode="toc" select="validationResult[generate-id()=generate-id(key('validationResult-by-domain', @domainName)[1])]">
						<xsl:sort select="@domainName"/>
					</xsl:apply-templates>
				</ul>
				<!-- End Jump Bar -->
	  
				<!-- Header -->
				<xsl:choose>
					<xsl:when test="$batchErrors = 0">
						<h2 id="results" class="valid">All documents were successfully checked!</h2>
					</xsl:when>
					<xsl:otherwise>
						<h2 id="results" class="invalid">Errors found while checking these documents!</h2>
					</xsl:otherwise>
				</xsl:choose>
				
				
				<table class="header">
					<tr>
						<th>Result:</th>
						<xsl:choose>
							<xsl:when test="$batchErrors = 0">
								<td colspan="2" style="width:70%;" class="valid">Passed</td>
							</xsl:when>
							<xsl:otherwise>
								<td colspan="2" style="width:70%;" class="invalid">Failed</td>
							</xsl:otherwise>
						</xsl:choose>
					</tr>
					<tr>
						<th>Total Errors:</th>
						<td colspan="2"><xsl:value-of select="$batchErrors"/></td>
					</tr>
					<tr>
						<th>Total Warnings:</th>
						<td colspan="2"><xsl:value-of select="sum(//w3cValidatorResults/validationResult/@warnings)"/></td>
					</tr>
					<tr>
						<th>Number of Documents:</th>
						<td colspan="2"><xsl:value-of select="count(//w3cValidatorResults/validationResult)" /></td>
					</tr>
					<tr>
						<th>Number of Domains:</th>
						<td colspan="2"><xsl:value-of select="count(validationResult[generate-id()=generate-id(key('validationResult-by-domain', @domainName)[1])])"/></td>
					</tr>
					<tr>
						<th>Started:</th>
						<td colspan="2">
							<xsl:call-template name="formatdateandtime">
								<xsl:with-param name="datestr" select="localStartTime"/>
							</xsl:call-template>
						</td>
					</tr>
					<tr>
						<th>Total Elapsed Time (HH:MM:SS):</th>
						<td colspan="2">
							<xsl:value-of select="elapsedTime"/>
						</td>
					</tr>
				</table>
				<!-- End Header -->
				
				<div id="don_program">&nbsp;</div>
				<script type="text/javascript" src="http://www.w3.org/QA/Tools/don_prog.js">&nbsp;</script>
			
				<p class="backtop"><a href="#jumpbar">&uarr; Top</a></p>
			
				<!-- Groups -->
				<div id="result">
					<xsl:apply-templates mode="group" select="validationResult[generate-id()=generate-id(key('validationResult-by-domain', @domainName)[1])]">
						<xsl:sort select="@domainName"/>
					</xsl:apply-templates>
				</div>
				<!-- End Groups -->
				
			</div>
			<!-- Footer -->
			<div id="footer">

				<p id="activity_logos">
					<a href="http://www.w3.org/Status" title="W3C's Open Source, bringing you free Web quality tools and more"><img src="http://www.w3.org/Icons/WWW/w3c_home_nb" alt="W3C" width="72" height="47" /><img src="./images/opensource-55x48.png" alt="Open-Source" title="We are building certified Open Source/Free Software. - see www.opensource.org" width="55" height="48" /></a>
				</p>

					<p id="support_logo">
						<a href="http://www.w3.org/QA/Tools/Donate">
						<img src="http://www.w3.org/QA/Tools/I_heart_validator" alt="I heart Validator logo" title=" Validators Donation Program" width="80" height="15" />
						</a>
					</p>
				<p id="version_info">
				  This service runs the W3C Markup Validator, <a href="whatsnew.html#v13"><abbr title="version">v</abbr>1.3</a>.
				</p>

					<p class="copyright">
						<a rel="Copyright" href="http://www.w3.org/Consortium/Legal/ipr-notice#Copyright">Copyright</a> &copy; 1994-2012
						<a href="http://www.w3.org/"><acronym title="World Wide Web Consortium">W3C</acronym></a>&reg;

						(<a href="http://www.csail.mit.edu/"><acronym title="Massachusetts Institute of Technology">MIT</acronym></a>,
						<a href="http://www.ercim.eu/"><acronym title="European Research Consortium for Informatics and Mathematics">ERCIM</acronym></a>,
						<a href="http://www.keio.ac.jp/">Keio</a>),
						All Rights Reserved.
						W3C <a href="http://www.w3.org/Consortium/Legal/ipr-notice#Legal_Disclaimer">liability</a>,
						<a href="http://www.w3.org/Consortium/Legal/ipr-notice#W3C_Trademarks">trademark</a>,
						<a rel="Copyright" href="http://www.w3.org/Consortium/Legal/copyright-documents">document use</a>

						and <a rel="Copyright" href="http://www.w3.org/Consortium/Legal/copyright-software">software licensing</a>

						rules apply. Your interactions with this site are in accordance
						with our <a href="http://www.w3.org/Consortium/Legal/privacy-statement#Public">public</a> and
						<a href="http://www.w3.org/Consortium/Legal/privacy-statement#Members">Member</a> privacy
						statements.
					</p>
			</div>
			<!-- End footer -->
			
			
		</body>
		</html>
	</xsl:template>
	
	<xsl:template match="validationResult">

		<div class="details">
			<table class="header">
				<tr>
					<th></th>
					<th>Document (Click for Details)</th>
					<th>Elapsed Time</th>
					<th>Errors</th>
					<th>Warnings</th>
					<th></th>
				</tr>
		
				<xsl:for-each select="key('validationResult-by-domain', @domainName)">
					<xsl:sort select="@errors" data-type="number" order="descending"/>
					<xsl:sort select="@warings" data-type="number" order="descending"/>
					
					<tr>
						<td>
							<span class="err_type">
								<xsl:choose>
									<xsl:when test="@errors &gt; 0">
										<img src="images/info_icons/error.png" alt="Error" title="Error"/>
									</xsl:when>
									<xsl:when test="@warnings &gt; 0">
										<img src="images/info_icons/warning.png" alt="Warning" title="Warning"/>
									</xsl:when>
									<xsl:otherwise>
										<img src="images/info_icons/ok.png" alt="Ok" title="Ok"/>
									</xsl:otherwise>
								</xsl:choose>
							</span>
						</td>
						<td>
							<a>
								<xsl:if test="@fileName">
									<xsl:attribute name="href">
										<xsl:value-of select="@fileName"/>
									</xsl:attribute>
								</xsl:if>
								<xsl:value-of select="@url"/>
							</a>
						</td>
						<td>
							<xsl:value-of select="elapsedTime"/>
						</td>
						<td>
							<xsl:value-of select="@errors"/>
						</td>
						<td>
							<xsl:value-of select="@warnings"/>
						</td>
						<td>
							<a>
								<xsl:attribute name="href"><xsl:value-of select="@url"/></xsl:attribute>
								View Original Document
							</a>
						</td>
					</tr>
					
					<xsl:if test="errorMessage">
						<tr>
							<td colspan="5" class="invalid">
								<xsl:value-of select="errorMessage"/>
							</td>
						</tr>
					</xsl:if>
				
				</xsl:for-each>
			
			</table>
		</div>
		
	</xsl:template>
	
	<xsl:template mode="group" match="validationResult">
		
		<xsl:variable name="var_group_errors">
			<xsl:value-of select="sum(key('validationResult-by-domain', @domainName)/@errors)"/>
		</xsl:variable>
		
		<xsl:choose>
			<xsl:when test="$var_group_errors = 0">
				<h2 id="results" class="valid">
					<xsl:attribute name="id">domain<xsl:value-of select="position()"/></xsl:attribute>
					Domain [<xsl:value-of select="@domainName"/>]: All documents were successfully checked!
				</h2>
			</xsl:when>
			<xsl:otherwise>
				<h2 id="results" class="invalid">
					<xsl:attribute name="id">domain<xsl:value-of select="position()"/></xsl:attribute>
					Domain [<xsl:value-of select="@domainName"/>]: Errors found while checking these documents!
				</h2>
			</xsl:otherwise>
		</xsl:choose>
			
		<table class="header">
			<tr>
				<th>Result:</th>
				<xsl:choose>
					<xsl:when test="$var_group_errors = 0">
						<td colspan="2" style="width:70%;" class="valid">Passed</td>
					</xsl:when>
					<xsl:otherwise>
						<td colspan="2" style="width:70%;" class="invalid">Failed</td>
					</xsl:otherwise>
				</xsl:choose>
			</tr>
			<tr>
				<th>Total Errors:</th>
				<td colspan="2"><xsl:value-of select="$var_group_errors"/></td>
			</tr>
			<tr>
				<th>Total Warnings:</th>
				<td colspan="2"><xsl:value-of select="sum(key('validationResult-by-domain', @domainName)/@warnings)"/></td>
			</tr>
			<tr>
				<th>Number of Documents:</th>
				<td colspan="2"><xsl:value-of select="count(key('validationResult-by-domain', @domainName))" /></td>
			</tr>
		</table>
			
		<p class="backtop"><a href="#jumpbar">&uarr; Top</a></p>
			
		<!-- Group Details -->
		<xsl:apply-templates select="."/>
		<!-- End Group Details -->
			
		<p class="backtop"><a href="#jumpbar">&uarr; Top</a></p>
		
	</xsl:template>

	<xsl:template mode="toc" match="validationResult">
		<li>
			<a>
				<xsl:attribute name="title"><xsl:value-of select="@domainName"/></xsl:attribute>
				<xsl:attribute name="href">#domain<xsl:value-of select="position()"/></xsl:attribute>
				<xsl:value-of select="@domainName"/>
			</a>
		</li>
	</xsl:template>

	<xsl:template name="formatdateandtime">
		<xsl:param name="datestr"/>
		<!-- input format yyyy-mm-ddThh:mm:ss.0000000TZ -->
		<!-- output format MonthName dd, yyyy  h:mm AM/PM -->

		<xsl:variable name="var_day_d">
			<xsl:value-of select="substring($datestr,9,2) - 0"/>
		</xsl:variable>
		
		<xsl:variable name="var_month_mm">
			<xsl:value-of select="substring($datestr,6,2)"/>
		</xsl:variable>

		<xsl:variable name="var_year_yyyy">
			<xsl:value-of select="substring($datestr,1,4)"/>
		</xsl:variable>

		<xsl:variable name="var_hour_hh">
			<xsl:value-of select="substring($datestr,12,2)"/>
		</xsl:variable>

		<xsl:variable name="var_minute_mm">
			<xsl:value-of select="substring($datestr,15,2)"/>
		</xsl:variable>

		<xsl:variable name="var_second_ss">
			<xsl:value-of select="substring($datestr,18,2)"/>
		</xsl:variable>

		<xsl:variable name="var_ampm">
			<xsl:choose>
				<xsl:when test="$var_hour_hh > 12">PM</xsl:when>
				<xsl:otherwise>AM</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="var_hour_12">
			<xsl:choose>
				<xsl:when test="$var_hour_hh > 12"><xsl:value-of select="$var_hour_hh - 12"/></xsl:when>
				<xsl:otherwise><xsl:value-of select="$var_hour_hh - 0"/></xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="var_month_name">
			<xsl:choose>
				<xsl:when test="$var_month_mm = 01">January</xsl:when>
				<xsl:when test="$var_month_mm = 02">February</xsl:when>
				<xsl:when test="$var_month_mm = 03">March</xsl:when>
				<xsl:when test="$var_month_mm = 04">April</xsl:when>
				<xsl:when test="$var_month_mm = 05">May</xsl:when>
				<xsl:when test="$var_month_mm = 06">June</xsl:when>
				<xsl:when test="$var_month_mm = 07">July</xsl:when>
				<xsl:when test="$var_month_mm = 08">August</xsl:when>
				<xsl:when test="$var_month_mm = 09">September</xsl:when>
				<xsl:when test="$var_month_mm = 10">October</xsl:when>
				<xsl:when test="$var_month_mm = 11">November</xsl:when>
				<xsl:when test="$var_month_mm = 12">December</xsl:when>
			</xsl:choose>
		</xsl:variable>
		

		<xsl:value-of select="$var_month_name"/>
		<xsl:value-of select="' '"/>
		<xsl:value-of select="$var_day_d"/>
		<xsl:value-of select="', '"/>
		<xsl:value-of select="$var_year_yyyy"/>
		&nbsp;&nbsp;
		<xsl:value-of select="$var_hour_12"/>
		<xsl:value-of select="':'"/>
		<xsl:value-of select="$var_minute_mm"/>
		<xsl:value-of select="' '"/>
		<xsl:value-of select="$var_ampm"/>
	</xsl:template>
	
</xsl:stylesheet>
