import React, { createContext, useContext, useState } from 'react';
import axios from 'axios';

const BookContext = createContext();

export const BookProvider = ({ children }) => {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  const createBook = async ({ eventId, categoryId }) => {
    setLoading(true);
    setError(null);

    try {
      const token = localStorage.getItem('UserToken');
      const response = await axios.post(
        'http://bookevent.runasp.net/api/Book/CreateBook',
        { eventId, categoryId },
        {
          headers: {
            'Content-Type': 'application/json',
            Authorization: `Bearer ${token}`,
          },
        }
      );
      return response.data;
    } catch (err) {
      setError(err?.response?.data || 'Error creating book');
      console.error('Create book error:', err);
      return null;
    } finally {
      setLoading(false);
    }
  };

  return (
    <BookContext.Provider value={{ createBook, loading, error }}>
      {children}
    </BookContext.Provider>
  );
};

export const useBook = () => useContext(BookContext);
export default BookProvider;