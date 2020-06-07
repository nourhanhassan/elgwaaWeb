app.directive("qvRepeat", ["qvRepeatIndexerService", "$timeout", function (qvRepeatIndexerService, $timeout) {

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
        currentForm.removeData("validator").removeData("unobtrusiveValidation");

        //Parse the form again
        $.validator.unobtrusive.parse(currentForm);

        // custom Rule Validation
        if (window.customRuleValidation && window.customRuleValidation.length > 0) {

            for (var i = 0; i < window.customRuleValidation.length; i++) {
                window.customRuleValidation[i]();
            }
        }

    }

}]);

app.factory("qvRepeatIndexerService", function () {

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

});