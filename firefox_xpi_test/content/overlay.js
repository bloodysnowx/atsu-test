var GetPTRSummary =
{
    GetSummary: function()
    {
        var target_table = window.content.document.getElementsByTagName("table")[0];
        // window.alert(target_table.tBodies.item(0).rows.item(0).toString());
        window.alert(target_table.rows.length);
        for ( var i = 0; i < target_table.rows.length; ++i)
        {
            // var src_html = window.document.documentElement.innerHTML;
            var src_html = target_table.rows[i].innerHTML;
            window.alert(src_html);
        }
    },
};