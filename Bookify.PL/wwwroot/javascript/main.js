// ================================================
// BOOKIFY - COMBINED CALENDAR SYSTEM
// Two-Month Side-by-Side Date Range Picker
// ================================================

'use strict';

class CombinedCalendar {
    constructor() {
        this.checkinInput = document.getElementById('checkin');
        this.checkoutInput = document.getElementById('checkout');
        this.calendar = document.getElementById('combinedCalendar');
        this.dateInputsWrapper = document.querySelector('.date-inputs-wrapper');
        this.backdrop = document.getElementById('calendarBackdrop');

        this.month1Days = document.getElementById('month1Days');
        this.month2Days = document.getElementById('month2Days');
        this.month1Title = document.getElementById('month1Title');
        this.month2Title = document.getElementById('month2Title');

        this.checkinDate = null;
        this.checkoutDate = null;
        this.selectingCheckout = false;

        // Start with current month
        this.currentMonth1 = new Date();
        this.currentMonth1.setDate(1);
        this.currentMonth2 = new Date(this.currentMonth1);
        this.currentMonth2.setMonth(this.currentMonth2.getMonth() + 1);

        this.months = [
            'January', 'February', 'March', 'April', 'May', 'June',
            'July', 'August', 'September', 'October', 'November', 'December'
        ];

        this.init();

        // Check for initial values in hidden inputs (for search results page)
        const checkInHidden = document.getElementById('checkInDateHidden');
        const checkOutHidden = document.getElementById('checkOutDateHidden');

        if (checkInHidden && checkInHidden.value) {
            this.checkinDate = new Date(checkInHidden.value);
            this.checkinDate.setHours(0, 0, 0, 0);
        }

        if (checkOutHidden && checkOutHidden.value) {
            this.checkoutDate = new Date(checkOutHidden.value);
            this.checkoutDate.setHours(0, 0, 0, 0);
        }

        if (this.checkinDate || this.checkoutDate) {
            this.updateInputs();
            this.renderCalendar();
        }
    }

    init() {
        this.attachEvents();
        this.renderCalendar();
    }

    attachEvents() {
        // Show calendar when clicking the date inputs wrapper
        if (this.dateInputsWrapper) {
            this.dateInputsWrapper.addEventListener('click', (e) => {
                e.stopPropagation();
                this.toggleCalendar();
            });
        }

        // Navigation buttons
        const prevBtn = document.getElementById('prevMonthBtn');
        const nextBtn = document.getElementById('nextMonthBtn');

        if (prevBtn) {
            prevBtn.addEventListener('click', (e) => {
                e.stopPropagation();
                this.previousMonth();
            });
        }

        if (nextBtn) {
            nextBtn.addEventListener('click', (e) => {
                e.stopPropagation();
                this.nextMonth();
            });
        }

        // Footer buttons
        const clearBtn = document.getElementById('clearDatesBtn');
        const todayBtn = document.getElementById('todayBtn');

        if (clearBtn) {
            clearBtn.addEventListener('click', (e) => {
                e.stopPropagation();
                this.clearDates();
            });
        }

        if (todayBtn) {
            todayBtn.addEventListener('click', (e) => {
                e.stopPropagation();
                this.selectToday();
            });
        }

        // Prevent calendar from closing when clicking inside
        if (this.calendar) {
            this.calendar.addEventListener('click', (e) => {
                e.stopPropagation();
            });
        }

        // Close calendar when clicking outside
        document.addEventListener('click', (e) => {
            if (!e.target.closest('.date-range-container')) {
                this.hideCalendar();
            }
        });

        // Backdrop click
        if (this.backdrop) {
            this.backdrop.addEventListener('click', () => {
                this.hideCalendar();
            });
        }
    }

    toggleCalendar() {
        if (this.calendar.classList.contains('show')) {
            this.hideCalendar();
        } else {
            this.showCalendar();
        }
    }

    showCalendar() {
        this.calendar.classList.add('show');
        this.dateInputsWrapper.classList.add('active');
        if (this.backdrop) {
            this.backdrop.classList.add('show');
        }
        this.renderCalendar();
    }

    hideCalendar() {
        this.calendar.classList.remove('show');
        this.dateInputsWrapper.classList.remove('active');
        if (this.backdrop) {
            this.backdrop.classList.remove('show');
        }
    }

    renderCalendar() {
        this.renderMonth(this.currentMonth1, this.month1Days, this.month1Title);
        this.renderMonth(this.currentMonth2, this.month2Days, this.month2Title);
    }

    renderMonth(monthDate, container, titleElement) {
        if (!container || !titleElement) return;

        const year = monthDate.getFullYear();
        const month = monthDate.getMonth();

        // Update title
        titleElement.textContent = `${this.months[month]} ${year}`;

        // Clear container
        container.innerHTML = '';

        const firstDay = new Date(year, month, 1).getDay();
        const daysInMonth = new Date(year, month + 1, 0).getDate();
        const daysInPrevMonth = new Date(year, month, 0).getDate();
        const today = new Date();
        today.setHours(0, 0, 0, 0);

        // Previous month days
        for (let i = firstDay - 1; i >= 0; i--) {
            const day = daysInPrevMonth - i;
            const dayEl = this.createDayElement(day, true, year, month - 1, today);
            container.appendChild(dayEl);
        }

        // Current month days
        for (let day = 1; day <= daysInMonth; day++) {
            const dayDate = new Date(year, month, day);
            dayDate.setHours(0, 0, 0, 0);
            const dayEl = this.createDayElement(day, false, year, month, today, dayDate);
            container.appendChild(dayEl);
        }

        // Next month days to fill grid
        const totalCells = container.children.length;
        const remainingCells = Math.ceil(totalCells / 7) * 7 - totalCells;
        for (let day = 1; day <= remainingCells; day++) {
            const dayEl = this.createDayElement(day, true, year, month + 1, today);
            container.appendChild(dayEl);
        }
    }

    createDayElement(day, isOtherMonth, year, month, today, dayDate = null) {
        const dayEl = document.createElement('div');
        dayEl.className = 'calendar-day';
        dayEl.textContent = day;

        if (!dayDate) {
            dayDate = new Date(year, month, day);
            dayDate.setHours(0, 0, 0, 0);
        }

        // Style other month days
        if (isOtherMonth) {
            dayEl.classList.add('other-month');
        }

        // Mark today
        if (dayDate.getTime() === today.getTime()) {
            dayEl.classList.add('today');
        }

        // Disable past dates
        if (dayDate < today && !isOtherMonth) {
            dayEl.classList.add('disabled');
        }

        // Highlight selected dates
        if (this.checkinDate && dayDate.getTime() === this.checkinDate.getTime()) {
            dayEl.classList.add('selected', 'range-start');
        }

        if (this.checkoutDate && dayDate.getTime() === this.checkoutDate.getTime()) {
            dayEl.classList.add('selected', 'range-end');
        }

        // Highlight range
        if (this.checkinDate && this.checkoutDate) {
            if (dayDate > this.checkinDate && dayDate < this.checkoutDate) {
                dayEl.classList.add('in-range');
            }
        }

        // Add click handler
        if (!dayEl.classList.contains('disabled') && !isOtherMonth) {
            dayEl.addEventListener('click', () => {
                this.selectDate(dayDate);
            });
        }

        return dayEl;
    }

    selectDate(date) {
        if (!this.checkinDate || this.selectingCheckout) {
            // Select check-in or if both are set, start over
            if (this.selectingCheckout) {
                // Setting checkout
                if (date <= this.checkinDate) {
                    // If selected date is before checkin, reset and set as new checkin
                    this.checkinDate = date;
                    this.checkoutDate = null;
                    this.selectingCheckout = false;
                } else {
                    this.checkoutDate = date;
                    this.selectingCheckout = false;
                    this.updateInputs();
                    setTimeout(() => this.hideCalendar(), 300);
                }
            } else {
                this.checkinDate = date;
                this.checkoutDate = null;
                this.selectingCheckout = true;
            }
        } else {
            // Select check-out
            if (date <= this.checkinDate) {
                // If selected date is before checkin, reset and set as new checkin
                this.checkinDate = date;
                this.checkoutDate = null;
                this.selectingCheckout = true;
            } else {
                this.checkoutDate = date;
                this.selectingCheckout = false;
                this.updateInputs();
                setTimeout(() => this.hideCalendar(), 300);
            }
        }

        this.renderCalendar();
    }

    updateInputs() {
        // Update visible inputs
        if (this.checkinDate) {
            this.checkinInput.value = this.formatDate(this.checkinDate);

            // Update hidden input for form submission
            const hiddenCheckin = document.getElementById('checkInDateHidden');
            if (hiddenCheckin) {
                const date = this.checkinDate;
                const year = date.getFullYear();
                const month = String(date.getMonth() + 1).padStart(2, '0');
                const day = String(date.getDate()).padStart(2, '0');
                hiddenCheckin.value = `${year}-${month}-${day}`;
            }
        } else {
            this.checkinInput.value = '';
            const hiddenCheckin = document.getElementById('checkInDateHidden');
            if (hiddenCheckin) hiddenCheckin.value = '';
        }

        if (this.checkoutDate) {
            this.checkoutInput.value = this.formatDate(this.checkoutDate);

            // Update hidden input for form submission
            const hiddenCheckout = document.getElementById('checkOutDateHidden');
            if (hiddenCheckout) {
                const date = this.checkoutDate;
                const year = date.getFullYear();
                const month = String(date.getMonth() + 1).padStart(2, '0');
                const day = String(date.getDate()).padStart(2, '0');
                hiddenCheckout.value = `${year}-${month}-${day}`;
            }
        } else {
            this.checkoutInput.value = '';
            const hiddenCheckout = document.getElementById('checkOutDateHidden');
            if (hiddenCheckout) hiddenCheckout.value = '';
        }
    }

    formatDate(date) {
        const days = ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'];
        const months = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];

        const dayName = days[date.getDay()];
        const day = date.getDate();
        const monthName = months[date.getMonth()];
        const year = date.getFullYear();

        return `${dayName}, ${day} ${monthName} ${year}`;
    }

    previousMonth() {
        this.currentMonth1.setMonth(this.currentMonth1.getMonth() - 1);
        this.currentMonth2.setMonth(this.currentMonth2.getMonth() - 1);
        this.renderCalendar();
    }

    nextMonth() {
        this.currentMonth1.setMonth(this.currentMonth1.getMonth() + 1);
        this.currentMonth2.setMonth(this.currentMonth2.getMonth() + 1);
        this.renderCalendar();
    }

    clearDates() {
        this.checkinDate = null;
        this.checkoutDate = null;
        this.selectingCheckout = false;
        this.checkinInput.value = '';
        this.checkoutInput.value = '';
        this.renderCalendar();
    }

    selectToday() {
        const today = new Date();
        today.setHours(0, 0, 0, 0);
        const tomorrow = new Date(today);
        tomorrow.setDate(tomorrow.getDate() + 1);

        this.checkinDate = today;
        this.checkoutDate = tomorrow;
        this.selectingCheckout = false;
        this.updateInputs();
        this.renderCalendar();
        setTimeout(() => this.hideCalendar(), 300);
    }
}

// Initialize calendar
window.combinedCalendar = null;

document.addEventListener('DOMContentLoaded', function () {
    window.combinedCalendar = new CombinedCalendar();
    console.log('✅ Combined Calendar initialized!');
});

// Rest of your existing JavaScript code (scroll, search, etc.)...
