
var express = require('express');
var app = express();
var http = require('http').Server(app);
var io = require('socket.io')(http);

io.on('connection', function (socket) {
    console.log('connected');
});

http.listen(process.env.PORT || 3000, function () {
    console.log('listening on port 3000');
});

/*const express = require('express');
const path = require('path');
const PORT = 5000; //process.env.PORT ||

const server = express()
    .use((req, res) => res.sendFile(INDEX) )
.listen(PORT, () => console.log(`Listening on ${ PORT }`));

const io = require('socket.io')(server);

console.log('server started');

io.on('connection', function (socket) {
    console.log('client connected');
    
    socket.on('move', function (data) {
        console.log("client moved");
    })
})*/

