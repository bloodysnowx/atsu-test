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
    // window.alert("GetPTRAllSummary");
    // var strDate = makeDate();
    // var Rate = getRate();
    var summaryString = getAllSummary();
    // window.alert(summaryString);
    summaryString = "R:" + getRate() + summaryString;
    // window.alert(summaryString);
    const CLIPBOARD = Components.classes["@mozilla.org/widget/clipboardhelper;1"].getService(Components.interfaces.nsIClipboardHelper);
    CLIPBOARD.copyString(summaryString);
}

function getHand(target_table_row_cell)
{
    // window.alert("getHand");
    var tmp_hand = target_table_row_cell;
    while(tmp_hand.childNodes.length > 0) tmp_hand = tmp_hand.childNodes[0];
    var Hand = eval(tmp_hand.nodeValue.replace(/,/g, ""));
    // window.alert(Hand);
    return Hand;
}

function getBB(target_table_row_cell, Hand)
{
    // window.alert("getBB");
    var tmp_bb = target_table_row_cell;
    while(tmp_bb.childNodes.length > 0) tmp_bb = tmp_bb.childNodes[0];
    BB = eval(tmp_bb.nodeValue.replace(/,/g, "")) * Hand;
    // window.alert(BB);
    return BB;
}

function getEarn(target_table_row_cell)
{
    // window.alert("getEarn");
    var tmp_earn = target_table_row_cell;
    while(tmp_earn.childNodes.length > 0) tmp_earn = tmp_earn.childNodes[0];
    var Earn = getNumOnly(tmp_earn.nodeValue.replace(/,/g, ""));
    // window.alert(Earn);
    return Earn;
}

function getAllSummary()
{
    // window.alert("getAllSummary");
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
    // window.alert("BB_sum = " + BB_sum);
    // window.alert("HUBB_sum = " + HUBB_sum);
    // window.alert("OBB_sum = " + OBB_sum)
    // window.alert("Hands = " + Hands);
    // window.alert("HUHands = " + HUHands);
    // window.alert("OHands = " + OHands)
    var BB100 = 0;
    if(Hands > 0) BB100 = Math.round(BB_sum * 100 / Hands) / 100;
    var HUBB100 = 0;
    if(HUHands > 0) HUBB100 = Math.round(HUBB_sum * 100 / HUHands) / 100;
    var OBB100 = 0;
    if(OHands > 0) OBB100 = Math.round(OBB_sum * 100 / OHands) / 100;

    // window.alert("BB100 = " + BB100);
    // window.alert("HUBB100 = " + HUBB100);
    // window.alert("OBB100 = " + OBB100);
    var summaryStr = ", H:" + Hands + ", $:" + Earns + ", BB:" + BB100.toFixed(2) + ", " + makeDate();
    // window.alert(summaryStr);
    summaryStr += ", HUBB:" + HUBB100.toFixed(2) + ", HUH:" + HUHands;
    // window.alert(summaryStr);
    summaryStr += ", OBB:" + OBB100.toFixed(2) + ", OH:" + OHands;
    // window.alert(summaryStr);
    return summaryStr;
}
