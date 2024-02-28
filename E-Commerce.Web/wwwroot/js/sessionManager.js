"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/sessionManager").build();

// signalR hub baðlantýsýnýn baþlatýldýðý metot

// burada kullanýcýnýn connectionId'si alýnýp bir listede tutulduktan sonra baþka tarayýcýdan giriþlerde
// bu listede var/yok kontrolü yapýlýp var ise çýkýþ yaptýrýlabilir.
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

// signalR baðlantýsý oluþturulduðunda invoke olan metot
connection.on("ReceiveConnectionId", function (connectionId) {
    var li = document.createElement("li");
    document.getElementById("connectionList").appendChild(li);

    li.textContent = `${connectionId}`;
});