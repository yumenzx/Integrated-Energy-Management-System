<template>
    <div class="client-role-view">
        <div class="header">
            <NavigationHeader :panelName="panelName"></NavigationHeader>
        </div>
        <div class="navigation-sidebar">
            <NavigationSidebar :handleChange="handleChange"></NavigationSidebar>
        </div>
        <div v-if="currentContent === 'devicesTable'" class="table-content">
            <DevicesTable></DevicesTable>
        </div>
        <div v-else-if="currentContent === 'chart'" class="chart-content">
            <ChartDiagram></ChartDiagram>
        </div>
        <div v-else-if="currentContent === 'chat'" class="chat-content">
            <ChatSection></ChatSection>
        </div>
    </div>
</template>
  
<script>

import NavigationHeader from '@/components/NavigationHeader.vue';
import DevicesTable from '@/components/clientRole/DevicesTable.vue'
import NavigationSidebar from '@/components/clientRole/NavigationSidebar.vue'
import ChartDiagram from '@/components/clientRole/ChartDiagram.vue'
import { logoutUser, isTokenExpired } from '@/services/authenticationApi';
import WebSocketService from '@/services/WebSocketService';
import ChatSection from '@/components/clientRole/ChatSection.vue';
import ChatWBService from '@/services/ChatWBService';

export default {
    name: 'ClientRoleView',
    components: { DevicesTable, NavigationHeader, NavigationSidebar, ChartDiagram, ChatSection },
    data: function () {
        return {
            panelName: 'Client',
            currentContent: 'devicesTable'
        }
    },
    methods: {
        handleChange: function (newContent) {
            this.currentContent = newContent;
        },
        notificationCallback: function (message) {
            this.$notifyInfo(message);
        },
        webSocketCallback: function (flag, obj) {

            switch (flag) {
                case 'MESAJ_TRIMIS':
                    if (this.currentContent !== 'chat')
                        return;
                    ChatWBService.callCallbackFunction('MESAJ_TRIMIS',obj);
                    break;
                case 'MESAJ_PRIMIT':
                    if (this.currentContent !== 'chat')
                        this.$notifyInfo("Ati primit un mesaj");
                    ChatWBService.callCallbackFunction('MESAJ_PRIMIT',obj);

                    if (this.currentContent === 'chat' ) {
                        ChatWBService.setLastMessageReaded({
                            userId: -1,
                            value: true
                        });
                        ChatWBService.notifyOtherUserMessageReaded(-1);
                    } else {
                        ChatWBService.setLastMessageReaded({
                            userId: -1,
                            value: false
                        });
                    }

                    break;
                case 'MESAJ_TRIMIS_FAIL':
                    this.$notifyInfo(obj.Message);
                    //ChatWBService.callCallbackFunction('MESAJ_PRIMIT',obj);
                    break;
                case 'NOTIFY_MESSAGE_READED':
                    this.$notifyInfo("Un administrator v-a citit mesajul");
                    break;
                case 'NOTIFY_TYPING':
                    ChatWBService.callCallbackFunction('NOTIFY_TYPING', obj);
                    break;
            }
        }
    },
    mounted: function () {

        if (isTokenExpired()) {
            this.$notifyError('tokenul a expirat');
            return;
        }

        const userRole = localStorage.getItem('userRole');
        if (userRole != "client") {
            logoutUser();
            this.$notifyError('Rolul utilizatorului nu este adecvat');
            return;
        }

        WebSocketService.initWebSocket(this.notificationCallback);
        ChatWBService.initWebSocket(this.webSocketCallback);
    }
}


</script>

<style scoped lang="less">
.client-role-view {
    display: flex;
    flex-direction: row;
    justify-content: center;
    height: 100%;
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

    .table-content {
        display: flex;
        flex-direction: column;
        justify-content: center;
        margin-left: 250px;

        width: 70vw;
    }

    .chart-content {
        display: flex;
        flex-direction: column;
        justify-content: center;
    }

    .chat-content {
        margin-left: 250px;
        margin-top: 100px;
        width: 70vw;
        height: 80vh;
    }
}
</style>
  