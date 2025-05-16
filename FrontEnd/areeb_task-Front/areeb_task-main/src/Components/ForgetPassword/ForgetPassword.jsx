import React from 'react';
import { useFormik } from 'formik';
import * as Yup from 'yup';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';

export default function ForgetPassword() {


  let navigate = useNavigate()

  // Function to handle form submission
  async function handleSubmit(formValues) {
    try {
      const response = await axios.post('http://bookevent.runasp.net/api/auth/Confirm-Email', {
        email: formValues.email
      });
      console.log('Response:', response.data);
      navigate('/verfiycode');
      alert('Password reset link sent successfully!');
    } catch (error) {
      console.error('Error sending reset request:', error.response?.data || error.message);
      alert('Failed to send reset request. Please try again.');
    }
  }

  // Validation schema for form validation
  let validationSchema = Yup.object().shape({
    email: Yup.string()
      .email('Invalid email address')
      .required('Email is required'),
  });

  // Formik setup for managing form state and validation
  let formik = useFormik({
    initialValues: {
      email: '',
    },
    validationSchema: validationSchema,
    onSubmit: handleSubmit,
  });

  return (

<div className="min-h-screen bg-gradient-to-b from-fuchsia-600 to-blue-950 flex flex-col items-center justify-center text-white">
     

      {/* Form for email input and submit button */}
      <form onSubmit={formik.handleSubmit} className="place-items-center bg-white p-6 rounded-xl shadow-xl flex flex-col items-center">
        {/* Header Section */}
      <div className="mt-10 place-items-center">
        <h1 className="lg:text-3xl text-2xl text-[#0B4261]">Forget Password</h1>
        <p className="text-lg font-semibold mt-2 text-[#0B4261]">Enter your email to reset your password</p>
      </div>

        {/* Email Input */}
        <div className="mt-10 ">
          <input onBlur={formik.handleBlur} onChange={formik.handleChange} type="email" name="email" id="email" value={formik.values.email}
            className="border-2 sm:w-[500px] p-3 rounded-full placeholder-black text-left w-[300px] border-[gray] focus:outline-none focus:border-[#0e1f2b]"
            placeholder="Email Address"
          />
          {formik.errors.email && formik.touched.email ? (
            <div className="flex items-center mx-auto sm:w-[500px] w-[300px] p-4 mt-2 text-sm text-red-800 rounded-lg bg-red-50">
              {formik.errors.email}
            </div>
          ) : null}
        </div>

        {/* Submit Button */}
        <div>
          <button type="submit" className="md:text-lg text-sm mt-10 bg-blue-700 text-white sm:w-[500px] w-[300px] p-3 rounded-md cursor-pointer hover:bg-amber-600 hover:text-white">
            SEND RESET LINK
          </button>
        </div>
      </form>
    </div>
  );
}
