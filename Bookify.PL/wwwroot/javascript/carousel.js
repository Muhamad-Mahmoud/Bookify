// =============================================
// CAROUSEL FUNCTIONALITY
// =============================================

document.addEventListener('DOMContentLoaded', function () {
    initializeCarousels();
});

function initializeCarousels() {
    const carouselButtons = document.querySelectorAll('.carousel-nav-btn');

    carouselButtons.forEach(button => {
        button.addEventListener('click', function () {
            const carouselId = this.getAttribute('data-carousel');
            const direction = this.classList.contains('prev') ? -1 : 1;
            const carousel = document.getElementById(carouselId + 'Carousel');

            if (!carousel) {
                // Try to find carousel by matching data attribute
                const wrapper = this.closest('.hotels-carousel-wrapper');
                const carouselElement = wrapper.querySelector('.hotels-carousel');
                if (carouselElement) {
                    scrollCarousel(carouselElement, direction);
                }
            } else {
                scrollCarousel(carousel, direction);
            }
        });
    });

    // Add touch/swipe support for mobile
    const carousels = document.querySelectorAll('.hotels-carousel');
    carousels.forEach(carousel => {
        addSwipeSupport(carousel);
    });
}

function scrollCarousel(carousel, direction) {
    // Dynamically calculate scroll amount based on card width and gap
    const card = carousel.querySelector('.hotel-card');
    let scrollAmount = 320; // Default fallback

    if (card) {
        const cardWidth = card.offsetWidth;
        // Get gap from container
        const containerStyle = window.getComputedStyle(carousel);
        const gap = parseFloat(containerStyle.gap) || 24;

        scrollAmount = cardWidth + gap;
    }

    const currentScroll = carousel.scrollLeft;
    const targetScroll = currentScroll + (scrollAmount * direction);

    carousel.scrollTo({
        left: targetScroll,
        behavior: 'smooth'
    });
}

function addSwipeSupport(carousel) {
    let isDown = false;
    let startX;
    let scrollLeft;

    carousel.addEventListener('mousedown', (e) => {
        isDown = true;
        carousel.style.cursor = 'grabbing';
        startX = e.pageX - carousel.offsetLeft;
        scrollLeft = carousel.scrollLeft;
    });

    carousel.addEventListener('mouseleave', () => {
        isDown = false;
        carousel.style.cursor = 'grab';
    });

    carousel.addEventListener('mouseup', () => {
        isDown = false;
        carousel.style.cursor = 'grab';
    });

    carousel.addEventListener('mousemove', (e) => {
        if (!isDown) return;
        e.preventDefault();
        const x = e.pageX - carousel.offsetLeft;
        const walk = (x - startX) * 2;
        carousel.scrollLeft = scrollLeft - walk;
    });

    // Touch events for mobile
    let touchStartX = 0;
    let touchEndX = 0;

    carousel.addEventListener('touchstart', (e) => {
        touchStartX = e.changedTouches[0].screenX;
    }, { passive: true });

    carousel.addEventListener('touchend', (e) => {
        touchEndX = e.changedTouches[0].screenX;
        handleSwipe(carousel, touchStartX, touchEndX);
    }, { passive: true });
}

function handleSwipe(carousel, startX, endX) {
    const swipeThreshold = 50;
    const diff = startX - endX;

    if (Math.abs(diff) > swipeThreshold) {
        const direction = diff > 0 ? 1 : -1;
        scrollCarousel(carousel, direction);
    }
}

// Auto-scroll functionality (optional)
function enableAutoScroll(carouselId, interval = 5000) {
    const carousel = document.getElementById(carouselId);
    if (!carousel) return;

    let autoScrollInterval = setInterval(() => {
        const maxScroll = carousel.scrollWidth - carousel.clientWidth;
        const currentScroll = carousel.scrollLeft;

        if (currentScroll >= maxScroll) {
            carousel.scrollTo({ left: 0, behavior: 'smooth' });
        } else {
            scrollCarousel(carousel, 1);
        }
    }, interval);

    // Pause auto-scroll on hover
    carousel.addEventListener('mouseenter', () => {
        clearInterval(autoScrollInterval);
    });

    carousel.addEventListener('mouseleave', () => {
        autoScrollInterval = setInterval(() => {
            const maxScroll = carousel.scrollWidth - carousel.clientWidth;
            const currentScroll = carousel.scrollLeft;

            if (currentScroll >= maxScroll) {
                carousel.scrollTo({ left: 0, behavior: 'smooth' });
            } else {
                scrollCarousel(carousel, 1);
            }
        }, interval);
    });
}

// Optional: Enable auto-scroll for featured carousel
// enableAutoScroll('featuredCarousel', 5000);
