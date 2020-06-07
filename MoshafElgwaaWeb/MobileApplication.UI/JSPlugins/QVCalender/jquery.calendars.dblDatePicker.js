var webApiConverterUrl = '/API/Umalqura/Convert?';

jQuery(function ($) {
    // double DatePicker
    $.fn.dblDatePicker = function () {
        var currentDate = new Date(); // to get the current date for comparison

        $(this).each(function (index, element) {
            if (!$(element).hasClass('hasCalendarsPicker')) {
                // containser divs
                var hijriCalendarDiv = $(element).parent();
                var gregCalendarDiv = $(hijriCalendarDiv).clone();

                var gregCalendarInput = gregCalendarDiv.find('input[type="text"]');

                var idAttr = $(gregCalendarInput).attr("id");
                if (typeof (idAttr) != "undefined") {
                    gregCalendarInput.attr("id", idAttr.replace('Hijri', ''));
                }

                var nameAttr = $(gregCalendarInput).attr("name");
                if (typeof (gregCalendarInput) != "undefined") {
                    gregCalendarInput.attr("name", idAttr.replace('Hijri', ''));
                }

                var ngModelAttr = $(gregCalendarInput).attr("ng-model");
                //debugger;
                if (typeof (ngModelAttr) != "undefined") {
                    gregCalendarInput.attr("ng-model", ngModelAttr.replace('Hijri', ''));
                }

                gregCalendarInput.removeAttr("qv-double-calendar");

                // insert greg div
                $(hijriCalendarDiv).after(gregCalendarDiv);

                // greg input
                var gregElement = $(gregCalendarDiv).children('input[type="text"]');

                // set placeholders
                $(element).attr('placeholder', 'تاريخ هجري');
                $(gregElement).attr('placeholder', 'تاريخ ميلادي');

                // set readonly
                //$(element).attr('readonly', 'readonly');
                //$(gregElement).attr('readonly', 'readonly');

                var calFromEelement = $('.minCal').eq(0);
                var calToEelement = $('.maxCal').eq(0);

                $(gregElement).removeClass("dblcal");
                $(gregElement).addClass("dblcal-gerg");

                //to handle if there are multiple calendars on the same page
                if ($(element).attr("dblcal-targets-specified")) {

                    var fromElementClassName = $(element).attr("dblcal-min-cal");
                    calFromEelement = $("." + fromElementClassName).eq(0);

                    var toElementClassName = $(element).attr("dblcal-max-cal");
                    calToEelement = $("." + toElementClassName).eq(0);

                }

                if ($(hijriCalendarDiv).hasClass('cal-picker')) { // for bootstrap only to  align text boxes next to each other
                    $(hijriCalendarDiv).css('width', '50%');
                    $(hijriCalendarDiv).next().css('width', '50%');
                }
                // debugger;

                if ($(element).hasClass("PreviousOrCurrent-date")) {
                    $(element).calendarsPicker({
                        changeMonth: true,
                        changeYear: true,
                        minDate: '1318-01-02',
                        maxDate: 0,
                        calendar: $.calendars.instance('ummalqura'),
                        onSelect: function () {
                            $(element).valid();
                            //refreshDblCalendarDates(element, 'htg');
                        },
                        onClose: function () { //fires when the calendar closes
                            if ($(element).hasClass('minCal')) {
                                setDblCalendarMinDate(calToEelement, $(gregElement).val(), currentDate); // to set the minimum date of the "to" calendar
                            }
                            if ($(element).hasClass('maxCal')) {
                                setDblCalendarMaxDate(calFromEelement, $(gregElement).val(), currentDate); // to set the maximum date of the "from" calendar
                            }
                        },
                    });
                    // add gregorian date picker
                    $(gregElement).calendarsPicker({
                        changeMonth: true,
                        changeYear: true,
                        minDate: '1900-05-01',
                        maxDate: 0,
                        //maxDate: -dayDiff,
                        onSelect: function () {
                            $(gregElement).valid();
                            //refreshDblCalendarDates(element, 'gth');
                        },
                        onClose: function () {
                            if ($(element).hasClass('minCal')) {
                                setDblCalendarMinDate(calToEelement, $(gregElement).val(), currentDate); // to set the minimum date of the "to" calendar
                            }
                            if ($(element).hasClass('maxCal')) {
                                setDblCalendarMaxDate(calFromEelement, $(gregElement).val(), currentDate); // to set the maximum date of the "from" calendar
                            }
                        },
                    });

                }


                else if ($(element).hasClass("future-date")) {

                    $(element).calendarsPicker({
                        changeMonth: true,
                        changeYear: true,
                        minDate: '0',
                        maxDate: '+50Y',
                        calendar: $.calendars.instance('ummalqura'),
                        onSelect: function () {
                            $(element).valid();
                            //refreshDblCalendarDates(element, 'htg');
                        },
                        onClose: function () { //fires when the calendar closes
                            if ($(element).hasClass('minCal')) {
                                setDblCalendarMinDate(calToEelement, $(gregElement).val(), currentDate); // to set the minimum date of the "to" calendar
                            }
                            if ($(element).hasClass('maxCal')) {
                                setDblCalendarMaxDate(calFromEelement, $(gregElement).val(), currentDate); // to set the maximum date of the "from" calendar
                            }
                        },
                    });
                    // add gregorian date picker
                    $(gregElement).calendarsPicker({
                        changeMonth: true,
                        changeYear: true,
                        minDate: '0',
                        maxDate: '+50Y',
                        //maxDate: -dayDiff,
                        onSelect: function () {
                            $(gregElement).valid();
                            //refreshDblCalendarDates(element, 'gth');
                        },
                        onClose: function () {
                            if ($(element).hasClass('minCal')) {
                                setDblCalendarMinDate(calToEelement, $(gregElement).val(), currentDate); // to set the minimum date of the "to" calendar
                            }
                            if ($(element).hasClass('maxCal')) {
                                setDblCalendarMaxDate(calFromEelement, $(gregElement).val(), currentDate); // to set the maximum date of the "from" calendar
                            }
                        },
                    });

                }

                else if (!$(element).hasClass("denyToday")) { // to allow today's date or not
                    // add ummalqura date picker
                    $(element).calendarsPicker({
                        changeMonth: true,
                        changeYear: true,
                        minDate: '1318-01-02',
                        maxDate: '+50Y',
                        calendar: $.calendars.instance('ummalqura'),
                        onSelect: function () {
                            $(element).valid();
                            //refreshDblCalendarDates(element, 'htg');
                        },
                        onClose: function () { //fires when the calendar closes
                            if ($(element).hasClass('minCal')) {
                                setDblCalendarMinDate(calToEelement, $(gregElement).val(), currentDate); // to set the minimum date of the "to" calendar
                            }
                            if ($(element).hasClass('maxCal')) {
                                setDblCalendarMaxDate(calFromEelement, $(gregElement).val(), currentDate); // to set the maximum date of the "from" calendar
                            }
                        },
                    });
                    // add gregorian date picker
                    $(gregElement).calendarsPicker({
                        changeMonth: true,
                        changeYear: true,
                        minDate: '1900-05-01',
                        maxDate: '+50Y',
                        //maxDate: -dayDiff,
                        onSelect: function () {
                            $(gregElement).valid();
                            //refreshDblCalendarDates(element, 'gth');
                        },
                        onClose: function () {
                            if ($(element).hasClass('minCal')) {
                                setDblCalendarMinDate(calToEelement, $(gregElement).val(), currentDate); // to set the minimum date of the "to" calendar
                            }
                            if ($(element).hasClass('maxCal')) {
                                setDblCalendarMaxDate(calFromEelement, $(gregElement).val(), currentDate); // to set the maximum date of the "from" calendar
                            }
                        },
                    });
                } else {
                    // add ummalqura date picker
                    $(element).calendarsPicker({
                        changeMonth: true,
                        changeYear: true,
                        minDate: '1318-01-02',
                        maxDate: -1,
                        calendar: $.calendars.instance('ummalqura'),
                        onSelect: function () {
                            $(element).valid();
                            //refreshDblCalendarDates(element, 'htg');
                        },
                        onClose: function () { //fires when the calendar closes
                            if ($(element).hasClass('minCal')) {
                                setDblCalendarMinDate(calToEelement, $(gregElement).val(), currentDate); // to set the minimum date of the "to" calendar
                            }
                            if ($(element).hasClass('maxCal')) {
                                setDblCalendarMaxDate(calFromEelement, $(gregElement).val(), currentDate); // to set the maximum date of the "from" calendar
                            }
                        },
                    });
                    // add gregorian date picker with today's date disallowed
                    $(gregElement).calendarsPicker({
                        changeMonth: true,
                        changeYear: true,
                        minDate: '1900-05-01',
                        maxDate: -1,
                        //maxDate: -dayDiff,
                        onSelect: function () {
                            $(gregElement).valid();
                            //refreshDblCalendarDates(element, 'gth');
                        },
                        onClose: function () {
                            if ($(element).hasClass('minCal')) {
                                setDblCalendarMinDate(calToEelement, $(gregElement).val(), currentDate); // to set the minimum date of the "to" calendar
                            }
                            if ($(element).hasClass('maxCal')) {
                                setDblCalendarMaxDate(calFromEelement, $(gregElement).val(), currentDate); // to set the maximum date of the "from" calendar
                            }
                        },
                    });
                }
                // handle data binding on init
                // $(element).val(function() { refreshDblCalendarDates(element, 'htg'); });
                //   alert($(element).val());
                // handle key stroke by user
                $(element).change(function () {
                    refreshDblCalendarDates(element, 'htg');
                    if ($(element).hasClass('minCal')) {
                        setDblCalendarMinDate(calToEelement, $(gregElement).val(), currentDate);
                    }
                    if ($(element).hasClass('maxCal')) {
                        setDblCalendarMaxDate(calFromEelement, $(gregElement).val(), currentDate);
                    }
                });
                $(gregElement).change(function () {
                    refreshDblCalendarDates(element, 'gth');
                    if ($(element).hasClass('minCal')) {
                        setDblCalendarMinDate(calToEelement, $(gregElement).val(), currentDate);
                    }
                    if ($(element).hasClass('maxCal')) {
                        setDblCalendarMaxDate(calFromEelement, $(gregElement).val(), currentDate);
                    }
                    //var formStoreKey = "applicationFormData";
                    //var storedobj = {

                    //    RegistrationDateHijri: $(element).val(),

                    //}
                    //window.localStorage.setItem(formStoreKey, JSON.stringify(storedobj));
                });
                //to show the calendar when the calendar icon is clicked (bootstrap only)
                $(document).on("click", ".cal-btn", function () {
                    $(this).parent().children('input[type="text"]').calendarsPicker("show");
                });

                //
                //$(element).calendarsPicker(pickerOpts);
                //$(gregElement).calendarsPicker(pickerOpts);
                //setTodayDate(".allowToday");
                //}
            }
        });
        return this;
    };
    $.fn.setDatePickersLimits = function (LimitStartDate, LimitEndDate) {
        var currentDate = new Date();
        var _minDateDiffDays;
        var _maxDateDiffDays;

        if (typeof (LimitStartDate) == 'undefined') {
            LimitStartDate = new Date('1900-05-01');
        } else if (typeof (LimitStartDate) == 'string') {
            LimitStartDate = new Date(LimitStartDate);
        }
        if (typeof (LimitEndDate) == 'undefined') {
            LimitEndDate = new Date('2050-12-31');
        } else if (typeof (LimitEndDate) == 'string') {
            LimitEndDate = new Date(LimitEndDate);
        }

        var _minDateDiffDays = Math.ceil((LimitStartDate.getTime() - currentDate.getTime()) / (1000 * 3600 * 24));
        var _maxDateDiffDays = Math.ceil((LimitEndDate.getTime() - currentDate.getTime()) / (1000 * 3600 * 24));

        //debugger;
        $(this).calendarsPicker("option", {
            minDate: '' + _minDateDiffDays.toString(),
            maxDate: '+' + _maxDateDiffDays.toString()
        });
    };
    // refresh calendar dates
    function refreshDblCalendarDates(dblpicker, convertDirection) {

        var hijriDate = $(dblpicker).val();
        var gregDate = $(dblpicker).parent().next().children('input[type="text"]').val();
        if (hijriDate || gregDate) {
            var url = webApiConverterUrl + 'convertDirection=' + convertDirection + '&hijriDate=' + hijriDate + '&gregDate=' + gregDate;
            $.getJSON(url, function (data) {
                //debugger;
                $(dblpicker).val(data.hijriDate);
                $(dblpicker).parent().next().children('input[type="text"].dblcal-gerg').val(data.gregDate);
                $(dblpicker).trigger("input")
                //$(dblpicker).parent().next().children('input[type="text"].dblcal-gerg').trigger("input");
            }).fail(function (jqXHR, textStatus, errorThrown) {
                console.log('Error: ' + textStatus);
            });
        }
    }
    /////////////////////////
    //set the maximum date of the "FROM" calendar after selecting the "to"  date
    function setDblCalendarMaxDate(dblpicker, maxGregDate, currentDate) {
        var gregElement = $(dblpicker).parent().next().children('input[type="text"].dblcal-gerg');
        var gregDate = new Date(maxGregDate); // converts the string to date
        var dayDiff = daydiff(gregDate, currentDate); // function that gets the difference in days between the 2 dates
        var calFromEelement = $('.minCal').eq(0);
        var calToEelement = $('.maxCal').eq(0);

        //to handle if there are multiple calendars on the same page
        if ($(dblpicker).attr("dblcal-targets-specified")) {

            var fromElementClassName = $(dblpicker).attr("dblcal-min-cal");
            calFromEelement = $("." + fromElementClassName).eq(0);

            var toElementClassName = $(dblpicker).attr("dblcal-max-cal");
            calToEelement = $("." + toElementClassName).eq(0);


        }

        // console.log(daydiff(gregDate, currentDate));

        //$(dblpicker).calendarsPicker('destroy'); // destroys the old calendar in order to set the new calendar with the new properties
        //$(gregElement).calendarsPicker('destroy');//$(dblpicker).next().calendarsPicker('destroy');


        $(dblpicker).calendarsPicker("option", {
            changeMonth: true,
            changeYear: true,
            minDate: '1318-01-02',
            maxDate: -dayDiff,
            calendar: $.calendars.instance('ummalqura'),
            onSelect: function () {
                $(dblpicker).valid();
                refreshDblCalendarDates(dblpicker, 'htg');
            },
            onClose: function () {
                if ($(dblpicker).hasClass('minCal')) {
                    setDblCalendarMinDate(calToEelement, $(gregElement).val(), currentDate);
                }
                if ($(dblpicker).hasClass('maxCal')) {
                    setDblCalendarMaxDate(calFromEelement, $(gregElement).val(), currentDate);
                }
            },
        });
        //set the properties of the greg calendar
        $(gregElement).calendarsPicker("option", {//$(dblpicker).next().calendarsPicker({
            changeMonth: true,
            changeYear: true,
            minDate: '1900-05-01',
            maxDate: -dayDiff,
            onSelect: function () {
                $(gregElement).valid();
                refreshDblCalendarDates(dblpicker, 'gth');
            },
            onClose: function () {
                if ($(dblpicker).hasClass('minCal')) {
                    setDblCalendarMinDate(calToEelement, $(gregElement).val(), currentDate);
                }
                if ($(dblpicker).hasClass('maxCal')) {
                    setDblCalendarMaxDate(calFromEelement, $(gregElement).val(), currentDate);
                }
            },
        });
    }

    //function to set the mindate of the "to" calendar after selecting the "from" date
    function setDblCalendarMinDate(dblpicker, minGregDate, currentDate) {
        var gregElement = $(dblpicker).parent().next().children('input[type="text"].dblcal-gerg');
        var gregDate = new Date(minGregDate);
        var dayDiff = daydiff(currentDate, gregDate);
        var calFromEelement = $('.minCal').eq(0);
        var calToEelement = $('.maxCal').eq(0);

        //to handle if there are multiple calendars on the same page
        if ($(dblpicker).attr("dblcal-targets-specified")) {

            var fromElementClassName = $(dblpicker).attr("dblcal-min-cal");
            calFromEelement = $("." + fromElementClassName).eq(0);

            var toElementClassName = $(dblpicker).attr("dblcal-max-cal");
            calToEelement = $("." + toElementClassName).eq(0);


        }

        /////// destroy is called in order to enable setting the new properties of the calendar
        //  $(dblpicker).calendarsPicker('destroy');
        //  $(gregElement).calendarsPicker('destroy');
        ///////////////////
        //set the properties of the hijri calendar
        $(dblpicker).calendarsPicker("option", {
            changeMonth: true,
            changeYear: true,
            minDate: +dayDiff,
            maxDate: '+50Y',
            calendar: $.calendars.instance('ummalqura'),
            onSelect: function () {
                $(dblpicker).valid();
                refreshDblCalendarDates(dblpicker, 'htg');
            },
            onClose: function () {
                if ($(dblpicker).hasClass('minCal')) {
                    setDblCalendarMinDate(calToEelement, $(gregElement).val(), currentDate);
                }
                if ($(dblpicker).hasClass('maxCal')) {
                    setDblCalendarMaxDate(calFromEelement, $(gregElement).val(), currentDate);
                }
            },
        });

        //set the properties of the greg calendar
        $(gregElement).calendarsPicker("option", {
            changeMonth: true,
            changeYear: true,
            minDate: +dayDiff,
            maxDate: '+50Y',
            onSelect: function () {
                $(gregElement).valid();
                refreshDblCalendarDates(dblpicker, 'gth');
            },
            onClose: function () {
                if ($(dblpicker).hasClass('minCal')) {
                    setDblCalendarMinDate(calToEelement, $(gregElement).val(), currentDate);
                }
                if ($(dblpicker).hasClass('maxCal')) {
                    setDblCalendarMaxDate(calFromEelement, $(gregElement).val(), currentDate);
                }
            },
        });
    }

    //function to get the difference between 2 dates in days
    function daydiff(first, second) {
        return (second - first) / (1000 * 60 * 60 * 24);
    }

    //to set the maximum allowed date of the calendar in case of birthdate for example
    function setTodayDate(birthDatePicker) {

        var pickerOpts = {
            maxDate: -10
        };
        var allowTodayCal = $(birthDatePicker);//;.find('input[type="text"]');
        allowTodayCal.calendarsPicker(pickerOpts);
    }
});