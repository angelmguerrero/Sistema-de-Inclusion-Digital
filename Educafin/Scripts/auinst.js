$(document).ready(function () {
    //var ruta = window.location.origin + "/Educafin/";
    var ruta = window.location.origin + "";

   $("#guardarA").hide();

    $("#cct_int").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: ruta + "/Datos/autocompletadoent",
                
                type: "POST",
                dataType: "json",
                data: { cct_int: request.term },
                success: function (data) {
                    response(data);
                }
            })
        },
        create: function () {
            $(this).data('ui-autocomplete')._renderItem = function (ul, item) {
                return $("<li>")
               .data("ui-autocomplete-item", item)
               .append("<a> " + item.CCT + "<br>" + item.nombre + "</a>")
               .appendTo(ul);
            }
        },
        focus: function (event, ui) {
            // prevent autocomplete from updating the textbox
            event.preventDefault();
            // manually update the textbox
            $(this).val(ui.item.CCT);
        },
        select: function (event, ui) {
            event.preventDefault();
            $(this).val(ui.item.CCT);
            $('#tags').val(ui.item.direccion);
        }
    });

    $("#cct_int1").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: ruta + "/Datos/autocompletadoentMP",
                type: "POST",
                dataType: "json",
                data: { cct_int1: request.term },
                success: function (data) {
                    response(data);
                }

            })
        },
        create: function () {
            $(this).data('ui-autocomplete')._renderItem = function (ul, item) {
                return $("<li>")
               .data("ui-autocomplete-item", item)
               .append("<a> " + item.CCT + "<br>" + item.nombre + "</a>")
               .appendTo(ul);
            }
        },
        focus: function (event, ui) {
            // prevent autocomplete from updating the textbox
            event.preventDefault();
            // manually update the textbox
            $(this).val(ui.item.CCT);
        },
        select: function (event, ui) {
            event.preventDefault();
            $(this).val(ui.item.CCT);
            $('#tags').val(ui.item.direccion);
        }
    });

    $("#pRFC").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: ruta + "/Datos/autocompletadoentInt",
                type: "POST",
                dataType: "json",
                data: { pRFC: request.term },
                success: function (data) {
                    response(data);
                }

            })
        },
        create: function () {
            $(this).data('ui-autocomplete')._renderItem = function (ul, item) {
                return $("<li>")
               .data("ui-autocomplete-item", item)
               .append("<a> " + item.CCT + "<br>" + item.nombre + "</a>")
               .appendTo(ul);
            }
        },
        focus: function (event, ui) {
            // prevent autocomplete from updating the textbox
            event.preventDefault();
            // manually update the textbox
            $(this).val(ui.item.CCT);
        },
        select: function (event, ui) {
            event.preventDefault();
            $(this).val(ui.item.CCT);
            $('#tags').val(ui.item.direccion);
        }
    });


    $("#cct_intELIM").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: ruta + "/Datos/autocompletadoentELIM",
                type: "POST",
                dataType: "json",
                data: { cct_intELIM: request.term },
                success: function (data) {
                    response(data);
                }

            })
        },
        create: function () {
            $(this).data('ui-autocomplete')._renderItem = function (ul, item) {
                return $("<li>")
               .data("ui-autocomplete-item", item)
               .append("<a> " + item.CCT + "<br>" + item.nombre + "</a>")
               .appendTo(ul);
            }
        },
        focus: function (event, ui) {
            // prevent autocomplete from updating the textbox
            event.preventDefault();
            // manually update the textbox
            $(this).val(ui.item.CCT);
        },
        select: function (event, ui) {
            event.preventDefault();
            $(this).val(ui.item.CCT);
            $('#tags').val(ui.item.direccion);
        }
    });


    $("#tipo").change(function () {
        $("#Aupertenece").val('');

    });
    
    $("#Aupertenece").autocomplete({
        source: function (request, response) {
            tipo = $("#tipo").val();
            if (tipo == "I") {
                $.ajax({
                    url: ruta + "/Datos/autoInstitucion",
                    type: "POST",
                    dataType: "json",
                    data: { cct: $("#Aupertenece").val() },
                    success: function (data) { response(data);  }
                })
            } else {
                $.ajax({
                    // url: window.location.origin+"/Educafin/Datos/autoProveedor",
                    url: window.location.origin + "/Datos/autoProveedor",
                    type: "POST",
                    dataType: "json",
                    data: { rfc: $("#Aupertenece").val() },
                    success: function (data) { response(data); }
                })
            }
        },
        create: function () {
            $(this).data('ui-autocomplete')._renderItem = function (ul, item) {
                return $("<li>")
               .data("ui-autocomplete-item", item)
               .append("<a> " + item.clave + "<br>" + item.nombre + "</a>")
               .appendTo(ul);
            }
        },
        focus: function (event, ui) {
            event.preventDefault();
            $(this).val(ui.item.clave);
        },
        select: function (event, ui) {
            event.preventDefault();
            $(this).val(ui.item.clave);
        }
    });



    //autocompletar para el medio proveedor
    $("#AucctMP").autocomplete({
        source: function (request, response) {
           
                $.ajax({
                    url: ruta + "/Datos/autoMedioProveedor",
                    type: "POST",
                    dataType: "json",
                    data: { txt: $("#AucctMP").val(), MP: $("#cct").val() },
                    success: function (data) {  response(data); $("#guardarA").hide(); }
                })
            
        },
        create: function () {
            $(this).data('ui-autocomplete')._renderItem = function (ul, item) {
                return $("<li>")
               .data("ui-autocomplete-item", item)
               .append("<a> " + item.cct + "<br>" + item.nombre + "</a>")
               .appendTo(ul);
            }
        },
        focus: function (event, ui) {
            event.preventDefault();
            $(this).val(ui.item.cct);
        },
        select: function (event, ui) {
            event.preventDefault();
            $(this).val(ui.item.cct);
            $("#guardarA").show();
        }
    });

    // Solo Poroveedor
    $("#AuProvIns").autocomplete({
        source: function (request, response) {
            
                $.ajax({
                    url: ruta + "/Datos/autoInstitucionP",
                    type: "POST",
                    dataType: "json",
                    data: { cct: $("#AuProvIns").val() },
                    success: function (data) { response(data); }
                })
           
           
        },
        create: function () {
            $(this).data('ui-autocomplete')._renderItem = function (ul, item) {
                return $("<li>")
               .data("ui-autocomplete-item", item)
               .append("<a> " + item.clave + "<br>" + item.nombre + "</a>")
               .appendTo(ul);
            }
        },
        focus: function (event, ui) {
            event.preventDefault();
            $(this).val(ui.item.clave);
        },
        select: function (event, ui) {
            event.preventDefault();
            $(this).val(ui.item.clave);
        }
    });

    });
