 $(document).ready(function () {

     var theTable = $('table#tablaLista');
     $("#filter").keyup(function () {
         if ($("#filtroBotones")) {
             $("label.btn").addClass("active");
             $.each($(".chelabl"), function (index, value) {
                 $(value).prop('checked', true);
             });
         }
         $.uiTableFilter(theTable, this.value);
     });

 });