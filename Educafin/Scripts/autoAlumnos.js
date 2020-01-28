$(document).ready(function () {
    //var ruta = window.location.origin + "/Educafin/";
    var ruta = window.location.origin + "";

    $("#datosTablet").hide();
    $("#datos").hide();
    $("#btnguardar").hide();
    

    $("#AUcurp").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: ruta + "/Datos/autoCurpAlumnos",
                type: "POST",
                dataType: "json",
                data: { curp: $("#AUcurp").val() },
                success: function (data) {
                    response(data);
                    $("#AUmatricula").val('');
                    $("#Auserie").val('');
                    $('.dt').remove();
                    $('.curpM').val('');
                    $('.matriculaM').val('');
                    $("#datosTablet").hide();
                    $("#datos").hide();
                    $("#btnguardar").hide();
                }

            })
        },
        create: function () {
            $(this).data('ui-autocomplete')._renderItem = function (ul, item) {
                return $("<li>")
               .data("ui-autocomplete-item", item)
               .append("<a> " + item.curp +"<br>" +item.matricula+ "<br>" + item.nombre + " " + item.pat + " " + item.mat + "</a>")
               .appendTo(ul);
            }
        },
        focus: function (event, ui) {
            event.preventDefault();
            $(this).val(ui.item.curp);
        },
        select: function (event, ui) {
            event.preventDefault();
            $('.dt').remove();
            $(this).val(ui.item.curp);
            $('.curpT').append('<label class="dt">'+ui.item.curp+'</label>');
            $('.matriculaT').append('<label class="dt">' + ui.item.matricula + '</label>');
            $('.nombreT').append('<label class="dt">' + ui.item.nombre + " " + ui.item.pat + " " + ui.item.mat + '</label>');

            $('.curpM').val(ui.item.curp);
            $('.matriculaM').val(ui.item.matricula);
            $("#datosTablet").show();
            $("#datos").show();
            
        }
    });

    $("#AUmatricula").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: ruta + "/Datos/autoMatriculaAlumnos",
                type: "POST",
                dataType: "json",
                data: { matricula: $("#AUmatricula").val() },
                success: function (data) {
                    response(data);
                    $("#AUcurp").val('');
                    $("#Auserie").val('');
                    $('.dt').remove();
                    $('.curpM').val('');
                    $('.matriculaM').val('');
                    $("#datosTablet").hide();
                    $("#datos").hide();
                    $("#btnguardar").hide();
                }
            })
        },
        create: function () {
            $(this).data('ui-autocomplete')._renderItem = function (ul, item) {
                return $("<li>")
               .data("ui-autocomplete-item", item)
               .append("<a> " + item.curp + "<br>" +item.matricula+ "<br>" + item.nombre + " " + item.pat + " " + item.mat + "</a>")
               .appendTo(ul);
            }
        },
        focus: function (event, ui) {
            event.preventDefault();
            $(this).val(ui.item.matricula);
        },
        select: function (event, ui) {
            event.preventDefault();
            $('.dt').remove();
            $(this).val(ui.item.matricula);
            $('.curpT').append('<label class="dt">' + ui.item.curp + '</label>');
            $('.matriculaT').append('<label class="dt">' + ui.item.matricula + '</label>');
            $('.nombreT').append('<label class="dt">' + ui.item.nombre + " " + ui.item.pat + " " + ui.item.mat + '</label>');

            $('.curpM').val(ui.item.curp);
            $('.matriculaM').val(ui.item.matricula);
            $("#datosTablet").show();
            $("#datos").show();

        }
    });

    $("#Auserie").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: ruta + "/Datos/autoTabletInstitución",
                type: "POST",
                dataType: "json",
                data: { serie: $("#Auserie").val() },
                success: function (data) { response(data); $("#btnguardar").hide(); }

            })
        },
        create: function () {$(this).data('ui-autocomplete')._renderItem = function (ul, item) {
                return $("<li>")
               .data("ui-autocomplete-item", item)
               .append("<a> " + item.serie + "</a>")
               .appendTo(ul);
            }
        },
        focus: function (event, ui) {
            // prevent autocomplete from updating the textbox
            event.preventDefault();
            // manually update the textbox
            $(this).val(ui.item.serie);
        },
        select: function (event, ui) {
            event.preventDefault();
            $(this).val(ui.item.serie);
            $("#btnguardar").show();
        }
    });

});


