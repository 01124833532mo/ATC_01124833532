import React, { useEffect, useState, useContext } from 'react';
import axios from 'axios';
import cad from '../../assets/cad.jpg';
import { useBook } from '../../Context/BookContext';

export default function Cars({ selectedCategoryId }) {
  const [events, setEvents] = useState([]);
  const [loadingEvents, setLoadingEvents] = useState(false);
  const [pageIndex, setPageIndex] = useState(1);
  const [bookingLoadingIds, setBookingLoadingIds] = useState([]); // Track loading per eventId

  const pageSize = 3;
  const token = localStorage.getItem('UserToken');
const { createBook, loading: bookingLoading } = useBook();

  const fetchEvents = async (categoryId = 2, page = 1) => {
    if (!categoryId) return;

    setLoadingEvents(true);
    try {
      const { data } = await axios.get('http://bookevent.runasp.net/api/Event/GetAllEvents', {
        params: {
          categoryId,
          pageIndex: page,
          pageSize,
        },
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });

      setEvents(data.data || []);
    } catch (error) {
      console.error('Error fetching events:', error.response?.data || error);
    } finally {
      setLoadingEvents(false);
    }
  };

  useEffect(() => {
    setPageIndex(1);
  }, [selectedCategoryId]);

  useEffect(() => {
    fetchEvents(selectedCategoryId || 2, pageIndex);
  }, [selectedCategoryId, pageIndex]);

  const handleBook = async (event) => {
    if (!token) {
      alert('User not logged in.');
      return;
    }

    // Mark this event as loading
    setBookingLoadingIds((prev) => [...prev, event.id]);

    const result = await createBook({
      eventId: event.id,
      categoryId: selectedCategoryId || 2,
    });

    if (result) {
      alert('Booking successful!');
    } else {
      alert('Booking failed.');
    }

    // Remove loading state for this event
    setBookingLoadingIds((prev) => prev.filter((id) => id !== event.id));
  };

  return (
    <div className="px-6 mb-12">
      {/* ... your header, image, etc. */}

  <div className="sm:flex items-center justify-center gap-8 px-8 py-4">
        <div className="sm:w-1/2">
          <img className="w-full h-[400px] object-cover rounded-md shadow" src={cad} alt="cars" />
        </div>
        <div className="sm:w-1/2 mt-5 sm:mt-0">
          <h2 className="text-3xl font-bold mb-4">Book Your Picnic Car Today</h2>
          <p className="text-lg text-gray-700">
            Explore beautiful destinations and book your picnic car easily with our seamless reservation system.
            Make your outing unforgettable with comfort and convenience from the very beginning.
          </p>
        </div>
      </div>
      
      <h1 className="text-3xl font-bold text-center mb-6 mt-20">Cars Events</h1>

      {loadingEvents ? (
        <p className="text-center">Loading events...</p>
      ) : (
        <>
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8 px-4">
            {events.map((event) => {
              const isLoading = bookingLoadingIds.includes(event.id);
              return (
                <div
                  key={event.id}
                  className="bg-white rounded-2xl overflow-hidden shadow-xl hover:shadow-2xl transition-shadow duration-300 ease-in-out transform hover:-translate-y-1"
                >
                  {event.imagePath ? (
                    <img src={event.imagePath} alt={event.name} className="w-full h-56 object-cover" />
                  ) : (
                    <div className="w-full h-56 bg-gray-200 flex items-center justify-center">
                      <span className="text-gray-500">No Image</span>
                    </div>
                  )}
                  <div className="text-center p-4">
                    <h3 className="text-2xl font-bold text-blue-800 mb-1">{event.name}</h3>
                    <p className="text-gray-700 text-lg mb-1">{event.description}</p>
                    <div className="text-sm text-gray-600 space-y-1">
                      <p>📅 {new Date(event.date).toLocaleDateString()}</p>
                      <p>📍 {event.venue}</p>
                      <p className="text-base font-semibold text-green-700 mt-2">Price: ${event.price}</p>
                    </div>
                    <button
                      onClick={() => handleBook(event)}
                      className="mt-2 bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-400"
                      disabled={isLoading}
                    >
                      {isLoading ? 'Booking...' : 'Book Now'}
                    </button>
                  </div>
                </div>
              );
            })}
          </div>

          {/* Pagination Controls */}
          <div className="flex justify-center mt-8 space-x-4">
            <button
              onClick={() => setPageIndex((prev) => Math.max(prev - 1, 1))}
              disabled={pageIndex === 1}
              className="px-4 py-2 bg-gray-300 rounded hover:bg-gray-400 disabled:opacity-50"
            >
              Previous
            </button>
            <span className="flex items-center font-semibold">Page {pageIndex}</span>
            <button
              onClick={() => setPageIndex((prev) => prev + 1)}
              disabled={events.length < pageSize}
              className="px-4 py-2 bg-gray-300 rounded hover:bg-gray-400 disabled:opacity-50"
            >
              Next
            </button>
          </div>
        </>
      )}
    </div>
  );
}
