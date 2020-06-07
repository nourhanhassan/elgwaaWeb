
var ControlPanelApp = {

    server: function (url, data, callback, async) {

        $.ajax({
            type: 'POST',
            url: url,
            async: typeof async !== 'undefined' ? async : false,
            data: typeof data !== 'undefined' ? data : "",
            dataType: 'json',
            contentType: 'application/json',
            success: function (data) {
                callback(data.d);
            }
        });
    },

    HijridateCompare: function (date1, date2) {
        if (date1.split('/').length < 3 || date2.split('/').length < 3)
            return -2;
        if (parseInt(date1.split('/')[2]) > parseInt(date2.split('/')[2]))
            return 1;
        else if (parseInt(date1.split('/')[2]) < parseInt(date2.split('/')[2]))
            return -1;
        else if (parseInt(date1.split('/')[1]) > parseInt(date2.split('/')[1]))
            return 1;
        else if (parseInt(date1.split('/')[1]) < parseInt(date2.split('/')[1]))
            return -1;
        else if (parseInt(date1.split('/')[0]) > parseInt(date2.split('/')[0]))
            return 1;
        else if (parseInt(date1.split('/')[0]) < parseInt(date2.split('/')[0]))
            return -1;
        else return 0;

        //Compare HIjriDate 
        //in format of dd/mm/yyyy
        //return 0 if equl
        //1 if first greater
        //-1 if first smaller
        //-2 if error 
    },

    getQueryString: function (name) {
        name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
        var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
            results = regex.exec(location.search);
        return results == null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
    },

    FormatCalendar: function () {
        $(".date").calendarsPicker({
            changeMonth: true,//this option for allowing user to select month
            changeYear: true, //this option for allowing user to select from year range
            dateFormat: 'yyyy-mm-dd',
            calendar: $.calendars.instance('ummalqura')
        });
        $(".date").css({ "cursor": "pointer" });
        $(".date").attr({ "readonly": "readonly" });
    },

    Numeric: function () {
        $('.Numeric').on('keydown', function (event) {
            if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 27 ||
                (event.keyCode == 65 && event.ctrlKey === true) ||
                (event.keyCode >= 35 && event.keyCode <= 39)) {
                return;
            }
            else {
                if ((event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105)) {
                    event.preventDefault();
                }
            }
        });
    },

    BackTop: function () {
        $('#back-to-top').click(function (event) {
            event.preventDefault();
            $('html, body').animate({ scrollTop: 0 }, 500);
            return false;
        });
    },

    //Disable the save button and shows loading icon beside the save button
    //Parameters:
    //btn : (Optional) the button to disable and show the loading icon beside, if not set, defaults to "#btn_save" button
    showLoadingOnSave: function (btn) {
        if (typeof (btn) == "undefined") {
            btn = $("#btn_save");
        }

        var loadingImage = $(btn).find(".save-loading-image");
        if (loadingImage.length == 0) {
            $(btn).append('<img src="/Areas/ControlPanel/Content/img/AjaxLoader.gif" style="width:20px" class="save-loading-image" />');
        }
        $(btn).find(".save-loading-image").show();
        $(btn).attr("disabled", "disabled");
    },

    //Enable the save button and hides loading icon beside the save button
    //Parameters:
    //btn : (Optional) the button to enable and hide the loading icon for, if not set, defaults to "#btn_save" button
    hideLoadingOnSave: function (btn) {

        if (typeof (btn) == "undefined") {
            btn = $("#btn_save");
        }

        $(btn).find(".save-loading-image").hide();
        $(btn).removeAttr("disabled");
    },


    countChar: function (val, tragetLength, tragetElement) {
        var len = val.value.length;
        $('#' + tragetElement + '').html(tragetLength - len);
    },

    setUploaderPopups: function () {
        $('.modal').on('shown.bs.modal', function (e) {
            $(this).find(".uploader_input").trigger("change");

        })
    }
}


ControlPanelApp.FormatCalendar();
ControlPanelApp.Numeric();
ControlPanelApp.BackTop();
ControlPanelApp.setUploaderPopups();