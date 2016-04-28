var isDelete = false;

function DeleteCall(data) {
    var isDelete = false;
    $('#deleteModal').modal('show');
    $('#commonDeleteYes').click(function () {
        debugger;
        isDelete = true;
    })
    return isDelete;
}