import React from 'react';
import './App.css';
import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import Layout from './Components/Layout/Layout';
import Login from './Components/Login/Login';
import Register from './Components/Register/Register';
import Home from './Components/Home/Home'; 
import Dashboard from './Components/Dashboard/Dashboard'; 
import ConfirmEmail from './Components/ConfirmEmail/ConfirmEmail'; 
import ForgetPassword from './Components/ForgetPassword/ForgetPassword'; 
import UserContextProvider from './Context/UserContext';
import AdminContextProvider from './Context/AdminContext';
import Categories from './Components/Dashboard/Categories/Categories';
import Events from './Components/Dashboard/Event/Events';
import Book from './Components/Dashboard/Book/Book';
import Cars from './Components/Cars/Cars';
import AirportTaxis from './Components/AirportTaxis/AirportTaxis';
import Hotels from './Components/Hotels/Hotels';
import Airplane from './Components/Airplane/Airplane';
import BookContextProvider from './Context/BookContext';
import Profile from './Components/Profile/Profile';
import ChangePassword from './Components/ChangePassword/ChangePassword';
import Books from './Components/Books/Books';


function App() {
  const router = createBrowserRouter([
    {
      path: '/',
      element: <Layout />, 
      children: [
        { index: true, element: <Login /> },         
        { path: 'login', element: <Login /> },       
        { path: 'register', element: <Register /> }, 
        {path: 'home', element: <Home />},         
        {path: 'dashboard', element: <Dashboard />}, 
        {path: 'confirmemail', element: <ConfirmEmail />}, 
        {path: 'forgetpassword', element: <ForgetPassword />}, 
        {path: 'categories', element: <Categories />}, 
        {path: 'events', element: <Events />},
        {path: 'book', element: <Book />}, 
        {path: 'cars', element: <Cars />}, 
        {path: 'airporttaxis', element: <AirportTaxis />}, 
        {path: 'hotels', element: <Hotels />},
        {path: 'airplane', element: <Airplane />}, 
        {path: 'profile', element: <Profile />}, 
        {path: 'changepassword', element: <ChangePassword />}, 
        {path: 'books', element: <Books />}, 
      ],
    },
  ]);

 
  return (
    <BookContextProvider>
    <UserContextProvider>
      <AdminContextProvider>
        <RouterProvider router={router} />
      </AdminContextProvider>
    </UserContextProvider>
    </BookContextProvider>
  );
}

export default App;
