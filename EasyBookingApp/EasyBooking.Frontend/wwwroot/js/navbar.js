// Funcion openLoginModal definido como global scope
function openLoginModal() {
    document.getElementById("modal-overlay").classList.add("active")
    document.getElementById("login-modal").classList.add("active")

    // Cierra el item de iniciar sesión del dropdwon-profile si se inició sesión
    const profileDropdown = document.getElementById("profile-dropdown")
    if (profileDropdown.classList.contains("show")) {
        profileDropdown.classList.remove("show")
    }
}

function closeLoginModal() {
    document.getElementById("modal-overlay").classList.remove("active")
    document.getElementById("login-modal").classList.remove("active")
}

function openRegisterModal() {
    document.getElementById("modal-overlay").classList.add("active")
    document.getElementById("register-modal").classList.add("active")

    // Cierra el item de registrase del dropdwon-profile si se inició sesión
    const profileDropdown = document.getElementById("profile-dropdown")
    if (profileDropdown.classList.contains("show")) {
        profileDropdown.classList.remove("show")
    }
}

function closeRegisterModal() {
    document.getElementById("modal-overlay").classList.remove("active")
    document.getElementById("register-modal").classList.remove("active")
}

//ANIMACIÓN PARA MOVERSE ENTRE MODALES DE FORMULARIOS
function switchToRegister(event) {
    event.preventDefault()

    const loginModal = document.getElementById("login-modal")
    const registerModal = document.getElementById("register-modal")

    loginModal.classList.add("slide-out")

    setTimeout(() => {
        loginModal.classList.remove("active")
        loginModal.classList.remove("slide-out")
        registerModal.classList.add("active")
        registerModal.classList.add("slide-in")

        setTimeout(() => {
            registerModal.classList.remove("slide-in")
        }, 300)
    }, 300)
}

function switchToLogin(event) {
    event.preventDefault()

    const loginModal = document.getElementById("login-modal")
    const registerModal = document.getElementById("register-modal")

    registerModal.classList.add("slide-out")

    setTimeout(() => {
        registerModal.classList.remove("active")
        registerModal.classList.remove("slide-out")
        loginModal.classList.add("active")
        loginModal.classList.add("slide-in")

        setTimeout(() => {
            loginModal.classList.remove("slide-in")
        }, 300)
    }, 300)
}

// Función para cerrar sesión
function logout() {
    // Elimina los datos del usuario del localStorage
    localStorage.removeItem("user")

    // Actualiza la interfaz
    window.updateAuthUI()

    window.location.reload()
}

// Función para actualizar la interfaz según el estado de autenticación
window.updateAuthUI = () => {
    const userJson = localStorage.getItem("user")
    const loggedInSections = document.querySelectorAll(".logged-in-section")
    const loggedOutSections = document.querySelectorAll(".logged-out-section")
    const userNameElement = document.querySelector(".user-name")

    console.log("User JSON:", userJson)
    console.log("Logged in sections:", loggedInSections.length)
    console.log("Logged out sections:", loggedOutSections.length)
    console.log("User name element:", userNameElement)

    if (userJson) {
        try {
            // El usuario está autenticado
            const user = JSON.parse(userJson)
            console.log("Usuario autenticado:", user)

            loggedInSections.forEach((section) => {
                section.style.display = "block"
            })

            loggedOutSections.forEach((section) => {
                section.style.display = "none"
            })

            // Muestra el nombre del usuario iniciado
            if (userNameElement && user.nombre) {
                userNameElement.textContent = user.nombre
                console.log("Nombre de usuario mostrado:", user.nombre)
            }
        } catch (e) {
            console.error("Error al parsear los datos del usuario:", e)
            handleNotAuthenticated()
        }
    } else {
        console.log("Usuario no autenticado")
        handleNotAuthenticated()
    }
}

function handleNotAuthenticated() {
    const loggedInSections = document.querySelectorAll(".logged-in-section")
    const loggedOutSections = document.querySelectorAll(".logged-out-section")

    loggedInSections.forEach((section) => {
        section.style.display = "none"
    })

    loggedOutSections.forEach((section) => {
        section.style.display = "block"
    })
}

document.addEventListener("DOMContentLoaded", () => {

    window.updateAuthUI()

    const dropdown = document.querySelector(".dropdown")
    const dropdownToggle = document.querySelector(".dropdown-toggle")
    const dropdownMenu = document.querySelector(".dropdown-menu")

    if (dropdown && dropdownToggle && dropdownMenu) {
        dropdownToggle.addEventListener("click", (e) => {
            e.preventDefault()
            dropdown.classList.toggle("active")
        })

        dropdown.addEventListener("mouseleave", () => {
            if (!dropdown.classList.contains("active")) {
                dropdownMenu.style.display = "none"
                setTimeout(() => {
                    dropdownMenu.style.display = ""
                }, 100)
            }
        })

        document.addEventListener("click", (e) => {
            if (dropdown && !dropdown.contains(e.target)) {
                dropdown.classList.remove("active")
            }
        })
    }

    const profileButton = document.getElementById("profile-button")
    const profileDropdown = document.getElementById("profile-dropdown")

    if (profileButton && profileDropdown) {
        profileButton.addEventListener("click", (e) => {
            e.stopPropagation()
            profileDropdown.classList.toggle("show")
        })

        document.addEventListener("click", (e) => {
            if (profileDropdown && !profileButton.contains(e.target) && !profileDropdown.contains(e.target)) {
                profileDropdown.classList.remove("show")
            }
        })

        profileDropdown.addEventListener("click", (e) => {
            e.stopPropagation()
        })
    }

    const modalOverlay = document.getElementById("modal-overlay")

    // FORMAS PERMITADAS PARA CERRAR EL MODAL
    //si se hace click fuera
    if (modalOverlay) {
        modalOverlay.addEventListener("click", () => {
            closeLoginModal()
            closeRegisterModal()
        })
    }

    // Evita el cierre del modal dentro del modal
    document.querySelectorAll(".modal-content").forEach((content) => {
        content.addEventListener("click", (event) => {
            event.stopPropagation()
        })
    })

    // tecla esc
    document.addEventListener("keydown", (event) => {
        if (event.key === "Escape") {
            closeLoginModal()
            closeRegisterModal()
        }
    })


    // Para depuración de los enlaces en el menú desplegable funcionen correctamente
    const profileLinks = document.querySelectorAll("#profile-dropdown a")
    profileLinks.forEach((link) => {
        link.addEventListener("click", (e) => {

            e.stopPropagation()

            return true
        })
    })
})