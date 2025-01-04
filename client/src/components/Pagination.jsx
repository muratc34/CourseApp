import React from 'react'

const Pagination = ({ totalPages, currentPage, onPageChange }) => {
    const getPageNumbers = () => {
      const delta = 2;
      const range = [];
      for (let i = Math.max(2, currentPage - delta); i <= Math.min(totalPages - 1, currentPage + delta); i++) {
        range.push(i);
      }
      if (currentPage - delta > 2) {
        range.unshift("...");
      }
      if (currentPage + delta < totalPages - 1) {
        range.push("...");
      }
      return [1, ...range, totalPages].filter((page, index, self) => self.indexOf(page) === index);
    };
  
    const handlePageClick = (page) => {
      if (page !== "..." && page !== currentPage) {
        onPageChange(page);
      }
    };
  
    return (
      <div className="flex justify-center items-center py-4">
        <button
          className={`px-3 py-1 rounded ${currentPage === 1 ? "text-gray-400 cursor-not-allowed" : "text-indigo-500"}`}
          disabled={currentPage === 1}
          onClick={() => onPageChange(currentPage - 1)}
        >
          ← Previous
        </button>
  
        <div className="flex mx-4 space-x-2">
          {getPageNumbers().map((page, index) => (
            <button
              key={index}
              className={`px-3 py-1 rounded ${
                page === currentPage ? "bg-indigo-500 text-white" : "text-indigo-500"
              } ${page === "..." ? "cursor-default" : "cursor-pointer"}`}
              onClick={() => handlePageClick(page)}
            >
              {page}
            </button>
          ))}
        </div>
  
        <button
          className={`px-3 py-1 rounded ${currentPage === totalPages ? "text-gray-400 cursor-not-allowed" : "text-indigo-500"}`}
          disabled={currentPage === totalPages}
          onClick={() => onPageChange(currentPage + 1)}
        >
          Next →
        </button>
      </div>
    );
  };

export default Pagination