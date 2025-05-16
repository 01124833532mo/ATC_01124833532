import axios from 'axios';
import React, { useState, useEffect } from 'react';

export default function Events() {
  const [name, setName] = useState('');
  const [description, setDescription] = useState('');
  const [categoryId, setCategoryId] = useState('');
  const [date, setDate] = useState('');
  const [venue, setVenue] = useState('');
  const [price, setPrice] = useState('');
  const [image, setImage] = useState(null);
  const [loading, setLoading] = useState(false);
  const [message, setMessage] = useState('');
  const [events, setEvents] = useState([]);
  const [eventsLoading, setEventsLoading] = useState(false);
  const [pageIndex, setPageIndex] = useState(1);
  const [editId, setEditId] = useState(null);
  const pageSize = 3;

  const handleImageChange = (e) => {
    const file = e.target.files[0];
    if (file) {
      setImage(file);
    }
  };

  const resetForm = () => {
    setName('');
    setDescription('');
    setCategoryId('');
    setDate('');
    setVenue('');
    setPrice('');
    setImage(null);
    setEditId(null);
  };

  const handleCreateEvent = async (e) => {
    e.preventDefault();
    setLoading(true);
    setMessage('');

    const token = localStorage.getItem('AdminToken');

    if (!token) {
      setMessage('❌ Admin token is missing.');
      setLoading(false);
      return;
    }

    const formData = new FormData();
    formData.append('name', name);
    formData.append('description', description);
    formData.append('categoryId', categoryId);
    formData.append('date', date);
    formData.append('venue', venue);
    formData.append('price', price);
    formData.append('imagePath', image);

    try {
      if (editId) {
        await axios.put(`http://bookevent.runasp.net/api/Event/UpdateEvent/${editId}`, formData, {
          headers: {
            Authorization: `Bearer ${token}`,
            'Content-Type': 'multipart/form-data',
          },
        });
        setMessage('✅ Event updated successfully!');
      } else {
        await axios.post('http://bookevent.runasp.net/api/Event/CreateEvent', formData, {
          headers: {
            Authorization: `Bearer ${token}`,
            'Content-Type': 'multipart/form-data',
          },
        });
        setMessage('✅ Event created successfully!');
      }

      resetForm();
      getAllEvents();
    } catch (error) {
      console.error('Error submitting event:', error.response?.data || error);
      setMessage(error.response?.data?.message || '❌ Failed to submit event.');
    } finally {
      setLoading(false);
    }
  };

  const getAllEvents = async (categoryIdParam = categoryId, page = pageIndex) => {
    setEventsLoading(true);

    const token = localStorage.getItem('AdminToken');
    if (!token) {
      console.error('Admin token is missing!');
      setEventsLoading(false);
      return;
    }

    try {
      const { data } = await axios.get('http://bookevent.runasp.net/api/Event/GetAllEvents', {
        params: {
          categoryId: categoryIdParam || undefined,
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
      setMessage(error.response?.data?.message || '❌ Failed to fetch events.');
    } finally {
      setEventsLoading(false);
    }
  };

  const handleDeleteEvent = async (id) => {
    const token = localStorage.getItem('AdminToken');

    if (!token) {
      alert('❌ Admin token is missing.');
      return;
    }

    const confirmDelete = window.confirm('Are you sure you want to delete this event?');
    if (!confirmDelete) return;

    try {
      await axios.delete(`http://bookevent.runasp.net/api/Event/DeleteEvent/${id}`, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });

      alert('✅ Event deleted successfully.');
      getAllEvents();
    } catch (error) {
      console.error('Error deleting event:', error.response?.data || error);
      alert(error.response?.data?.message || '❌ Failed to delete event.');
    }
  };

  const handleEditEvent = (event) => {
    setEditId(event.id);
    setName(event.name);
    setDescription(event.description);
    setCategoryId(event.categoryId);
    setDate(event.date.split('T')[0]);
    setVenue(event.venue);
    setPrice(event.price);
    setImage(null);
    window.scrollTo({ top: 0, behavior: 'smooth' });
  };

  useEffect(() => {
    getAllEvents(categoryId, pageIndex);
  }, [categoryId, pageIndex]);

  const handleFilterChange = (category) => {
    setCategoryId(category);
    setPageIndex(1);
  };

  return (
    <div className="min-h-screen bg-gray-100 flex flex-col items-center p-6">
      {/* Filter Buttons */}
      <div className="mb-6">
        {[
          { id: '1', label: 'AirPlane' },
          { id: '2', label: 'Cars' },
          { id: '7', label: 'Hotels' },
          { id: '8', label: 'Taxi Airport' },
        ].map((cat) => (
          <button
            key={cat.id}
            onClick={() => handleFilterChange(cat.id)}
            className={`px-4 py-2 rounded mx-2 ${categoryId === cat.id ? 'bg-blue-600 text-white' : 'bg-gray-200'}`}
          >
            {cat.label}
          </button>
        ))}
      </div>

      {/* Create/Update Event Form */}
      <div className="bg-white shadow-md rounded p-8 w-full max-w-xl mb-10">
        <h2 className="text-2xl font-bold mb-4 text-center text-blue-700">
          {editId ? 'Update Event' : 'Create New Event'}
        </h2>
        <form onSubmit={handleCreateEvent} className="space-y-4">
          <input type="text" placeholder="Event Name" value={name} onChange={(e) => setName(e.target.value)} className="w-full p-2 border rounded" required />
          <textarea placeholder="Description" value={description} onChange={(e) => setDescription(e.target.value)} className="w-full p-2 border rounded" required />
          <input type="number" placeholder="Category ID" value={categoryId} onChange={(e) => setCategoryId(e.target.value)} className="w-full p-2 border rounded" required />
          <input type="date" value={date} onChange={(e) => setDate(e.target.value)} className="w-full p-2 border rounded" required />
          <input type="text" placeholder="Venue" value={venue} onChange={(e) => setVenue(e.target.value)} className="w-full p-2 border rounded" required />
          <input type="number" placeholder="Price" value={price} onChange={(e) => setPrice(e.target.value)} className="w-full p-2 border rounded" required />
          <input type="file" accept="image/*" onChange={handleImageChange} className="w-full p-2 border rounded" />
          <button type="submit" disabled={loading} className="w-full bg-blue-600 text-white p-2 rounded hover:bg-blue-700 transition">
            {loading ? (editId ? 'Updating Event...' : 'Creating Event...') : editId ? 'Update Event' : 'Create Event'}
          </button>
          {editId && (
            <button type="button" onClick={resetForm} className="w-full bg-gray-300 text-gray-700 p-2 rounded mt-2 hover:bg-gray-400">
              Cancel Edit
            </button>
          )}
        </form>
        {message && <p className="mt-4 text-center text-green-600 font-semibold">{message}</p>}
      </div>

      {/* Events List */}
      <div className="bg-white shadow-md rounded p-6 w-full max-w-7xl mx-auto">
        <h2 className="text-xl font-bold text-blue-700 mb-4">All Events</h2>
        {eventsLoading ? (
          <p className="text-gray-600 text-center">Loading events...</p>
        ) : events.length === 0 ? (
          <p className="text-gray-600 text-center">No events found.</p>
        ) : (
          <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
            {events.map((event) => (
              <div key={event.id} className="bg-white shadow-lg rounded-lg p-4 flex flex-col items-center">
                {event.imagePath ? (
                  <img src={event.imagePath} alt={event.name} className="w-full h-40 object-cover rounded-lg mb-3" />
                ) : (
                  <div className="w-full h-40 bg-gray-200 rounded-lg flex items-center justify-center mb-3">
                    <span className="text-gray-500">No Image</span>
                  </div>
                )}
                <h3 className="text-lg font-semibold text-blue-800 text-center">{event.name}</h3>
                <p className="text-center text-sm mb-1">{event.description}</p>
                <p className="text-xs text-gray-600 text-center">📅 {new Date(event.date).toLocaleDateString()}</p>
                <p className="text-xs text-gray-600 text-center">📍 {event.venue}</p>
                <p className="text-sm text-gray-800 font-medium text-center mb-2">Price: ${event.price}</p>
                <div className="flex space-x-2">
                  <button onClick={() => handleEditEvent(event)} className="text-white bg-green-600 px-4 py-1 rounded hover:bg-green-700">
                    Edit
                  </button>
                  <button onClick={() => handleDeleteEvent(event.id)} className="text-white bg-red-600 px-4 py-1 rounded hover:bg-red-700">
                    Delete
                  </button>
                </div>
              </div>
            ))}
          </div>
        )}
        <div className="flex justify-center mt-8 space-x-4">
          <button onClick={() => setPageIndex((prev) => Math.max(prev - 1, 1))} disabled={pageIndex === 1} className="px-4 py-2 bg-gray-300 rounded hover:bg-gray-400 disabled:opacity-50">
            Previous
          </button>
          <span className="px-4 py-2 text-gray-700">Page {pageIndex}</span>
          <button onClick={() => setPageIndex((prev) => prev + 1)} className="px-4 py-2 bg-gray-300 rounded hover:bg-gray-400">
            Next
          </button>
        </div>
      </div>
    </div>
  );
}
