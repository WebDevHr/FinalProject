$(document).ready(function () {
  changeLetterColors(); // Trigger the function at the start

  function changeLetterColors() {
    var letters = ["A", "B", "C", "D", "E", "F"];

    // Function to change letter colors
    function changeColors() {
      $(".letter").each(function () {
        var randomColor = getRandomColor();
        $(this).css("color", randomColor);
      });
    }

    // Initial call to change colors
    changeColors();

    // Set interval to change colors every 2 seconds
    setInterval(changeColors, 2000);
  }

  function getRandomColor() {
    var letters = "0123456789ABCDEF";
    var color = "#";
    for (var i = 0; i < 6; i++) {
      color += letters[Math.floor(Math.random() * 16)];
    }
    return color;
  }
});
document.addEventListener("DOMContentLoaded", function () {
  const searchInput = document.querySelector(".search-input");
  const searchResults = document.querySelector(".search-results");

  searchInput.addEventListener("input", function () {
    if (searchInput.value.trim() === "") {
      searchResults.innerHTML = "";
      searchResults.classList.remove("show");
      return;
    }

    fetch(`/Products/SearchJson?query=${encodeURIComponent(searchInput.value)}`)
      .then((response) => response.json())
      .then((data) => {
        searchResults.innerHTML = "";

        if (data.length === 0) {
          searchResults.classList.remove("show");
          return;
        }

        data.forEach((product) => {
          const resultItem = document.createElement("a");
          resultItem.href = `/Products/Details/${product.id}`;
          resultItem.classList.add("dropdown-item");
          resultItem.textContent = product.title;
          searchResults.appendChild(resultItem);
        });

        searchResults.classList.add("show");
      });
  });

  // Hide the search results when clicking outside the search bar
  document.addEventListener("click", function (event) {
    if (!searchInput.contains(event.target)) {
      searchResults.classList.remove("show");
    }
  });
});
