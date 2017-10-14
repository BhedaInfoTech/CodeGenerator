var InitializeAutoComplete = function (elementObject, callBack) {
    $(elementObject).autocomplete({
        source: function (request, response) {

            var urlString = ''
           

            if ($(elementObject).attr("id") == 'txtEnquiryNo') {
                urlString = "/report/get-enquiry-no/" + $('#txtEnquiryNo').val();
            }

            // Start : Added by Kaustubh | Date : 02/25/2015
            if ($(elementObject).attr("id") == 'txtItemName') {
                urlString = "/report/get-item-names/" + $('#txtItemName').val();
            }
            // End by Kaustubh

            $.ajax({
                url: urlString,
                dataType: "json",
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.Label,
                            value: item.Value
                        }
                    }));
                }
            });
        },
        minLength: 2,
        focus: function (event, ui) {
            $(this).val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $(this).val(ui.item.label);
            callBack(ui);
            return false;
        },
        open: function () {
            $(this).removeClass("ui-corner-all").addClass("ui-corner-top");
        },
        close: function () {
            $(this).removeClass("ui-corner-top").addClass("ui-corner-all");
        }
    });
}