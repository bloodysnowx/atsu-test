var pnir = new ActiveXObject("Sleipnir.API");
var id = pnir.GetDocumentID(pnir.ActiveIndex);

var document = pnir.GetDocumentObject(id);
var window = pnir.GetWindowObject(id);

var strDate = makeDate();
var Rate = getRate();
var Earn, BB100, Hands, Rate;
getSummary();

clipboardData.setData("Text", "R:" + Rate + ", H:" + Hands + ", $:" + Earn + ", BB:" + BB100.toFixed(2) + ", " + strDate);

// “ú•t‚ğ¶¬‚·‚é
function makeDate()
{
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
    rObj = new RegExp("<DIV id=number_reading>(.*?)</DIV>");
    rObj.ignoreCase = true;
    document.body.innerHTML.match(rObj);
    return RegExp.$1;
}

// Earn, BB100, Hands‚ğæ“¾‚·‚é
function getSummary()
{
    var BB_sum = 0;
    Earn = 0;
    Hands = 0;
    var target_table = document.getElementsByTagName("table")[0];
    for(var i = 1; i < target_table.rows.length; ++i)
    {
        var stakesName = target_table.rows[i].cells[0];
        while(stakesName.childNodes.length > 0) stakesName = stakesName.childNodes[0];
        if(is10maxNLabove(stakesName.toString()))
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
    BB100 = Math.round(BB_sum * 100 / Hands) / 100;
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

// ”’l‚Ì‚İ‚ğæ‚èo‚·
function getNumOnly(str)
{
    if(str.search(/(-?[0-9]+)/) != -1)
        return Number(RegExp.$1);
    else if(!isNaN(str))
        return Number(str);

    return 0;
}