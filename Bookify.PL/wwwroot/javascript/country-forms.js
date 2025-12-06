/* ============================================
   COUNTRY MANAGEMENT FORMS - SHARED SCRIPTS
   ============================================ */

/**
 * Initialize country input validation
 * Validates that only countries from the datalist are accepted
 */
function initializeCountryValidation() {
    const countryInput = document.getElementById('Country_CountryName');
    const countriesList = document.getElementById('countries');
    const countryError = document.getElementById('countryError');
    const form = document.querySelector('form');

    // Exit if elements don't exist
    if (!countryInput || !countriesList || !form) {
        return;
    }

    /**
     * Get all valid country options from the datalist
     * @returns {Array} Array of country names
     */
    function getValidCountries() {
        const options = countriesList.querySelectorAll('option');
        return Array.from(options).map(option => option.value);
    }

    /**
     * Show validation error
     * @param {string} message - Error message to display
     */
    function showError(message = 'Please select a country from the list') {
        if (countryError) {
            countryError.textContent = message;
            countryError.style.display = 'block';
        }
        countryInput.classList.add('is-invalid');
    }

    /**
     * Hide validation error
     */
    function hideError() {
        if (countryError) {
            countryError.style.display = 'none';
        }
        countryInput.classList.remove('is-invalid');
    }

    /**
     * Validate country input
     * @returns {boolean} True if valid, false otherwise
     */
    function validateCountry() {
        const value = countryInput.value.trim();
        const validCountries = getValidCountries();

        if (value && !validCountries.includes(value)) {
            return false;
        }
        return true;
    }

    // Show datalist when input is focused
    countryInput.addEventListener('focus', function() {
        this.click();
    });

    // Validate on blur
    countryInput.addEventListener('blur', function() {
        if (!validateCountry()) {
            showError();
            this.value = '';
        } else {
            hideError();
        }
    });

    // Clear error when user starts typing a new value
    countryInput.addEventListener('input', function() {
        if (this.value === '') {
            hideError();
        }
    });

    // Prevent form submission if invalid
    form.addEventListener('submit', function(e) {
        if (!validateCountry()) {
            e.preventDefault();
            showError();
            countryInput.focus();
            return false;
        }
    });
}

/**
 * Initialize all form features when DOM is ready
 */
document.addEventListener('DOMContentLoaded', function() {
    initializeCountryValidation();
});
