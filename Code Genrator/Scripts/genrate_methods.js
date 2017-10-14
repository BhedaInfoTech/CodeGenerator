
function BindTables(data)
{
    $("#drpTables").show();
    var htmlText = "";
    htmlText += "<option value=''>- Select table-</option>"
    for (var i = 0; i < data.Tables.length; i++)
    {
        htmlText += "<option value='" + data.Tables[i] + "'>" + data.Tables[i] + "</option>";
    }

    $("#drpTables").html(htmlText);
}