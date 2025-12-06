/* ================================================
   OPTIMIZED NAVBAR + MOBILE MENU + SMOOTH SCROLL
   ================================================ */
document.addEventListener('DOMContentLoaded', function () {

    // ===== GET ELEMENTS SAFELY =====
    const mobileToggle = document.getElementById('mobileMenuToggle');
    const navLinks = document.querySelector('.nav-links');
    const navbar = document.querySelector('.main-navbar');
    const body = document.body;

    /* =========================================
       1) MOBILE MENU TOGGLE
       ========================================= */
    if (mobileToggle && navLinks) {

        const toggleMenu = () => {
            const isActive = navLinks.classList.toggle('active');
            const icon = mobileToggle.querySelector('i');

            if (icon) {
                icon.classList.toggle('fa-bars', !isActive);
                icon.classList.toggle('fa-times', isActive);
            }

            body.style.overflow = isActive ? 'hidden' : '';
        };

        mobileToggle.addEventListener('click', toggleMenu);

        // Close menu when clicking outside
        document.addEventListener('click', function (event) {
            if (!event.target.closest('.navbar-content') && navLinks.classList.contains('active')) {
                navLinks.classList.remove('active');

                const icon = mobileToggle.querySelector('i');
                if (icon) {
                    icon.classList.remove('fa-times');
                    icon.classList.add('fa-bars');
                }

                body.style.overflow = '';
            }
        });

        // Close when clicking a link (mobile only)
        navLinks.querySelectorAll('a').forEach(link => {
            link.addEventListener('click', function () {
                if (window.innerWidth <= 992 && navLinks.classList.contains('active')) {
                    navLinks.classList.remove('active');

                    const icon = mobileToggle.querySelector('i');
                    if (icon) {
                        icon.classList.remove('fa-times');
                        icon.classList.add('fa-bars');
                    }

                    body.style.overflow = '';
                }
            });
        });
    }

    ///* =========================================
    //   2) NAVBAR SCROLL EFFECT
    //   ========================================= */
    //if (navbar) {
    //    window.addEventListener('scroll', function () {
    //        const scrollTop = window.pageYOffset || document.documentElement.scrollTop;
    //        navbar.classList.toggle('scrolled', scrollTop > 50);
    //    });
    //}

    /* =========================================
       3) SMOOTH SCROLL FOR ANCHOR LINKS
       ========================================= */
    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener('click', function (e) {
            const href = this.getAttribute('href');

            if (!href || href === '#' || href === '#!') return;

            const target = document.querySelector(href);
            if (!target) return;

            e.preventDefault();

            const navHeight = navbar ? navbar.offsetHeight : 0;
            const offsetTop = target.offsetTop - navHeight;

            window.scrollTo({
                top: offsetTop,
                behavior: 'smooth'
            });
        });
    });

    /* =========================================
       4) RESIZE HANDLER (AUTO CLOSE MENU)
       ========================================= */
    let resizeTimer;
    window.addEventListener('resize', function () {
        clearTimeout(resizeTimer);

        resizeTimer = setTimeout(function () {
            if (window.innerWidth > 992 && navLinks?.classList.contains('active')) {
                navLinks.classList.remove('active');

                const icon = mobileToggle?.querySelector('i');
                if (icon) {
                    icon.classList.remove('fa-times');
                    icon.classList.add('fa-bars');
                }

                body.style.overflow = '';
            }
        }, 200);
    });

    /* =========================================
       5) KEYBOARD ACCESSIBILITY (ESC CLOSES MENU)
       ========================================= */
    document.addEventListener('keydown', function (e) {
        if (e.key === 'Escape' && navLinks?.classList.contains('active')) {
            navLinks.classList.remove('active');

            const icon = mobileToggle?.querySelector('i');
            if (icon) {
                icon.classList.remove('fa-times');
                icon.classList.add('fa-bars');
            }

            body.style.overflow = '';

            mobileToggle?.focus();
        }
    });

});
