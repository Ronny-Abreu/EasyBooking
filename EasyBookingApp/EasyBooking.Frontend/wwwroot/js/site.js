document.addEventListener("DOMContentLoaded", () => {
    // Inicializar todos los carruseles
    var carousels = document.querySelectorAll(".carousel")
    carousels.forEach((carousel) => {
        const bsCarousel = new bootstrap.Carousel(carousel, {
            interval: 3000,
            wrap: true,
            touch: true,
        })
    })

    // Pausar el carrusel al pasar el mouse por encima
    carousels.forEach((carousel) => {
        carousel.addEventListener("mouseenter", () => {
            bootstrap.Carousel.getInstance(carousel).pause()
        })

        carousel.addEventListener("mouseleave", () => {
            bootstrap.Carousel.getInstance(carousel).cycle()
        })
    })
})


//fade-in para las cards
document.addEventListener('DOMContentLoaded', function () {
    const cards = document.querySelectorAll('.hotel-card');

    const observer = new IntersectionObserver(entries => {
        entries.forEach((entry, index) => {
            if (entry.isIntersecting) {
                setTimeout(() => {
                    entry.target.classList.add('show');
                    entry.target.classList.remove('fade-reset');
                }, index * 100);
            } else {
                entry.target.classList.remove('show');
                entry.target.classList.add('fade-reset');
            }
        });
    }, {
        threshold: 0.2
    });

    cards.forEach(card => {
        observer.observe(card);
    });
});