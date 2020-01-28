$(document).ready(function () {

    $(".Eliminar").click(function (event) {
        var formid = $(this).data("form");
        $('#dialog-confirm').show();
        $('#txt').remove();
        $('#Divparticipante').append("<div id=\"txt\"> ¿Desea eliminar el registro? </div>");

        $("#dialog-confirm").dialog({
            resizable: false,
            height: 150,
            width: 200,
            modal: true,
            buttons: {
                "Continuar": function () {
                    $("#"+formid).submit();
                    $(this).dialog("close");
                    
                },
                Cancelar: function () {
                    $(this).dialog("close");
                }
            }
        });
    });

});