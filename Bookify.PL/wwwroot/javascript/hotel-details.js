/* wwwroot/javascript/hotel-details.js
   Lightweight, defensive; works if reviews/rules/fineprint removed.
*/

class HotelDetails {
    constructor() {
        this.elements = {
            lightbox: document.getElementById('lightbox'),
            lightboxImage: document.getElementById('lightbox-image'),
            prevBtn: document.getElementById('lightbox-prev'),
            nextBtn: document.getElementById('lightbox-next'),
            closeBtn: document.getElementById('lightbox-close'),
            triggers: Array.from(document.querySelectorAll('.js-lightbox-trigger') || []),
            tabLinks: Array.from(document.querySelectorAll('.js-tab-link') || []),
            scrollBtn: document.getElementById('btn-scroll-rooms'),
            calendar: document.getElementById('combinedCalendar'),
            dateInputsWrapper: document.querySelector('.date-inputs-wrapper')
        };

        this.images = Array.isArray(window.hotelGalleryImages) ? window.hotelGalleryImages.slice() : [];
        this.currentIndex = 0;

        try {
            this.attachEvents();
            // init calendar only if present and class available
            if (this.elements.calendar && typeof window.CombinedCalendar === 'function') {
                try { window.combinedCalendar = new window.CombinedCalendar(); } catch (err) { console.warn('Calendar init failed', err); }
            }
        } catch (err) {
            console.error('HotelDetails init error:', err);
        }
    }

    // ---------- Helpers ----------
    preload(idx) { if (this.images[idx]) new Image().src = this.images[idx]; }

    fadeTo(src) {
        const img = this.elements.lightboxImage;
        if (!img) return;
        img.style.transition = 'opacity .22s';
        img.style.opacity = 0;
        setTimeout(() => {
            img.src = src;
            img.onload = () => { img.style.opacity = 1; };
        }, 160);
    }

    // ---------- Events ----------
    attachEvents() {
        // Delegated click for triggers (in case elements added later)
        document.addEventListener('click', (e) => {
            const trg = e.target.closest && e.target.closest('.js-lightbox-trigger');
            if (trg) {
                e.preventDefault();
                const idx = Number.parseInt(trg.dataset.index || '0', 10) || 0;
                this.openLightbox(idx);
            }

            // backdrop close
            if (this.elements.lightbox && e.target === this.elements.lightbox) this.closeLightbox();
        });

        // controls
        this.elements.prevBtn?.addEventListener('click', (ev) => { ev.stopPropagation(); this.navigate(-1); });
        this.elements.nextBtn?.addEventListener('click', (ev) => { ev.stopPropagation(); this.navigate(1); });
        this.elements.closeBtn?.addEventListener('click', () => this.closeLightbox());

        // keyboard navigation (only when lightbox exists)
        document.addEventListener('keydown', (e) => {
            if (!this.elements.lightbox || !this.elements.lightbox.classList.contains('active')) return;
            if (e.key === 'Escape') this.closeLightbox();
            if (e.key === 'ArrowLeft') this.navigate(-1);
            if (e.key === 'ArrowRight') this.navigate(1);
        });

        // swipe
        if (this.elements.lightbox) {
            let startX = 0;
            this.elements.lightbox.addEventListener('touchstart', (e) => { startX = e.touches?.[0]?.clientX || 0; }, { passive: true });
            this.elements.lightbox.addEventListener('touchend', (e) => {
                const endX = e.changedTouches?.[0]?.clientX || 0;
                const dx = endX - startX;
                if (dx > 50) this.navigate(-1);
                if (dx < -50) this.navigate(1);
            }, { passive: true });
        }

        // tabs
        this.elements.tabLinks.forEach(btn => btn.addEventListener('click', (e) => this.switchTab(e)));

        // scroll to rooms (debounced)
        if (this.elements.scrollBtn) {
            this.elements.scrollBtn.addEventListener('click', this.debounce(() => {
                const target = document.getElementById('room-options');
                if (target) target.scrollIntoView({ behavior: 'smooth' });
            }, 180));
        }
    }

    // ---------- Lightbox ----------
    openLightbox(index = 0) {
        if (!this.images.length) return;
        this.currentIndex = ((index % this.images.length) + this.images.length) % this.images.length;
        this.preload((this.currentIndex + 1) % this.images.length);
        this.preload((this.currentIndex - 1 + this.images.length) % this.images.length);
        this.fadeTo(this.images[this.currentIndex]);

        if (this.elements.lightbox) {
            this.elements.lightbox.classList.add('active');
            this.elements.lightbox.hidden = false;
            document.body.style.overflow = 'hidden';
        }
    }

    closeLightbox() {
        if (!this.elements.lightbox) return;
        this.elements.lightbox.classList.remove('active');
        this.elements.lightbox.hidden = true;
        document.body.style.overflow = '';
    }

    navigate(dir = 1) {
        if (!this.images.length) return;
        this.currentIndex = (this.currentIndex + dir + this.images.length) % this.images.length;
        this.preload((this.currentIndex + 1) % this.images.length);
        this.fadeTo(this.images[this.currentIndex]);
    }

    // ---------- Tabs ----------
    switchTab(e) {
        const btn = e.currentTarget || e.target;
        const tab = btn.dataset?.tab;
        if (!tab) return;
        this.elements.tabLinks.forEach(b => { b.classList.remove('active'); b.setAttribute('aria-selected', 'false'); });
        btn.classList.add('active'); btn.setAttribute('aria-selected', 'true');
        document.querySelectorAll('.tab-content').forEach(c => c.classList.remove('active'));
        const content = document.getElementById(`tab-${tab}`);
        if (content) {
            content.classList.add('active');
            content.scrollIntoView({ behavior: 'smooth', block: 'start' });
        }
    }

    // ---------- Small utilities ----------
    debounce(fn, wait = 200) {
        let t; return (...args) => { clearTimeout(t); t = setTimeout(() => fn(...args), wait); };
    }
}

// init guard: only if page has either triggers or lightbox image
document.addEventListener('DOMContentLoaded', () => {
    const hasTriggers = !!document.querySelector('.js-lightbox-trigger');
    const hasLightboxImg = !!document.getElementById('lightbox-image');
    if (!hasTriggers && !hasLightboxImg) return; // nothing to do on this page
    window.hotelDetails = new HotelDetails();
});
