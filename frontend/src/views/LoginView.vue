<template>
    <div class="login-view">
        <LoginPanel></LoginPanel>
    </div>
</template>
  
<script>
import LoginPanel from '@/components/LoginPanel.vue';
import {isTokenValid} from '@/services/authenticationApi';


export default {
    name: 'LoginView',
    components: { LoginPanel },
    mounted: function () {
        isTokenValid();
    }
}
</script>

<style scoped lang="less">
.login-view{
    display: flex;
    justify-content: center;
    align-items: center;
    height: 100%;
    width: 100%;
    background-color: rgb(1,1,55);

}

</style>
  