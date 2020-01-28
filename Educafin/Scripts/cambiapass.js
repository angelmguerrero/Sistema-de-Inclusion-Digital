$(document).ready(function () {
    $('.errorI').hide();
    $('#errorPass').hide();
    $('#enviar').prop("disabled", true);

    $('#passwordR2').keyup(function () { //funcion comparra cajas de texto para contraseñas correctas
        var pas1 = $('#passwordR').val();

        if ($('#passwordR2').val() != pas1) {
            $('#otraPass').addClass("has-error has-feedback");
            $('.errorI').show();
            $('#errorPass').show();
            $('#enviar').prop("disabled", true);
        } else {
            $('#otraPass').removeClass("has-error has-feedback");
            $('.errorI').hide();
            $('#errorPass').hide();
            if (pas1.length > 0) {
                $('#enviar').prop("disabled", false);
            }
        }
    });

    $('#passwordR').keyup(function () { //funcion comparra cajas de texto para contraseñas correctas
        var pas2 = $('#passwordR2').val();

        if ($('#passwordR').val() != pas2) {
            $('#otraPass').addClass("has-error has-feedback");
            $('.errorI').show();
            $('#errorPass').show();
            $('#enviar').prop("disabled", true);
        } else {
            $('#otraPass').removeClass("has-error has-feedback");
            $('.errorI').hide();
            $('#errorPass').hide();
            if (pas2.length > 0) {
                $('#enviar').prop("disabled", false);
            }
        }
    });

});