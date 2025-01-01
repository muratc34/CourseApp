import React from "react";
const LoadingSpinner = () => {
  return (
    <div className="min-h-96 bg-opacity-50 flex items-center justify-center">
      <div className="w-16 h-16 border-4 border-blue-500 border-t-transparent border-solid rounded-full animate-spin"></div>
    </div>
  );
};

export default LoadingSpinner;
