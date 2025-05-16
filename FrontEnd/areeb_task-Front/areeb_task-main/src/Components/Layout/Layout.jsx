import React from 'react';
import { Outlet, useLocation } from 'react-router-dom';
import Navbar from '../Navbar/Navbar';
import Footer from '../Footer/Footer';

function Layout() {
  const location = useLocation();

  // Determine if current route is home
  const isHomePage = location.pathname === '/home';

  return (
    <div>
      {!isHomePage && <Navbar />}
      <Outlet />
      <Footer />
    </div>
  );
}

export default Layout;
