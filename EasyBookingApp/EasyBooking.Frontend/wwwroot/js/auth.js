// Este script Maneja la autenticación y registro de usuarios

const updateAuthUI = () => { }

document.addEventListener("DOMContentLoaded", () => {
    // Configuración para los formularios para envío AJAX
    setupLoginForm()
    setupRegisterForm()

    // Verifica si el usuario está autenticado y actualizar la información confirmada
    if (typeof updateAuthUI === "function") {
        updateAuthUI()
    }

})

//FORMULARIO LOGIN
function setupLoginForm() {
    const loginForm = document.getElementById("login-form")
    if (loginForm) {
        loginForm.addEventListener("submit", (e) => {
            e.preventDefault()

            const email = document.getElementById("email").value
            const password = document.getElementById("password").value

            // Validación campos
            if (!email || !password) {
                showLoginError("Por favor, completa todos los campos.")
                return
            }

            // Datos para enviar al API
            const loginData = {
                email: email,
                password: password,
            }

            // Solicitud al API
            fetch("https://localhost:7191/api/ApiUsuario/Login", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(loginData),
            })
                .then((response) => {
                    if (!response.ok) {
                        return response
                            .json()
                            .then((data) => {
                                throw new Error(data.message || "Error en la autenticación")
                            })
                            .catch((e) => {
                                // Si no se puede parsear el JSON, se le pasa el mensaje genérico
                                throw new Error("Error en la autenticación, verifica las credenciales.")
                            })
                    }
                    return response.json()
                })
                .then((data) => {
                    // Se guardan los datos del usuario en localStorage
                    localStorage.setItem("user", JSON.stringify(data.data))

                    closeLoginModal()

                    window.location.reload()
                })
                .catch((error) => {
                    showLoginError(error.message || "Credenciales inválidas. Por favor, verifica tu email y contraseña.")
                    console.error("Error:", error)
                })
        })
    }
}


// Función para validar la contraseña
function validatePassword(password) {
    if (!password) return false

    const hasMinLength = password.length >= 8
    const hasNumbers = (password.match(/\d/g) || []).length >= 2
    const hasLetters = (password.match(/[a-zA-Z]/g) || []).length >= 2

    return hasMinLength && hasNumbers && hasLetters
}

//FORMULARIO REGISTER
function setupRegisterForm() {
    const registerForm = document.getElementById("register-form")
    if (registerForm) {
        registerForm.addEventListener("submit", (e) => {
            e.preventDefault()

            // Valores del formulario
            const nombre = document.getElementById("register-nombre").value
            const apellido = document.getElementById("register-apellido").value
            const email = document.getElementById("register-email").value
            const username = document.getElementById("register-username").value
            const telefono = document.getElementById("register-telefono").value
            const password = document.getElementById("register-password").value
            const confirmPassword = document.getElementById("register-confirm-password").value

            // Validar campos
            if (!nombre || !apellido || !email || !username || !telefono || !password || !confirmPassword) {
                showRegisterError("Por favor, completa todos los campos.")
                return
            }

            // Validar que las contraseñas coincidan
            if (password !== confirmPassword) {
                showRegisterError("Las contraseñas no coinciden.")
                return
            }

            // Validar la fortaleza de la contraseña
            if (!validatePassword(password)) {
                showRegisterError("La contraseña debe tener al menos 8 caracteres, incluyendo al menos 2 números y 2 letras.")
                return
            }

            // Datos para enviar al API
            const userData = {
                nombre: nombre,
                apellido: apellido,
                email: email,
                username: username,
                telefono: telefono,
                password: password,
            }

            // Solicitud al API
            fetch("https://localhost:7191/api/ApiUsuario", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(userData),
            })
                .then((response) => {
                    
                    if (!response.ok) {
                        
                        return response.text().then((text) => {
                            let errorMsg = "Error al registrar usuario"
                            try {
                                
                                if (text) {
                                    const data = JSON.parse(text)
                                    errorMsg = data.message || errorMsg
                                }
                            } catch (e) {
                                // Si el JSON no es válido, usar el texto como un mensaje genérico
                                errorMsg = text || errorMsg
                            }
                            throw new Error(errorMsg)
                        })
                    }

                    // Si la respuesta es exitosa, se inteta parsear el JSON
                    return response.text().then((text) => {
                        if (!text) {
                            return { message: "Usuario registrado correctamente" }
                        }
                        try {
                            return JSON.parse(text)
                        } catch (e) {
                            return { message: "Usuario registrado correctamente" }
                        }
                    })
                })
                .then((data) => {
                    
                    alert(data.message || "Usuario registrado correctamente. Por favor, inicia sesión.")

                    closeRegisterModal()
                    openLoginModal()

                    registerForm.reset() // Limpiar el formulario
                })
                .catch((error) => {
                    showRegisterError(error.message || "Error al registrar usuario. Por favor, intenta nuevamente.")
                    console.error("Error:", error)
                })
        })
    }
}

function showLoginError(message) {
    const errorElement = document.getElementById("login-error-message")
    if (errorElement) {
        errorElement.textContent = message
        errorElement.style.display = "block"
    }
}

function showRegisterError(message) {
    const errorElement = document.getElementById("register-error-message")
    if (errorElement) {
        errorElement.textContent = message
        errorElement.style.display = "block"
    }
}