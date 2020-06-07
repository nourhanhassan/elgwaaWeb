//========== * required indicator ====================
$(function () {
    debugger
  //  $('[data-val-required]:not(.notRequiredVal)').each(function (index, element) { $(element).parents('.form-group').find('label').eq(0).after('<span class="field-validation-indicator valspan">*</span>'); })
    $('[data-val-required]:not(.notRequiredVal)').each(function (index, element) { $(element).parents('.form-group').find('label').eq(0).after('<span class="field-validation-indicator">*</span>'); })
});

//========== remain characters ====================
$(function () {
    var maxLengthMsg = 'متبقي لديك {0} حرفاً';
    $('[data-max-length]').each(function () {
        // configer
        var maxLengthlbl = $(this).parent().find('[data-max-length-msg]');
        $(this).bind('change keyup', function () {
            $(maxLengthlbl).text(maxLengthMsg.replace('{0}', $(this).attr('data-max-length') - $(this).val().length));
        });
        // init
        $(this).change();
    });
});
//========== Custom Validation for arbitrage ====================
$.validator.addMethod("CheckDuplicateName", function (value, element, options) {
    var count = 0;
    $("." + options[0]).each(function () {
        if ($(this).val() == value) {
            count++
        }
    })
    return count < 2;
});

//========== if the difference  of given two date elements is less than a number or bigger than other  ====================
//Note that it deals with hijryDate not gregorian
$.validator.addMethod("checkDateRange", function (value, element, options) {
    var startDate = new Date(($(options[0]).val())).getTime();
    var endDate = new Date(($(options[1]).val())).getTime();
    var diff = (startDate - endDate)*(-1);
    var oneDay = 24 * 60 * 60 * 1000;
    //if less than 1 month or greater than 10 years 
    if (diff < oneDay * 30 || diff > oneDay * 3650)
    {
        return false;
    }
    return true;
});
//========== validate numbers range by sending class of textboxes that dont exceed the parent element ====================
//v
//$.validator.addMethod("rangeLimitByClassWithParent", function (value, element, options) {
//    //var currentValue = parseFloat($(element).val());
//    //console.log(options[0]);
//    //console.log($("." + options[0]));
//    //console.log($("." + options[1]));
//    //currentValue = 9000;
//    // options[0]: for base input element
//    var totalAmount = 0;
//    //$("." + options[0]).each(function () {
//    //    totalAmount = totalAmount + Number($(this).val());
//    //});
//    //console.log(totalAmount);
//    //return totalAmount <= currentValue

//});

//=====voca===== validate date to be today or future date only ====================
$.validator.addMethod("todayAndFutureDateOnly", function (value, element, options) {
        // options[0]: for base input element
    var baseValue = new Date().getTime();
    var currentValue = new Date(($(element).val())).getTime();
    return currentValue >= baseValue;
});

//========== validate numbers range by sending class of textboxes by adding the total of there value ====================
$.validator.addMethod("rangeLimitByClass", function (value, element, options) {
    var totalAmount = 0;
    $("." + options[0]).each(function () {
        totalAmount = totalAmount + Number($(this).val());
    })
    return totalAmount <= options[1]
});
//========== validate numbers range by int ====================
$.validator.addMethod("rangeLimitInt", function (value, element, options) {
    var baseValue = parseInt(options);
    var currentValue = parseInt($(element).val());
    return (!baseValue || !currentValue) || currentValue <= baseValue;
});

//========== validate numbers range ====================
$.validator.addMethod("rangeLimit", function (value, element, options) {
    var baseValue = parseInt($(options[0]).val());
    var currentValue = parseInt($(element).val());
    return (!baseValue || !currentValue) || currentValue <= baseValue;
});
//======== validate numbers range that must be greater than or equal the other(Date)=========V
$.validator.addMethod("rangeLimitGreaterThanEqualDate", function (value, element, options) {
    // options[0]: for base input element
    var baseValue = new Date(($(options[0]).val())).getTime();
    var currentValue = new Date(($(element).val())).getTime();
    return currentValue >= baseValue;
});
//======== validate numbers range that must be greater than the other(Date)=========V
$.validator.addMethod("rangeLimitGreaterThanDate", function (value, element, options) {
    // options[0]: for base input element
    var baseValue = new Date(($(options[0]).val())).getTime();
    var currentValue = new Date(($(element).val())).getTime();
    return currentValue > baseValue;
});
//======== validate numbers range that must be less than the other(Date)=========V
$.validator.addMethod("rangeLimitLessThanDate", function (value, element, options) {
    // options[0]: for base input element
    var baseValue = new Date(($(options[0]).val())).getTime();
    var currentValue = new Date(($(element).val())).getTime();
    if (isNaN(baseValue) && isNaN(currentValue)) {
        return currentValue < baseValue;
    }
    else {
        return true;
    }
});
//======== validate numbers range that must be less than the other=========V
$.validator.addMethod("rangeLimitLessThan", function (value, element, options) {
    // options[0]: for base input element
    var baseValue = parseInt($(options[0]).val());
    var currentValue = parseInt($(element).val());
    return (!baseValue || !currentValue) || currentValue < baseValue;
});
//========== validate checklist for 1 minimum selected input ====================
$.validator.addMethod("minimumOneSelected", function (value, element, options) {
    // options[0]: for checkListContainer div
    var checkListContainer = options[0];
    if (!$(element).attr('minimumOneSelected')) {
        // refresh validation
        $(':checkbox', checkListContainer).change(function () {
            $(element).valid();
        });
        // mark element 
        $(element).attr('minimumOneSelected', 'true')
    }
    var count = $(checkListContainer).find(':checked').length;
    return count > 0;
});

//========== calendar: dbl date picker ====================
/*$(function () {
    $('.dblcal').dblDatePicker();
});*/

/* ui extentions */
// remove ng-option generated by angular
$.fn.removeNgGeneratedSelectOptions = function () {
    $('select option[value^="?"]', this).remove();
}
// reset validation
$.fn.resetValidation = function () {
    $('.field-validation-error', this).text('');
    if ($('.spanUpload', this).text().indexOf('خطأ') == 0) {
        $('.spanUpload', this).text('');
    }
    if ($(':file', this).val() == '') {
        $('.clearUpload', this).hide();
    }
}
// reset form inputs
$.fn.resetForm = function () {
    $(this).find('input:not(:button), textarea, select, :file').val('');
    $(this).find('img').attr('src', "");
    $('.spanUpload', this).text('');
    $('.loadedImage', this).hide();
    $('.clearUpload', this).hide();
}

//validator take into consideration hidden elemets
//$(function () {

$.validator.setDefaults({ ignore: ':hidden:not(".must-validate")' });
//});

// updateValidation
(function ($) {
    $.fn.updateValidation = function () {
        var $this = $(this);
        var form = $this.closest("form")
            .removeData("validator")
            .removeData("unobtrusiveValidation");
        $.validator.unobtrusive.parse(form);
        return $this;
    };
})(jQuery);