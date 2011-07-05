// 0-> EXSummary, 1-> NLSummary, 2-> FLSummary, 3-> PLOSummary

function GetPTRSummary(summary_type)
{
    // window.alert("GetPTRSummary");
    var strDate = makeDate();
    var Rate = getRate();
    var summariString = getSummary(summary_type);

    const CLIPBOARD = Components.classes["@mozilla.org/widget/clipboardhelper;1"].getService(Components.interfaces.nsIClipboardHelper);
    CLIPBOARD.copyString("R:" + Rate + summariString + strDate);
}

// ì˙ïtÇê∂ê¨Ç∑ÇÈ
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

// RateÇéÊìæÇ∑ÇÈ
function getRate()
{
    // window.alert("getRate");
    var d = window._content.document;
    rObj = new RegExp("<div id=\"number_reading\">(.*?)</div>");
    rObj.ignoreCase = true;
    d.body.innerHTML.match(rObj);
    // window.alert(RegExp.$1);
    return RegExp.$1;
}

// Earn, BB100, HandsÇéÊìæÇ∑ÇÈ
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
        if(isSummaryType(stakesName.nodeValue, summary_type))
        {
           var tmp_hand = target_table.rows[i].cells[1];
           while(tmp_hand.childNodes.length > 0) tmp_hand = tmp_hand.childNodes[0];
           Hands += eval(tmp_hand.nodeValue.replace(/,/g, ""));
           
           var tmp_bb = target_table.rows[i].cells[3];
           while(tmp_bb.childNodes.length > 0) tmp_bb = tmp_bb.childNodes[0];
           BB_sum += eval(tmp_bb.nodeValue.replace(/,/g, "")) * eval(tmp_hand.nodeValue.replace(/,/g, ""));
           
           var tmp_earn = target_table.rows[i].cells[2];
           while(tmp_earn.childNodes.length > 0) tmp_earn = tmp_earn.childNodes[0];
           Earn += getNumOnly(tmp_earn.nodeValue.replace(/,/g, ""));
        }
    }
    var BB100 = Math.round(BB_sum * 100 / Hands) / 100;

    return ", H:" + Hands + ", $:" + Earn + ", BB:" + BB100.toFixed(2) + ", ";
}

// summary_type == 0(EX) 10maxÇÊÇËëÂÇ´Ç¢NLÇ©îªíËÇ∑ÇÈ
// summary_type == 1(NL)
// summary_type == 2(FL)
// summary_type == 3(PLO)
// summary_type == 4(HU)
function isSummaryType(stakesName, summary_type)
{
    var ret = true;

    if(summary_type == 0)
    {
        if (stakesName.indexOf("NLH") < 0) ret = false;
        else if (stakesName.indexOf("HU") > -1) ret = false;
        else if (stakesName.indexOf("0.02") > -1) ret = false;
        else if (stakesName.indexOf("0.05") > -1) ret = false;
    }
    else if(summary_type == 1)
    {
        if (stakesName.indexOf("NLH") < 0) ret = false;
    }
    else if(summary_type == 2)
    {
        if (stakesName.indexOf("FLH") < 0) ret = false;
    }
    else if(summary_type == 3)
    {
        if (stakesName.indexOf("PLO") < 0) ret = false;
        else if (stakesName.indexOf("HU") > -1) ret = false;
        else if (stakesName.indexOf("0.02") > -1) ret = false;
        else if (stakesName.indexOf("0.05") > -1) ret = false;
    }
    else if(summary_type == 4)
    {
        if (stakesName.indexOf("NLH HU") < 0) ret = false;
    }
    else ret = false;

    return ret;
}

// êîílÇÃÇ›ÇéÊÇËèoÇ∑
function getNumOnly(str)
{
    if(str.search(/(-?[0-9]+)/) != -1)
        return Number(RegExp.$1);
    else if(!isNaN(str))
        return Number(str);

    return 0;
}

function GetPTRAllSummary()
{
    var strDate = makeDate();
    // var Rate = getRate();
    var summariString = getAllSummary();

    const CLIPBOARD = Components.classes["@mozilla.org/widget/clipboardhelper;1"].getService(Components.interfaces.nsIClipboardHelper);
    CLIPBOARD.copyString("R:" + Rate + summariString);
}

function getHand(target_table_row_cell)
{
    var tmp_hand = target_table_row_cell;
    while(tmp_hand.childNodes.length > 0) tmp_hand = tmp_hand.childNodes[0];
    var Hand = eval(tmp_hand.nodeValue.replace(/,/g, ""));
    return Hand;
}

function getBB(target_table_row_cell, Hand)
{
    var tmp_bb = target_table_row_cell;
    while(tmp_bb.childNodes.length > 0) tmp_bb = tmp_bb.childNodes[0];
    BB = eval(tmp_bb.nodeValue.replace(/,/g, "")) * Hand;
    return BB;
}

function getEarn(target_table_row_cell)
{
    var tmp_earn = target_table_row_cell;
    while(tmp_earn.childNodes.length > 0) tmp_earn = tmp_earn.childNodes[0];
    var Earn = getNumOnly(tmp_earn.nodeValue.replace(/,/g, ""));
    return Earn;
}

function getAllSummary()
{
    var BB_sum = 0;
    var Earns = 0;
    var Hands = 0;
    var HUBB_sum = 0;
    var HUHands = 0;
    var OBB_sum = 0;
    var OHands = 0;
    var target_table = window._content.document.body.getElementsByTagName("table")[0];

    for(var i = 1; i < target_table.rows.length; ++i)
    {
        var stakesName = target_table.rows[i].cells[0];
        while(stakesName.childNodes.length > 0) stakesName = stakesName.childNodes[0];

        if(isSummaryType(stakesName.nodeValue, 0))
        {
           var Hand = getHand(target_table.rows[i].cells[1]);
           Hands += Hand;
           
           BB_sum += getBB(target_table.rows[i].cells[3], Hand);
           
           Earns += getEarn(target_table.rows[i].cells[2]);
        }
        else if(isSummaryType(stakesName.nodeValue, 4))
        {
           var HUHand = getHand(target_table.rows[i].cells[1]);
           HUHands += HUHand;
           
           HUBB_sum += getBB(target_table.rows[i].cells[3], HUHand);
        }
        else if(isSummaryType(stakesName.nodeValue, 3))
        {
           var OHand = getHand(target_table.rows[i].cells[1]);
           OHands += OHand;
           
           OBB_sum += getBB(target_table.rows[i].cells[3], OHand);
        }
    }
    var BB100 = Math.round(BB_sum * 100 / Hands) / 100;
    var HUBB100 = Math.round(HUBB_sum * 100 / HUHands) / 100;
    var OBB100 = Math.round(OBB_sum * 100 / OHands) / 100;

    return ", H:" + Hands + ", $:" + Earns + ", BB:" + BB100.toFixed(2) + ", " + makeDate() +
           ", HUBB:" + HUBB100.toFixed(2) + ", HUH:" + HUHands + ", OBB:" + OBB100.toFixed(2) + ", OH:" + OHands;
}
