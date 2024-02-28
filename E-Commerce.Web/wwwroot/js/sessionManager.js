"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/sessionManager").build();

// signalR hub bağlantısının başlatıldığı metot

// burada kullanıcının connectionId'si alınıp bir listede tutulduktan sonra başka tarayıcıdan girişlerde
// bu listede var/yok kontrolü yapılıp var ise çıkış yaptırılabilir.
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

// signalR bağlantısı oluşturulduğunda invoke olan metot
connection.on("ReceiveConnectionId", function (connectionId) {
    var li = document.createElement("li");
    document.getElementById("connectionList").appendChild(li);

    li.textContent = `${connectionId}`;
});