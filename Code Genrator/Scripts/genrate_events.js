
$(function () {

    $("#btnGetTables").click(function () {

        $.ajax({
            url: "/Home/Get_Table_Names",

            data: { DBName: $("#txtDBName").val()},

            method: 'GET',

            async: false,

            success: function (data) {
              
                    alert("Success");

                    BindTables(data);
               
            }
        });
    });

    $("#drpTables").change(function ()
    {
        if($(this).val() != "")
        {
            $("#btnGenrate").show();
        }
        else
        {
            $("#btnGenrate").hide();
        }
    });

    $("#btnGenrate").click(function () {

        $("#dvCode").load("/Home/GenrateCode", { DBName: $("#txtDBName").val(), TableName: $("#drpTables").val() }, AfterGenerateClick)

    });

    function AfterGenerateClick()
    {
        //alert();

        $("#dvCode").empty();

        alert("Code Generated");

    }

});