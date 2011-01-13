// 0-> EXSummary, 1-> NLSummary, 2-> FLSummary

function GetPTRSummary(summary_type)
{
    // window.alert("GetPTRSummary");
    var strDate = makeDate();
    var Rate = getRate();
    var summariString = getSummary(summary_type);

    const CLIPBOARD = Components.classes["@mozilla.org/widget/clipboardhelper;1"].getService(Components.interfaces.nsIClipboardHelper);
    CLIPBOARD.copyString("R" + Rate + summariString + strDate);
}

// “ú•t‚ğ¶¬‚·‚é
function makeDate()
{
    // window.alert("makeDate");
    dd = new Date();
    yy = dd.getYear();
    mm = dd.getMonth() + 1;
    dd = dd.getDate();
    if (yy < 2000) { yy += 1900; }
    if (mm < 10) { mm = "0" + mm; }
    if (dd < 10) { dd = "0" + dd; }
    return yy + "/" + mm + "/" + dd;
}

// Rate‚ğæ“¾‚·‚é
function getRate()
{
    // window.alert("getRate");
    var d = window._content.document;
    rObj = new RegExp("<div class=\"ov_main_rate\".*?>(.*?)</div>");
    rObj.ignoreCase = true;
    d.body.innerHTML.match(rObj);
    // window.alert(RegExp.$1);
    return RegExp.$1;
}

// Earn, BB100, Hands‚ğæ“¾‚·‚é
function getSummary(summary_type)
{
    // window.alert("getSummary");
    var BB_sum = 0;
    var Earn = 0;
    var Hands = 0;
    var target_table = window._content.document.body.getElementsByTagName("table")[0];
    // window.alert(target_table.innerHTML);
    // window.alert(target_table.rows.length);
    for(var i = 1; i < target_table.rows.length; ++i)
    {
        var stakesName = target_table.rows[i].cells[0];
        while(stakesName.childNodes.length > 0) stakesName = stakesName.childNodes[0];
        // window.alert(stakesName.nodeValue);
        if(is10maxNLabove(stakesName.nodeValue))
        {
           var tmp_hand = target_table.rows[i].cells[1];
           while(tmp_hand.childNodes.length > 0) tmp_hand = tmp_hand.childNodes[0];
           Hands += eval(tmp_hand.nodeValue.replace(",", ""));
           
           var tmp_bb = target_table.rows[i].cells[3];
           while(tmp_bb.childNodes.length > 0) tmp_bb = tmp_bb.childNodes[0];
           BB_sum += eval(tmp_bb.nodeValue.replace(",", "")) * eval(tmp_hand.nodeValue.replace(",", ""));
           
           var tmp_earn = target_table.rows[i].cells[2];
           while(tmp_earn.childNodes.length > 0) tmp_earn = tmp_earn.childNodes[0];
           Earn += eval(tmp_earn.nodeValue.replace("\$", "").replace(",", ""));
        }
    }
    var BB100 = Math.round(BB_sum * 100 / Hands) / 100;

    return "  H " + Hands + "  $ " + Earn + "  BB " + BB100.toFixed(2) + "  ";
}

// 10max‚æ‚è‘å‚«‚¢NL‚©”»’è‚·‚é
function is10maxNLabove(stakesName)
{
    var ret = true;

    if (stakesName.indexOf("NLH") < 0) ret = false;
    else if (stakesName.indexOf("HU") > -1) ret = false;
    else if (stakesName.indexOf("0.02") > -1) ret = false;
    else if (stakesName.indexOf("0.05") > -1) ret = false;

    return ret;
}