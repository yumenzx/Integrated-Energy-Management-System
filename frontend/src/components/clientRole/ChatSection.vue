<template>
    <div class="chat-section">

        <div class="message-content">
            <v-container class="fill-height">
                <v-row class="fill-height">
                    <v-col>
                        <div class="message-container">
                            <div v-for="(item, index) in chat" :key="index"
                                :class="['d-flex flex-row align-center my-2', String(item.from) === String(userId) ? 'justify-end' : null]">

                                <div v-if="String(item.from) === String(userId)" class="message-box user-message-box"
                                    v-bind="attrs" v-on="on">
                                    {{ item.message }}
                                </div>
                                <div v-else class="message-box other-message-box" v-bind="attrs" v-on="on">
                                    {{ item.message }}
                                </div>

                            </div>

                            <div v-if="userIsTyping" class="ticontainer">
                                <div class="tiblock">
                                    <div class="tidot"></div>
                                    <div class="tidot"></div>
                                    <div class="tidot"></div>
                                </div>
                            </div>

                        </div>
                    </v-col>
                </v-row>
            </v-container>
        </div>

        <div class="footer-send-section">
            <v-footer>
                <v-container class="ma-0 pa-0">
                    <v-row no-gutters>
                        <v-col>
                            <div class="d-flex flex-row align-baseline smaller">
                                <v-text-field v-model="msg" dense outlined placeholder="Text Administrators"
                                    @input="notifyTyping" @keypress.enter="send"></v-text-field>
                                <v-btn icon class="ml-4" @click="send">
                                    <v-icon color="primary">mdi-send</v-icon>
                                </v-btn>
                            </div>
                        </v-col>
                    </v-row>
                </v-container>
            </v-footer>
        </div>
    </div>
</template>
    
<script>
import { isTokenExpired } from '@/services/authenticationApi';
import ChatWBService from '@/services/ChatWBService';
import { getMessages } from '@/services/clientRole/clientApi';

export default {
    name: 'ChatSection',
    data: function () {
        return {
            chat: [],
            msg: null,
            userId: "",
            userIsTyping: false,
            timeoutRef: null
        }
    },
    methods: {
        send: function () {
            const obj = {
                type: "message",
                from: "",
                to: -1,
                message: this.msg
            };

            ChatWBService.sendMessage(obj);
        },
        callbackFct: function (flag, obj) {
            switch (flag) {
                case 'MESAJ_TRIMIS':
                    this.chat.push({
                        from: this.userId,
                        message: this.msg,
                        to: 0
                    })
                    this.msg = null
                    break;

                case 'MESAJ_PRIMIT':
                    this.chat.push({
                        from: obj.from,
                        message: obj.Message,
                        to: this.userId
                    })
                    break;

                case 'NOTIFY_TYPING':
                    this.userIsTyping = true;
                    clearTimeout(this.timeoutRef);
                    this.timeoutRef = setTimeout(() => {
                        this.userIsTyping = false;
                    }, 600);
                    break;
            }

        },
        notifyTyping() {
            ChatWBService.notifyOtherUserTyping(-1);
        },
        verifyAuthorization: function () {
            if (isTokenExpired()) {
                alert("tokenul a exiprat");
                return false
            }

            return true;

        }
    },
    mounted: async function () {
        if (!this.verifyAuthorization())
            return;

        const userId = localStorage.getItem("userId");
        this.userId = userId;

        ChatWBService.setCallbackFunction(this.callbackFct);

        this.chat = await getMessages();

        if (ChatWBService.lastMessageWasReaded(-1) === false) // daca ultimul mesaj nu a fost citit
        {
            ChatWBService.setLastMessageReaded({
                userId: -1,
                value: true
            });
            // notify the clientID that the message was readed
            ChatWBService.notifyOtherUserMessageReaded(-1);
        }
    }
}


</script>
  
<style scoped lang="less">
@import "@/styles/clientRole/devicesTableStyles.css";

.chat-section {
    background-color: rgb(230, 232, 236);
    width: 100%;
    height: 100%;
    position: relative;

    border-style: groove;
    border-radius: 4px;
    border-color: lightgrey;

    .header {
        display: flex;
        justify-content: space-between;

        height: 50px;
        padding: 5px 15px 5px 15px;

    }

    .message-content {
        padding-bottom: 100px;
        max-height: 100%;
        overflow-y: auto;

        .message-container {
            width: 100%;

            .message-box {
                padding: 8px;
                border-radius: 8px;
                word-wrap: break-word;
                max-width: 60%;
            }

            .user-message-box {
                background-color: #2196f3;
                color: white;
                margin-right: 8px;
            }

            .other-message-box {
                background-color: #ccc;
                color: black;
                margin-left: 8px;
            }
        }
    }

    .footer-send-section {
        position: absolute;
        bottom: 0px;
        width: 100%;

        .smaller {
            max-height: 50px;
        }
    }

}

.tiblock {
    align-items: center;
    display: flex;
    height: 17px;
}

.ticontainer .tidot {
    background-color: #90949c;
}

.tidot {
    animation: mercuryTypingAnimation 1.5s infinite ease-in-out;
    border-radius: 2px;
    display: inline-block;
    height: 6px;
    margin-right: 2px;
    width: 6px;
}

@keyframes mercuryTypingAnimation {
    0% {
        transform: translateY(0px)
    }

    28% {
        transform: translateY(-5px)
    }

    44% {
        transform: translateY(0px)
    }
}

.tidot:nth-child(1) {
    animation-delay: 200ms;
}

.tidot:nth-child(2) {
    animation-delay: 300ms;
}

.tidot:nth-child(3) {
    animation-delay: 400ms;
}
</style>
    