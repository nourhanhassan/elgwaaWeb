
(function ($) {

    $.fn.QvAjax = function (options) {

        var setting = {
            "sender": null,
            "url": "Default.aspx/Save",
            "jsonData": "",
            "success": "",
            "error": null,
            "debug": false
        };

        if (options) { $.extend(setting, options); }
        if (setting.sender != null)
            setting.sender.preventDefault();
        var json = [];
        params = setting.jsonData
        for (var param in params)
            json.push("'" + param + "':'" + params[param] + "'");

        json = "{" + json.join(',') + "}";
        
        $.ajax({
            type: "POST",
             url: setting.url,
            data: json,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {
                setting.success(data);
                if (setting.debug)
                    console.log(data);
            },
            error: function (data) {
                console.log(data.responseText)
                if (!setting.error && setting.error == null) {
                    UserControl_ShowMessage('ErrorSave', '');
                    // alert(data.responseText)
                }
                else {
                    setting.error();
                }
                if (setting.debug)
                    console.log(data);
            }
        });        // end of ajax
    }



    // plugin to add new row to ListView when insert new object
    $.fn.AddRow = function (options) {
        debugger 
        var setting = {
            "id": "0",    // identity of inserted data
            "object": null    // row object (data in tds of table)
        };

        if (options) { $.extend(setting, options); }


        var str = '<tr id="tr_' + setting.id + '">';


        for (var key in setting.object) {
            var td_class = '';
            if (key.indexOf('class') != -1) {
                if (key.indexOf('update') != -1)
                    td_class = 'action1'
                else if (key.indexOf('delete') != -1)
                    td_class = 'action2'
            }
            if (key == 'update' || key == 'delete' || key.indexOf('center') != -1)
                str += '<td class="' + td_class + '" style="text-align:center;">' + setting.object[key] + '</td>';
            else if (key.indexOf('left') != -1)
                str += '<td class="' + td_class + '" style="text-align:left;">' + setting.object[key] + '</td>';
            else if (key.indexOf('right') != -1)
                str += '<td class="' + td_class + '" style="text-align:right;">' + setting.object[key] + '</td>';
            else
                str += '<td class="' + td_class + '">' + setting.object[key] + '</td>';
        }
        str += '</tr>';
       // $(this).append(str);
        $(this).find('tr:eq(0)').before(str);
       // $("#mainTable tr:first").after(row);

        if ($('.empty_row'))
            $('.empty_row').remove();

    } // end of add new row




    // plugin to update  row in ListView when update existing object
    $.fn.UpdateRow = function (options) {

        var setting = {
            "id": "0",    // id of table row
            "object": null    // row object (data in tds of table)
        };

        if (options) { $.extend(setting, options); }

        var i = -1;
        for (var key in setting.object) {
            i++;
            $($('#tr_' + setting.id).children('td')[i]).html(setting.object[key]);
        }
    }  // end  of update plugin


    //this plugin to fill drop down list with json
    $.fn.addItems = function (data) {
        return this.each(function () {
            var list = this;
            $.each(data, function (index, itemData) {
                var option = new Option(itemData.Text, itemData.Value);
                list.add(option);
            });
        });
    };

    //this method to check an arry contains element
    $.fn.ArrayContains = function (element) {
        var i = this.length;
        while (i--) {
            if (this[i] === element) {
                return true;
            }
        }
        return false;
    };


    // to validate required select element
    $.fn.RequiredSelectValid = function () {
        if (this.val() == 0) {
            this.nextAll('span.required').show();
            return false;
        }
        else {
            this.nextAll('span.required').hide();
            return true;
        }
    };

    // to validate text with regular expression
    $.fn.TextValid = function (exp) {
        if (this.val().trim() == '' || this.val().match(exp)) {
            this.nextAll('span.regular').hide();
            return true;
        }
        else {
            this.nextAll('span.regular').show();
            return false;
        }
    };


    // to validate required text 
    $.fn.RequiredTextValid = function () {
        if (this.val().trim() == '') {
            this.nextAll('span.required').show();
            return false;
        }
        else {
            this.nextAll('span.required').hide();
            return true;
        }
    };
    // log
    $.fn.log = function () {
        console.log(this);
    };
})(jQuery);

var i=0;

function OnItemDataBound(row) {
    $('.gridHolder table tbody tr:even').addClass('even');
}



/////////////////////////////
//Compare HIjriDate 
//in format of dd/mm/yyyy
//return 0 if equl
//1 if first greater
//-1 if first smaller
//-2 if error 
function HijridateCompare(date1, date2) {
    //
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
}


