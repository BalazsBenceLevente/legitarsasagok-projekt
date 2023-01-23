// Get the slides
var slides = document.querySelectorAll(".slide");

// Get the previous and next buttons
var prevBtn = document.querySelector(".prev");
var nextBtn = document.querySelector(".next");

// Set the initial slide index to 0
var slideIndex = 0;

// Show the first slide
slides[slideIndex].style.display = "block";

// Function to move to the next slide
function nextSlide() {
  // Hide the current slide
  slides[slideIndex].style.display = "none";

  // Increment the slide index
  slideIndex++;

  // If the slide index is greater than the number of slides, set it back to 0
  if (slideIndex >= slides.length) {
    slideIndex = 0;
  }

  // Show the next slide
  slides[slideIndex].style.display = "block";
}

// Function to move to the previous slide
function prevSlide() {
  // Hide the current slide
  slides[slideIndex].style.display = "none";

  // Decrement the slide index
  slideIndex--;

  // If the slide index is less than 0, set it to the last slide
  if (slideIndex < 0) {
    slideIndex = slides.length - 1;
  }

  // Show the previous slide
  slides[slideIndex].style.display = "block";
}

// Add event listeners to the previous and next buttons
prevBtn.addEventListener("click", prevSlide);
nextBtn.addEventListener("click", nextSlide);

