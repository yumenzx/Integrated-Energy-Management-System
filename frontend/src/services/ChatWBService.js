import axios from "axios";

//const chatBaseURL = 'https://localhost:7268/api/Messages';
//const chatBaseURL = 'http://localhost:7004/api/Messages';
const chatBaseURL = 'https://localhost:7100/api/Messages';

const ChatWBService = {
    socket: null,
    currentUser: "",
    adminIdDestination: -1,
    chatCallbackFunction: null,
    lastMessageReaded: [],

    initWebSocket(callbackFunction) {

        const token = localStorage.getItem("token");
        //const wsUrl = `wss://localhost:7268/ws/` + token;
	const wsUrl = `ws://localhost:7004/ws/` + token;

        this.socket = new WebSocket(wsUrl);
        this.currentUser = token;
        //this.currentUser = localStorage.getItem('userId');

        this.socket.addEventListener('open', (event) => {
            console.log('Conexiune WebSocket deschisa', event);
        });

        this.socket.addEventListener('message', (event) => {
            console.log('Mesaj primit:', event.data);
            const receivedObject = JSON.parse(event.data);
            console.log('mesajul deserializat:', receivedObject);

            const type = receivedObject.Type;
            switch (type) {
                case "MESAJ_TRIMIS_SUCCESS": // mesajul s-a trimis cu success
                    callbackFunction("MESAJ_TRIMIS", receivedObject);
                    break;
                case "MESAJ_PRIMIT_SUCCESS": // s-a primit un mesaj
                    callbackFunction("MESAJ_PRIMIT", receivedObject);
                    break;
                case "MESAJ_PRIMIT_SUCCESS_CONV_NOU": // s-a primit un nou mesaj. de la client la un admin
                    callbackFunction("MESAJ_PRIMIT_CONV_NOU", receivedObject);
                    break;
                case "CLIENT_DECONECTAT": // un client s-a deconectat
                    callbackFunction("CLIENT_DECONECTAT", receivedObject);
                    break;
                case "MESAJ_TRIMIS_FAIL": // un client s-a deconectat
                    console.log("Mesajul nu a fost trimis:", receivedObject);
                    callbackFunction("MESAJ_TRIMIS_FAIL", receivedObject);
                    //this.$notifyInfo("Mesajul nu a fost trimis: " + receivedObject.Message);
                    break;
                case "NOTIFY_MESSAGE_READED": // mesajul a fost citit
                    callbackFunction("NOTIFY_MESSAGE_READED", receivedObject);
                    break;
                case "NOTIFY_TYPING": // se scrie un mesaj
                    callbackFunction("NOTIFY_TYPING", receivedObject);
                    break;
                default:
                    alert("receivedobj.type nu este valid");
                    console.log('receivedobj.type nu este valid', type);
                    break;
            }

            //callbackFct(event.data);
            //this.$notifyInfo(event.data);
        });

        this.socket.addEventListener('close', (event) => {
            console.log('Conexiune WebSocket închisa', event);
        });
    },

    setCallbackFunction(fct) {
        this.chatCallbackFunction = fct;
    },
    callCallbackFunction(flag, obj) {
        this.chatCallbackFunction(flag, obj);
    },

    setLastMessageReaded(newValue) {
        const idx = this.lastMessageReaded.findIndex(m => m.userId === newValue.userId);
        if (idx === -1)
            this.lastMessageReaded.push(newValue);
        else
            this.lastMessageReaded[idx] = newValue;
    },
    lastMessageWasReaded(userId) {
        if (this.lastMessageReaded.length === 0) {
            return true;
        }

        const idx = this.lastMessageReaded.findIndex(m => m.userId === userId);
        return this.lastMessageReaded[idx].value;
    },
    async notifyOtherUserMessageReaded(clientId) {
        const token = localStorage.getItem('token');
        const config = {
            headers: {
                Authorization: 'Bearer ' + token
            }
        };

        const response = await axios.get(chatBaseURL + '/notifyMessageReaded/' + String(clientId), config);
        console.log("valoare notify message readed: ", response.data);
    },

    async notifyOtherUserTyping(clientId) {
        const token = localStorage.getItem('token');
        const config = {
            headers: {
                Authorization: 'Bearer ' + token
            }
        };

        const response = await axios.get(chatBaseURL + '/notifyTyping/' + String(clientId), config);
        console.log("valoare notify typing: ", response.data);
    },


    sendMessage(messageObject) {

        messageObject.from = this.currentUser;

        if (messageObject.to === -1) // logica inca nu functioneaza, clientul nu stiu cu care admin vorbeste deocamdata
            messageObject.to = this.adminIdDestination;

        if (this.socket.readyState === WebSocket.OPEN) {
            const messageString = JSON.stringify(messageObject);
            this.socket.send(messageString);
            console.log('Mesaj trimis cu succes!');
        } else {
            console.log('Conexiunea WebSocket nu este deschisa');
        }
    },

    async closeWebSocket() {
        if (this.socket !== null)
            this.socket.close();
        else
            console.log("logout:: socketul este null");

        const token = localStorage.getItem("token");
        const wsUrl = `http://localhost:7004/ws/` + token;
        await axios.delete(wsUrl);

        this.socket = null;
    }
};

export default ChatWBService;
