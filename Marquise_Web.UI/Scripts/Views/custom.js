// Function to start the counter animation
function startCounter(element, maxCounterValue) {
    let counterValue = 0;
    const duration = 1000; // Duration in milliseconds (3 seconds)
    const incrementRate = maxCounterValue / (duration / 50); // Calculate step per 50ms interval

    // Function to increment the counter value and update the element
    function incrementCounter() {
        counterValue += incrementRate;
        if (counterValue >= maxCounterValue) {
            element.textContent = maxCounterValue; // Set to max value once done
            clearInterval(intervalId); // Stop the interval when max value is reached
        } else {
            element.textContent = Math.floor(counterValue); // Update the element with current value
        }
    }

    // Start the counter with 50ms intervals for smoother animation
    const intervalId = setInterval(incrementCounter, 50);
}

// Function to set up intersection observer and start counter when element is in view
function createIntersectionObserver(element) {
    const maxValue = parseInt(element.getAttribute('data-max'), 10);

    const observerOptions = {
        root: null, // Observe in the viewport
        rootMargin: '0px',
        threshold: 0.5 // Trigger when 50% of the element is visible
    };

    const observer = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                element.style.opacity = 1; // Make the counter visible
                startCounter(element, maxValue);
                observer.unobserve(element); // Unobserve after starting the counter
            }
        });
    }, observerOptions);

    observer.observe(element);
}

// Function to convert English numbers to Persian numbers
function convertToPersianNumbers(element) {
    const persianNumbers = ['۰', '۱', '۲', '۳', '۴', '۵', '۶', '۷', '۸', '۹'];

    // Function to replace English numbers with Persian numbers
    function replaceWithPersianNumbers(text) {
        return text.replace(/\d/g, function (digit) {
            return persianNumbers[digit];
        });
    }

    // Traverse through all child nodes
    element.childNodes.forEach(function (node) {
        if (node.nodeType === Node.TEXT_NODE) {
            // Only change text nodes
            node.textContent = replaceWithPersianNumbers(node.textContent);
        } else if (node.nodeType === Node.ELEMENT_NODE) {
            // Recursively traverse through element nodes
            convertToPersianNumbers(node);
        }
    });
}

// Function to toggle accordion display
function toggleAccordion(header) {
    const content = header.nextElementSibling;
    const isOpen = content.style.display === "block";

    // Toggle content display
    content.style.display = isOpen ? "none" : "block";

    // Toggle the open class for the header
    header.classList.toggle("open", !isOpen);
}

// Initialize all counters when document is ready
$(document).ready(function () {
    // Convert numbers to Persian
    convertToPersianNumbers(document.body);

    // Select all elements with the class 'counter' and set up observers
    const counters = document.querySelectorAll('.counter');
    counters.forEach(counterElement => {
        createIntersectionObserver(counterElement);
    });

    // Initialize Owl Carousel
    var owl = $('.niwax-logo-slider');
    owl.owlCarousel({
        loop: true,
        center: false,
        autoplay: true,
        margin: 20,
        nav: false,
        dots: false,
        autoplayTimeout: 3500,
        autoplayHoverPause: true,
        smartSpeed: 2000,
        responsive: {
            0: {
                items: 3,
            },
            520: {
                items: 3
            },
            768: {
                items: 4
            },
            1200: {
                items: 4
            },
            1400: {
                items: 5
            },
            1600: {
                items: 6
            },
        }
    });
});

// Function to resize custom box when clicked
function resizeCustomBox(clickedBox) {
    const row = clickedBox.closest('.custom-row'); // Find the closest parent row
    const boxes = row.querySelectorAll('.custom-col'); // Get all columns in the row

    // Check the current state of the clicked column
    if (clickedBox.classList.contains('custom-col-6')) {
        // If column is in col-6 state, reset to default state
        boxes.forEach(box => {
            box.classList.remove('custom-col-6', 'custom-col-2');
            box.classList.add('custom-col-3');
        });
    } else {
        // Change column sizes to col-6 and other columns to col-2
        boxes.forEach(box => {
            if (box === clickedBox) {
                box.classList.remove('custom-col-3', 'custom-col-2');
                box.classList.add('custom-col-6');
            } else {
                box.classList.remove('custom-col-3', 'custom-col-6');
                box.classList.add('custom-col-2');
            }
        });
    }
}
