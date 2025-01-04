import React, { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { useAuth } from "../contexts/AuthContext";
import Input from "../components/Input";

const Login = () => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [errors, setErrors] = useState([]);
  const navigate = useNavigate();

  const { login } = useAuth();

  const handleLogin = async (e) => {
    e.preventDefault();
    await login(email, password)
      .then(() => {
        navigate("/");
      })
      .catch((error) => {
        console.log(error.errors);
        setErrors(error.errors);
      });
  };

  const handleInputChange = (setter) => (e) => {
    setter(e.target.value);
    setErrors([]);
  };

  return (
    <div className="flex min-h-full flex-col justify-center px-6 py-12 lg:px-8">
      <div className="sm:mx-auto sm:w-full sm:max-w-sm">
        <h2 className="my-6 text-center text-4xl font-bold tracking-tight text-gray-700">
          Sign in to your account
        </h2>
      </div>
      <div className="mt-10 sm:mx-auto sm:w-full sm:max-w-sm">
        <form onSubmit={handleLogin} className="space-y-6">
          <Input
            id={"email"}
            type={"email"}
            required={true}
            value={email}
            onChange={handleInputChange(setEmail)}
            label={"Email"}
            placeholder={"Enter your email"}
          />
          <Input
            id={"password"}
            type={"password"}
            required={true}
            value={password}
            onChange={handleInputChange(setPassword)}
            label={"Password"}
            placeholder={"Enter your password"}
          />

          <div>
            <button
              type="submit"
              className="flex w-full justify-center rounded-md bg-indigo-600 px-3 py-1.5 text-sm/6 font-semibold text-white shadow-sm hover:bg-indigo-500 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-indigo-600"
            >
              Sign in
            </button>
          </div>
        </form>

        <p className="mt-5 text-center text-sm/6 text-gray-500">
          Not a member?{" "}
          <Link
            to={"/register"}
            className="font-semibold text-indigo-600 hover:text-indigo-500"
          >
            Create an account
          </Link>
        </p>
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
    </div>
  );
};

export default Login;
