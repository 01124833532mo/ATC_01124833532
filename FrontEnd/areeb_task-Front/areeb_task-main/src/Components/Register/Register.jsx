import React from 'react';
import { Formik, Form, Field, ErrorMessage } from 'formik';
import * as Yup from 'yup';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';

export default function Register() {
  const nav = useNavigate();

  // Initial form values
  const initialValues = {
    fullName: '',
    email: '',
    phoneNumber: '',
    password: '',
  };

  // Validation schema using Yup
  const validationSchema = Yup.object({
    fullName: Yup.string()
      .min(3, 'Must be at least 3 characters')
      .required('fullName is required'),
    email: Yup.string()
      .email('Invalid email address')
      .required('Email is required'),
    phoneNumber: Yup.string()
      .matches(/^01[0125][0-9]{8}$/, 'Phone number must start with 010, 011, 012, or 015 and be 11 digits')
      .required('Phone is required'),
    password: Yup.string()
      .min(6, 'Must be at least 6 characters')
      .required('Password is required'),
  });

  // Handle form submission
  const handleSubmit = async (values, { resetForm }) => {
    console.log('Form values:', values);
    try {
      const response = await axios.post('http://bookevent.runasp.net/api/Auth/Register', values);
      alert('User registered successfully!');
      console.log('User registered:', response.data);
      resetForm(); // Clear the form after submission
      nav('/confirmemail'); // Optional: redirect to login after registration
    } catch (error) {
      console.error('Registration error:', error.response?.data || error.message);
    }
  };

  return (
    <div className="w-full mx-auto py-20 border rounded shadow bg-gradient-to-b from-fuchsia-600 to-blue-950">
      <div className="max-w-md mx-auto bg-white p-6 rounded shadow-md">
        <h2 className="text-3xl font-bold mb-6 text-center text-blue-600">Register</h2>

        {/* Formik form for registration */}
        <Formik initialValues={initialValues} validationSchema={validationSchema} onSubmit={handleSubmit}>
          <Form className="space-y-4">
            <div>
              <label className="block mb-1 text-blue-600 font-semibold">fullName</label>
              <Field type="text" name="fullName" className="w-full border-1 border-amber-200 px-3 py-2 rounded"/>
              <ErrorMessage name="fullName" component="div" className="text-red-500 text-sm" />
            </div>

            <div>
              <label className="block mb-1 text-blue-600 font-semibold">Email</label>
              <Field type="email" name="email" className="w-full border-1 border-amber-200 px-3 py-2 rounded"/>
              <ErrorMessage name="email" component="div" className="text-red-500 text-sm" />
            </div>

            <div>
              <label className="block mb-1 text-blue-600 font-semibold">Phone</label>
              <Field type="tel" name="phoneNumber" className="w-full border-1 border-amber-200 px-3 py-2 rounded"/>
              <ErrorMessage name="phoneNumber" component="div" className="text-red-500 text-sm" />
            </div>

            <div>
              <label className="block mb-1 text-blue-600 font-semibold">Password</label>
              <Field type="password" name="password" className="w-full border-1 border-amber-200 px-3 py-2 rounded"/>
              <ErrorMessage name="password" component="div" className="text-red-500 text-sm" />
            </div>

            <button type="submit" className="w-full bg-blue-700 text-white py-2 rounded  hover:bg-amber-600">
              Register
            </button>
          </Form>
        </Formik>
      </div>
    </div>
  );
}
