EmployeeApp.service("EmployeeService", ['$http', function ($http) {
    this.GetEmployee = function () {
        return $http.get(apiPath + 'EmployeeAPI/GetEmployee/')
    };

    this.GetEmployeeById = function (EmpId) {
        return $http.get(webPath + 'Employee/InsertUdateEmployee?EmpId=' + EmpId)
    }

    this.AddEditEmployee = function (emp) {
        var response = $http({
            method: 'post',
            url: webPath + 'Employee/SaveEmployee',
            data: JSON.stringify(emp),
            dataType: 'json'
        });
        debugger
        return response;
    }

    this.CheckExistEmployee = function (emps) {
        debugger;
        var response = $http({
            method: 'post',
            async: false,
            url: apiPath + 'EmployeeAPI/CheckExistEmployee/',
            data: JSON.stringify(emps),
            dataType: 'json'
        });
        debugger
        return response;
    }

    this.DeleteEmployee = function (EmpIds) {
        var Ids = $.param({
            EmpId: EmpIds,
        });
        var response = $http.get(apiPath + 'EmployeeAPI/DeleteEmployee?' + Ids)
             .success(function (data, status, headers) {

             })
             .error(function (data, status, header, config) {
             });

        return response;
    }

}])

