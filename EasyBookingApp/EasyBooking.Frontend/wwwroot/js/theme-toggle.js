// Theme toggle functionality - simplified
document.addEventListener('DOMContentLoaded', function () {
    const themeToggleBtn = document.getElementById('theme-toggle-button');
    const sunIcon = document.querySelector('.sun-icon');
    const moonIcon = document.querySelector('.moon-icon');

    // Check if user has a theme preference in localStorage
    const currentTheme = localStorage.getItem('theme');

    // If the user has a theme preference, apply it
    if (currentTheme === 'dark') {
        document.body.classList.add('dark-mode');
    }

    // Toggle theme when button is clicked
    themeToggleBtn.addEventListener('click', function () {
        document.body.classList.toggle('dark-mode');

        // Save preference to localStorage
        if (document.body.classList.contains('dark-mode')) {
            localStorage.setItem('theme', 'dark');
        } else {
            localStorage.setItem('theme', 'light');
        }
    });
});