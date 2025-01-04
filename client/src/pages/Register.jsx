import React, { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { useAuth } from "../contexts/AuthContext";
import Input from "../components/Input";

const Register = () => {
  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [userName, setUserName] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [errors, setErrors] = useState([]);
  const navigate = useNavigate();
  const { register } = useAuth();

  const handleRegister = async (e) => {
    e.preventDefault();
    await register({
      firstName: firstName,
      lastName: lastName,
      userName: userName,
      email: email,
      password: password,
    }).then(() => {
      navigate("/email-confirm");
    }).catch((error) => {
      setErrors(error.errors);
    });
  };

  const handleInputChange = (setter) => (e) => {
    setter(e.target.value);
    setErrors([]);
  };

  return (
    <div className="flex min-h-full flex-1 flex-col justify-center px-6 my-20 lg:px-8">
      <div className="sm:mx-auto sm:w-full sm:max-w-sm">
        <h2 className="my-6 text-center text-4xl font-bold tracking-tight text-gray-700">
          Create a new account
        </h2>
      </div>

      <div className="mt-10 sm:mx-auto sm:w-full sm:max-w-sm">
        <form onSubmit={handleRegister} className="space-y-6">
          <div className="flex flex-col md:flex-row justify-between gap-5">
            <Input
              id={"firstName"}
              type={"text"}
              required={true}
              value={firstName}
              onChange={handleInputChange(setFirstName)}
              label={"First Name"}
              placeholder={"Enter your first name"}
            />
            <Input
              id={"lastName"}
              type={"text"}
              required={true}
              value={lastName}
              onChange={handleInputChange(setLastName)}
              label={"Last Name"}
              placeholder={"Enter your last name"}
            />
          </div>
          <Input
            id={"userName"}
            type={"text"}
            required={true}
            value={userName}
            onChange={handleInputChange(setUserName)}
            label={"Username"}
            placeholder={"Enter your username"}
          />
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
              Create an account
            </button>
          </div>
        </form>

        <p className="mt-5 text-center text-sm/6 text-gray-500">
          Already have an account?{" "}
          <Link
            to={"/login"}
            className="font-semibold text-indigo-600 hover:text-indigo-500"
          >
            Sign in your account
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

export default Register;
