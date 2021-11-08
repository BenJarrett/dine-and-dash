import axios from 'axios';
import { sqlConfig } from '../apiKeys';

const dbUrl = sqlConfig.sqlDatabaseURL;

const getAllUsers = () => new Promise((resolve, reject) => {
  axios.get(`${dbUrl}/api/user`)
    .then((response) => resolve(response.data))
    .catch((error) => reject(error));
});

const getSingleUser = (id) => new Promise((resolve, reject) => {
  axios.get(`${dbUrl}/api/user/${id}`)
    .then((response) => resolve(response))
    .catch((error) => reject(error));
});

const createUser = (userInfo) => new Promise((resolve, reject) => {
  axios.post(`${dbUrl}/api/user`, userInfo)
    .then((response) => resolve(response.data))
    .catch((error) => reject(error));
});

export { getAllUsers, getSingleUser, createUser };