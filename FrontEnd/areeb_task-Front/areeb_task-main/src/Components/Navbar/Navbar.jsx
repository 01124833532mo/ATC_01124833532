import React, { useContext } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { UserContext } from '../../Context/UserContext';
import { AdminContext } from '../../Context/AdminContext';
import { FaUserCircle, FaCalendarCheck } from 'react-icons/fa';

export default function Navbar() {
    const { UserLogin, setUserLogin } = useContext(UserContext);
    const { AdminLogin, setAdminLogin } = useContext(AdminContext);
    const navigate = useNavigate();

    function logOut() {
        localStorage.removeItem("UserToken");
        localStorage.removeItem("AdminToken");
        setUserLogin(null);
        setAdminLogin(null);
        navigate('/login');
    }

    const goToProfile = () => {
        if (UserLogin) navigate('/profile');
    };

    return (
        <div className='bg-blue-500 flex items-center justify-between px-4 py-2'>
            <h1 className='text-2xl text-white font-bold'>Book With Us</h1>

            {(!UserLogin && !AdminLogin) ? (
                <div>
                    <Link to="/login" className="text-white hover:text-gray-300 px-4 py-2">
                        Login
                    </Link>
                    <Link to="/register" className="text-white hover:text-gray-300 px-4 py-2">
                        Register
                    </Link>
                </div>
            ) : (
                <div className='flex items-center space-x-4'>
                    {/* Show only for users (not admins) */}
                    {UserLogin && (
                        <>
                            <FaUserCircle
                                size={25}
                                className="text-white cursor-pointer hover:text-gray-300"
                                onClick={goToProfile}
                                title="Profile"
                            />
                            <FaCalendarCheck
                                size={22}
                                className="text-white cursor-pointer hover:text-gray-300"
                                onClick={() => navigate('/books')}
                                title="My Bookings"
                            />
                        </>
                    )}

                    <span
                        className="text-white hover:text-gray-300 px-2 py-1 cursor-pointer"
                        onClick={logOut}
                    >
                        Logout
                    </span>
                </div>
            )}
        </div>
    );
}
