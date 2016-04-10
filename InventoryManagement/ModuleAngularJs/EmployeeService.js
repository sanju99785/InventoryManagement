EmployeeApp.service("EmployeeService", ['$http', function ($http) {
    debugger;
    this.GetEmployee = function () {
        return $http.get(apiPath + '/EmployeeAPI/')
    };

    this.AddEditEmployee = function (emp) {
        debugger;
        var response = $http({
            method: 'post',
            url: apiPath + '/Employee/SaveEmployee/',
            data: JSON.stringify(emp),
            dataTyoe: 'json'
        });
        debugger;
        return response;
    }
}])

