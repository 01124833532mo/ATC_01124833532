import React, { useEffect, useState } from 'react';
import axios from 'axios';

export default function CategoriesList({ selectedCategoryId }) {
  const [events, setEvents] = useState([]);
  const [loadingEvents, setLoadingEvents] = useState(false);

  const token = localStorage.getItem('UserToken');

  const fetchEvents = async (categoryId) => {
    if (!categoryId) return;

    setLoadingEvents(true);
    try {
      const { data } = await axios.get('http://bookevent.runasp.net/api/Event/GetAllEvents', {
        params: { categoryId },
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
    if (selectedCategoryId) {
      fetchEvents(selectedCategoryId);
    }
  }, [selectedCategoryId]);

  return (
    <div className="px-6 mb-12">
      {loadingEvents ? (
        <p>Loading events...</p>
      ) : (
        <ul className="space-y-6">
          {events.map((event) => (
            <li key={event.id} className="bg-white shadow-lg rounded-lg p-4 border-b py-4 flex flex-col items-center">
              {event.imagePath ? (
                <img src={event.imagePath} alt={event.name} className="mt-2 w-1/3 h-60 object-cover rounded-lg mx-auto" />
              ) : (
                <div className="w-40 h-40 bg-gray-200 rounded-lg flex items-center justify-center mx-auto">
                  <span className="text-gray-500">No Image</span>
                </div>
              )}
              <h3 className="text-2xl font-semibold text-blue-800 text-center">{event.name}</h3>
              <p className="text-center text-lg">{event.description}</p>
              <p className="text-sm text-gray-600 text-center">📅 {new Date(event.date).toLocaleDateString()}</p>
              <p className="text-sm text-gray-600 text-center">📍 {event.venue}</p>
              <p className="text-sm text-gray-800 font-medium text-center">Price: ${event.price}</p>
            </li>
          ))}
        </ul>
      )}
    </div>
  );
}
