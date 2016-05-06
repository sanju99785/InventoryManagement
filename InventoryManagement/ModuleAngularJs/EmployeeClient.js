var EmployeeApp = angular.module('EmployeeApp', ['ui.grid', 'ui.grid.edit']);

EmployeeApp.controller('EmployeeController', function ($scope, $filter, EmployeeService) {
    $scope.EmployeeId = 0;
    $scope.noOfRecord = 0;


    $scope.update = function (user) {
        $scope.master = angular.copy(user);
    };

    $scope.reset = function (form) {
        if (form) {
            form.$setPristine();
            form.$setUntouched();
        }
        $scope.user = angular.copy($scope.master);
    };

    $scope.reset();
    $scope.GetEmployee = function () {
        $('#loader').show();
        $scope.gridOptions.data = [];
        if (typeof $scope.filterValue === 'undefined') {
            $scope.filterValue = "";
        }

        EmployeeService.GetEmployee()
        .success(function (data) {
            $('#loader').hide();
            $scope.employees = data.Table;
            $scope.gridOptions.data = data.Table;
            $scope.noOfRecord = $scope.gridOptions.data.length;
        })
         .error(function (error) {
             $('#loader').hide();
         });
    };

    $scope.SearchGrid = function () {
        $scope.gridOptions.data = $filter('filter')($scope.employees, $scope.filterValue, undefined);
        $scope.noOfRecord = $scope.gridOptions.data.length;
    };

    $scope.gridOptions = {
        enableSorting: true,
        rowHeight: 40,
        headerRowHeight: 40,
        footerRowHeight: 40,
        enableHighlighting: true,
        columnDefs: [
                              { field: 'Name', displayName: 'Name' },
                              { field: 'Address', displayName: 'Address' },
                              { field: 'Phone', displayName: 'Phone' },
                              { field: 'Mobile', displayName: 'Mobile' },
                              { field: 'Description', displayName: 'Description' },
                               {
                                   field: 'EmpId', name: "", enableSorting: false, width: 140,
                                   cellTemplate: '<div class="action-button-padding"><span ng-click="grid.appScope.EditClick(row.entity)" class="action-button-margin btn btn-info ">Edit</span><span ng-click="grid.appScope.DeleteClick(row.entity)" class="action-button-margin btn btn-danger">Delete</span></div> '
                               }
        ],
        rowTemplate: "<div ng-dblclick=\"grid.appScope.EditClick(row.entity)\" ng-repeat=\"(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name\" class=\"ui-grid-cell\" ng-class=\"{ 'ui-grid-row-header-cell': col.isRowHeader }\" ui-grid-cell dbl-click-row></div>"
    };


    $scope.AddClick = function (e) {
        $scope.Clear();
        $scope.popupTitle = "Add";//// Set Title Popup
        $('#empModal').modal('show');
    }

    $scope.EditClick = function (emps) {

        $scope.popupTitle = "Edit"; //// Set Title Popup
        $scope.Clear();
        var empData = EmployeeService.GetEmployeeById(emps.EmpId).success(function (data) {
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
        if (!$scope.empForm.$valid) {
            return;
        }
        if (!$scope.empForm.$dirty) { //// If form is untuched
            $('#empModal').modal('hide');
            return;
        }
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
                    if (emp.EmpId > 0) {
                        ShowSuccess(RecordUpdateSuccess);
                    }
                    else {
                        ShowSuccess(RecordCreatedSuccess);
                    }

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

    $scope.DeleteClick = function (data) {
        $scope.EmployeeId = data.EmpId;
        $('#deleteModal').modal('show');
    }

    $scope.DeleteConfirm = function () {
        if ($scope.EmployeeId > 0) {
            $('#loader').show();
            var getData = EmployeeService.DeleteEmployee($scope.EmployeeId);
            getData.then(function (msg) {
                $('#deleteModal').modal('hide');
                $('#loader').hide();
                $scope.GetEmployee();
                ShowSuccess(RecordDeletedSuccess);
            }, function (e, e, e) {
                ShowError(RecordDeletedFail);
                $('#loader').hide();
            });
        }
    }

    $scope.ClearSearch = function () {
        $scope.filterValue = "";
        $scope.noOfRecord = 0;
        $scope.gridOptions.data = $filter('filter')($scope.employees, "", undefined);
    }
    $scope.Clear = function () {
        $scope.EmpId = "";
        $scope.Name = "";
        $scope.Address = "";
        $scope.Phone = "";
        $scope.Mobile = "";
        $scope.Description = "";
        $scope.empForm.$setUntouched();
        $scope.empForm.$setPristine();
    }

    $scope.GetEmployee();////Initialize Grid data
})

