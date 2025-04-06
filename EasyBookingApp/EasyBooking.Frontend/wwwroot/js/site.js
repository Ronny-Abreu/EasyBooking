//Loading configuracion

document.addEventListener("DOMContentLoaded", function () {
    const existingSpinner = document.querySelector('.spinner.center');

    if (existingSpinner) {
        let container = existingSpinner.parentElement;
        if (!container.classList.contains('loading-container')) {
            container = document.createElement('div');
            container.className = 'loading-container';
            existingSpinner.parentNode.insertBefore(container, existingSpinner);
            container.appendChild(existingSpinner);
        }

        container.style.position = 'fixed';
        container.style.top = '0';
        container.style.left = '0';
        container.style.width = '100%';
        container.style.height = '100%';
        container.style.backgroundColor = 'rgba(255, 255, 255, 0.8)';
        container.style.zIndex = '9999';
        container.style.display = 'flex';
        container.style.justifyContent = 'center';
        container.style.alignItems = 'center';

        container.style.display = 'flex';

        // Ocultar el spinner cuando la página termine de cargar
        window.addEventListener('load', function () {
            container.style.display = 'none';
        });
    }
});