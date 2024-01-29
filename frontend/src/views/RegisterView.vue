<template>
    <div class="register-view">
        <RegisterPanel></RegisterPanel>
    </div>
</template>
  
<script>
import RegisterPanel from '@/components/RegisterPanel.vue';
import { isTokenValid } from '@/services/authenticationApi';


export default {
    name: 'RegisterView',
    components: { RegisterPanel },
    mounted: function () {
        isTokenValid();
    }
}
</script>

<style scoped lang="less">
.register-view{
    display: flex;
    justify-content: center;
    align-items: center;
    height: 100%;
    width: 100%;
    background-color: rgb(1,1,55);

}

</style>
  