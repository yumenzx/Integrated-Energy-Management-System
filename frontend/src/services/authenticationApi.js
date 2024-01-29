import axios from 'axios';
import jwt_decode from "jwt-decode";
import router from '@/router';
import ChatWBService from './ChatWBService';
import WebSocketService from './WebSocketService';

//import WebSocketService from './WebSocketService';

//const baseURL = 'https://localhost:7158/api/UserAuthentication';
//const baseURL = 'http://localhost:7000/api/UserAuthentication';
const baseURL = 'https://localhost:7100/api/UserAuthentication';

export const loginUser = async (loginCredentials) => {
  try {
    const response = await axios.post(baseURL + '/login', loginCredentials);

    if (response.data.response !== "Success") {
      alert('Logarea a esuat');
      return {
        userId: "-1",
        userRole: ""
      };
    }

    const token = response.data.token;
    localStorage.setItem('token', token);
  
    //WebSocketService.initWebSocket(token);

    // Construiește URL-ul WebSocket folosind tokenul JWT
  //const wsUrl = `ws://localhost:7038?token=${token}`;
  //const wsUrl = `wss://localhost:7038/ws`;

    try {
      var decoded = jwt_decode(token);
      console.log(decoded);
      localStorage.setItem('userId', decoded.userId);
      localStorage.setItem('userRole', decoded.userRole);
    } catch (error) {
      console.error(error);
    }
  } catch (error) {
    console.log("A aparut o eroare: " + error);
    alert('Eroare la logare, verifica consola');
  }


  return {
    userId: decoded.userId,
    userRole: decoded.userRole
  };
}



export const registerUser = async (registerCredentials) => {
  const response = await axios.post(`${baseURL}/register`, registerCredentials)
    .catch(error => {
      console.log("A aparut o eroare: " + error);
      alert('Eroare la registrare, verifica consola');
      return false;
    });
  if (response.data !== "success") {
    alert('Registrarea a esuat');
    return false;
  }

  return true;
}


export const logoutUser = () => {
  WebSocketService.closeWebSocket();
  ChatWBService.closeWebSocket();
  router.push("/login");
  localStorage.clear();
}

export const isTokenExpired = () => {
  const token = localStorage.getItem("token");

  if (!token) {
    logoutUser();
    alert('Tokenul nu a fost gasit');
    return true;
  }

  const decodedToken = jwt_decode(token);
  const currentTime = Date.now() / 1000; // Obține timpul actual în secunde

  if (decodedToken.exp > currentTime) {
    localStorage.setItem('userId', decodedToken.userId);
    localStorage.setItem('userRole', decodedToken.userRole);
    return false;
  }

  logoutUser();
  return true;
}

export const isTokenValid = () => {
  const token = localStorage.getItem("token");

  if (token) {
    const decodedToken = jwt_decode(token);
    const currentTime = Date.now() / 1000; // Obține timpul actual în secunde

    if (decodedToken.exp > currentTime) {
      localStorage.setItem('userId', decodedToken.userId);
      localStorage.setItem('userRole', decodedToken.userRole);

      alert('Already logged in. Please log ou');

      switch (decodedToken.userRole) {
        case "client":
          router.push('/clientHome');
          break;
        case "administrator":
          router.push('/adminHome');
          break;
        default:
          //router.push('/login');
          break;
      }
    }
  }
  else
    localStorage.clear();

}