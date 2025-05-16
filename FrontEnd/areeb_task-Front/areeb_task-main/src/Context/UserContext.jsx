import React, { createContext, useEffect, useState } from "react";

export const UserContext = createContext(null);

export default function UserContextProvider(props) {
  const [UserLogin, setUserLogin] = useState(null);

  useEffect(() => {
    if (localStorage.getItem('UserToken') !== null) {
      setUserLogin(localStorage.getItem('UserToken'));
    }
  }, []);

  return (
    <UserContext.Provider value={{ UserLogin, setUserLogin }}>
      {props.children}
    </UserContext.Provider>
  );
}
