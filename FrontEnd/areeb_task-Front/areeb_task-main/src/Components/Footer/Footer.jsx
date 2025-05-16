// src/components/Footer.jsx
import React from "react";

const Footer = () => {
  return (
    <footer className="bg-gray-900 text-white py-10 ">
      <div className="max-w-7xl mx-auto px-4 grid grid-cols-1 md:grid-cols-4 gap-8">
        {/* Logo and Description */}
        <div>
          <h2 className="text-2xl font-bold mb-2">BookTick</h2>
          <p className="text-gray-400">
            Book your tickets easily for events, movies, and more. Fast, secure, and reliable!
          </p>
        </div>

        {/* Quick Links */}
        <div>
          <h3 className="text-xl font-semibold mb-3">Quick Links</h3>
          <ul className="space-y-2 text-gray-300">
            <li><a href="/" className="hover:underline">Home</a></li>
            <li><a href="/events" className="hover:underline">Events</a></li>
            <li><a href="/contact" className="hover:underline">Contact</a></li>
            <li><a href="/about" className="hover:underline">About Us</a></li>
          </ul>
        </div>

        {/* Support */}
        <div>
          <h3 className="text-xl font-semibold mb-3">Support</h3>
          <ul className="space-y-2 text-gray-300">
            <li><a href="/faq" className="hover:underline">FAQs</a></li>
            <li><a href="/support" className="hover:underline">Customer Support</a></li>
            <li><a href="/terms" className="hover:underline">Terms & Conditions</a></li>
            <li><a href="/privacy" className="hover:underline">Privacy Policy</a></li>
          </ul>
        </div>

        {/* Newsletter */}
        <div>
          <h3 className="text-xl font-semibold mb-3">Newsletter</h3>
          <p className="text-gray-400 mb-4">Stay updated with the latest offers and events.</p>
          <form className="flex">
            <input
              type="email"
              placeholder="Your email"
              className="w-full px-3 py-2 rounded-l bg-gray-800 text-white focus:outline-none"
            />
            <button
              type="submit"
              className="bg-blue-600 px-4 py-2 rounded-r hover:bg-blue-700"
            >
              Subscribe
            </button>
          </form>
        </div>
      </div>

      {/* Bottom Line */}
      <div className="text-center text-gray-500 mt-10 border-t border-gray-700 pt-6">
        © 2025 BookTick. All rights reserved.
      </div>
    </footer>
  );
};

export default Footer;
