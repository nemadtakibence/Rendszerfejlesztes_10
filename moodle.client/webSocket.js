const socket = new WebSocket("wss://localhost:7258/ws");

socket.onopen = (event) => {
    console.log("WebSocket connection established.");
};

socket.onmessage = (event) => {
    console.log(event.data)
    let webSocketResponse = document.getElementById("webSocketResponse");
    webSocketResponse.innerHTML = `Ezt kaptam a szervertol: ${event.data}`;
};

socket.onclose = (event) => {
    if (event.wasClean) {
        console.log(`WebSocket connection closed cleanly, code=${event.code}, reason=${event.reason}`);
    } else {
        console.error(`WebSocket connection died`);
    }
};

 // Handle errors
        socket.onerror = function(error) {
            console.error('WebSocket error:', error);
        };

// Send message to server
        function sendMessage() {
            const messageInput = document.getElementById('messageInput');
            const message = messageInput.value;
            socket.send(message);
            messageInput.value = '';
        }