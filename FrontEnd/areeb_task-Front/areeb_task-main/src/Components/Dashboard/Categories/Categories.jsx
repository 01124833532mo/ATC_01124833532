import React, { useEffect, useState } from 'react';
import axios from 'axios';

export default function CreateCategoryPage() {
  const [name, setName] = useState('');
  const [description, setDescription] = useState('');
  const [loading, setLoading] = useState(false);
  const [message, setMessage] = useState('');
  const [categories, setCategories] = useState(null);
  const [pageIndex, setPageIndex] = useState(1);
  const [pageSize] = useState(2);
  const [totalPages, setTotalPages] = useState(0);

  // Update states
  const [editingCategoryId, setEditingCategoryId] = useState(null);
  const [editName, setEditName] = useState('');
  const [editDescription, setEditDescription] = useState('');

  const handleCreateCategory = async (e) => {
    e.preventDefault();
    setLoading(true);
    setMessage('');

    const token = localStorage.getItem('AdminToken');

    try {
      const { data } = await axios.post(
        'http://bookevent.runasp.net/api/Category/CreateCategory',
        { name, description },
        {
          headers: {
            Authorization: `Bearer ${token}`,
            'Content-Type': 'application/json',
          },
        }
      );

      setMessage('✅ Category created successfully!');
      setName('');
      setDescription('');
      getAllCategories(pageIndex, pageSize);
    } catch (error) {
      const err = error.response?.data?.message || '❌ Failed to create category.';
      setMessage(err);
    } finally {
      setLoading(false);
    }
  };

  const getAllCategories = async (pageIndex, pageSize) => {
    const token = localStorage.getItem('AdminToken');
    if (!token) {
      console.error('Admin token is missing!');
      return;
    }

    try {
      const { data } = await axios.get(
        'http://bookevent.runasp.net/api/Category/GetAllCategories',
        {
          params: {
            pageIndex,
            pageSize,
          },
          headers: {
            Authorization: `Bearer ${token}`,
          },
        }
      );

      setCategories(data);
      setTotalPages(Math.ceil(data.totalCount / pageSize));
    } catch (error) {
      setCategories(null);
    }
  };

  const handleDeleteCategory = async (categoryId) => {
    const token = localStorage.getItem('AdminToken');
    if (!token || !categoryId) return;

    try {
      const { data } = await axios.delete(
        `http://bookevent.runasp.net/api/Category/DeleteCategory/${categoryId}`,
        {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        }
      );

      setMessage('✅ Category deleted successfully!');
      getAllCategories(pageIndex, pageSize);
    } catch (error) {
      const errMsg =
        error.response?.data?.message || error.response?.statusText || '❌ Failed to delete category.';
      setMessage(errMsg);
    }
  };

  const startEditingCategory = (category) => {
    setEditingCategoryId(category.id);
    setEditName(category.name);
    setEditDescription(category.description);
  };

  const cancelEditing = () => {
    setEditingCategoryId(null);
    setEditName('');
    setEditDescription('');
  };

  const handleUpdateCategory = async () => {
    const token = localStorage.getItem('AdminToken');
    if (!token) return;

    try {
      const { data } = await axios.put(
        `http://bookevent.runasp.net/api/Category/UpdateCategory/${editingCategoryId}`,
        {
          name: editName,
          description: editDescription,
        },
        {
          headers: {
            Authorization: `Bearer ${token}`,
            'Content-Type': 'application/json',
          },
        }
      );

      setMessage('✅ Category updated successfully!');
      setEditingCategoryId(null);
      getAllCategories(pageIndex, pageSize);
    } catch (error) {
      const err = error.response?.data?.message || '❌ Failed to update category.';
      setMessage(err);
    }
  };

  useEffect(() => {
    getAllCategories(pageIndex, pageSize);
  }, [pageIndex, pageSize]);

  const handlePageChange = (newPageIndex) => {
    setPageIndex(newPageIndex);
  };

  return (
    <div className="min-h-screen bg-gray-100 flex flex-col items-center p-6">
      {/* Create Category Form */}
      <div className="bg-white shadow-md rounded p-8 w-full max-w-xl mb-10">
        <h2 className="text-2xl font-bold mb-4 text-center text-blue-700">Create New Category</h2>
        <form onSubmit={handleCreateCategory} className="space-y-4">
          <div>
            <label className="block mb-1 font-medium">Category Name</label>
            <input
              type="text"
              value={name}
              onChange={(e) => setName(e.target.value)}
              className="w-full p-2 border rounded"
              required
            />
          </div>
          <div>
            <label className="block mb-1 font-medium">Description</label>
            <textarea
              value={description}
              onChange={(e) => setDescription(e.target.value)}
              className="w-full p-2 border rounded"
              required
            ></textarea>
          </div>
          <button
            type="submit"
            disabled={loading}
            className="w-full bg-blue-600 text-white p-2 rounded hover:bg-blue-700 transition"
          >
            {loading ? 'Creating...' : 'Create Category'}
          </button>
        </form>
        {message && <p className="mt-4 text-center text-green-600 font-semibold">{message}</p>}
      </div>

      {/* Categories List */}
      <div className="bg-white shadow-md rounded-lg p-8 w-full max-w-3xl mb-10 mx-auto">
        <ul>
          {categories && categories?.data?.map((category) => (
            <li key={category.id} className="border-b py-4">
              {editingCategoryId === category.id ? (
                <>
                  <input
                    type="text"
                    value={editName}
                    onChange={(e) => setEditName(e.target.value)}
                    className="w-full p-2 border rounded mb-2"
                  />
                  <textarea
                    value={editDescription}
                    onChange={(e) => setEditDescription(e.target.value)}
                    className="w-full p-2 border rounded mb-2"
                  ></textarea>
                  <div className="flex gap-2">
                    <button
                      onClick={handleUpdateCategory}
                      className="bg-green-600 text-white px-4 py-1 rounded hover:bg-green-700"
                    >
                      Save
                    </button>
                    <button
                      onClick={cancelEditing}
                      className="bg-gray-400 text-white px-4 py-1 rounded hover:bg-gray-500"
                    >
                      Cancel
                    </button>
                  </div>
                </>
              ) : (
                <>
                  <h3 className="text-xl font-semibold text-blue-700">{category.name}</h3>
                  <p className="text-lg text-gray-600">{category.description}</p>
                  <div className="flex gap-3 mt-2">
                    <button
                      onClick={() => startEditingCategory(category)}
                      className="bg-yellow-500 text-white px-3 py-1 rounded hover:bg-yellow-600"
                    >
                      Edit
                    </button>
                    <button
                      disabled={category.protected || category.inUse}
                      onClick={() => handleDeleteCategory(category.id)}
                      className="bg-red-600 px-3 py-1 rounded text-white hover:bg-red-800 disabled:opacity-50"
                    >
                      Delete
                    </button>
                  </div>
                </>
              )}
            </li>
          ))}
        </ul>

        {/* Pagination */}
        <div className="mt-8 flex justify-between items-center">
          <button
            onClick={() => handlePageChange(pageIndex - 1)}
            disabled={pageIndex <= 1}
            className="px-6 py-2 bg-gray-300 hover:bg-gray-400 text-gray-700 rounded-lg shadow-sm disabled:opacity-50 transition"
          >
            Previous
          </button>

          <span className="text-gray-600 font-semibold">
            Page {pageIndex} of {totalPages}
          </span>

          <button
            onClick={() => handlePageChange(pageIndex + 1)}
            disabled={pageIndex >= totalPages}
            className="px-6 py-2 bg-blue-600 text-white hover:bg-blue-700 rounded-lg shadow-sm disabled:opacity-50 transition"
          >
            Next
          </button>
        </div>
      </div>
    </div>
  );
}
