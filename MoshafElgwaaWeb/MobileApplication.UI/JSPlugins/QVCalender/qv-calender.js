myapp.directive("qvDoubleCalendar", ["$timeout", function ($timeout) {
    return {
        restrict: "A",
        link: function (scope, element, attrs) {
            $timeout(function () {
                $(element).removeClass("hasCalendarsPicker");
                $(element).parents(".doubleCalendarContainer").first().siblings(".doubleCalendarContainer").remove();
                
                $(element).dblDatePicker();

                    $(element).trigger("change");


            });
        }
    }
}]);
