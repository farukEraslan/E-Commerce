"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/sessionManager").build();

// signalR hub baðlantýsýnýn baþlatýldýðý metot

// burada kullanýcýnýn connectionId'si alýnýp bir listede tutulduktan sonra baþka tarayýcýdan giriþlerde
// bu listede var/yok kontrolü yapýlýp var ise çýkýþ yaptýrýlabilir.
connection.start().then(() => {
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
connection.on("SessionCheck", function (hasExistSession) {
    // *** signalR server'ýndan buraya istek gelmiyor.
    if (hasExistSession) {
        console.log("SessionCheck çalýþýyor.");
        document.cookie = `name=JWTToken; expires=Thu, 01 Jan 1970 00:00:00 GMT; path=/`;
    }
});
