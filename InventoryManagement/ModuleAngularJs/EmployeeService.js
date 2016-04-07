EmployeeApp.service("EmployeeService", ['$http', function ($http) {
    debugger;
    this.GetEmployee = function () {
       return $http.get('http://localhost/InventoryManagement.API/api/EmployeeAPI/')
    };
}])

