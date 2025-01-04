import React, { useState } from "react";
import { useCart } from "../contexts/CartContext";
import { Link } from "react-router-dom";
import { IoTrashOutline } from "react-icons/io5";
import defaultCourseImg from "/src/assets/default-course-img.png";
import orderApi from "../services/modules/orderApi";
import { useAuth } from "../contexts/AuthContext";
import paymentApi from "../services/modules/paymentApi";
import Input from "../components/Input";
import { ToastContainer, toast } from 'react-toastify';

const Order = () => {
  const [address, setAddress] = useState("");
  const [country, setCountry] = useState("");
  const [city, setCity] = useState("");
  const [zipcode, setZipcode] = useState("");
  const [tcNo, setTcNo] = useState("74300864791");
  const [errors, setErrors] = useState([]);

  const { cart, removeFromCart, clearCart } = useCart();
  const { user } = useAuth();

  const cartItemIds = cart.map((item) => item.id);
  const subtotal = cart.reduce((total, item) => total + item.price, 0);
  const tax = (subtotal * 20) / 100;
  const total = subtotal + tax;

  const handleCheckout = async (e) => {
    e.preventDefault();
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
        toast.success("Your order has been created, you are directed to the payment page.")
        setTimeout(() => {
          createPayment(response.data.id);
        }, 2000)
      })
      .catch((error) => {
        setErrors(error.errors);
      });
  };

  const createPayment = (orderId) => {
    paymentApi.createPayment({ orderId: orderId }).then((response) => {
      window.location.href = response.data.url;
    });
  };

  const handleInputChange = (setter) => (e) => {
    setter(e.target.value);
    setErrors([]);
  };

  return (
    <form
      onSubmit={(e) => handleCheckout(e)}
      className="flex flex-col lg:flex-row gap-6 rounded-lg shadow-lg p-6 bg-white 2xl:mx-48 xl:mx-24 lg:mx-12 md:mx-8 mx-2 my-24"
    >
      <div className="p-6 w-full">
        <div className="flex items-start justify-between">
          <h1 className="text-lg font-medium text-gray-900">
            Enter Your Billing Information
          </h1>
        </div>
        <div className="space-y-6 mt-5">
          <Input
            id={"address"}
            type={"text"}
            required={true}
            value={address}
            onChange={handleInputChange(setAddress)}
            label={"Address Line"}
            placeholder={"Enter your address"}
          />
          <div className="flex md:flex-row flex-col flex-column gap-6 justify-between">
            <Input
              id={"country"}
              type={"text"}
              required={true}
              value={country}
              onChange={handleInputChange(setCountry)}
              label={"Country"}
              placeholder={"Enter your country"}
            />
            <Input
              id={"city"}
              type={"text"}
              required={true}
              value={city}
              onChange={handleInputChange(setCity)}
              label={"City"}
              placeholder={"Enter your city"}
            />
          </div>
          <div className="flex md:flex-row flex-col gap-6 justify-between">
            <Input
              id={"zipcode"}
              type={"text"}
              required={true}
              value={zipcode}
              onChange={handleInputChange(setZipcode)}
              label={"Zipcode"}
              placeholder={"Enter your zipcode"}
            />
            <Input
              id={"tcNo"}
              type={"text"}
              required={true}
              value={tcNo}
              onChange={handleInputChange(setTcNo)}
              label={"TC Number"}
              placeholder={"Enter your tc identity number"}
            />
          </div>
        </div>
        <div className="flex justify-start items-center mt-5 m-1">
          <input
            type="checkbox"
            name="privacy"
            required
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
            required
          />
          <label htmlFor="privacy" className="ms-2 text-sm">
            I have read and accept the Delivery and Return Policies.
          </label>
        </div>
        {errors.length > 0 ? (
          errors.map((error, index) => (
            <div key={index} className="text-md text-red-500 m-1 mt-3">
              {error.description}
            </div>
          ))
        ) : (
          <></>
        )}
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
              <ul role="list" className="-my-6 divide-y divide-gray-200">
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
            type="submit"
            className="w-full mt-4 bg-indigo-600 text-white py-2 rounded-md hover:bg-indigo-700"
          >
            Checkout
          </button>
          <ToastContainer 
              position="bottom-center"
              autoClose={1500}
              hideProgressBar={true}
              newestOnTop={false}
              closeOnClick
              rtl={false}/>
        </div>
      </div>
    </form>
  );
};

export default Order;
