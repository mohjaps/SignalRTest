"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    li.textContent = `${user} says ${message}`;
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

connection.on('UserConnected', function (connectionId, userName) {
    var groupElement = document.getElementById("senendingGroup");
    var option = document.createElement('option');
    option.text = userName;
    option.value = connectionId;
    groupElement.add(option);
})

connection.on('UserDisconnected', function (connectionId) {
    var groupElement = document.getElementById("senendingGroup");
    for (var i = 0; i < groupElement.length; i++) {
        if (groupElement.options[i].value == connectionId) {
            groupElement.remove(i);
        }
    }
})

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var typeOfSender = document.getElementById("senendingGroup").value;
    var message = document.getElementById("messageInput").value;

    if (typeOfSender == 'all') {
        connection.invoke("SendMessage", user, message).catch(function (err) {
            return console.error(err.toString());
        });
    }
    else if (typeOfSender == 'caller') {
        connection.invoke("SendMessageToCaller", 'I', message).catch(function (err) {
            return console.error(err.toString());
        });
    }
    else {
        connection.invoke("SendMessageToUser", typeOfSender, user, message).catch(function (err) {
            return console.error(err.toString());
        });
    }
    event.preventDefault();
});