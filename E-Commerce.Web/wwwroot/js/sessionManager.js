"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/sessionManager").build();

// signalR hub ba�lant�s�n�n ba�lat�ld��� metot

// burada kullan�c�n�n connectionId'si al�n�p bir listede tutulduktan sonra ba�ka taray�c�dan giri�lerde
// bu listede var/yok kontrol� yap�l�p var ise ��k�� yapt�r�labilir.
connection.start().then(() => {
    //var li = document.createElement("li");
    //document.getElementById("connectionList").appendChild(li);
    //li.textContent = `${connection.connectionId}`;


    document.getElementById("LoginButton").addEventListener("click", function (event) {

        var userName = document.getElementById("Email").value;
        var password = document.getElementById("Password").value;

        connection.invoke("LoginAsync", userName, password)
            .catch((error) => {
                return console.log(error.toString());
            });
    });


}).catch((error) => {
    return console.log(error.toString());
});

// signalR ba�lant�s� olu�turuldu�unda invoke olan metot
connection.on("ReceiveConnectionId", function (connectionId) {
    var li = document.createElement("li");
    document.getElementById("connectionList").appendChild(li);

    li.textContent = `${connectionId}`;
});