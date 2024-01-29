<template>
    <div class="admin-role-view">
        <div class="header">
            <NavigationHeader :panelName="panelName"></NavigationHeader>
        </div>
        <div class="navigation-sidebar">
            <NavigationSidebar :handleChange="handleChange" :messagesList="messagesList"></NavigationSidebar>
        </div>
        <div v-if="currentContent === 'usersTable'" class="content">
            <UsersTable></UsersTable>
        </div>
        <div v-else-if="currentContent === 'devicesTable'" class="content">
            <DevicesTable></DevicesTable>
        </div>
        <div v-else-if="currentContent === 'chat'" class="chat-content">
            <ChatSection :clientId="clientId"></ChatSection>
        </div>
    </div>
</template>
  
<script>

import DevicesTable from '@/components/adminRole/DevicesTable.vue'
import NavigationSidebar from '@/components/adminRole/NavigationSidebar.vue';
import UsersTable from '@/components/adminRole/UsersTable.vue'
import NavigationHeader from '@/components/NavigationHeader.vue';
import ChatSection from '@/components/adminRole/ChatSection.vue';
//import router from '@/router';
import { logoutUser, isTokenExpired } from '@/services/authenticationApi';
import ChatWBService from '@/services/ChatWBService';


export default {
    name: 'AdminRoleView',
    components: { UsersTable, DevicesTable, NavigationHeader, NavigationSidebar, ChatSection },
    data: function () {
        return {
            panelName: 'Admin',
            currentContent: 'usersTable',
            messagesList: [],
            clientId: -1
        }
    },
    methods: {
        handleChange: function (newContent, userId) {
            if (newContent === 'chat') {
                this.currentContent = '';
                this.clientId = userId;
                this.$nextTick(() => {
                    this.currentContent = 'chat';
                });
            }
            else
                this.currentContent = newContent;
        },
        webSocketCallback: function (flag, obj) {

            const message = obj.Message;
            const to = obj.To;
            const from = obj.From;
            const userName = "User " + from;
            switch (flag) {
                case 'MESAJ_TRIMIS':
                    if (this.currentContent !== 'chat')
                        return;
                    ChatWBService.callCallbackFunction('MESAJ_TRIMIS', obj);
                    break;
                case 'MESAJ_PRIMIT':
                    if (this.currentContent !== 'chat')
                        this.$notifyInfo("Ati primit un mesaj");
                    ChatWBService.callCallbackFunction('MESAJ_PRIMIT', obj);

                    if (this.currentContent === 'chat' && this.clientId === Number(from)) {
                        ChatWBService.setLastMessageReaded({
                            userId: Number(from),
                            value: true
                        });
                        ChatWBService.notifyOtherUserMessageReaded(Number(from));
                    } else {
                        ChatWBService.setLastMessageReaded({
                            userId: Number(from),
                            value: false
                        });
                    }

                    break;
                case 'MESAJ_PRIMIT_CONV_NOU':
                    this.$notifyInfo("Aveti un nou mesaj");

                    this.messagesList.push({
                        userId: Number(from),
                        name: userName
                    });

                    ChatWBService.setLastMessageReaded({
                        userId: Number(from),
                        value: false
                    });

                    break;
                case 'CLIENT_DECONECTAT':
                    this.$notifyInfo(message);
                    if (this.currentContent === 'chat' && this.clientId === Number(to))
                        this.currentContent = 'usersTable';

                    this.messagesList = this.messagesList.filter(item => item.userId !== Number(to));

                    break;
                case 'NOTIFY_MESSAGE_READED':
                    this.$notifyInfo(message);
                    break;
                case 'NOTIFY_TYPING':
                    ChatWBService.callCallbackFunction('NOTIFY_TYPING', obj);
                    break;
            }
        }
    },
    mounted: function () {

        if (isTokenExpired()) {
            this.$notifyError("tokenul a exiprat");
            return;
        }

        const userRole = localStorage.getItem('userRole');
        if (userRole != "administrator") {
            logoutUser();
            this.$notifyError("Rolul utilizatorului nu este adecvat");
            return;
        }

        ChatWBService.initWebSocket(this.webSocketCallback);
    }
}


</script>

<style scoped lang="less">
.admin-role-view {
    display: flex;
    flex-direction: row;
    justify-content: center;
    height: 100vh;
    width: 100%;
    background-color: rgb(240, 242, 246);

    .header {
        height: 50px;
        position: fixed;
        width: 100%;
        z-index: 10;
    }

    .navigation-sidebar {
        position: fixed;
        height: 100%;
        width: 250px;
        top: 50px;
        left: 0;
    }

    .content {
        display: flex;
        flex-direction: column;
        justify-content: center;
        margin-left: 250px;

        width: 70vw;
    }

    .chat-content {
        margin-left: 250px;
        margin-top: 100px;
        width: 70vw;
        height: 80vh;
    }
}
</style>
  