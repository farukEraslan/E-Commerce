"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/sessionManager").build();

// signalR hub baðlantýsýnýn baþlatýldýðý metot

// burada kullanýcýnýn connectionId'si alýnýp bir listede tutulduktan sonra baþka tarayýcýdan giriþlerde
// bu listede var/yok kontrolü yapýlýp var ise çýkýþ yaptýrýlabilir.
connection.start().then(() => {
    var li = document.createElement("li");
    document.getElementById("connectionList").appendChild(li);
    li.textContent = `${connection.connectionId}`;

}).catch((error) => {
    return console.log(error.toString());
});

// signalR baðlantýsý oluþturulduðunda invoke olan metot
connection.on("ReceiveConnectionId", function (connectionId) {
    var li = document.createElement("li");
    document.getElementById("connectionList").appendChild(li);

    li.textContent = `${connectionId}`;
});