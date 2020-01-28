$(document).ready(function () {
    
    var mensajelogin = $('#mensajelogin').val();
       
     if (mensajelogin == -1) {
        $("#output").addClass("alert alert-danger animated fadeInUp").html("ERROR Nombre de Usuario incorrecto");
    }
    if (mensajelogin == 0) {
        $("#output").addClass("alert alert-danger animated fadeInUp").html("ERROR Usuario o contraseña es incorrecto");
    }
    if (mensajelogin == 2) {
        $("#output").addClass("alert alert-danger animated fadeInUp").html("ERROR El usuario esta inactivo");
    }
    if (mensajelogin == 5) {
        $("#output").addClass("alert alert-danger animated fadeInUp").html("ERROR Su vigencia como Usuario expiro");
    }

    $('#formlogin').submit(function (event) {
        var user = $("input[name=txtNombre]");
        var pass = $("input[name=txtpass]");

        if (user.val() != "" && pass.val() != "") {
            return;
        }
        event.preventDefault();

        $("#output").addClass("alert alert-danger animated fadeInUp").html("ERROR Usuario o Contraseña vacío");

    });
});
