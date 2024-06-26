"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/sessionManager").build();

var jwtToken = document.cookie.split("=")[1];

// signalR hub bağlantısının başlatıldığı metot

// burada kullanıcının jwtToken'i alınıp bir listede tutulduktan sonra başka tarayıcıdan girişlerde
// bu listede var/yok kontrolü yapılıp var ise çıkış yaptırılacak.
connection.start()
    .then(() => {

        // hangi tarayıcıdan girildiğini veren property
        var browser = navigator.userAgentData.brands[0].brand;
        console.log(browser);

        connection.invoke("ConnectionCheck", jwtToken, browser).catch((error) => {
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
        window.location.href = "https://localhost:7284/auth/logout";
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
