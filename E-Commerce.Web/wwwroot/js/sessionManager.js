"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/sessionManager").build();

// signalR hub baðlantýsýnýn baþlatýldýðý metot

// burada kullanýcýnýn jwtToken'i alýnýp bir listede tutulduktan sonra baþka tarayýcýdan giriþlerde
// bu listede var/yok kontrolü yapýlýp var ise çýkýþ yaptýrýlacak.
connection.start()
    .then(() => {
        var jwtToken = document.cookie.split("=")[1];

        // hangi tarayýcýdan girildiðini veren property
        console.log(navigator.userAgentData.brands[2])

        connection.invoke("ConnectionCheck", jwtToken).catch((error) => {
            return console.log(error.toString());
        });
    })
    .catch((error) => {
    return console.log(error.toString());
});

// signalR baðlantýsý oluþturulduðunda invoke olan metot
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
