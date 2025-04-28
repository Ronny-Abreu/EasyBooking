document.addEventListener("DOMContentLoaded", function () {
    const hamburger = document.getElementById('hamburger-menu');
    const mobileNav = document.getElementById('mobile-nav');
    const backdrop = document.getElementById('mobile-nav-backdrop');

    function openMenu() {
        mobileNav.classList.add('show');
        backdrop.classList.add('show');
        document.body.classList.add('no-scroll'); // Desactivar scroll (solo dentro del menú hamburguesa)
    }

    function closeMenu() {
        mobileNav.classList.remove('show');
        backdrop.classList.remove('show');
        document.body.classList.remove('no-scroll');
    }

    // Cerrar menú al hacer click en cualquier enlace
    hamburger.addEventListener('click', openMenu);

    backdrop.addEventListener('click', closeMenu);

    const mobileLinks = document.querySelectorAll('.mobile-nav-links a, .footer-btn');

    mobileLinks.forEach(link => {
        link.addEventListener('click', closeMenu);
    });
});


//Función para cargar la inicial del avatar dentro del menú hamburguesa
document.addEventListener("DOMContentLoaded", function () {
    const profileAvatarLink = document.querySelector('.profile-avatar-link');
    const mobileProfileAvatar = document.getElementById('mobile-profile-avatar');

    if (profileAvatarLink && mobileProfileAvatar) {
        const userName = profileAvatarLink.getAttribute('data-username')?.trim();
        if (userName && userName.length > 0) {
            const initial = userName.charAt(0).toUpperCase();
            mobileProfileAvatar.textContent = initial;
        }
    }
});

document.addEventListener("DOMContentLoaded", function () {
    // Publicaciones
    const submenuToggle = document.getElementById('toggle-submenu');
    const subMenu = document.getElementById('sub-menu');
    const arrowIcon = document.getElementById('arrow-icon');

    // Mis Reservas
    const reservasToggle = document.getElementById('toggle-reservas');
    const reservasSubMenu = document.getElementById('reservas-submenu');
    const arrowReservasIcon = document.getElementById('arrow-reservas-icon');

    function closeAllSubmenus() {
        subMenu.style.maxHeight = null;
        reservasSubMenu.style.maxHeight = null;
        arrowIcon.classList.remove('rotate');
        arrowReservasIcon.classList.remove('rotate');
    }

    submenuToggle.addEventListener('click', function () {
        const isOpen = subMenu.style.maxHeight;
        closeAllSubmenus();
        if (!isOpen) {
            subMenu.style.maxHeight = subMenu.scrollHeight + "px";
            arrowIcon.classList.add('rotate');
        }
    });

    reservasToggle.addEventListener('click', function () {
        const isOpen = reservasSubMenu.style.maxHeight;
        closeAllSubmenus();
        if (!isOpen) {
            reservasSubMenu.style.maxHeight = reservasSubMenu.scrollHeight + "px";
            arrowReservasIcon.classList.add('rotate');
        }
    });
});





document.addEventListener("DOMContentLoaded", () => {
    // Menú items del boton profile (solo en Destokp y tablets)
    const profileButton = document.getElementById("profile-button")
    const profileDropdown = document.getElementById("profile-dropdown")

    if (profileButton && profileDropdown) {
        profileButton.addEventListener("click", (e) => {
            e.stopPropagation()
            profileDropdown.classList.toggle("show")
        })

        // Close dropdown when clicking outside
        document.addEventListener("click", (e) => {
            if (!profileButton.contains(e.target) && !profileDropdown.contains(e.target)) {
                profileDropdown.classList.remove("show")
            }
        })
    }

    // Boton de volver al principio
    const backToTopButton = document.getElementById("back-to-top")

    if (backToTopButton) {
        window.addEventListener("scroll", () => {
            if (window.pageYOffset > 300) {
                backToTopButton.classList.add("visible")
            } else {
                backToTopButton.classList.remove("visible")
            }
        })

        backToTopButton.addEventListener("click", () => {
            window.scrollTo({
                top: 0,
                behavior: "smooth",
            })
        })
    }

    let lastScrollTop = 0
    const navbar = document.querySelector(".EasyBooking-navbar")

    if (navbar) {
        window.addEventListener("scroll", () => {
            const scrollTop = window.pageYOffset || document.documentElement.scrollTop

            if (scrollTop > lastScrollTop && scrollTop > 100) {
                navbar.classList.add("navbar-hidden")
            } else {
                navbar.classList.remove("navbar-hidden")
            }

            lastScrollTop = scrollTop
        })
    }

})