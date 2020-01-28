$(document).ready(function () {
    
    var mensajelogin = $('#mensajelogin').val();
       
     if (mensajelogin == -1) {
        $("#output").addClass("alert alert-danger animated fadeInUp").html("ERROR Nombre de Usuario incorrecto");
    }
    if (mensajelogin == 0) {
        $("#output").addClass("alert alert-danger animated fadeInUp").html("ERROR Contraseña incorrecta");
    }

    $('#formlogin').submit(function (event) {
        var user = $("input[name=username]");
        var pass = $("input[name=password]");

        if (user.val() != "" && pass.val() != "") {
            return;
        }
        event.preventDefault();

        $("#output").addClass("alert alert-danger animated fadeInUp").html("ERROR Usuario o Contraseña vacío");

    });


});
