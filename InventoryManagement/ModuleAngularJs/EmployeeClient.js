
var EmployeeApp = angular.module('EmployeeApp', []);
EmployeeApp.controller('EmployeeController', function ($scope, EmployeeService) {
    $scope.GetEmployee = function () {
        EmployeeService.GetEmployee()
        .success(function (data) {
            $scope.employees = data.Table;
        })
         .error(function (error) {
             $scope.status = 'Unable to load Student data: ' + error.message;
             console.log($scope.status);
         });
    };
    $scope.AddClick = function (e) {
        $('#empModal').modal('show');
        $('#empModal .modal-title')[0].textContent = "Add";//// Set Title Popup
    }

    $scope.editClick = function (emps) {
        $('#empModal .modal-title')[0].textContent = "Edit"; //// Set Title Popup
        $('#empModal').modal('show');
        $scope.Name = emps.Name;
        $scope.Address = emps.Address;
        $scope.Phone = emps.Phone;
        $scope.Mobile = emps.Mobile;
        $scope.Description = emps.Description;
    }

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
            //$('#empModal').modal('hide');
            $scope.getStudents(5);
            Clear();
            $scope.divStudent = false;
            $scope.message = "Student Added Successfully";
        }, function () {
            alert('Error in adding record');
        });
    }

    $scope.deleteClick = function (data) {
        var isDelete = DeleteCall(data);
        debugger;
        console.log(isDelete);
    }

    $scope.test = function () {
        debugger;
        var isDelete = DeleteCall(data);
        debugger;
        console.log(isDelete);
    }
})