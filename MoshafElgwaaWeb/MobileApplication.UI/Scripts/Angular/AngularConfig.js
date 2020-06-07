function isNumber(n) {
    return !isNaN(parseFloat(n)) && isFinite(n);
}
var plugins = angular.module('plugins', ['ngResource', 'customSelectModule']);

var simpleApp = angular.module('simpleApp', ['ngResource', 'customSelectModule', 'plugins', 'ngSanitize']);

plugins.directive('toNumber', function () {
    debugger;

    return {
        require: 'ngModel',
        link: function (scope, element, attrs, ngModel) {
            ngModel.$parsers.push(function (val) {
                console.log("parse");
                if (val == '' || val == 'null' || val == null) {
                    val = 0;
                }
                return parseInt(val, 10);
            });
            ngModel.$formatters.push(function (val) {
                if (typeof (val) == 'undefined' || val == 'null' || val == null) {
                    val = 0;
                }
                console.log(val);
                if (val == 0) {
                    val = '';
                }
                return val + '';
            });
        }
    }
})
.directive('fixDdl', function () {
    debugger;

    return {
        scope: {
            ngModel: "=",
        },
        link: function (scope, element, attrs) {

            if (!scope.ngModel) {
                scope.ngModel = '';
            }

        }
    }
})
.directive('qvValidation', function () {

    return {
        restrict: "A",
        scope: false,
        link: function (scope, element, attrs) {

            addValidationHooksFunctions();

            var parentForm = $(element).parents("form").first();

            if (parentForm && !parentForm.hasClass("validation-hooks-applied")) {
                parentForm.addTriggersToJqueryValidate().triggerElementValidationsOnFormValidation();
                parentForm.addClass("validation-hooks-applied");

                //this is to add the validation hooks again after applying qv-repeat
                if (!window.customRuleValidation) {
                    window.customRuleValidation = [];
                }
                window.customRuleValidation.push(function () {

                    parentForm.addTriggersToJqueryValidate().triggerElementValidationsOnFormValidation();
                })

            }
            var elementName = attrs["name"];

            removeDefaultValidationMessage(element, elementName);

            console.log(element);
            $(element).elementValidation(function (element, result) {
                if (result) { //valid
                    hideValidationError(element);

                }
                else {
                    showValidationError(element);
                }
                console.log(['validation ran for element:', element, 'and the result was:', result]);
            });

        }

    };

    function getDefaultValidationElement(element) {
        var defaultValidationElement = $(element).parents("div").first().find("[data-valmsg-for='" + $(element).attr("name") + "']").first();
        var elementName = $(element).attr("name");
        console.log(element);
       // var defaultValidationElement = $("[data-valmsg-for='" + elementName + "']").first();
        console.log(defaultValidationElement)
        return defaultValidationElement
    }
    function removeDefaultValidationMessage(element) {
        var defaultValidationElement = getDefaultValidationElement(element);
        defaultValidationElement.attr("style", "display: none !important");
    }
    function showValidationError(element) {
        var containerDiv = getParentElementContainer(element);//$(element).parents("div").first();
        if (!containerDiv.hasClass("has-error")) {
            containerDiv.addClass("has-error").addClass("has-feedback").addClass("has-feedback-left");
            $(element).after('<span class="qv-validation-icon fa fa-times form-control-feedback" aria-hidden="true"></span>');

        }
        showErrorMessageText(element);

    }
    function hideValidationError(element) {
        var containerDiv = getParentElementContainer(element);// $(element).parents("div").first();
        containerDiv.removeClass("has-error").removeClass("has-feedback");
        $(element).parents("div").find(".qv-validation-icon").remove();
        hideErrorMessageText(element);
    }
    function showErrorMessageText(element) {
        var defaultValidationElement = getDefaultValidationElement(element);
        var validationMessage = defaultValidationElement.text();
        console.log(validationMessage);
        //  $(element).attr("title", validationMessage);
        $(element).popover("destroy");
        $(element).popover({
            content: validationMessage,
            placement: $(element).attr("placement") ? $(element).attr("placement") : "left",
            trigger: "manual"
        }).popover("show");
    }
    function hideErrorMessageText(element) {

        $(element).popover("destroy");
    }

    function getParentElementContainer(element) {
        return $(element).parent().parent();
    }

    function addValidationHooksFunctions() {
        //The following code provides hooks (events) to bind to when jQuery validation occurs
        if (true) {
            (function ($) {

                $.fn.addTriggersToJqueryValidate = function () {
                    console.log("adding triggers")
                    // Loop thru the elements that we jQuery validate is attached to
                    // and return the loop, so jQuery function chaining will work.
                    return this.each(function () {
                        var form = $(this);

                        // Grab this element's validator object (if it has one)
                        var validator = form.data('validator');

                        // Only run this code if there's a validator associated with this element
                        if (!validator)
                            return;

                        // Only add these triggers to each element once
                        if (form.data('jQueryValidateTriggersAdded'))
                            return;
                        else
                            form.data('jQueryValidateTriggersAdded', true);

                        // Override the function that validates the whole form to trigger a 
                        // formValidation event and either formValidationSuccess or formValidationError
                        var oldForm = validator.form;
                        validator.form = function () {
                            var result = oldForm.apply(this, arguments);
                            var form = this.currentForm;
                            $(form).trigger((result == true) ? 'formValidationSuccess' : 'formValidationError', form);
                            $(form).trigger('formValidation', [form, result]);
                            return result;
                        };

                        // Override the function that validates the whole element to trigger a 
                        // elementValidation event and either elementValidationSuccess or elementValidationError
                        var oldElement = validator.element;
                        validator.element = function (element) {
                            var result = oldElement.apply(this, arguments);
                            $(element).trigger((result == true) ? 'elementValidationSuccess' : 'elementValidationError', element);
                            $(element).trigger('elementValidation', [element, result]);
                            return result;
                        };
                    });
                };

                /* Below here are helper methods for calling .bind() for you */

                $.fn.extend({

                    // Wouldn't it be nice if, when the full form's validation runs, it triggers the 
                    // element* validation events?  Well, that's what this does!
                    //
                    // NOTE: This is VERY coupled with jquery.validation.unobtrusive and uses its 
                    //       element attributes to figure out which fields use validation and 
                    //       whether or not they're currently valid.
                    //
                    triggerElementValidationsOnFormValidation: function () {
                        return this.each(function () {

                            $(this).bind('formValidation', function (e, form, result) {
                                $(form).find('*[data-val=true]').each(function (i, field) {
                                    if ($(field).hasClass('input-validation-error')) {
                                        $(field).trigger('elementValidationError', field);
                                        $(field).trigger('elementValidation', [field, false]);
                                    } else {
                                        $(field).trigger('elementValidationSuccess', field);
                                        $(field).trigger('elementValidation', [field, true]);
                                    }
                                });
                            });
                        });
                    },

                    formValidation: function (fn) {
                        return this.each(function () {
                            $(this).bind('formValidation', function (e, element, result) { fn(element, result); });
                        });
                    },

                    formValidationSuccess: function (fn) {
                        return this.each(function () {
                            $(this).bind('formValidationSuccess', function (e, element) { fn(element); });
                        });
                    },

                    formValidationError: function (fn) {
                        return this.each(function () {
                            $(this).bind('formValidationError', function (e, element) { fn(element); });
                        });
                    },

                    formValidAndInvalid: function (valid, invalid) {
                        return this.each(function () {
                            $(this).bind('formValidationSuccess', function (e, element) { valid(element); });
                            $(this).bind('formValidationError', function (e, element) { invalid(element); });
                        });
                    },

                    elementValidation: function (fn) {
                        return this.each(function () {
                            $(this).bind('elementValidation', function (e, element, result) { fn(element, result); });
                        });
                    },

                    elementValidationSuccess: function (fn) {
                        return this.each(function () {
                            $(this).bind('elementValidationSuccess', function (e, element) { fn(element); });
                        });
                    },

                    elementValidationError: function (fn) {
                        return this.each(function () {
                            $(this).bind('elementValidationError', function (e, element) { fn(element); });
                        });
                    },

                    elementValidAndInvalid: function (valid, invalid) {
                        return this.each(function () {
                            $(this).bind('elementValidationSuccess', function (e, element) { valid(element); });
                            $(this).bind('elementValidationError', function (e, element) { invalid(element); });
                        });
                    }

                });

            })(jQuery);
        }

    }



})
////---------------------------prevent type special character sent to directive ----------------------------------//
.directive("regExpRequire", function () {

    var regexp;
    return {
        restrict: "A",
        link: function (scope, elem, attrs) {
            regexp = eval(attrs.regExpRequire);

            var char;
            elem.on("keypress", function (event) {
                char = String.fromCharCode(event.which)
                if (regexp.test(char)) {
                    event.preventDefault();
                }
            })
        }
    }

})

.directive("qvRepeat", ["qvRepeatIndexerService", "$timeout", function (qvRepeatIndexerService, $timeout) {

    return {
        restrict: "A",
        scope: true,

        link: function (scope, element, attr) {
           
            //watch for attribute change
            attr.$observe('qvRepeat', function (val) {
                applyIndexing(scope, element, attr)
                callBack(scope, element, attr)

            });
            //console.log(attr);

        }
    }

    function callBack(scope, element, attr) {
        if (attr["onDone"]) {
            $timeout(function () {

                scope[attr["onDone"]](element);
            });
        }
    }

    function applyIndexing(scope, element, attr) {

        var allIndexes = attr["qvRepeat"];
        var elementName = attr["name"];
        //  var level=attr["level"];
        var indexesArr = allIndexes.split(",");
        for (var i = 0; i < indexesArr.length; i++) {
            var currentIndex = indexesArr[i];
            var level = i;

            var elementName = element.attr("name");
            if (typeof (elementName) != "undefined") {
                var newElementName = qvRepeatIndexerService.getNameWithIndex(elementName, currentIndex, level);
                element.attr("name", newElementName);
                var elementDefaultValue = attr["defaultValue"];
                if (typeof (elementDefaultValue) != "undefined") {
                    element.attr("value", "0")
                }
            }

            var elementID = element.attr("id");
            if (typeof (elementID) != "undefined") {
                var newElementID = qvRepeatIndexerService.getIDWithIndex(elementID, currentIndex, level);
                element.attr("id", newElementID);
            }

            var dataValFor = element.attr("data-valmsg-for");
            if (typeof (dataValFor) != "undefined") {
                var newDataValFor = qvRepeatIndexerService.getNameWithIndex(dataValFor, currentIndex, level);
                element.attr("data-valmsg-for", newDataValFor)

            }


        }


        //console.log(currentIndex);
        var currentForm = $(element).parents("form").eq(0);
        //Remove current form validation information
        //////////  currentForm.removeData("validator").removeData("unobtrusiveValidation"); // this remove unobtersev but not custom validation


        //Parse the form again
        $.validator.unobtrusive.parse(currentForm);

        var unobtrusiveValidation = currentForm.data('unobtrusiveValidation');
        var validator = currentForm.validate();

        // add unobtersive validation for newly added elemnts
        $.each(unobtrusiveValidation.options.rules, function (elname, elrules) {
            // get all rules on element added by unobtersive

            if (validator.settings.rules[elname] == undefined) {
                var args = {};
                $.extend(args, elrules);
                args.messages = unobtrusiveValidation.options.messages[elname];
                //edit:use quoted strings for the name selector
                $("[name='" + elname + "']").rules("add", args);
            } else {
                $.each(elrules, function (rulename, data) {

                    if (validator.settings.rules[elname][rulename] == undefined) {
                        var args = {};
                        args[rulename] = data;
                        args.messages = unobtrusiveValidation.options.messages[elname][rulename];
                        //edit:use quoted strings for the name selector
                        $("[name='" + elname + "']").rules("add", args);
                    }
                });
            }
        });


        // custom Rule Validation
        if (window.customRuleValidation && window.customRuleValidation.length > 0) {


            for (var i = 0; i < window.customRuleValidation.length; i++) {

                window.customRuleValidation[i]();
            }
        }

    }



}])

.factory("qvRepeatIndexerService", function () {

    return {

        getNameWithIndex: function (name, index, level) {
            var currentMatchIndex = 0;
            return name.replace(/\[(\d)\]/gi, function (match) {

                if (currentMatchIndex == level) {
                    currentMatchIndex++;
                    return "[" + index + "]";
                }
                else {

                    currentMatchIndex++;
                    return match;
                }

            });

        },

        getIDWithIndex: function (id, index, level) {

            var currentMatchIndex = 0;
            return id.replace(/_(\d)_(.*?)/gi, function (match) {
                if (currentMatchIndex == level) {
                    currentMatchIndex++;
                    return "_" + index + "_";
                }
                else {
                    currentMatchIndex++;
                    return match;
                }

            });
        },

    }

})



/*----------------------------------------------*/



var app = angular.module('app', ['ngTable', 'ui', 'ngDragDrop', 'ngResource', 'ngMockE2E', 'plugins']).

config(['$httpProvider', function ($httpProvider) {

    $httpProvider.defaults.headers.common["FROM-ANGULAR"] = "true";

}]).
run(["$httpBackend", "$filter", "$log", "NgTableParams", function ($httpBackend, $filter, $log, NgTableParams) {
    // emulation of api server
    $httpBackend.whenGET(/data.*/).respond(function (method, url, data, headers) {
        var query = url.split('?')[1],
            requestParams = {};

        $log.log('Ajax request: ', url);

        var vars = query.split('&');
        for (var i = 0; i < vars.length; i++) {
            var pair = vars[i].split('=');
            requestParams[decodeURIComponent(pair[0])] = decodeURIComponent(pair[1]);
        }
        // parse url params
        for (var key in requestParams) {
            if (key.indexOf('[') >= 0) {
                var params = key.split(/\[(.*)\]/), value = requestParams[key], lastKey = '';

                angular.forEach(params.reverse(), function (name) {
                    if (name != '') {
                        var v = value;
                        value = {};
                        value[lastKey = name] = isNumber(v) ? parseFloat(v) : v;
                    }
                });
                requestParams[lastKey] = angular.extend(requestParams[lastKey] || {}, value[lastKey]);
            } else {
                requestParams[key] = isNumber(requestParams[key]) ? parseFloat(requestParams[key]) : requestParams[key];
            }
        }

        data = [];

        var params = new NgTableParams(requestParams);

        data = params.filter() ? $filter('filter')(data, params.filter()) : data;
        data = params.sorting() ? $filter('orderBy')(data, params.orderBy()) : data;

        var total = data.length;
        data = data.slice((params.page() - 1) * params.count(), params.page() * params.count());

        return [200, {
            result: data,
            total: total
        }];
    });
    $httpBackend.whenGET(/.*/).passThrough();
    $httpBackend.whenPOST(/.*/).passThrough();
}])


//---------------------------for loading

.directive('loadingContainer', function () {
    return {
        restrict: 'A',
        scope: false,
        link: function (scope, element, attrs) {
            var loadingLayer = angular.element('<div class="loading"></div>');
            element.append(loadingLayer);
            element.addClass('loading-container');
            scope.$watch(attrs.loadingContainer, function (value) {
                loadingLayer.toggleClass('ng-hide', !value);
            });
        }
    };
})


//---------------------------for numbers only inputs

.directive('realNumbers', function () {
    return {
        require: '?ngModel',
        link: function (scope, element, attrs, ngModelCtrl) {
            if (!ngModelCtrl) {
                return;
            }

            ngModelCtrl.$parsers.push(function (val) {
                if (angular.isUndefined(val)) {
                    var val = '';
                }
                var clean = val.replace(/[^1-9]+/g, '');
                if (val !== clean) {
                    ngModelCtrl.$setViewValue(clean);
                    ngModelCtrl.$render();
                }
                return clean;
            });

            element.bind('keypress', function (event) {
                if (event.keyCode === 32) {
                    event.preventDefault();
                }
            });
        }
    };
})
.directive('numbersOnly', function () {
    return {
        require: '?ngModel',
        link: function (scope, element, attrs, ngModelCtrl) {
            if (!ngModelCtrl) {
                return;
            }

            ngModelCtrl.$parsers.push(function (val) {
                if (angular.isUndefined(val)) {
                    var val = '';
                }
                var clean = val.replace(/[^0-9]+/g, '');
                if (val !== clean) {
                    ngModelCtrl.$setViewValue(clean);
                    ngModelCtrl.$render();
                }
                return clean;
            });

            element.bind('keypress', function (event) {
                if (event.keyCode === 32) {
                    event.preventDefault();
                }
            });
        }
    };
})
.directive('myEnter', function () {
    return function (scope, element, attrs) {
        element.bind("keydown keypress", function (event) {
            if (event.which === 13) {
                scope.$apply(function () {
                    scope.$eval(attrs.myEnter);
                });

                event.preventDefault();
            }
        });
    };
})
////---------------------------prevent type special character sent to directive ----------------------------------//
.directive("regExpRequire", function () {

    var regexp;
    return {
        restrict: "A",
        link: function (scope, elem, attrs) {
            regexp = eval(attrs.regExpRequire);

            var char;
            elem.on("keypress", function (event) {
                char = String.fromCharCode(event.which)
                if (regexp.test(char)) {
                    event.preventDefault();
                }
            })
        }
    }

})


app.config(['$httpProvider', function ($httpProvider) {
    $httpProvider.defaults.headers.common['X-Requested-With'] = 'XMLHttpRequest';
}]);

simpleApp.config(['$httpProvider', function ($httpProvider) {
    $httpProvider.defaults.headers.common['X-Requested-With'] = 'XMLHttpRequest';
}]);
plugins.config(['$httpProvider', function ($httpProvider) {
    $httpProvider.defaults.headers.common['X-Requested-With'] = 'XMLHttpRequest';
}]);
//---------------------------------------------//

/*---------------------------------------------*/

