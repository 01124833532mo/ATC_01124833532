import React, { useEffect, useState, createContext } from "react";

export const AdminContext = createContext(null);

export default function AdminContextProvider(props) {
  const [AdminLogin, setAdminLogin] = useState(null);

  useEffect(() => {
    if (localStorage.getItem('AdminToken') !== null) {
      setAdminLogin(localStorage.getItem('AdminToken'));
    }
  }, []);

  return (
    <AdminContext.Provider value={{ AdminLogin, setAdminLogin }}>
      {props.children}
    </AdminContext.Provider>
  );
}
