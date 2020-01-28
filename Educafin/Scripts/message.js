
function Confirma() {
    var mensaje = confirm('¿Seguro que desea asignar folios?');
    if (mensaje) {
        window.location.href = 'updFolio';
    }
    else {
        alert('¡Haz denegado el mensaje!');
    }
}
