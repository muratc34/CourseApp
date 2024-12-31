import React from "react";
import { useCart } from "../contexts/CartContext";
import { Link } from "react-router-dom";
import { IoTrashOutline } from "react-icons/io5";

const Order = () => {
  const { cart, removeFromCart, clearCart } = useCart();
  const subtotal = cart.reduce((total, item) => total + item.price, 0);
  const tax = (subtotal * 20) / 100;
  const total = subtotal + tax;

  const handleCheckout = () => {

  }

  return (
    <div className="p-6 mt-24">
      <div className="max-w-6xl mx-auto bg-white rounded-lg shadow-lg p-6">
        <div className="flex items-start justify-between">
          <h1 className="text-lg font-medium text-gray-900">Your Cart</h1>
          <button onClick={clearCart} type="button" className="relative -m-2 p-2 bg-indigo-600 text-white py-2 rounded-md hover:bg-indigo-700">
            Clear Cart
          </button>
        </div>

        <div className="mt-8">
          <div className="flow-root">
            {cart.length > 0 ? (
              <ul role="list" className="-my-6 divide-y divide-gray-200">
                {cart.map((item) => (
                  <li key={item.id} className="flex py-6">
                    <div className="size-24 shrink-0 overflow-hidden rounded-md border border-gray-200">
                      <img
                        src={item.imageUrl}
                        alt={item.name}
                        className="size-full object-cover"
                      />
                    </div>
                    <div className="ml-4 flex flex-1 flex-col">
                      <div>
                        <div className="flex justify-between text-base font-medium text-gray-900 truncate">
                          <h3>
                            <Link to={`/course/${item.id}`}>{item.name}</Link>
                          </h3>
                          <p className="ml-4">{item.price} ₺</p>
                        </div>
                        <p className="mt-1 text-sm text-gray-500">
                          {item.user.name}
                        </p>
                      </div>
                      <div className="flex flex-1 items-end justify-end text-sm">
                        <button
                          type="button"
                          className="font-medium text-indigo-600 hover:text-indigo-500"
                          onClick={() => removeFromCart(item.id)}
                        >
                          <IoTrashOutline size={24} />
                        </button>
                      </div>
                    </div>
                  </li>
                ))}
              </ul>
            ) : (
              <div className="flex flex-col justify-center items-center w-full mx-auto text-gray-900">
                <IoTrashOutline size={100} className="text-indigo-600"/>
                <p className="mt-5">Your basket is empty!</p>
              </div>
            )}
          </div>
        </div>

        <div className="mt-6 bg-gray-50 p-4 rounded-lg shadow-inner">
          <h2 className="text-lg font-semibold mb-4">Order summary</h2>
          <div className="flex justify-between text-sm text-gray-500">
            <p>Subtotal</p>
            <p>${subtotal.toFixed(2)}</p>
          </div>
          <div className="flex justify-between text-sm mt-2 text-gray-500">
            <p>Tax estimate</p>
            <p>${tax.toFixed(2)}</p>
          </div>
          <div className="flex justify-between text-lg font-bold mt-4 text-gray-900">
            <p>Order total</p>
            <p>${total.toFixed(2)}</p>
          </div>
          <button onClick={handleCheckout} className="w-full mt-4 bg-indigo-600 text-white py-2 rounded-md hover:bg-indigo-700">
            Checkout
          </button>
        </div>
      </div>
    </div>
  );
};

export default Order;
