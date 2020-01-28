$(document).ready(function () {
var er = $('#error').val();
    if (er == 0) {
        $('#divalert').append('<div class="alert alert-danger" id="diverror"> <a href="../../#" class="close" data-dismiss="alert" aria-label="close">&times;</a><strong>Error!</strong> Datos No guardados.</div>');
    }
    if (er == 1) {
        $('#divalert').append('<div class="alert alert-success" id="diverror"> <a href="../../#" class="close" data-dismiss="alert" aria-label="close">&times;</a><strong>Exito!</strong> Datos guardados.</div>');

    }
});