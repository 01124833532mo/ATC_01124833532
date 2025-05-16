import React, { useState } from 'react';
import card1 from '../../assets/card1.jpg';
import card2 from '../../assets/card2.jpg';
import { useNavigate } from 'react-router-dom';



export default function Dashboard() {

  let navigator = useNavigate()
  const [isOpen, setIsOpen] = useState(true);

  return (
    <div className="flex min-h-screen">
     {/* Main Content - Cards */}
<div className="flex-1 bg-gray-100 p-6">
  <h1 className="text-2xl font-bold mb-6">Dashboard Overview</h1>
  <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
    {/* Card 1 */}
    <div className="bg-white shadow-md rounded-lg p-4 flex flex-col items-center">
      <img src={card1} alt="Card 1" className="rounded-lg w-full h-60 object-cover mb-4"/>
      <button onClick={() => navigator('/categories')} className="mt-auto bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700 transition">
        View Category
      </button>
    </div>

    {/* Card 2 */}
    <div className="bg-white shadow-md rounded-lg p-4 flex flex-col items-center">
      <img src={card2} alt="Card 2" className="rounded-lg w-full h-60 object-cover mb-4"/>
      <button onClick={()=> navigator('/events')} className="mt-auto bg-green-600 text-white px-4 py-2 rounded hover:bg-green-700 transition">
        View Events
      </button>
    </div>

   
  </div>
</div>

    
    </div>
  );
}
