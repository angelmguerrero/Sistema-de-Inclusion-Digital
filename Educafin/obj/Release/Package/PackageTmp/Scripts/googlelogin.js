function onSuccess(googleUser) {

    $.ajax({
        url: '../../php/logica/logingoogle.php'
        , type: "post"
        , dataType: 'json'
        , data: {
            email: googleUser.getBasicProfile().getEmail()
        , }
    }).success(function (response) {
        if (response == 1) {
            $(location).attr('href', 'inicio.php');
        }
        else{
            $("#output").addClass("alert alert-danger animated fadeInUp").html("ERROR Nombre de Usuario incorrecto");
        }
        //console.log(response); 
    });
}


function onFailure(error) {
    console.log(error);
}

function renderButton() {
    gapi.signin2.render('my-signin2', {
        'scope': 'profile email'
        , 'width': 220
        , 'height': 50
        , 'longtitle': true
        , 'onsuccess': onSuccess
        , 'onfailure': onFailure
    });
}