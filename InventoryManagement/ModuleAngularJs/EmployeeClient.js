
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
    };

    $scope.AddUpdateEmployee = function () {
        var emp = {
            name: $scope.Name,
            address: $scope.Address,
            Phone: $scope.Phone,
            Mobile: $scope.Mobile,
            Description: $scope.Description
        }
            
        var getData = EmployeeService.AddEditEmployee(emp);
        getData.then(function (msg) {
            debugger;
            $scope.getStudents(5);
            Clear();
            $scope.divStudent = false;
            $scope.message = "Student Added Successfully";
        }, function () {
            alert('Error in adding record');
        });
    }
})