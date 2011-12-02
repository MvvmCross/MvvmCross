<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<BestSellers.CategoryList>" %>
<html>

<head>
<style type="text/css">
.iMore:not(.__lod):active, li.iRadio.__sel, .iMenu li.__sel, .iList li.__sel
        {
            background-color: #000099;
        }
</style>
<title>Best Sellers</title>
<meta content="width=device-width; initial-scale=1.0; maximum-scale=1.0; user-scalable=0;" name="viewport">
<meta content="yes" name="apple-mobile-web-app-capable">
<meta content="black" name="apple-mobile-web-app-status-bar-style">
<meta content="chrome=1" http-equiv="X-UA-Compatible">
<link href="../../appIcon.png" rel="apple-touch-icon">
<link href="../../appSplash.png" rel="apple-touch-startup-image">
<script src="../../WebApp/Action/DebugLogic.js" type="text/javascript"></script>
<link href="../../WebApp/Design/Render.css" rel="stylesheet">
<link href="../../WebApp/Design/Firefox.css" rel="stylesheet">
<link href="../../WebApp/Design/Render.css" rel="stylesheet">
<script src="../../WebApp/Action/spinningwheel.js" type="text/javascript"></script>
<link href="../../WebApp/Design/spinningwheel.css" rel="stylesheet">
<script type="text/javascript">function searchLoader() { WA.Loader(&#39;searchSpan&#39;, 1); } function actionLoader() { WA.Loader(&#39;actionSpan&#39;, 1); } </script>
</head>

<body bgcolor="#AEBAC2">

<div id="WebApp" class="landscape" style="background-color: rgb(174, 186, 194); min-height: 764px;">
	<div id="iHeader" style="background-color: #000000; color: #FFFFFF">
		<div style="opacity: 1; display: block;">
			<span id="waHeadTitle" style="display: block;">Best Sellers</span><a id="waHomeButton" href="#" style="display: none;">Home</a><a id="waBackButton" href="#" style="display: none;">Back</a><form id="searchForm" class="iForm">
				<a id="gogo" class="iBClassic " href="#_NoLayer_" onclick="WA.Submit('searchForm'); searchLoader();" rel="action">
				Search</a><a class="iBClassic " href="#" onclick="document.getElementById('searchCriteria').value = null;  WA.Submit('searchForm');" rel="back">Cancel</a><fieldset>
				<legend>Search</legend>
				<input id="searchCriteria" name="searchCriteria" placeholder="Search term here" type="search"></fieldset>
			</form>
		</div>
	</div>
	<div id="iGroup">
		<div id="waBestSellers.CategoryList" class="iLayer" style="left: 0px; display: block;" title="Categories">
			<div class="iMenu">
				<h3>New York Times Best Sellers</h3>
				<ul class="iArrow" style="background-color: #FFFFFF; color: #000000">
                <% foreach (BestSellers.Category category in Model)
                   { %>
					<li><a href="<%Response.Write(HttpUtility.UrlPathEncode(category.DisplayName));%>" rev="async">
                        <img src="../../NytIcon.png" style="max-height: 44px; max-width: 32px" alt="">
                        <em><%Response.Write(category.DisplayName);%></em><small class="iFactrSmall" style="color: #666666"></small></a>
                    </li>
                <% } %>
				</ul>
			</div>
		</div>
		<div id="wa__radio" class="iLayer">
		</div>
	</div>
	<div id="iFooter">
	</div>
	<div id="__wa_shadow">
	</div>
</div>

</body>

</html>

