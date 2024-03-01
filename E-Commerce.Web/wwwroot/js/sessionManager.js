"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/sessionManager").build();

// signalR hub ba�lant�s�n�n ba�lat�ld��� metot

// burada kullan�c�n�n jwtToken'i al�n�p bir listede tutulduktan sonra ba�ka taray�c�dan giri�lerde
// bu listede var/yok kontrol� yap�l�p var ise ��k�� yapt�r�lacak.
connection.start()
    .then(() => {
        var jwtToken = document.cookie.split("=")[1];

        // hangi taray�c�dan girildi�ini veren property
        console.log(navigator.userAgentData.brands[2])

        connection.invoke("ConnectionCheck", jwtToken).catch((error) => {
            return console.log(error.toString());
        });
    })
    .catch((error) => {
    return console.log(error.toString());
});

// signalR ba�lant�s� olu�turuldu�unda invoke olan metot
connection.on("SignOut", (hasExistSession) => {
    if (hasExistSession) {
        console.log(hasExistSession);
        document.cookie = "JWTToken=; expires=Thu, 01 Jan 1970 00:00:00 GMT; SameSite=None; Secure";
    }
});













// LoginAsync
/*
    document.getElementById("LoginButton").addEventListener("click", function (event) {

        var userName = document.getElementById("Email").value;
        var password = document.getElementById("Password").value;

        connection.invoke("LoginAsync", userName, password)
            .catch((error) => {
                return console.log(error.toString());
            });
    });
*/
