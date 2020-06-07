$(function () {
    $('[data-val-required]:not(.notRequiredVal)').each(function (index, element) { $(element).parents('.row').first().find('label').eq(0).after('<span class="field-validation-indicator">*</span>'); })
});

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

/* ui extentions */
$.fn.resetValidation = function () {
    $('.field-validation-error', this).text('');
    if ($('.spanUpload', this).text().indexOf('خطأ') == 0) {
        $('.spanUpload', this).text('');
    }
    if ($(':file', this).val() == '') {
        $('.clearUpload', this).hide();
    }
};
$.fn.resetForm = function () {
    $(this).find('input:not(:button), textarea, select, :file').val('');
    $(this).find('img').attr('src', "");
    $('.spanUpload', this).text('');
    $('.loadedImage', this).hide();
    $('.clearUpload', this).hide();
};