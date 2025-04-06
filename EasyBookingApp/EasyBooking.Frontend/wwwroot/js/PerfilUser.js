async function checkEmailVerificationStatus(userId) {
    try {
        const response = await fetch(`https://localhost:7191/api/ApiUsuario/GetUserStatus?userId=${userId}`)
        if (response.ok) {
            const data = await response.json()

            // Actualización del la card de Información verificada
            const verificationStatus = document.querySelector(".d-flex.align-items-center.mb-3")
            if (data.isEmailVerified) {
                verificationStatus.innerHTML = `
                    <i class="bi bi-check-circle-fill text-success me-2"></i>
                    <span>
                                Dirección de correo electrónico
                                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="text-success" viewBox="0 0 16 16">
                                    <path d="M16 8A8 8 0 1 1 8 0a8 8 0 0 1 8 8zm-3.97-4.03a.75.75 0 0 0-1.06 0L7.25 9.69 5.03 7.47a.75.75 0 1 0-1.06 1.06l2.75 2.75a.75.75 0 0 0 1.06 0l4.25-4.25a.75.75 0 0 0 0-1.06z" />
                                </svg>
                            </span>
                `

                // Oculta el botón "Verificar", si ya se autenticó el correo
                const verificationBtn = document.getElementById("verification-btn")
                if (verificationBtn) {
                    verificationBtn.style.display = "none"
                    const verifiedBtn = document.createElement("button")
                    verifiedBtn.className = "btn btn-success px-4 py-2"
                    verifiedBtn.disabled = true
                    verifiedBtn.textContent = "Verificado"
                    verificationBtn.parentNode.replaceChild(verifiedBtn, verificationBtn)
                }

                const userJson = localStorage.getItem("user")
                if (userJson) {
                    const user = JSON.parse(userJson)
                    user.isEmailVerified = true
                    localStorage.setItem("user", JSON.stringify(user))
                }
            } else {
                verificationStatus.innerHTML = `
                    <i class="bi bi-x-circle-fill text-danger me-2"></i>
                    <span>
                                Todavía su cuenta de correo no ha sido validada
                                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="text-danger" viewBox="0 0 16 16">
                                    <path d="M16 8A8 8 0 1 1 8 0a8 8 0 0 1 8 8zM4.646 4.646a.5.5 0 0 1 .708 0L8 7.293l2.646-2.647a.5.5 0 1 1 .708.708L8.707 8l2.647 2.646a.5.5 0 0 1-.708.708L8 8.707l-2.646 2.647a.5.5 0 1 1-.708-.708L7.293 8 4.646 5.354a.5.5 0 0 1 0-.708z" />
                                </svg>
                    </span>
                `
            }
        }
    } catch (error) {
        console.error("Error checking verification status:", error)
    }
}

document.addEventListener("DOMContentLoaded", () => {
    // Scroll
    setTimeout(() => {
        window.scrollTo({
            top: 0,
            behavior: "auto",
        })
    }, 100)

    const userJson = localStorage.getItem("user")

    if (userJson) {
        try {
            const user = JSON.parse(userJson)

            checkEmailVerificationStatus(user.id)

            // Actualiza la información del perfil
            document.getElementById("profile-name").textContent = user.nombre
            document.getElementById("confirmed-info-title").textContent = `Información confirmada de ${user.nombre}`

            // Inicial del nombre en el avatar
            const avatarElement = document.getElementById("profile-avatar")
            if (avatarElement && user.nombre) {
                avatarElement.textContent = user.nombre.charAt(0).toUpperCase()
            }

            document.getElementById("user-fullname").textContent = `${user.nombre} ${user.apellido}`
            document.getElementById("user-email").textContent = user.email
            document.getElementById("user-username").textContent = user.username
            document.getElementById("user-phone").textContent = user.telefono

            const verificationBtn = document.getElementById("verification-btn")
            if (verificationBtn) {
                verificationBtn.addEventListener("click", () => {
                    sendVerificationEmail(user.email, user.id)
                })
            }

            setupActionButtons(user)
        } catch (e) {
            console.error("Error al parsear los datos del usuario:", e)
            window.location.href = "/"
        }
    } else {
        // Si no hay usuario autenticado, redirigir a la página principal
        window.location.href = "/"
    }
})

function setupActionButtons(user) {
    // Botón de editar perfil
    const editProfileBtn = document.getElementById("editProfileBtn")
    const editProfileModal = document.getElementById("editProfileModal")
    const closeEditModal = document.getElementById("closeEditModal")
    const editProfileForm = document.getElementById("editProfileForm")

    if (editProfileBtn) {
        editProfileBtn.addEventListener("click", () => {

            // El formulario aparece con los datos actuales del usuario
            document.getElementById("editNombre").value = user.nombre || ""
            document.getElementById("editApellido").value = user.apellido || ""
            document.getElementById("editUsername").value = user.username || ""
            document.getElementById("currentPassword").value = ""
            document.getElementById("editPassword").value = ""
            document.getElementById("password-error").style.display = "none"

            editProfileModal.style.display = "block"
        })
    }

    if (closeEditModal) {
        closeEditModal.addEventListener("click", () => {
            editProfileModal.style.display = "none"
        })
    }

    window.addEventListener("click", (event) => {
        if (event.target === editProfileModal) {
            editProfileModal.style.display = "none"
        }
    })

    if (editProfileForm) {
        editProfileForm.addEventListener("submit", (event) => {
            event.preventDefault()

            // Valores del formulario
            const nombre = document.getElementById("editNombre").value
            const apellido = document.getElementById("editApellido").value
            const username = document.getElementById("editUsername").value
            const currentPassword = document.getElementById("currentPassword").value
            const newPassword = document.getElementById("editPassword").value
            const passwordError = document.getElementById("password-error")

            passwordError.style.display = "none"

            // Validar campos obligatorios
            if (!nombre || !apellido || !username) {
                alert("Por favor, completa todos los campos requeridos.")
                return
            }

            // Solo verificar la contraseña actual si el usuario está intentando cambiar su contraseña
            if (newPassword) {
                if (!currentPassword) {
                    passwordError.textContent = "Debes ingresar tu contraseña actual para cambiarla."
                    passwordError.style.display = "block"
                    return
                }

                verifyCurrentPassword(user.id, currentPassword, (isValid) => {
                    if (isValid) {
                        updateUserProfile()
                    } else {
                        passwordError.textContent = "La contraseña actual es incorrecta."
                        passwordError.style.display = "block"
                    }
                })
            } else {
                updateUserProfile()
            }

            function updateUserProfile() {
                if (newPassword) {
                    if (!validatePassword(newPassword)) {
                        passwordError.textContent =
                            "La contraseña debe tener al menos 8 caracteres, incluyendo al menos 2 números y 2 letras."
                        passwordError.style.display = "block"
                        return
                    }
                }

                const updatedUser = {
                    id: user.id,
                    nombre: nombre,
                    apellido: apellido,
                    username: username,
                    email: user.email,
                    telefono: user.telefono,
                    isEmailVerified: user.isEmailVerified,
                }

                if (newPassword) {
                    updatedUser.password = newPassword
                    updatedUser.currentPassword = currentPassword
                }

                updateProfile(updatedUser)
            }
        })
    }

    // Función para verificar la contraseña actual
    function verifyCurrentPassword(userId, password, callback) {
        // URL de la API
        const apiUrl = "https://localhost:7191/api/ApiUsuario/VerifyPassword"

        //Solicitud para verificar la contraseña
        fetch(apiUrl, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify({
                userId: userId,
                password: password,
            }),
        })
            .then((response) => {
                if (!response.ok) {
                    throw new Error("Error al verificar la contraseña")
                }
                return response.json()
            })
            .then((data) => {
                callback(data.success)
            })
            .catch((error) => {
                console.error("Error:", error)
                callback(false)
            })
    }

    // Botón de eliminar perfil y modales
    const deleteProfileBtn = document.getElementById("deleteProfileBtn")
    const passwordVerificationModal = document.getElementById("passwordVerificationModal")
    const deleteConfirmationModal = document.getElementById("deleteConfirmationModal")
    const modalBackdrop = document.getElementById("modalBackdrop")
    const confirmDeleteBtn = document.getElementById("confirmDeleteBtn")
    const cancelDeleteBtn = document.getElementById("cancelDeleteBtn")
    const verifyPasswordBtn = document.getElementById("verifyPasswordBtn")
    const cancelPasswordBtn = document.getElementById("cancelPasswordBtn")
    const deletePasswordInput = document.getElementById("deletePassword")
    const passwordError = document.getElementById("passwordError")

    passwordVerificationModal.addEventListener("click", (event) => {
        event.stopPropagation()
    })

    deleteConfirmationModal.addEventListener("click", (event) => {
        event.stopPropagation()
    })

    // Modal de verificación
    deleteProfileBtn.addEventListener("click", () => {
        console.log("Botón de eliminar clickeado")
        passwordVerificationModal.style.display = "block"
        modalBackdrop.style.display = "block"
        deletePasswordInput.value = ""
        passwordError.style.display = "none"
        deletePasswordInput.classList.remove("shake")
    })

    cancelPasswordBtn.addEventListener("click", () => {
        passwordVerificationModal.style.display = "none"
        modalBackdrop.style.display = "none"
    })

    verifyPasswordBtn.addEventListener("click", () => {
        verifyPassword(user.id, deletePasswordInput.value)
    })

    deletePasswordInput.addEventListener("keypress", (e) => {
        if (e.key === "Enter") {
            e.preventDefault()
            verifyPassword(user.id, deletePasswordInput.value)
        }
    })

    cancelDeleteBtn.addEventListener("click", () => {
        deleteConfirmationModal.style.display = "none"
        modalBackdrop.style.display = "none"
    })

    confirmDeleteBtn.addEventListener("click", () => {
        requestAccountDeletion(user.id, deletePasswordInput.value)
    })

    function requestAccountDeletion(userId, password) {
        const confirmDeleteBtn = document.getElementById("confirmDeleteBtn")
        confirmDeleteBtn.disabled = true
        confirmDeleteBtn.innerHTML =
            '<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> Procesando...'

        // URL de la API
        const apiUrl = "https://localhost:7191/api/ApiUsuario/RequestAccountDeletion"

        // Solicitud para iniciar el proceso de eliminación de cuenta
        fetch(apiUrl, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify({
                userId: userId,
                password: password,
            }),
        })
            .then((response) => {
                if (!response.ok) {
                    throw new Error("Error al procesar la solicitud")
                }
                return response.json()
            })
            .then((data) => {

                document.getElementById("deleteConfirmationModal").style.display = "none"
                document.getElementById("modalBackdrop").style.display = "none"

                if (data.success) {

                    if (typeof Swal !== "undefined") {
                        Swal.fire({
                            title: "Solicitud enviada",
                            text: "Se ha enviado un correo electrónico con instrucciones para confirmar la eliminación de tu cuenta.",
                            icon: "info",
                            confirmButtonText: "Aceptar",
                        }).then(() => {

                            localStorage.removeItem("user")
                            window.location.href = "/"
                        })
                    } else {
                        alert("Se ha enviado un correo electrónico con instrucciones para confirmar la eliminación de tu cuenta.")

                        localStorage.removeItem("user")
                        window.location.href = "/"
                    }
                } else {
                    throw new Error(data.message || "Error al procesar la solicitud de eliminación de cuenta.")
                }

                confirmDeleteBtn.innerHTML = "Eliminar"
                confirmDeleteBtn.disabled = false
            })
            .catch((error) => {
                console.error("Error:", error)

                if (typeof Swal !== "undefined") {
                    Swal.fire({
                        title: "Error",
                        text: "Ocurrió un error al procesar tu solicitud. Por favor, intenta nuevamente más tarde.",
                        icon: "error",
                        confirmButtonText: "Aceptar",
                    })
                } else {
                    alert("Ocurrió un error al procesar tu solicitud. Por favor, intenta nuevamente más tarde.")
                }

                confirmDeleteBtn.innerHTML = "Eliminar"
                confirmDeleteBtn.disabled = false

                // Ocultar el modal de confirmación
                document.getElementById("deleteConfirmationModal").style.display = "none"
                document.getElementById("modalBackdrop").style.display = "none"
            })
    }

    modalBackdrop.addEventListener("click", (event) => {
        if (event.target === modalBackdrop) {
            if (passwordVerificationModal.style.display === "block") {
                passwordVerificationModal.style.display = "none"
            }
            if (deleteConfirmationModal.style.display === "block") {
                deleteConfirmationModal.style.display = "none"
            }
            modalBackdrop.style.display = "none"
        }
    })
}

// Función para verificar la contraseña
function verifyPassword(userId, password) {
    if (!password) {
        const passwordInput = document.getElementById("deletePassword")
        const passwordError = document.getElementById("passwordError")

        passwordError.textContent = "Por favor, ingresa tu contraseña."
        passwordError.style.display = "block"

        // Animación de error
        passwordInput.classList.remove("shake")
        void passwordInput.offsetWidth
        passwordInput.classList.add("shake")

        return
    }

    const verifyPasswordBtn = document.getElementById("verifyPasswordBtn")
    verifyPasswordBtn.disabled = true
    verifyPasswordBtn.innerHTML =
        '<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> Verificando...'

    // URL de la API
    const apiUrl = "https://localhost:7191/api/ApiUsuario/VerifyPassword"

    // Solicitud para verificar la contraseña
    fetch(apiUrl, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify({
            userId: userId,
            password: password,
        }),
    })
        .then((response) => {
            if (!response.ok) {
                throw new Error("Error al verificar la contraseña")
            }
            return response.json()
        })
        .then((data) => {
            verifyPasswordBtn.innerHTML = "Verificar"
            verifyPasswordBtn.disabled = false

            if (data.success) {
                document.getElementById("passwordVerificationModal").style.display = "none"

                document.getElementById("deleteConfirmationModal").style.display = "block"
            } else {
                const passwordInput = document.getElementById("deletePassword")
                const passwordError = document.getElementById("passwordError")

                passwordError.textContent = data.message || "Contraseña incorrecta. Por favor, intenta nuevamente."
                passwordError.style.display = "block"

                //Animación de error
                passwordInput.classList.remove("shake")
                void passwordInput.offsetWidth
                passwordInput.classList.add("shake")
            }
        })
        .catch((error) => {
            console.error("Error:", error)

            verifyPasswordBtn.innerHTML = "Verificar"
            verifyPasswordBtn.disabled = false

            const passwordError = document.getElementById("passwordError")
            passwordError.textContent = "Error al verificar la contraseña. Por favor, intenta nuevamente."
            passwordError.style.display = "block"

            const passwordInput = document.getElementById("deletePassword")
            passwordInput.classList.remove("shake")
            void passwordInput.offsetWidth
            passwordInput.classList.add("shake")
        })
}


//FUNCIÓN PARA AUTENTICACIÓN DE LA CONTRASEÑA SEGURA
function validatePassword(password) {
    if (!password) return false

    const hasMinLength = password.length >= 8
    const hasNumbers = (password.match(/\d/g) || []).length >= 2
    const hasLetters = (password.match(/[a-zA-Z]/g) || []).length >= 2

    return hasMinLength && hasNumbers && hasLetters
}

function updateProfile(updatedUser) {
    const updateProfileBtn = document.getElementById("updateProfileBtn")
    const passwordError = document.getElementById("password-error")

    const userJson = localStorage.getItem("user")
    if (userJson) {
        const user = JSON.parse(userJson)

        if (
            user.nombre === updatedUser.nombre &&
            user.apellido === updatedUser.apellido &&
            user.username === updatedUser.username &&
            !updatedUser.password
        ) {
            if (typeof Swal !== "undefined") {
                Swal.fire({
                    title: "Sin cambios",
                    text: "No se detectaron cambios para actualizar.",
                    icon: "info",
                    confirmButtonText: "Aceptar",
                })
            } else {
                alert("No se detectaron cambios para actualizar.")
            }

            return
        }
    }

    updateProfileBtn.disabled = true
    updateProfileBtn.innerHTML =
        '<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> Actualizando...'

    console.log("Enviando datos al servidor:", JSON.stringify(updatedUser))

    // URL de la API
    const apiUrl = "https://localhost:7191/api/ApiUsuario/UpdateProfile"

    // Enviar solicitud para actualizar el perfil
    fetch(apiUrl, {
        method: "PUT",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(updatedUser),
    })
        .then((response) => {
            console.log("Respuesta del servidor:", response.status)
            if (!response.ok) {
                return response.json().then((data) => {
                    console.log("Error del servidor:", data)
                    throw new Error(data.message || "Error al actualizar el perfil")
                })
            }
            return response.json()
        })
        .then((data) => {
            console.log("Datos de respuesta:", data)
            if (data.success) {
                const userJson = localStorage.getItem("user")
                if (userJson) {
                    const user = JSON.parse(userJson)
                    user.nombre = updatedUser.nombre
                    user.apellido = updatedUser.apellido
                    user.username = updatedUser.username
                    localStorage.setItem("user", JSON.stringify(user))
                }

                if (typeof Swal !== "undefined") {
                    Swal.fire({
                        title: "¡Perfil actualizado!",
                        text: "Tu perfil ha sido actualizado correctamente.",
                        icon: "success",
                        confirmButtonText: "Aceptar",
                    }).then(() => {
                        window.location.reload()
                    })
                } else {
                    alert("Tu perfil ha sido actualizado correctamente.")
                    window.location.reload()
                }
            } else {
                throw new Error(data.message || "Error al actualizar el perfil")
            }
        })
        .catch((error) => {
            console.error("Error:", error)

            if (error.message && error.message.includes("contraseña")) {
                passwordError.textContent = error.message
                passwordError.style.display = "block"
                passwordError.scrollIntoView({ behavior: "smooth", block: "center" })
            } else {
                if (typeof Swal !== "undefined") {
                    Swal.fire({
                        title: "Error al actualizar el perfil",
                        text: error.message || "Error al actualizar el perfil.",
                        icon: "error",
                        confirmButtonText: "Aceptar",
                    })
                } else {
                    alert("Ocurrió un error al actualizar el perfil: " + (error.message || "Error al actualizar el perfil."))
                }
            }

            updateProfileBtn.innerHTML = "Actualizar"
            updateProfileBtn.disabled = false
        })
}

function sendVerificationEmail(email, userId) {
    const verificationBtn = document.getElementById("verification-btn")
    verificationBtn.disabled = true
    verificationBtn.innerHTML =
        '<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> Enviando...'

    const apiUrl = "https://localhost:7191/api/ApiUsuario/SendVerificationEmail"

    // Solicitud para enviar correo de verificación
    fetch(`${apiUrl}?email=${encodeURIComponent(email)}&userId=${userId}`, {
        method: "POST",
    })
        .then((response) => {
            console.log("Respuesta:", response)
            if (!response.ok) {
                throw new Error("Error al enviar el correo de verificación")
            }
            return response.json()
        })
        .then((data) => {
            alert(
                "Se ha enviado un correo de verificación a tu dirección de correo electrónico. Por favor, revisa tu bandeja de entrada y sigue las instrucciones para verificar tu cuenta.",
            )
            verificationBtn.innerHTML = "Verificación"
            verificationBtn.disabled = false
        })
        .catch((error) => {
            console.error("Error:", error)
            alert("Ocurrió un error al enviar el correo de verificación. Por favor, intenta nuevamente más tarde.")
            verificationBtn.innerHTML = "Verificación"
            verificationBtn.disabled = false
        })
}