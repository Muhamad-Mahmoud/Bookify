/*
    Hotel Details Page Logic
    Path: /wwwroot/js/hotel-details.js
*/

class HotelDetails {
    constructor() {
        try {
            this.validateDOM();
            this.initImages();
            this.initEventListeners();
            this.initObservers();

            // Cache DOM elements
            this.elements = {
                lightbox: document.getElementById('lightbox'),
                lightboxImage: document.getElementById('lightbox-image'),
                tabLinks: document.querySelectorAll('.js-tab-link'),
                lightboxTriggers: document.querySelectorAll('.js-lightbox-trigger'),
                scrollBtn: document.getElementById('btn-scroll-rooms')
            };

            this.currentIndex = 0;
            this.images = [];
        } catch (error) {
            console.error('HotelDetails initialization failed:', error);
        }
    }

    validateDOM() {
        const required = ['lightbox', 'lightbox-image'];
        required.forEach(id => {
            if (!document.getElementById(id)) {
                console.warn(`Required element #${id} not found`);
            }
        });
    }

    initImages() {
        // Images are populated from the view via a global variable or data attribute
        // We'll look for a global variable 'hotelGalleryImages' set in the view
        if (window.hotelGalleryImages) {
            this.images = window.hotelGalleryImages;
        }
    }

    initEventListeners() {
        // Scroll to rooms
        const scrollBtn = document.getElementById('btn-scroll-rooms');
        if (scrollBtn) {
            scrollBtn.addEventListener('click', this.debounce(() => {
                const target = document.getElementById('room-options');
                if (target) target.scrollIntoView({ behavior: 'smooth' });
            }, 200));
        }

        // Tabs
        document.querySelectorAll('.js-tab-link').forEach(btn => {
            btn.addEventListener('click', (e) => this.switchTab(e));
        });

        // Lightbox Triggers
        document.querySelectorAll('.js-lightbox-trigger').forEach(item => {
            item.addEventListener('click', () => {
                const index = parseInt(item.dataset.index);
                this.openLightbox(index);
            });
        });

        // Lightbox Controls
        const lightbox = document.getElementById('lightbox');
        if (lightbox) {
            lightbox.addEventListener('click', (e) => {
                if (e.target === lightbox) this.closeLightbox();
            });

            const closeBtn = document.getElementById('lightbox-close');
            if (closeBtn) closeBtn.addEventListener('click', () => this.closeLightbox());

            const prevBtn = document.getElementById('lightbox-prev');
            if (prevBtn) prevBtn.addEventListener('click', (e) => {
                e.stopPropagation();
                this.navigate(-1);
            });

            const nextBtn = document.getElementById('lightbox-next');
            if (nextBtn) nextBtn.addEventListener('click', (e) => {
                e.stopPropagation();
                this.navigate(1);
            });

            // Keyboard
            document.addEventListener('keydown', (e) => {
                if (!lightbox.classList.contains('active')) return;
                if (e.key === 'Escape') this.closeLightbox();
                if (e.key === 'ArrowLeft') this.navigate(-1);
                if (e.key === 'ArrowRight') this.navigate(1);
            });

            // Touch Swipe
            let touchStartX = 0;
            let touchEndX = 0;

            lightbox.addEventListener('touchstart', (e) => {
                touchStartX = e.changedTouches[0].screenX;
            }, { passive: true });

            lightbox.addEventListener('touchend', (e) => {
                touchEndX = e.changedTouches[0].screenX;
                this.handleSwipe(touchStartX, touchEndX);
            }, { passive: true });
        }
    }

    handleSwipe(start, end) {
        if (end < start - 50) this.navigate(1); // Swipe Left -> Next
        if (end > start + 50) this.navigate(-1); // Swipe Right -> Prev
    }

    debounce(func, wait) {
        let timeout;
        return function executedFunction(...args) {
            const later = () => {
                clearTimeout(timeout);
                func(...args);
            };
            clearTimeout(timeout);
            timeout = setTimeout(later, wait);
        };
    }

    switchTab(e) {
        const btn = e.target;
        const tabName = btn.dataset.tab;

        document.querySelectorAll('.js-tab-link').forEach(b => {
            b.classList.remove('active');
            b.setAttribute('aria-selected', 'false');
        });
        btn.classList.add('active');
        btn.setAttribute('aria-selected', 'true');

        document.querySelectorAll('.tab-content').forEach(c => c.classList.remove('active'));
        const content = document.getElementById(`tab-${tabName}`);
        if (content) {
            content.classList.add('active');
            const yOffset = -80;
            const y = content.getBoundingClientRect().top + window.pageYOffset + yOffset;
            window.scrollTo({ top: y, behavior: 'smooth' });
        }
    }

    openLightbox(index) {
        if (this.images.length === 0) return;
        this.currentIndex = index;
        this.updateLightboxImage();
        const lightbox = document.getElementById('lightbox');
        lightbox.classList.add('active');
        document.body.style.overflow = 'hidden';
    }

    closeLightbox() {
        const lightbox = document.getElementById('lightbox');
        lightbox.classList.remove('active');
        document.body.style.overflow = '';
    }

    navigate(direction) {
        this.currentIndex = (this.currentIndex + direction + this.images.length) % this.images.length;
        this.updateLightboxImage();
    }

    updateLightboxImage() {
        // Preload next/prev images
        const preloadIndexes = [
            (this.currentIndex + 1) % this.images.length,
            (this.currentIndex - 1 + this.images.length) % this.images.length
        ];

        preloadIndexes.forEach(idx => {
            const img = new Image();
            img.src = this.images[idx];
        });

        const imgElement = document.getElementById('lightbox-image');

        // Update current with fade
        imgElement.style.transition = 'opacity 0.2s';
        imgElement.style.opacity = '0';

        setTimeout(() => {
            imgElement.src = this.images[this.currentIndex];
            imgElement.onload = () => {
                imgElement.style.opacity = '1';
            };
        }, 200);
    }

    initObservers() {
        const observerOptions = { threshold: 0.5 };

        const observer = new IntersectionObserver((entries) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    entry.target.querySelectorAll('.review-bar-fill').forEach(bar => {
                        const width = bar.style.width;
                        bar.style.width = '0%';
                        void bar.offsetWidth; // Force reflow
                        bar.style.width = width;
                    });
                    observer.unobserve(entry.target);
                }
            });
        }, observerOptions);

        const reviewSummary = document.querySelector('.review-summary');
        if (reviewSummary) observer.observe(reviewSummary);
    }
}

// Initialize when DOM is ready
document.addEventListener('DOMContentLoaded', () => {
    window.hotelDetails = new HotelDetails();
});
