import React, { useContext } from 'react';
import { Formik, Form, Field, ErrorMessage } from 'formik';
import * as Yup from 'yup';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import { Link } from 'react-router-dom';
import { jwtDecode } from 'jwt-decode';
import { UserContext } from '../../Context/UserContext';
import { AdminContext } from '../../Context/AdminContext';



const Login = () => {
    const { setUserLogin } = useContext(UserContext);
  const { setAdminLogin } = useContext(AdminContext);

  const nav = useNavigate();

  // Initial form values
  const initialValues = {
    email: '',
    password: '',
  };

  // Validation schema using Yup
  const validationSchema = Yup.object({
    email: Yup.string().email('Invalid email address').required('Email is required'),
    password: Yup.string().min(6, 'Password must be at least 6 characters').required('Password is required'),
  });

  // Handle form submission

const handleSubmit = async (values) => {
  const { email, password } = values;
  try {
    const { data } = await axios.post('http://bookevent.runasp.net/api/Auth/login', {
      email,
      password,
    }, {
      headers: {
        'Content-Type': 'application/json',
      },
    });


    console.log('Login response:', data);
    alert('Login successful!');
    const token = data.token;

const decoded = jwtDecode(token);
console.log('Decoded token:', decoded); // <- ✅ Helps verify structure

// Check for role under different key names
const userRole =
  decoded.role ||
  decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] ||
  decoded["roles"] ||
  null;

console.log('User Role:', userRole);

if (userRole === 'Admin') {
  localStorage.setItem("AdminToken", token);
  setAdminLogin(token);
  nav('/dashboard');
} else {
  localStorage.setItem("UserToken", token);
  setUserLogin(token);
  nav('/home');
}


  } catch (err) {
    console.error('Login error:', err.response?.data || err.message);
    alert(err.response?.data?.message || 'Login failed. Please check your credentials.');
  }
};




  return (
    <div className="min-h-screen flex items-center justify-center bg-gradient-to-b from-fuchsia-600 to-blue-950">
      <div className="bg-white p-8 rounded-lg shadow-md w-full max-w-md">
        <h2 className="text-2xl font-bold mb-6 text-center text-blue-600">Login</h2>

        {/* Formik form for login */}
        <Formik initialValues={initialValues} validationSchema={validationSchema} onSubmit={handleSubmit}>
          {({ isSubmitting }) => (
            <Form className="space-y-4">
              <div>
                <label htmlFor="email" className="block text-blue-600 font-medium">Email</label>
                <Field type="email" name="email" id="email"
                  className="w-full px-4 py-2 mt-1 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500" />
                <ErrorMessage name="email" component="div" className="text-red-500 text-sm mt-1" />
              </div>

              <div>
                <label htmlFor="password" className="block text-blue-600 font-medium">Password</label>
                <Field type="password" name="password" id="password"
                  className="w-full px-4 py-2 mt-1 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500" />
                <ErrorMessage name="password" component="div" className="text-red-500 text-sm mt-1" />
              </div>

              <button type="submit" disabled={isSubmitting}
                className="w-full bg-blue-700 text-white py-2 rounded-md  hover:bg-amber-600 transition ">
                {isSubmitting ? 'Logging in...' : 'Login'}
              </button>
            </Form>
          )}
        </Formik>

          {/* Links for registration and forget password */}
      <div className="flex justify-between w-full max-w-md mt-4 text-sm text-gray-600 flex-wrap sm:flex-nowrap">
        <p className="w-full sm:w-auto text-center sm:text-left">
          Don't have an account?{' '}
          <Link to='/register' className="text-blue-600 hover:underline">
            Create account
          </Link>
        </p>
        <Link to='/forgetpassword' className="text-blue-600 hover:underline sm:text-right w-full sm:w-auto text-center mt-2 sm:mt-0">
          Forget Password
        </Link>
      </div>
      </div>
      
    </div>
  );
};

export default Login;
