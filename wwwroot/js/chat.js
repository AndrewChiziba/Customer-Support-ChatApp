////"use strict";

////var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

////alert("in function")
//////Disable send button until connection is established
////document.getElementById("sendButton").disabled = true;

////connection.on("ReceiveMessage", function (user, message) {
////    var li = document.createElement("li");
////    document.getElementById("messagesList").appendChild(li); // for appending new messages to the list of existing messages
////    // We can assign user-supplied strings to an element's textContent because it
////    // is not interpreted as markup. If you're assigning in any other way, you 
////    // should be aware of possible script injection concerns.
////    li.textContent = `${user} says ${message}`;
////});

////connection.start().then(function () {
////    document.getElementById("sendButton").disabled = false;
////}).catch(function (err) {
////    return console.error(err.toString());
////});

////document.getElementById("sendButton").addEventListener("click", function (event) {
////    var user = document.getElementById("userInput").value; // input for specific messafee to specific user
////    var message = document.getElementById("messageInput").value; // message to be sent to db
////    connection.invoke("SendMessage", user, message).catch(function (err) {
////        return console.error(err.toString());
////    });
////    event.preventDefault();
////});



var message_input = document.getElementById('NewMessage');
var message_history = document.getElementById('msg_history');

//for when the kepad occupies space
message_input.addEventListener('focus', (event) => {
    if (window.screen.height < "1000") {
        document.getElementById("msg_history").style.height = "38vh";
    }

}, true)

message_history.addEventListener('click', (event) => {
    if (window.screen.height < "1000") {
        document.getElementById("msg_history").style.height = "65vh";
    }

}, true)


var connection = new signalR.HubConnectionBuilder().withUrl("/ChatHub").build(); //create 

//////Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (CurrentUser, SenderId, RecieverId, message) {
    //let elem = document.createElement("p");
    //elem.appendChild(document.createTextNode(message));

    setTimeout(function () {
        location.reload();
    }, 200);
    ////document.getElementById("msg_history").appendChild(elem); // for appending new messages to the list of existing messages
    //location.reload();

});

connection.start().then(function () {

    document.getElementById("sendButton").disabled = false;

}).catch(function (err) {
    return console.error(err.toString());
});

var input = document.getElementById("NewMessage");

function CreateMessage(headerId, CurrentUser, SenderId, RecieverId, time, month) {
    //alert("in function");
    if (/\S/.test(input.value)) {
        var HeaderId = headerId;
        var message = document.getElementById('NewMessage').value;
        document.getElementById('NewMessage').value = "";

        //Create the message for current user in html
        //document.getElementById("msg_history").innerHTML += "<small style='color: gray'>sent</small><div class='received_msg'><div class='received_withd_msg' id = 'received_withd_msg'><p id = 'msg'>" + message + " </p> <span class='time_date' id = 'time_date'>" + time +" | "+ month +"</span> </div></div> ";
        ////document.getElementById("msg").innerHTML += message;
        ////document.getElementById("time_date").innerHTML += "" + "";

        document.getElementById("msg_history").insertAdjacentHTML("afterbegin", "<div class='received_msg'><small style='color: gray'>sent</small><div class='received_withd_msg' id = 'received_withd_msg'><p id = 'msg'>" + message + " </p> <span class='time_date' id = 'time_date'>" + time + " | " + month + "</span> </div></div> ")

        $.ajax({
            url: '/Chat/Create',
            type: 'POST',
            // passing JSON objects as comma(,) separated values
            data: {
                HeaderId,
                message
            },
            success: (response) => {
                // response from PHP back-end PHP server
                $("#resultID").show().html(response);
            }
        })

        connection.invoke("SendMessage", CurrentUser, SenderId, RecieverId, message).catch(function (err) {
            return console.error(err.toString());
        });
        event.preventDefault();
        // jQuery Ajax Post Request using $.ajax()
    }

}


input.addEventListener("keyup", (event) => {
    if (event.key === "Enter" && /\S/.test(input.value)) {
        // Cancel the default action, if needed
        event.preventDefault();
        // Trigger the button element with a click
        document.getElementById("sendButton").click();
    }
})


function reload() {
    var container = document.getElementById("msg_history");
    var content = container.innerHTML;
    container.innerHTML = content;

    //this line is to watch the result in console , you can remove it later	
    console.log("Refreshed");
}


function TurnOn_Off() {
    var checkbox = document.getElementById("ImageOnOff");

    if (checkbox.checked == true) {

    }
    else {

    }
}

//Control chat_box height
function ChatBoxHeightControl() {
    var msg_history_height = (0.683994528 * window.screen.height);
    document.getElementById("msg_history").style.height = msg_history_height.toString();
}

function Scroll() {
    // Some code
    var myDiv = document.getElementById("msg_history");
    myDiv.scrollTop = myDiv.scrollHeight;
};

