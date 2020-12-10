var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

var connectionIdGlobal = '';

connection.on("RecieveMessage", function (data) {
    const  message = document.createElement("div");

    if (data.userName === document.querySelector("#user").value) {
        $(message).addClass('media media-chat media-chat-reverse');
        $(message).css("margin-left", "70%");
    }
    else {
        $(message).addClass('media media-chat');
    }

    var messageBody = document.createElement("div");
    messageBody.classList.add('media-body');

    var userName = document.createElement("p");
    $(userName).addClass('meta');
    $(userName).css("color", "black");
    userName.appendChild(document.createTextNode(data.userName));

    var content = document.createElement("p");
    $(content).css("border-radius", "10px");
    content.appendChild(document.createTextNode(data.content));

    var time = document.createElement("p");
    $(time).addClass('meta');
    $(time).css("color", "black");
    time.appendChild(document.createTextNode(data.time));

    messageBody.appendChild(userName);
    messageBody.appendChild(content);
    messageBody.appendChild(time);

    message.appendChild(messageBody);

    document.querySelector('#chat-content').append(message);

    var objDiv = document.getElementById("chat-content");
    objDiv.scrollTop = objDiv.scrollHeight;
});

var joinRoom = function () {
    var url = '/Chat/JoinRoom/' + connectionIdGlobal + '/@Model.Id';
    axios.post(url, null)
        .then(res => {
            console.log("Room Joined!", res);
        })
        .catch(err => {
            console.err("Failed to join Room!", res);
        })
}

connection.start().then(function () {
    connection.invoke('getConnectionId')
        .then(function (connectionId) {
            document.getElementById("connection").value = connectionId;

        });
})
    .catch(function (err) {
        console.log(err);
    });

var sendMessage = function (event) {
    event.preventDefault();
    var data = new FormData(event.target);
    document.getElementById("sender").value = '';
    axios.post("/Chat/CreateMessage", data).then(res => {
        console.log("Message Sent!");
    })
        .catch(err => {
            console.log("Failed to send message!");
        })
}
