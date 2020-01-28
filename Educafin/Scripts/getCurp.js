$(document).ready(function () {
    //var ruta = window.location.origin + "/Educafin/";
    var ruta = window.location.origin + "";

    $("#curpUser").keyup(function () {
        
        var curp = $("#curpUser").val();

        if (curp.length > 17) {
            $.ajax({
                url: ruta + "/Datos/DatosUsuario",
                type: "POST",
                dataType: "json",
                data: { CURP: curp }

            }).done(function (data) {

                $("#nombre").val(data.nombre);
                $("#apellido_pat").val(data.ape_pat);
                $("#apellido_mat").val(data.ape_mat);
                $("#estado_nac").val(data.lugar_nac);
                $("#sexo").val(data.sexo);
                $("#fecha_nac").val(data.fecha_nac);

            }).fail(function (data) {
                console.log(data);
            });
            
            
        }
    });

});



 