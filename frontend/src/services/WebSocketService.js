import axios from "axios";

const WebSocketService = {
    socket: null,

    initWebSocket(callbackFct) {

        const token = localStorage.getItem("token");
        const wsUrl = `ws://localhost:7003/ws/` + token;
        this.socket = new WebSocket(wsUrl);

        this.socket.addEventListener('open', (event) => {
            console.log('Conexiune WebSocket deschisa', event);
        });

        this.socket.addEventListener('message', (event) => {
            console.log('Mesaj primit:', event.data);
            callbackFct(event.data);
            //this.$notifyInfo(event.data);
        });

        this.socket.addEventListener('close', (event) => {
            console.log('Conexiune WebSocket închisa', event);
        });
    },
/*
    sendMessage(message) {--------------------------------------------------////////-
        if (this.socket.readyState === WebSocket.OPEN) {
            this.socket.send(message);
            console.log('Mesaj trimis cu succes!');
        } else {
            console.log('Conexiunea WebSocket nu este deschisa');
        }
    },*/

    async closeWebSocket() {
        if(this.socket !== null)
            this.socket.close();
        else
            console.log("logout:: socketul este null");

        const token = localStorage.getItem("token");
        const wsUrl = `http://localhost:7003/ws/` + token;
        await axios.delete(wsUrl);

        this.socket = null;
    }
};

export default WebSocketService;
