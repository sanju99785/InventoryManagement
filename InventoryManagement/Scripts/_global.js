function ShowError(message) {
    $("#warning").empty();
    $("#warning").append('<div class="alert alert-danger afix" id="alertwarning" >' + message + '</div>');
    $("#alertwarning").alert();
    window.setTimeout(function () { $("#alertwarning").alert('close'); }, 4000);
}

function ShowSuccess(message) {
    $("#warning").empty();
    $("#warning").append('<div class="alert alert-success afix" id="alertwarning">' + message + '</div>');
    $("#alertwarning").alert();
    window.setTimeout(function () { $("#alertwarning").alert('close'); }, 4000);
}
