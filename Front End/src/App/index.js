import React, { useState, useEffect } from 'react';
import { BrowserRouter as Router } from 'react-router-dom';
import firebase from 'firebase';
import './App.scss';
import NavBar from '../components/NavBar';
import Routes from '../helpers/Routes';
import { getAllProducts } from '../helpers/data/ProductData';
import { getAllUsers } from '../helpers/data/UserData';

function App() {
  const [fbUser, setFbUser] = useState(null);
  const [products, setProducts] = useState([]);
  const [users, setUsers] = useState([]);

  useEffect(() => {
    firebase.auth().onAuthStateChanged((authed) => {
      if (authed) {
        authed.getIdToken().then((token) => window.sessionStorage.setItem('token', token));
        setFbUser(true);
        // getAllProducts().then(setProducts);
        // getAllUsers().then(setUsers);
      } else if (fbUser || fbUser === null) {
        setFbUser(false);
      }
    });
  }, []);

  useEffect(() => {
    getAllProducts().then(setProducts);
    getAllUsers().then(setUsers);
  }, []);

  return (
    <>
      <Router>
        <div className='App'>
          <NavBar
            fbUser={fbUser}
          />
          <Routes
            fbUser={fbUser}
            products={products}
            users={users}
          />
        </div>
      </Router>
    </>
  );
}

export default App;
