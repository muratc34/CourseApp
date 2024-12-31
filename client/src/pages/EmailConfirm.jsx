import React, { useState } from "react";
import authApi from "../services/modules/authApi";
import { useAuth } from "../contexts/AuthContext";
import { useNavigate } from "react-router-dom";
import LoadingSpinner from "../components/LoadingSpinner";

const EmailConfirm = () => {
  const [values, setValues] = useState(Array(6).fill(""));
  const [otpCode, setOtpCode] = useState();

  const {user, refreshAuthToken} = useAuth();
  const navigate = useNavigate();

  const handleInput = (e, index) => {
    const value = e.target.value;

    if (value === "" || /^\d$/.test(value)) {
      const newValues = [...values];
      newValues[index] = value;
      setValues(newValues);

      if (value !== "" && index < 5) {
        const nextInput = document.getElementById(`digit-${index + 1}`);
        if (nextInput) nextInput.focus();
      }
      setOtpCode(newValues.join(""));
    }
  };

  const handleKeyDown = (e, index) => {
    if (e.key === "Backspace") {
      const newValues = [...values];

      if (values[index] === "" && index > 0) {
        const prevInput = document.getElementById(`digit-${index - 1}`);
        if (prevInput) prevInput.focus();
        newValues[index - 1] = "";
      } else {
        newValues[index] = "";
      }
      setValues(newValues);
      setOtpCode(newValues.join(""));
    }
  };

  const handleConfirm = async (e) => {
    e.preventDefault();
    await authApi.confirmEmail({userId:user.id, token: otpCode})
    .then(response => {
        console.log(response)
        refreshAuthToken();
        navigate("/")
    }).catch(error => {
        console.error(error);
    })
  }

  const handleResendCode = async() => {
    await authApi.resendEmailConfirmationToken(user.id)
    .then((response) => {
      console.log(response)
    }).catch(err =>{
      console.error(err);
    })
  }

  console.log(user);
  return (
    <main className="relative flex flex-col justify-center overflow-hidden mt-24">
      <div className="w-full max-w-6xl mx-auto px-4 md:px-6 py-24">
        <div className="flex justify-center">
          <div className="max-w-md mx-auto text-center bg-white px-4 sm:px-8 py-10 rounded-xl shadow-md">
            <header className="mb-8">
              <h1 className="text-2xl font-bold mb-1">Email Verification</h1>
              <p className="text-[15px] text-slate-500">
                Enter the 6-digit verification code that was sent to your email.
              </p>
            </header>
            <form onSubmit={handleConfirm}>
              <div className="flex items-center justify-center gap-3">
                {Array.from({ length: 6 }).map((_, index) => (
                  <input
                    key={index}
                    id={`digit-${index}`}
                    type="text"
                    className="w-14 h-14 text-center text-2xl font-extrabold text-slate-900 bg-slate-100 border border-transparent hover:border-slate-200 appearance-none rounded p-4 outline-none focus:bg-white focus:border-indigo-400 focus:ring-2 focus:ring-indigo-100"
                    maxLength="1"
                    value={values[index]}
                    onChange={(e) => handleInput(e, index)}
                    onKeyDown={(e) => handleKeyDown(e, index)}
                  />
                ))}
              </div>
              <div className="max-w-[260px] mx-auto mt-4">
                <button
                  type="submit"
                  className="w-full inline-flex justify-center whitespace-nowrap rounded-lg bg-indigo-500 px-3.5 py-2.5 text-sm font-medium text-white shadow-sm shadow-indigo-950/10 hover:bg-indigo-600 focus:outline-none focus:ring focus:ring-indigo-300 focus-visible:outline-none focus-visible:ring focus-visible:ring-indigo-300 transition-colors duration-150"
                >
                  Verify Email
                </button>
              </div>
            </form>
            <div className="text-sm text-slate-500 mt-4">
              Didn't receive code?{" "}
              <span
                className="font-medium text-indigo-500 hover:text-indigo-600 hover:cursor-pointer"
                onClick={handleResendCode}
              >
                Resend
              </span>
            </div>
          </div>
        </div>
      </div>
    </main>
  );
};

export default EmailConfirm;
