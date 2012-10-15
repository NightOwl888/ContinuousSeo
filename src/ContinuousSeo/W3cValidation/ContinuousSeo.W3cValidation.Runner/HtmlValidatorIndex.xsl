<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE xsl:stylesheet [
	<!ENTITY hellip "&#8230;">
	<!ENTITY copy "&#169;">
	<!ENTITY reg "&#174;">
	<!ENTITY uarr "&#8593;">
	<!ENTITY nbsp "&#160;">
]>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:key name="validationResult-by-domain" match="validationResult" use="@domainName" />
	<xsl:output method="html" encoding="utf-8" indent="yes"/>
	
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
			<link rev="made" href="mailto:ww-validator@w3.org" />
			<link rev="start" href="./" title="Home Page" />
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
	
				<ul class="navbar" id="jumpbar">
					<li>
						<strong>Jump To:</strong>
					</li>
					<xsl:apply-templates mode="toc" select="validationResult[generate-id()=generate-id(key('validationResult-by-domain', @domainName)[1])]">
						<xsl:sort select="@domainName"/>
					</xsl:apply-templates>
				</ul>
	  
				
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
				</table>
				
				<div id="don_program">&nbsp;</div>
				<script type="text/javascript" src="http://www.w3.org/QA/Tools/don_prog.js">&nbsp;</script>
			
				<p class="backtop"><a href="#jumpbar">&uarr; Top</a></p>
			
				<!-- Groups -->
				<!--<xsl:copy>-->
				<div id="result">
					<xsl:apply-templates mode="group" select="validationResult[generate-id()=generate-id(key('validationResult-by-domain', @domainName)[1])]">
						<xsl:sort select="@domainName"/>
					</xsl:apply-templates>
				</div>
					
				<!--</xsl:copy>-->
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
					
					<xsl:if test="@errorMessage">
						<tr>
							<td colspan="5" class="invalid">
								<xsl:value-of select="@errorMessage"/>
							</td>
						</tr>
					</xsl:if>
				
				</xsl:for-each>
			
			</table>
		</div>
		
		<!--</xsl:for-each>-->
		<!--</xsl:copy>-->
	</xsl:template>
	
	<xsl:template mode="group" match="validationResult">
		<xsl:variable name="var_group_errors">
			<!--<xsl:copy>
				<sum>-->
					<xsl:value-of select="sum(key('validationResult-by-domain', @domainName)/@errors)"/>
				<!--</sum>
			</xsl:copy>-->
		</xsl:variable>
		
		<!--<div class="group_container">
			<xsl:attribute name="id">domain<xsl:value-of select="position()"/></xsl:attribute>-->
		
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
				
				<!--<xsl:copy>-->
					<tr>
						<th>Total Warnings:</th>
						<td colspan="2"><xsl:value-of select="sum(key('validationResult-by-domain', @domainName)/@warnings)"/></td>
					</tr>
					<tr>
						<th>Number of Documents:</th>
						<td colspan="2"><xsl:value-of select="count(key('validationResult-by-domain', @domainName))" /></td>
					</tr>
				<!--</xsl:copy>-->
			</table>
			
			<p class="backtop"><a href="#jumpbar">&uarr; Top</a></p>
			
			<!-- Group Details -->
			
				
					<!--<xsl:copy>-->
						<xsl:apply-templates select="."/>
					<!--</xsl:copy>-->
				
			
			<p class="backtop"><a href="#jumpbar">&uarr; Top</a></p>
		
		<!--</div>-->
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
	

	
	
</xsl:stylesheet>
