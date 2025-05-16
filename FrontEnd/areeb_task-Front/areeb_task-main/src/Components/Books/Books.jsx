import React, { useEffect, useState } from 'react';
import axios from 'axios';

export default function Books() {
  const [bookings, setBookings] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [deletingId, setDeletingId] = useState(null);

  const fetchBookings = async () => {
    const token = localStorage.getItem('UserToken');
    try {
      const { data: responseData } = await axios.get(
        'http://bookevent.runasp.net/api/Book/GetAllBooksForUser',
        {
          headers: { Authorization: `Bearer ${token}` },
        }
      );

      const bookingsArray = responseData.data || responseData.bookings || [];

      if (Array.isArray(bookingsArray)) {
        setBookings(bookingsArray);
        setError(null);
      } else {
        setBookings([]);
        setError('Bookings data is not an array.');
      }
    } catch (err) {
      console.error('Error fetching bookings:', err);
      setError('Failed to load bookings. Please try again.');
      setBookings([]);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchBookings();
  }, []);

  const formatDate = (dateString) => {
    if (!dateString) return 'N/A';
    const options = { year: 'numeric', month: 'long', day: 'numeric' };
    return new Date(dateString).toLocaleDateString(undefined, options);
  };

  // Delete booking function
  const deleteBooking = async (id) => {
    const token = localStorage.getItem('UserToken');
    if (!window.confirm('Are you sure you want to cancel this booking?')) return;

    try {
      setDeletingId(id);
      await axios.delete(`http://bookevent.runasp.net/api/Book/CancelBook/${id}`, {
        headers: { Authorization: `Bearer ${token}` },
      });
      // After delete, refresh bookings
      fetchBookings();
      setBookings((prevBookings) => prevBookings.filter((booking) => booking.id !== id));

      alert('Booking cancelled successfully.');
    } catch (err) {
      console.error('Error deleting booking:', err);
      alert('Failed to cancel booking. Please try again.');
    } finally {
      setDeletingId(null);
    }
  };

  if (loading)
    return <p className="text-center mt-10 text-gray-600">Loading bookings...</p>;
  if (error)
    return <p className="text-center mt-10 text-red-600">{error}</p>;

  if (bookings.length === 0)
    return (
      <div className="max-w-5xl mx-auto px-4 mt-10">
        <h1 className="text-3xl font-semibold mb-6">My Bookings</h1>
        <p className="text-gray-500">No bookings found.</p>
      </div>
    );

  return (
    <div className="max-w-5xl mx-auto px-4 mt-10 mb-20">
      <h1 className="text-3xl font-semibold mb-6">My Bookings</h1>
      <div className="grid gap-6 grid-cols-1 sm:grid-cols-2 md:grid-cols-3">
        {bookings.map((booking) => (
          <div
            key={booking.id}
            className="bg-white rounded-lg shadow-md p-6 hover:shadow-lg transition-shadow duration-300 relative"
          >
            <img
              src={booking.imagePath}
              alt={booking.eventName}
              className="w-full h-40 object-cover rounded-md mb-4"
            />
            <h2 className="text-xl font-bold mb-2">{booking.eventName}</h2>
            <p className="text-gray-700 mb-1">
              <span className="font-semibold">Category:</span> {booking.categoryName}
            </p>
            <p className="text-gray-700 mb-1">
              <span className="font-semibold">Venue:</span> {booking.eventVenue}
            </p>
            <p className="text-gray-700 mb-1">
              <span className="font-semibold">Price:</span> ${booking.eventPrice}
            </p>
            <p className="text-gray-700 mb-2">{booking.eventDescription}</p>
            <p className="text-gray-700 mb-1">
              <span className="font-semibold">Booking Date:</span> {formatDate(booking.bookingDate)}
            </p>
            <p className="text-gray-500 text-sm mb-4">Booking ID: {booking.id}</p>
            <button
              onClick={() => deleteBooking(booking.id)}
              disabled={deletingId === booking.id}
              className="absolute top-4 right-4 bg-red-500 hover:bg-red-600 text-white px-3 py-1 rounded"
            >
              {deletingId === booking.id ? 'Cancelling...' : 'Cancel'}
            </button>
          </div>
        ))}
      </div>
    </div>
  );
}
