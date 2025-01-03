import React from 'react'

const Input = ({label, type, id, className, value, onChange, required, placeholder}) => {
  return (
    <div className="flex flex-col w-full gap-2">
      <div className="flex justify-between">
        <label htmlFor={id} className="block text-sm/6 font-medium text-gray-900">
          {label}
        </label>
      </div>
      <input
        value={value}
        onChange={onChange}
        id={id}
        name={id}
        type={type}
        className="block w-full rounded-md bg-white px-3 py-1.5 text-base text-gray-900 outline outline-1 -outline-offset-1 outline-gray-300 placeholder:text-gray-400 focus:outline focus:outline-2 focus:-outline-offset-2 focus:outline-indigo-600 sm:text-sm/6"
        placeholder={placeholder}
        required={required}
      />
    </div>
  )
}

export default Input