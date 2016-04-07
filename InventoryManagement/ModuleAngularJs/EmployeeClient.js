
var EmployeeApp = angular.module('EmployeeApp', []);
debugger;
EmployeeApp.controller('EmployeeController', function ($scope, EmployeeService) {
    $scope.GetEmployee = function () {
        debugger;
        EmployeeService.GetEmployee()
        .success(function (data) {
            debugger;
            $scope.employees = data.Table;
        })
         .error(function (error) {
             debugger
             $scope.status = 'Unable to load Student data: ' + error.message;
             console.log($scope.status);
         });
    }
})