import React, { useEffect, useState } from "react";
import { useLocation } from "react-router-dom";
import paymentApi from "../services/modules/paymentApi";
import LoadingSpinner from "../components/LoadingSpinner";
import { useCart } from "../contexts/CartContext";
import { CiCircleAlert, CiCircleCheck } from "react-icons/ci";

const PaymentCallback = () => {
  const [isLoading, setIsLoading] = useState(true);
  const [isPaymentSuccess, setIsPaymentSuccess] = useState(false);
  const location = useLocation();
  const queryParams = new URLSearchParams(location.search);
  const token = queryParams.get("token");

  const { clearCart } = useCart();

  useEffect(() => {
    if (false) {
      paymentApi
        .confirmPayment({ token: token })
        .then((response) => {
          console.log(response);
          setIsPaymentSuccess(true);
          setIsLoading(false);
          clearCart();
        })
        .catch((error) => {
          console.error(error);
          setIsLoading(false);
        });
    } else {
      setIsLoading(false);
    }
  }, [token]);

  if (isLoading) {
    return <LoadingSpinner />;
  }

  return isPaymentSuccess ? (
    <div className="flex flex-col items-center justify-center py-12 space-y-4 md:py-24">
      <div className="flex flex-col items-center justify-center space-y-2">
        <CiCircleCheck size={150} className="text-green-600" />
        <h1 className="text-3xl font-bold tracking-tighter sm:text-5xl">
          Payment successful
        </h1>
        <p className="max-w-[600px] text-center text-gray-700 md:text-xl/relaxed">
          Your order has been confirmed and is now being processed. Thank you
          for shopping with us.
        </p>
      </div>
    </div>
  ) : (
    <div className="flex flex-col items-center justify-center py-12 space-y-4 md:py-24">
      <div className="flex flex-col items-center justify-center space-y-2">
        <CiCircleAlert size={150} className="text-red-600" />
        <h1 className="text-3xl font-bold tracking-tighter sm:text-5xl">
          Payment Failed
        </h1>
        <p className="max-w-[600px] text-center text-gray-700 md:text-xl/relaxed">
        Sorry, your order could not be processed. Please check your payment details or contact our customer service. Thank you for shopping with us.
        </p>
      </div>
    </div>
  );
};

export default PaymentCallback;
