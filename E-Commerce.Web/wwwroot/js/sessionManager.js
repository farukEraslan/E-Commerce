"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/sessionManager").build();

// signalR hub bağlantısının başlatıldığı metot

// burada kullanıcının jwtToken'i alınıp bir listede tutulduktan sonra başka tarayıcıdan girişlerde
// bu listede var/yok kontrolü yapılıp var ise çıkış yaptırılacak.
connection.start()
    .then(() => {
        var jwtToken = document.cookie.split("=")[1];

        // hangi tarayıcıdan girildiğini veren property
        console.log(navigator.userAgentData.brands[2])

        connection.invoke("ConnectionCheck", jwtToken).catch((error) => {
            return console.log(error.toString());
        });
    })
    .catch((error) => {
    return console.log(error.toString());
});

// signalR bağlantısı oluşturulduğunda invoke olan metot
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
