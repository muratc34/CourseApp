import React, { useState } from "react";
import { useCart } from "../contexts/CartContext";
import { Link, Navigate } from "react-router-dom";
import { IoTrashOutline } from "react-icons/io5";
import defaultCourseImg from "/src/assets/default-course-img.png";
import orderApi from "../services/modules/orderApi";
import { useAuth } from "../contexts/AuthContext";
import paymentApi from "../services/modules/paymentApi";

const Order = () => {
  const [address, setAddress] = useState("");
  const [country, setCountry] = useState("");
  const [city, setCity] = useState("");
  const [zipcode, setZipcode] = useState("");
  const [tcNo, setTcNo] = useState("74300864791");

  const { cart, removeFromCart, clearCart } = useCart();
  const { user } = useAuth();

  const cartItemIds = cart.map((item) => item.id);
  const subtotal = cart.reduce((total, item) => total + item.price, 0);
  const tax = (subtotal * 20) / 100;
  const total = subtotal + tax;

  const handleCheckout = async () => {
    await orderApi
      .createOrder({
        userId: user.id,
        courseIds: cartItemIds,
        city: city,
        country: country,
        address: address,
        zipCode: zipcode,
        tcNo: tcNo,
      })
      .then((response) => {
        createPayment(response.data.id);
      });
  };

  const createPayment = (orderId) => {
    paymentApi.createPayment({ orderId: orderId }).then((response) => {
      window.location.href = response.data.url;
    });
  };

  return (
    <div className="flex flex-col lg:flex-row gap-6 rounded-lg shadow-lg p-6 bg-white 2xl:mx-48 xl:mx-24 lg:mx-12 md:mx-8 mx-2 my-24">
      <div className="p-6 w-full">
        <div className="flex items-start justify-between">
          <h1 className="text-lg font-medium text-gray-900">
            Enter Your Billing Information
          </h1>
        </div>
        <div className="space-y-6 mt-5">
          <div>
            <label
              htmlFor="address"
              className="block text-sm/6 font-medium text-gray-900"
            >
              Address Line
            </label>
            <div className="mt-1">
              <input
                value={address}
                onChange={(e) => setAddress(e.target.value)}
                id="address"
                name="address"
                type="text"
                required
                autoComplete="address"
                className="block w-full rounded-md bg-white px-3 py-1.5 text-base text-gray-900 outline outline-1 -outline-offset-1 outline-gray-300 placeholder:text-gray-400 focus:outline focus:outline-2 focus:-outline-offset-2 focus:outline-indigo-600 sm:text-sm/6"
              />
            </div>
          </div>
          <div className="flex md:flex-row flex-col flex-column gap-6 justify-between">
            <div className="w-full">
              <label
                htmlFor="country"
                className="block text-sm/6 font-medium text-gray-900"
              >
                Country
              </label>
              <div className="mt-1">
                <input
                  value={country}
                  onChange={(e) => setCountry(e.target.value)}
                  id="country"
                  name="country"
                  type="text"
                  required
                  autoComplete="country"
                  className="block w-full rounded-md bg-white px-3 py-1.5 text-base text-gray-900 outline outline-1 -outline-offset-1 outline-gray-300 placeholder:text-gray-400 focus:outline focus:outline-2 focus:-outline-offset-2 focus:outline-indigo-600 sm:text-sm/6"
                />
              </div>
            </div>
            <div className="w-full">
              <label
                htmlFor="city"
                className="block text-sm/6 font-medium text-gray-900"
              >
                City
              </label>
              <div className="mt-1">
                <input
                  value={city}
                  onChange={(e) => setCity(e.target.value)}
                  id="city"
                  name="city"
                  type="text"
                  required
                  autoComplete="city"
                  className="block w-full rounded-md bg-white px-3 py-1.5 text-base text-gray-900 outline outline-1 -outline-offset-1 outline-gray-300 placeholder:text-gray-400 focus:outline focus:outline-2 focus:-outline-offset-2 focus:outline-indigo-600 sm:text-sm/6"
                />
              </div>
            </div>
          </div>
          <div className="flex md:flex-row flex-col gap-6 justify-between">
            <div className="w-full">
              <label
                htmlFor="zipcode"
                className="block text-sm/6 font-medium text-gray-900"
              >
                Zip Code
              </label>
              <div className="mt-1">
                <input
                  value={zipcode}
                  onChange={(e) => setZipcode(e.target.value)}
                  id="zipcode"
                  name="zipcode"
                  type="text"
                  required
                  autoComplete="zipcode"
                  className="block w-full rounded-md bg-white px-3 py-1.5 text-base text-gray-900 outline outline-1 -outline-offset-1 outline-gray-300 placeholder:text-gray-400 focus:outline focus:outline-2 focus:-outline-offset-2 focus:outline-indigo-600 sm:text-sm/6"
                />
              </div>
            </div>
            <div className="w-full">
              <label
                htmlFor="tcNo"
                className="block text-sm/6 font-medium text-gray-900"
              >
                TC Number
              </label>
              <div className="mt-1">
                <input
                  value={tcNo}
                  onChange={(e) => setTcNo(e.target.value)}
                  id="tcNo"
                  name="tcNo"
                  type="text"
                  required
                  autoComplete="tcNo"
                  className="block w-full rounded-md bg-white px-3 py-1.5 text-base text-gray-900 outline outline-1 -outline-offset-1 outline-gray-300 placeholder:text-gray-400 focus:outline focus:outline-2 focus:-outline-offset-2 focus:outline-indigo-600 sm:text-sm/6"
                />
              </div>
            </div>
          </div>
        </div>
        <div className="flex justify-start items-center mt-5 m-1">
          <input
            type="checkbox"
            name="privacy"
            className="min-w-3 min-h-3 rounded accent-indigo-600"
          />
          <label htmlFor="privacy" className="ms-2 text-sm">
            I have read and agree to the Privacy Policy and Terms of Service.
          </label>
        </div>
        <div className="flex justify-start items-center m-1">
          <input
            type="checkbox"
            name="privacy"
            className="min-w-3 min-h-3 rounded accent-indigo-600"
          />
          <label htmlFor="privacy" className="ms-2 text-sm">
            I have read and accept the Delivery and Return Policies.
          </label>
        </div>
      </div>
      {/*Order Summary*/}
      <div className="rounded-lg bg-slate-50 p-6 w-full">
        <div className="flex items-start justify-between">
          <h1 className="text-lg font-medium text-gray-900">Your Cart</h1>
          <button
            onClick={clearCart}
            type="button"
            className="relative -m-2 p-2 bg-indigo-600 text-white py-2 rounded-md hover:bg-indigo-700"
          >
            Clear Cart
          </button>
        </div>
        <div className="mt-8 ">
          <div className="flow-root">
            {cart.length > 0 ? (
              <ul
                role="list"
                className="-my-6 divide-y divide-gray-200"
              >
                {cart.map((item) => (
                  <li key={item.id} className="flex py-6">
                    <div className="size-24 shrink-0 overflow-hidden rounded-md border border-gray-200">
                      <img
                        src={item.imageUrl ?? defaultCourseImg}
                        alt={item.name}
                        className="size-full object-cover"
                      />
                    </div>
                    <div className="ml-4 flex flex-1 flex-col">
                      <div>
                        <div className="flex justify-between text-base font-medium text-gray-900">
                          <h3>
                            <Link
                              to={`/course/${item.id}`}
                              className="text-pretty"
                            >
                              {item.name}
                            </Link>
                          </h3>
                          <p className="ml-4 text-nowrap">{item.price} ₺</p>
                        </div>
                        <p className="mt-1 text-sm text-gray-500">
                          {item.user.fullName}
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
                <IoTrashOutline size={100} className="text-indigo-600" />
                <p className="mt-5">Your basket is empty!</p>
              </div>
            )}
          </div>
        </div>

        <div className="mt-6 bg-gray-50 p-4 rounded-lg shadow-inner">
          <h2 className="text-lg font-semibold mb-4">Order summary</h2>
          <div className="flex justify-between text-sm text-gray-500">
            <p>Subtotal</p>
            <p>{subtotal.toFixed(2)} ₺</p>
          </div>
          <div className="flex justify-between text-sm mt-2 text-gray-500">
            <p>Tax estimate</p>
            <p>{tax.toFixed(2)} ₺</p>
          </div>
          <div className="flex justify-between text-lg font-bold mt-4 text-gray-900">
            <p>Order total</p>
            <p>{total.toFixed(2)} ₺</p>
          </div>
          <button
            onClick={handleCheckout}
            className="w-full mt-4 bg-indigo-600 text-white py-2 rounded-md hover:bg-indigo-700"
          >
            Checkout
          </button>
        </div>
      </div>
    </div>
  );
};

export default Order;
