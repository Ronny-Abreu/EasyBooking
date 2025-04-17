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
