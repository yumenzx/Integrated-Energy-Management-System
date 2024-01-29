import axios from 'axios';

//const usersBaseURL = 'https://localhost:7220/api/UserAuthentication';
//const devicesBaseURL = 'https://localhost:7066/api/Device';

//const usersBaseURL = 'http://localhost:7000/api/UserAuthentication';
//const devicesBaseURL = 'http://localhost:7000/api/Device';
//const chatBaseURL = 'http://localhost:7004/api/Messages';
const usersBaseURL = 'https://localhost:7100/api/UserAuthentication';
const devicesBaseURL = 'https://localhost:7100/api/Device';
const chatBaseURL = 'https://localhost:7100/api/Messages';

export const registerUser = async (registerCredentials) => {
  const token = localStorage.getItem('token');
  const config = {
    headers: {
      Authorization: 'Bearer ' + token
    }
  };

  const response = await axios.post(`${usersBaseURL}/registerByAdmin`, registerCredentials, config)
    .catch(error => {
      console.log("A aparut o eroare: " + error);
      return false;
    });
  if (response.data !== "success") {
    alert("Registrarea a esuat");
    return false;
  }

  return true;
}

export const getUsers = async () => {
  const token = localStorage.getItem('token');
  const config = {
    headers: {
      Authorization: 'Bearer ' + token
    }
  };

  const response = await axios.get(usersBaseURL + '/getAllUsers', config);
  const users = response.data.users.$values;
  
  return users;
}

export const updateUser = async (newCredentials) => {
  const token = localStorage.getItem('token');
  const config = {
    headers: {
      Authorization: 'Bearer ' + token
    }
  };

  const response = await axios.put(usersBaseURL + '/updateCredentials', newCredentials, config);
  
  console.log("valoare user update " + response);
}

export const deleteUser =  async (userId) => {
  const token = localStorage.getItem('token');
  const config = {
    headers: {
      Authorization: 'Bearer ' + token
    }
  };

  const response = await axios.delete(usersBaseURL + '/deleteUser/' + userId, config);
  //const response = await axios.delete(usersBaseURL + '/delete/id?id=' + userId, config);
  
  console.log("valoare user delete " + response);
}



export const getDevices = async () => {
  const token = localStorage.getItem('token');
  const config = {
    headers: {
      Authorization: 'Bearer ' + token
    }
  };

  const response = await axios.get(devicesBaseURL + '/getAllDevices', config);
  const devices = response.data.devices.$values;
  return devices;
}

export const registerDevice = async (registerCredentials) => {
  const token = localStorage.getItem('token');
  const config = {
    headers: {
      Authorization: 'Bearer ' + token
    }
  };
  
  const response = await axios.post(`${devicesBaseURL}/insertDevice`, registerCredentials, config)
    .catch(error => {
      console.log("A aparut o eroare: " + error);
      return false;
    });
  if (response.data !== "success") {
    alert("Registrarea a esuat");
    return false;
  }

  return true;
}

export const updateDevice = async (datas) => {
  const token = localStorage.getItem('token');
  const config = {
    headers: {
      Authorization: 'Bearer ' + token
    }
  };

  const response = await axios.put(devicesBaseURL + '/updateDevice', datas, config);

  console.log("valoare device update " + response);
}

export const deleteDevice =  async (deviceId) => {
  const token = localStorage.getItem('token');
  const config = {
    headers: {
      Authorization: 'Bearer ' + token
    }
  };

  const response = await axios.delete(devicesBaseURL + '/deleteDevice/' + deviceId, config);
  
  console.log("valoare device delete " + response);
}

export const mapDevice = async (mapData) => {
  const token = localStorage.getItem('token');
  const config = {
    headers: {
      Authorization: 'Bearer ' + token
    }
  };

  const response = await axios.put(devicesBaseURL + '/mapDevice', mapData, config);

  console.log("valoare map device " + response);
}

export const unMapDevice = async (deviceId) => {
  const token = localStorage.getItem('token');
  const config = {
    headers: {
      Authorization: 'Bearer ' + token
    }
  };

  const response = await axios.put(devicesBaseURL + '/unMapDevice', { deviceId: String(deviceId) }, config);
  
  console.log("valoare unmap device " + response);
}

export const getMessages = async (clientId) => {
  const token = localStorage.getItem('token');
  const config = {
    headers: {
      Authorization: 'Bearer ' + token
    }
  };

  const response = await axios.get(chatBaseURL + '/getMessages/' + clientId, config);
  console.log("am primit mesajele: ", response.data);
  const messages = response.data.messagess;
  console.log(messages);
  return messages;
}