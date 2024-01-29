import axios from 'axios';

//const devicesBaseURL = 'http://localhost:7000/api/Device';
//const consumptionBaseURL = 'http://localhost:7000/api/Consumption';
//const chatBaseURL = 'http://localhost:7004/api/Messages';
const devicesBaseURL = 'https://localhost:7100/api/Device';
const consumptionBaseURL = 'https://localhost:7100/api/Consumption';
const chatBaseURL = 'https://localhost:7100/api/Messages';

export const getDevices = async () => {
  const token = localStorage.getItem('token');
  const config = {
    headers: {
      Authorization: 'Bearer ' + token
    }
  };

  const response = await axios.post(devicesBaseURL + '/getUserDevices', { id: "post" }, config);
  const devices = response.data.devices.$values;
  return devices;
}

export const getChartData = async (date) => {
  /*const token = localStorage.getItem('token');
  const config = {
    headers: {
      Authorization: 'Bearer ' + token
    }
  };
*/
  const userId = localStorage.getItem('userId');
  const body = {
    userId: Number(userId),
    timestampt: date
  };
  //console.log(body);
  const response = await axios.post(consumptionBaseURL + '/getChartData', body);
  const chartData = response.data.consumptions;

  return chartData;
}

export const getMessages = async () => {
  const token = localStorage.getItem('token');
  const config = {
    headers: {
      Authorization: 'Bearer ' + token
    }
  };

  const response = await axios.get(chatBaseURL + '/getMessages/0', config);
  console.log("am primit mesajele: ", response.data);
  const messages = response.data.messagess;
  return messages;
}