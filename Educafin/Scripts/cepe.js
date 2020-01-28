$(document).ready(function () {
    if (parseInt($("#edad").val()) < 18) {
        $("#LeyendaCredencial").hide();
    }
    if (parseInt($("#credencial").val()) > 0) {
        $("#divtutor").fadeOut(1000);
        $("#someSwitchOptioninfo16240840").prop('checked', true);
    }
    $("#someSwitchOptioninfo16240840").click(function () {

        $("#divtutor").fadeToggle(1000);
    });

    $("#codigopostal").keyup(function () {
       
        var cepe = $("#codigopostal").val();

        if (cepe.length > 4) {

        $("#Button1").hide();
        
        $.getJSON("http://api.geonames.org/postalCodeLookupJSON?postalcode=" + cepe + "&country=mx&username=sonata47", function (result) {
            $.each(result, function (i, field) {
                $("#colonia").empty();
                $("#estado").val("");
                $("#municipio").val("");
                try {
                    for (var i = 0; i < field.length; i++) {
                        $("#colonia").append("<option value=\"" + field[i].placeName + "\" >" + field[i].placeName + "</option>");
                    }
                    $("#estado").val(field[0].adminName1);
                    $("#municipio").val(field[0].adminName2);
                } catch (err) {
                    $("#colonia").empty();
                    $("#estado").val("");
                    $("#municipio").val("");
                }
                //$("#textEstado").append("<option value=\"" + field[0].adminName1 + "\" >" + field[0].adminName1 + "</option>");
                //$("#textCiudad").append("<option value=\"" + field[0].adminName2 + "\" >" + field[0].adminName2 + "</option>");
            
            });
        });
        }
    });
});