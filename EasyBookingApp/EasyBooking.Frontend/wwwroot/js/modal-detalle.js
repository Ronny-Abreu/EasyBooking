document.addEventListener('DOMContentLoaded', function () {
    initCardClicks();
    initModalClose();
    initImageGallery();

});

// Función para inicializar los clics en las cards
function initCardClicks() {
    const cards = document.querySelectorAll('.card-item');

    cards.forEach(card => {
        card.addEventListener('click', function (e) {
            if (e.target.closest('.carousel-control') || e.target.closest('.favorite-btn')) {
                return;
            }
            const titulo = this.querySelector('.card-title').textContent;
            const ubicacion = this.querySelector('.card-location').textContent;
            const fechas = this.querySelector('.card-dates').textContent;
            const precio = this.querySelector('.card-price').textContent;
            const rating = this.querySelector('.rating').textContent;

            document.getElementById('detalle-titulo').textContent = titulo;
            document.getElementById('detalle-subtitulo').textContent = `Alojamiento entero: condominio en ${ubicacion.split('A')[0].trim()}`;

            const precioMatch = precio.match(/\$(\d+)/);
            if (precioMatch && precioMatch[1]) {
                document.getElementById('detalle-precio').textContent = `$${precioMatch[1]}`;
            }

            document.getElementById('detalle-noches').textContent = `por noche`;
            document.getElementById('detalle-rating').textContent = rating;
            document.getElementById('reserva-rating').textContent = rating;

            const cardImages = Array.from(this.querySelectorAll('.carousel-item img')).map(img => img.src);
            updateModalImages(cardImages);

            // Mostrar el modal
            const detalleModal = document.getElementById('detalle-modal');
            detalleModal.style.display = 'block';
            document.body.style.overflow = 'hidden';

        });
    });
}

// Función para inicializar el cierre del modal
function initModalClose() {
    // Cerrar modal al hacer clic en el botón de cerrar
    const closeBtn = document.getElementById('cerrar-modal');
    if (closeBtn) {
        closeBtn.addEventListener('click', function () {
            closeModal();
        });
    }

    // Cerrar modal al hacer clic fuera del contenido
    const modal = document.getElementById('detalle-modal');
    if (modal) {
        modal.addEventListener('click', function (e) {
            if (e.target === this) {
                closeModal();
            }
        });
    }

    // Cerrar modal con la tecla Escape
    document.addEventListener('keydown', function (e) {
        if (e.key === 'Escape') {
            closeModal();
        }
    });
}

// Función para cerrar el modal
function closeModal() {
    const modal = document.getElementById('detalle-modal');
    if (modal) {
        modal.style.display = 'none';
        document.body.style.overflow = '';
    }

    const galeriaCompleta = document.getElementById('galeria-completa');
    if (galeriaCompleta) {
        galeriaCompleta.style.display = 'none';
    }
}

// Función para inicializar la galería de imágenes
function initImageGallery() {
    console.log('Inicializando galería de imágenes');

    initThumbnails();
    initGalleryControls();
    initFullGallery();
}

// Función para inicializar las miniaturas
function initThumbnails() {
    const miniaturas = document.querySelectorAll('.miniatura');

    miniaturas.forEach(miniatura => {
        miniatura.addEventListener('click', function () {
            const imgSrc = this.getAttribute('data-img');
            const imagenPrincipal = document.getElementById('imagen-principal');

            if (imagenPrincipal && imgSrc) {
                imagenPrincipal.src = imgSrc;

                miniaturas.forEach(m => m.classList.remove('active'));
                this.classList.add('active');

                console.log('Miniatura cambiada:', imgSrc);
            }
        });
    });

    console.log('Miniaturas inicializadas:', miniaturas.length);
}

// Función para inicializar los controles de la galería principal
function initGalleryControls() {
    const prevBtn = document.querySelector('.galeria-control.prev');
    const nextBtn = document.querySelector('.galeria-control.next');

    if (prevBtn && nextBtn) {
        prevBtn.addEventListener('click', function (e) {
            e.preventDefault();
            cambiarMiniatura(-1);
        });

        nextBtn.addEventListener('click', function (e) {
            e.preventDefault();
            cambiarMiniatura(1);
        });

        console.log('Controles de galería inicializados');
    } else {
        console.log('No se encontraron los controles de galería');
    }
}

// Función para cambiar la miniatura activa
function cambiarMiniatura(direccion) {
    const miniaturas = document.querySelectorAll('.miniatura');
    if (miniaturas.length === 0) return;

    // Encontrar la miniatura activa actual
    let indexActual = 0;
    for (let i = 0; i < miniaturas.length; i++) {
        if (miniaturas[i].classList.contains('active')) {
            indexActual = i;
            break;
        }
    }

    let nuevoIndex = (indexActual + direccion + miniaturas.length) % miniaturas.length;

    miniaturas[nuevoIndex].click();

}

// Función para inicializar la galería completa
function initFullGallery() {
    const verFotosBtn = document.querySelector('.ver-todas-fotos');
    const cerrarGaleriaBtn = document.getElementById('cerrar-galeria');
    const prevFotoBtn = document.getElementById('prev-foto');
    const nextFotoBtn = document.getElementById('next-foto');

    if (verFotosBtn) {
        verFotosBtn.addEventListener('click', function (e) {
            e.preventDefault();

            // Obtener todas las imágenes de las miniaturas
            const miniaturas = document.querySelectorAll('.miniatura');
            const imagenes = Array.from(miniaturas).map(m => m.getAttribute('data-img'));

            let indexActual = 0;
            for (let i = 0; i < miniaturas.length; i++) {
                if (miniaturas[i].classList.contains('active')) {
                    indexActual = i;
                    break;
                }
            }

            actualizarGaleriaCompleta(imagenes, indexActual);

            // Mostrar la galería completa
            const galeriaCompleta = document.getElementById('galeria-completa');
            if (galeriaCompleta) {
                galeriaCompleta.style.display = 'flex';;
            }
        });
    }

    if (cerrarGaleriaBtn) {
        cerrarGaleriaBtn.addEventListener('click', function () {
            const galeriaCompleta = document.getElementById('galeria-completa');
            if (galeriaCompleta) {
                galeriaCompleta.style.display = 'none';
                console.log('Galería completa cerrada');
            }
        });
    }

    if (prevFotoBtn && nextFotoBtn) {
        prevFotoBtn.addEventListener('click', function () {
            cambiarFoto(-1);
        });

        nextFotoBtn.addEventListener('click', function () {
            cambiarFoto(1);
        });

        console.log('Controles de galería completa inicializados');
    }
}

// Función para actualizar la galería completa
function actualizarGaleriaCompleta(imagenes, indexActual) {
    const imagenActual = document.getElementById('galeria-imagen-actual');
    const miniaturasContainer = document.querySelector('.galeria-completa-miniaturas');
    const contadorActual = document.getElementById('imagen-actual-num');
    const contadorTotal = document.getElementById('imagen-total-num');

    if (!imagenActual || !miniaturasContainer || !contadorActual || !contadorTotal) {
        console.log('No se encontraron elementos necesarios para la galería completa');
        return;
    }

    // Actualizar la imagen actual
    if (imagenes.length > 0 && indexActual < imagenes.length) {
        imagenActual.src = imagenes[indexActual];
    }

    contadorActual.textContent = indexActual + 1;
    contadorTotal.textContent = imagenes.length;

    miniaturasContainer.innerHTML = '';

    imagenes.forEach((src, index) => {
        const miniatura = document.createElement('div');
        miniatura.className = `miniatura-completa ${index === indexActual ? 'active' : ''}`;
        miniatura.setAttribute('data-index', index);

        const img = document.createElement('img');
        img.src = src;
        img.alt = `Miniatura ${index + 1}`;

        miniatura.appendChild(img);
        miniaturasContainer.appendChild(miniatura);

        // Evento de clic a la miniatura
        miniatura.addEventListener('click', function () {
            const index = parseInt(this.getAttribute('data-index'));

            imagenActual.src = imagenes[index];
            contadorActual.textContent = index + 1;

            document.querySelectorAll('.miniatura-completa').forEach(m => m.classList.remove('active'));
            this.classList.add('active');
        });
    });
}

// Función para cambiar la foto en la galería completa
function cambiarFoto(direccion) {
    const miniaturas = document.querySelectorAll('.miniatura-completa');
    const totalFotos = miniaturas.length;

    if (totalFotos === 0) return;

    // Encontrar la miniatura activa actual
    let indexActual = 0;
    for (let i = 0; i < miniaturas.length; i++) {
        if (miniaturas[i].classList.contains('active')) {
            indexActual = i;
            break;
        }
    }

    let nuevoIndex = (indexActual + direccion + totalFotos) % totalFotos;

    miniaturas[nuevoIndex].click();

}

// Función para actualizar las imágenes en el modal
function updateModalImages(images) {
    // Actualizar imagen principal
    const imagenPrincipal = document.getElementById('imagen-principal');
    if (imagenPrincipal && images.length > 0) {
        imagenPrincipal.src = images[0];
    }

    // Actualizar miniaturas
    const galeriaMiniaturas = document.querySelector('.galeria-miniaturas');
    if (galeriaMiniaturas) {
        galeriaMiniaturas.innerHTML = '';

        images.forEach((src, index) => {
            const miniatura = document.createElement('div');
            miniatura.className = `miniatura ${index === 0 ? 'active' : ''}`;
            miniatura.setAttribute('data-img', src);

            const img = document.createElement('img');
            img.src = src;
            img.alt = `Miniatura ${index + 1}`;

            miniatura.appendChild(img);
            galeriaMiniaturas.appendChild(miniatura);
        });
        initThumbnails();
    }

    console.log('Imágenes del modal actualizadas con', images.length, 'imágenes'); //Para depuración, verificar que las imagenes se actualicen correctamente.
}

document.addEventListener('DOMContentLoaded', function () {
    console.log('DOM cargado, inicializando modal...');
    initCardClicks();
    initModalClose();
    initImageGallery();
});