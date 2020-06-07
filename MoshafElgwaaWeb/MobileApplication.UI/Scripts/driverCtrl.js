//var app = angular.module('driverApp', []);

app.controller('driverCtrl', function ($scope, $http) {
    
    $scope.viewChangePass = function (id) { alert('gfghfhf')
        console.log(id);
        $scope.EditedUserID = id;
        $("#txtPassword").val("");
        $("#changePassModal").modal("show");
    }
    $scope.ChangeUserPassword = function()
    {
        console.log('saveeee')
    }
   

});