//CARRUSEL
document.addEventListener('DOMContentLoaded', function () {
    // Inicializar todos los carruseles
    initCarousels();

    // Inicializar botones de favoritos
    initFavoriteButtons();
});

function initCarousels() {
    const carousels = document.querySelectorAll('.card-carousel');

    carousels.forEach(carousel => {
        const slides = carousel.querySelectorAll('.carousel-item');
        const dots = carousel.querySelectorAll('.dot');
        const prevBtn = carousel.querySelector('.prev');
        const nextBtn = carousel.querySelector('.next');
        let currentIndex = 0;

        if (prevBtn) {
            prevBtn.addEventListener('click', () => {
                currentIndex = (currentIndex - 1 + slides.length) % slides.length;
                updateCarousel();
            });
        }

        if (nextBtn) {
            nextBtn.addEventListener('click', () => {
                currentIndex = (currentIndex + 1) % slides.length;
                updateCarousel();
            });
        }

        dots.forEach((dot, index) => {
            dot.addEventListener('click', () => {
                currentIndex = index;
                updateCarousel();
            });
        });

        // Función para actualizar el carrusel
        function updateCarousel() {
            slides.forEach((slide, index) => {
                if (index === currentIndex) {
                    slide.classList.add('active');
                } else {
                    slide.classList.remove('active');
                }
            });

            dots.forEach((dot, index) => {
                if (index === currentIndex) {
                    dot.classList.add('active');
                } else {
                    dot.classList.remove('active');
                }
            });
        }

        let autoplayInterval = setInterval(() => {
            currentIndex = (currentIndex + 1) % slides.length;
            updateCarousel();
        }, 5000);

        carousel.addEventListener('mouseenter', () => {
            clearInterval(autoplayInterval);
        });

        // Reanudar autoplay al quitar el mouse
        carousel.addEventListener('mouseleave', () => {
            autoplayInterval = setInterval(() => {
                currentIndex = (currentIndex + 1) % slides.length;
                updateCarousel();
            }, 5000);
        });
    });
}

function initFavoriteButtons() {
    const favoriteButtons = document.querySelectorAll('.favorite-btn');

    favoriteButtons.forEach(button => {
        button.addEventListener('click', (e) => {
            e.preventDefault();
            button.classList.toggle('active');
        });
    });
}

document.addEventListener('DOMContentLoaded', function () {
    const navbar = document.querySelector('.EasyBooking-navbar');
    const backToTopButton = document.getElementById('back-to-top');
    const scrollDownButton = document.getElementById('scroll-down');
    const heroSection = document.querySelector('.hero-section');

    let lastScrollTop = 0;
    const scrollThreshold = 100;

    // Función para manejar el scroll
    function handleScroll() {
        const currentScroll = window.pageYOffset || document.documentElement.scrollTop;

        // Mostrar/ocultar navbar según dirección del scroll
        if (currentScroll > lastScrollTop && currentScroll > scrollThreshold) {
            // Scroll hacia abajo (se oculta el navbar)
            navbar.classList.add('navbar-hidden');
        } else {
            // Scroll hacia arriba (se vuelve a mostrar el navbar)
            navbar.classList.remove('navbar-hidden');
        }

        // Mostrar/ocultar botón de volver arriba
        if (currentScroll > window.innerHeight / 2) {
            backToTopButton.classList.add('visible');
        } else {
            backToTopButton.classList.remove('visible');
        }

        lastScrollTop = currentScroll <= 0 ? 0 : currentScroll;
    }

    window.addEventListener('scroll', handleScroll);

    // Evento para el botón de volver arriba
    backToTopButton.addEventListener('click', function () {
        window.scrollTo({
            top: 0,
            behavior: 'smooth'
        });
    });

    if (scrollDownButton) {
        scrollDownButton.addEventListener('click', function () {
            const sectionsContainer = document.querySelector('.sections-container');
            if (sectionsContainer) {
                sectionsContainer.scrollIntoView({ behavior: 'smooth' });
            } else {
                const hoteles = document.getElementById('hoteles');
                if (hoteles) {
                    hoteles.scrollIntoView({ behavior: 'smooth' });
                }
            }
        });
    }

    handleScroll();
});