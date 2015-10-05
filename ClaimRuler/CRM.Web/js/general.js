var credentials = "ApjKKP3xo-UePHYy4EWVj7kzRUAx40rbKSGGCzw-E_jK2YAtyhs5Na0_PunRgr2Y";

function deformatAmount(strAmount) {
	return parseFloat(strAmount.replace(/,/g, ''));
}

function showAlert(msg) {
	$('#divAlert').html('').append('<p>' + msg + '</p>');
	$(function () {
		$('#divAlert').dialog({
			modal: true,
			title: 'Alert',
			width: 'auto',
			buttons: { "Ok": function () { $(this).dialog("close"); } }
		});
	});
}

var dialogConfirmed = false;
function ConfirmDialog(control, dialogText) {
	if (!dialogConfirmed) {
		$('body').append(String.Format("<div id='dialogConfirm' title='Confirm' <p>{0}</p></div>", dialogText));

		$('#dialogConfirm').dialog({
		    height: 150,
            width:600,
			modal: true,
			resizable: false,
			draggable: true,
			close: function (event, ui) { $('body').find('#dialogConfirm').remove(); },
			buttons:
			{
				'Yes': function () {
					$(this).dialog('close');
					dialogConfirmed = true;
					if (control) control.click();
					dialogConfirmed = false;
				},
				'No': function () {
					$(this).dialog('close');
				}
			}
		});

	}
	return dialogConfirmed;
}

function IsValidTime(source, args) {
	

	var timeStr = args.Value;

	// control returns this
	if (timeStr == ": ") {
		args.IsValid = false;
		return;
	}

	var regEx = /^(\d{1,2}):(\d{2})(:(\d{2}))?(\s?(AM|am|PM|pm))?$/;
	//var regEx = /^\d{1,2}:\d{2}([ap]m)?$/;

	var matchArray = timeStr.match(regEx);

	if (matchArray == null) { args.IsValid = false; return; }

	hour = matchArray[1];
	minute = matchArray[2];
	second = matchArray[4];
	ampm = matchArray[6];

	if (second == "") { second = null; }
	if (ampm == "") { args.IsValid = false; return;}

	if (hour < 0 || hour > 12) { args.IsValid = false; return;}

	if (minute < 0 || minute > 59) { args.IsValid = false; return;}

	args.IsValid = true;
}

String.Format = function () {
	var s = arguments[0];
	for (var i = 0; i < arguments.length - 1; i++) {
		var reg = new RegExp("\\{" + i + "\\}", "gm");
		s = s.replace(reg, arguments[i + 1]);
	}
	return s;
}

function getDatePart(d) {
	return (d.getMonth() + 1) + "/" + d.getDate() + "/" + d.getFullYear();
}
function getTimePart(d) {
	var hhmm = d.toTimeString().split(':');
	var pmam = (hhmm[0] > 12 ? " PM" : " AM");

	var hh = hhmm[0] > 12 ? hhmm[0] - 12 : hhmm[0];
	var mm = hhmm[1];

	return (hh.toString().length < 2 ? '0' + hh : hh) + ':' + mm + pmam;
	//return (d.getHours() + 1) + ":" + d.getMinutes() + (d.getHours() > 12 ? " PM" : " AM");
}

function showHideDiv(divID) {
	var divControl = '#' + divID;

	var css = $(divControl).css('display');

	if (css == 'none') {
		$(divControl).show();
	} else {
		$(divControl).hide();
	}
}
function showHide(obj, targetID, hideMsg, showMsg) {
	var targetControl = '#' + targetID;

	var css = $(targetControl).css('display');

	if (css == 'none') {
		$(targetControl).show();
		obj.innerHTML = hideMsg;
	} else {
		$(targetControl).hide();
		obj.innerHTML = showMsg;
	}
}
function jsRound(number, X)
{
	X = (!X ? 2 : X);
	return Math.round(number*Math.pow(10,X))/Math.pow(10,X);
}

function removeDollar(n)
{
	//function added by sanjeev kumar on 26 sep 2003
	var length;
	n=new String(n);
	length=n.length;
	tmpval=n.substr(0,1);
	if (tmpval=="$")
	{
		n=n.substr(1,length);
		return n;
	}	
	else
		return n.valueOf();
}
function jsCur(n)
{
	n=removeDollar(n);
	n=n.replace(",","");
	n=n.replace(",","");
	n=n.replace(",","");
	n=n.replace(",","");
	n=n.replace(",","");
	n=n.replace(",","");
	n=n.replace(",","");
	n=n.replace(",","");
	var s = Math.round(n * 100) / 100
	if (isNaN(s)==true)
		return parseFloat(0);
	else
		return s;
}

function jsTrim(strValue)
{
    	return LTrim(RTrim(strValue));
}
function LTrim(strValue)
{
	var LTRIMrgExp = /^\s */;
    return strValue.replace(LTRIMrgExp, '');
}
function RTrim(strValue)
{
	var RTRIMrgExp = /\s *$/;
    return strValue.replace(RTRIMrgExp, '');
}

function validateEmail(elementValue)
{  
	//filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
	filter = /^([A-Za-z0-9_\-\.\'])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;
	if (filter.test(elementValue)) 
	{
	  return true;
	}
	else
	{
		return false;
	}
 } 

function NavigateMail(TextBoxName)
{	
	window.navigate ('mailto:'+TextBoxName.value);
}

function PopupWindow(url)
{
	window.open(url,"","status=yes,toolbar=no,menubar=no,location=no");
}
function PopupCenter(url, title, w, h) {
	// Fixes dual-screen position                       Most browsers      Firefox
	var dualScreenLeft = window.screenLeft != undefined ? window.screenLeft : screen.left;
	var dualScreenTop = window.screenTop != undefined ? window.screenTop : screen.top;

	var left = ((screen.width / 2) - (w / 2)) + dualScreenLeft;
	var top = ((screen.height / 2) - (h / 2)) + dualScreenTop;
	var newWindow = window.open(url, title, 'locaiton=no,scrollbars=yes, width=' + w + ', height=' + h + ', top=' + top + ', left=' + left);

	
	// Puts focus on the newWindow
	if (window.focus) {
		newWindow.focus();
	}
}
function RemoveAll(selectobj)
{
	var count =  selectobj.options.length;
	for(icounter=count-1;icounter>=0; icounter--)
	{
		selectobj.options.remove(icounter);
	}
}

function ReplaceAll(Str,Str1,Str2)
{
	var r;
	try
	{
	r= parseInt(Str.search(Str1));
	while(r>=0)
	{
		Str = Str.replace(Str1,Str2);
		r= parseInt(Str.search(Str1));	
	}
	}
	catch(e){ }
	return Str;
}


jQuery.fn.center = function () {
	this.css("position","absolute");
	this.css("top", ( $(window).height() - this.height() ) / 2+$(window).scrollTop() + "px");
	
	if ( this.width() == 3)
		window_width = 990;
	else
		window_width = this.width();

	this.css("left", ( $(window).width() - window_width ) / 2+$(window).scrollLeft() + "px");
	return this;
}

function show_no_file_dialog()
{

	var message = '<div id="no_file_available_for_download" style="position:absolute;top:300px;left:450px;display:none;z-index:1023"><div class="get_aspera" align="center"><table width="465" border="0" cellspacing="0" cellpadding="0" bgcolor="#000000"><tr><td height="24"  bgcolor="#000000" ></td></tr><tr><td bgcolor="#000000"><table width="465" border="0" cellspacing="0" cellpadding="0" bgcolor="#000000"><tr><td align="center"><B><font color="white" style="font-size:12px">Though your ADS-Xpress account has the ability to download, this asset has no associated downloadable files.  If you need to download files for this asset, please contact your ADSX representative.</font></b></td></tr><tr><td align="center"><B><font color="white" style="font-size:12px">&nbsp;</font></b></td></tr><tr><td align="center"><table width="238" border="0" cellspacing="0" cellpadding="0" bgcolor="#000000" ><tr><td width="414" align="center"><a href="javascript:close_dialog(this)" onMouseOut="MM_startTimeout();MM_swapImgRestore()" onMouseOver="MM_swapImage(\'w_popup_cance_button\',\'\',\'images/close_button_over.png\',1)"><img name="w_popup_cance_button" src="images/close_button.png" title="Cancel" width="101" height="24" border="0" /></a></td></tr></table></td></tr><tr><td align="center">&nbsp;</td></tr></table><br></td></tr><tr><td height="8" bgcolor="#000000"></td></tr></table></div></div>';

	 $("body").append(message);
}

function numberWithCommas(x) {
    return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}

function isEmail(str)
{
	var at="@";
	var dot=".";
	var lat=str.indexOf(at);
	var ldot=str.indexOf(dot);
	var lstr=str.length;

	if(str.indexOf(at)==-1 || str.indexOf(at)==0 || str.indexOf(at)==lstr){
		return false;
	}
	if(str.indexOf(dot)==-1 || str.indexOf(dot)==0 || str.indexOf(dot)==lstr){
		return false;
	}
	if(str.indexOf(" ")!=-1){
		return false;
	}
	if(str.indexOf(at,(lat+1))!=-1){
		return false;
	}
	if(str.indexOf(dot,(lat+2))==-1){
		return false;
	}
	if(str.substring(lat-1,lat)==dot || str.substring(lat+1,lat+2)==dot){
		return false;
	}
	return true;
}

function isTelephone(str)
{
	var validNum="0123456789-";
	var isValid=true;
	var char;

	for (var i=0;i<str.length && isValid==true;i++)
	{
		char=str.charAt(i);
		if(validNum.indexOf(char)==-1)
		{
			isValid=false;
		}
	}
	return isValid;
}

function validateUserID(myid)
{
	specialchars=".,|!@#$%^&*()~`'?;: ";
	for (var i = 0; i < myid.length; i++) 
	{
		if (specialchars.indexOf(myid.charAt(i)) != -1) 
		{
			return false;
		}
	}
}
function Left(str, n){
	if (n <= 0)
	    return "";
	else if (n > String(str).length)
	    return str;
	else
	    return String(str).substring(0,n);
}
function Right(str, n){
    if (n <= 0)
       return "";
    else if (n > String(str).length)
       return str;
    else {
       var iLen = String(str).length;
       return String(str).substring(iLen, iLen - n);
    }
}

function IsNumeric(sText)
{
   var ValidChars = "0123456789.";
   var IsNumber=true;
   var Char;
   for (i = 0; i < sText.length && IsNumber == true; i++) 
      { 
      Char = sText.charAt(i); 
      if (ValidChars.indexOf(Char) == -1) 
         {
         IsNumber = false;
         }
      }
   return IsNumber;
}

function GotoNext(Jump,Start)
{
	document.form1.StartingPoint.value = Start
	document.form1.PageNo.value=parseInt(Jump,10)
	document.form1.ShowData.value = "TRUE"
	document.form1.submit()

}
