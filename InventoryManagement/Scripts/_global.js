function ShowError(message) {
    $("#warning").empty();
    $("#warning").append('<div class="alert alert-danger" id="alertwarning">' + message + '</div>');
    $("#alertwarning").alert();
    window.setTimeout(function () { $("#alertwarning").alert('close'); }, 4000);
}

function ShowSuccess(message) {
    $("#warning").empty();
    $("#warning").append('<div class="alert alert-success" id="alertwarning">' + message + '</div>');
    $("#alertwarning").alert();
    window.setTimeout(function () { $("#alertwarning").alert('close'); }, 4000);
}
