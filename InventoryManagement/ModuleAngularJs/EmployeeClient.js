
//var app = angular.module('myApp', ['ngGrid']);


var EmployeeApp = angular.module('EmployeeApp', ['ui.grid', 'ui.grid.edit']);
EmployeeApp.controller('EmployeeController', function ($scope, EmployeeService) {
    $scope.EmployeeId = 0;


    $scope.GetEmployee = function () {
        $('#loader').show();
        EmployeeService.GetEmployee()
        .success(function (data) {
            $('#loader').hide();
            debugger
            $scope.employees = data.Table;
            $scope.gridOptions1.data = data.Table;
        })
         .error(function (error) {
             $('#loader').hide();
             $scope.status = 'Unable to load Student data: ' + error.message;
         });
    };

    debugger
    $scope.gridOptions1 = {
        enableSorting: true,
        columnDefs: [
                              { field: 'Name', displayName: 'Name', rowHeight: '560px' },
                              { field: 'Address', displayName: 'Address' },
                              { field: 'Phone', displayName: 'Phone' },
                              { field: 'Mobile', displayName: 'Mobile' },
                              { field: 'Description', displayName: 'Description' },

                              {
                                  field: '', displayName: 'Save',
                                  cellTemplate: '<span ng-click="editClick(row)" class="btn btn-info">Edit</span><span ng-click="deleteClick(row)" class="btn btn-danger">Delete</span>'
                              }
        ],
    
    };

    var columnDefs1 = [
    { name: 'firstName' },
    { name: 'lastName' },
    { name: 'company' },
    { name: 'gender' }
    ];

    var data1 = [
      {
          "firstName": "Cox",
          "lastName": "Carney",
          "company": "Enormo",
          "gender": "male"
      },
      {
          "firstName": "Lorraine",
          "lastName": "Wise",
          "company": "Comveyer",
          "gender": "female"
      },
      {
          "firstName": "Nancy",
          "lastName": "Waters",
          "company": "Fuelton",
          "gender": "female"
      },
      {
          "firstName": "Misty",
          "lastName": "Oneill",
          "company": "Letpro",
          "gender": "female"
      }
    ];

    var origdata1 = angular.copy(data1);

    var columnDefs2 = [
      { name: 'firstName' },
      { name: 'lastName' },
      { name: 'company' },
      { name: 'employed' }
    ];

    $scope.gridOpts = {
        columnDefs: columnDefs1,
        data: data1
    };



    $scope.AddClick = function (e) {
        $scope.Clear();
        $('#empModal').modal('show');
        $('#empModal .modal-title')[0].textContent = "Add";//// Set Title Popup
    }

    $scope.editClick = function (emps) {
        debugger;
        $('#empModal .modal-title')[0].textContent = "Edit"; //// Set Title Popup
        $scope.Clear();
        var empData = EmployeeService.GetEmployeeById(emps.entity.EmpId).success(function (data) {
            $('#loader').hide();
            $('#empModal').modal('show');
            $scope.EmpId = data.EmpId;
            $scope.Name = data.Name;
            $scope.Address = data.Address;
            $scope.Phone = data.Phone;
            $scope.Mobile = data.Mobile;
            $scope.Description = data.Description;
        }).error(function (error) {
            $('#loader').hide();
        });
    }

    $scope.AddUpdateEmployee = function () {
        var emp = {
            EmpId: $scope.EmpId,
            name: $scope.Name,
            address: $scope.Address,
            Phone: $scope.Phone,
            Mobile: $scope.Mobile,
            Description: $scope.Description
        }

        var existCheckData = EmployeeService.CheckExistEmployee(emp);
        $('#loader').show();
        existCheckData.then(function (msg) {
            if (msg.data == 1) {
                getData = EmployeeService.AddEditEmployee(emp)
                getData.then(function (msg) {
                    $('#empModal').modal('hide');
                    $('#loader').hide();
                    ShowSuccess(RecordCreatedSuccess);
                    $scope.GetEmployee();
                }, function () {
                    $('#loader').hide();
                    ShowSuccess(RecordCreatedFail);
                });
            }
            else if (msg.data == 2) {
                $('#loader').hide();
                ShowError(RecordExist);
            }
        }, function () {
        });

    }

    $scope.deleteClick = function (data) {
        $scope.EmployeeId = data.EmpId;
        $('#deleteModal').modal('show');
    }

    $scope.deleteConfirm = function () {
        if ($scope.EmployeeId > 0) {
            $('#loader').show();
            var getData = EmployeeService.DeleteEmployee($scope.EmployeeId);
            getData.then(function (msg) {
                $('#deleteModal').modal('hide');
                $('#loader').hide();
                $scope.GetEmployee();
                ShowSuccess(RecordDeletedSuccess);
                //$scope.message = "Student Deleted Successfully";
            }, function (e, e, e) {
                ShowError(RecordDeletedFail);
                $('#loader').hide();
            });
        }
    }

    $scope.Clear = function () {
        $scope.EmpId = "";
        $scope.Name = "";
        $scope.Address = "";
        $scope.Phone = "";
        $scope.Mobile = "";
        $scope.Description = "";
    }
})