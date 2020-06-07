$(function () {
    // [Instant-Loading-Indicator] vars
    var iliForm_class = 'ili-form';
    var iliRemote_class = 'ili-remote';
    var iliActionLink_class = 'ili-actionLink';
    var iliForm_markup = '<div class="' + iliForm_class + '"><i class="fa fa-refresh fa-spin"></i></div>';
    var iliRemote_markup = '<div class="' + iliRemote_class + '"><i class="fa fa-refresh fa-spin"></i></div>';
    var iliActionLink_markup = '<div class="' + iliActionLink_class + '"><span class="fa fa-refresh fa-spin"></span></div>';
    var customLoadingPanelAttr = 'data-ili-loading-panel';

    // ajax send
    $(document).ajaxSend(function (evt, request, settings) {
        try {
            // [Instant-Loading-Indicator] element 
            var ili_container;

            if (settings.port && settings.port.indexOf('validate') == 0) {
                // remote validated field
                ili_container = $('#' + settings.port.replace('validate', '')).parent();
                // add loaingElement to loaingElementContainer
                $(ili_container).find('.' + iliRemote_class).remove();
                $(ili_container).append(iliRemote_markup)
                // fadeIn [Instant-Loading-Indicator] element
                .fadeIn();
            }
            else if (settings.url) {
                var url = new URL(settings.url).pathname;
                // action link
                var actionLink = $('a[data-ajax="true"][href="' + url + '"]');
                if (actionLink.length == 1 && settings.type == "GET") {
                    ili_container =
                        $(actionLink).attr(customLoadingPanelAttr)
                        ? $($(actionLink).attr(customLoadingPanelAttr))
                        : ($(actionLink).find('i.fa').length > 0 ? null : $(actionLink));
                    if (ili_container) {
                        // add loaingElement to loaingElementContainer
                        $(ili_container).find('.' + iliActionLink_class).remove();
                        $(ili_container).append(iliActionLink_markup)
                        // fadeIn [Instant-Loading-Indicator] element
                        .fadeIn();
                    } else {
                        $(actionLink).find('i.fa')
                            .attr('data-ili-icon', $(actionLink).find('i.fa').attr('class'))
                            .attr('class', 'fa fa-refresh fa-spin');
                    }
                }
                else {
                    // regular form
                    var form = $('form[action="' + url + '"]');
                    if (form.length == 1) {
                        // add loaingElement to loaingElementContainer
                        $(form).find('.' + iliForm_class).remove();
                        $(form).append(iliForm_markup)
                        // fadeIn [Instant-Loading-Indicator] element
                        .fadeIn();
                    }
                }
            }
        } catch (error) {
            console.log(error)
        }
    });

    // ajax complete
    $(document).ajaxComplete(function (evt, request, settings) {
        try {
            // [Instant-Loading-Indicator] element 
            var element;

            if (settings.port && settings.port.indexOf('validate') == 0) {
                // remote validated field
                element = $('#' + settings.port.replace('validate', '')).parent().find('.' + iliRemote_class);
                element.fadeOut();
            }
            else if (settings.url) {
                var url = new URL(settings.url).pathname;
                // action link
                var actionLink = $('a[data-ajax="true"][href="' + url + '"]');
                if (actionLink.length == 1 && settings.type == "GET") {
                    element = $('.' + iliActionLink_class);
                    if (element.length > 0) {
                        element.fadeOut();
                    } else {
                        $(actionLink).find('i.fa').attr('class', $(actionLink).find('i.fa').attr('data-ili-icon'));
                    }
                }
                else {
                    // regular form
                    var form = $('form[action="' + url + '"]');
                    if (form.length == 1) {
                        element = $('form[action="' + url + '"] .' + iliForm_class);
                        element.fadeOut();
                    }
                }
            }
        } catch (error) {
            console.log(error)
        }
    });
});