document.addEventListener("DOMContentLoaded", () => {
    const userIdElement = document.getElementById("userId")

    if (!userIdElement) {
        console.warn("No se encontró el elemento #userId. Se omite la carga de datos de usuario.")
        return // Salimos sin hacer nada
    }

    const userId = userIdElement.value

    if (userId) {
        fetchUserData(userId)
    } else {
        console.warn("El elemento #userId no tiene valor asignado.")
    }

    // Configurar el botón de verificación de email
    const verificationBtn = document.getElementById("verification-btn")
    if (verificationBtn) {
        verificationBtn.addEventListener("click", () => {
            requestEmailVerification()
        })
    }
})

// Variable global para almacenar los datos del usuario
let userData = null

function fetchUserData(userId) {
    fetch(`/Usuarios/GetUserData?id=${userId}`, {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
        },
    })
        .then((response) => {
            if (!response.ok) {
                throw new Error("Error al obtener los datos del usuario")
            }
            return response.json()
        })
        .then((response) => {
            if (response.success && response.data) {
                userData = response.data
                updateUserInterface(userData)
                setupActionButtons(userData)
            } else {
                throw new Error(response.message || "Error al obtener los datos del usuario")
            }
        })
        .catch((error) => {
            console.error("Error:", error)
            showToast("Error al cargar los datos del usuario: " + error.message, "error")
        })
}

function updateUserInterface(user) {
    if (!user) return

    // Actualiza la información del perfil
    const profileNameElement = document.getElementById("profile-name")
    if (profileNameElement) profileNameElement.textContent = user.nombre || ""

    const confirmedInfoTitleElement = document.getElementById("confirmed-info-title")
    if (confirmedInfoTitleElement)
        confirmedInfoTitleElement.textContent = `Información confirmada de ${user.nombre || ""}`

    // Nombre en el avatar (Solo la inicial)
    const avatarElement = document.getElementById("profile-avatar")
    if (avatarElement && user.nombre) {
        avatarElement.textContent = (user.nombre.charAt(0) || "").toUpperCase()
    }

    const userFullnameElement = document.getElementById("user-fullname")
    if (userFullnameElement) userFullnameElement.textContent = `${user.nombre || ""} ${user.apellido || ""}`

    const userEmailElement = document.getElementById("user-email")
    if (userEmailElement) userEmailElement.textContent = user.email || ""

    const userPhoneElement = document.getElementById("user-phone")
    if (userPhoneElement) userPhoneElement.textContent = user.telefono || ""
}

function setupActionButtons(user) {
    if (!user) return

    // Botón de editar perfil
    const editProfileBtn = document.getElementById("editProfileBtn")
    const editProfileModal = document.getElementById("editProfileModal")
    const closeEditModal = document.getElementById("closeEditModal")
    const editProfileForm = document.getElementById("editProfileForm")
    const modalBackdrop = document.getElementById("modalBackdrop")

    if (!modalBackdrop) {
        const backdropElement = document.createElement("div")
        backdropElement.id = "modalBackdrop"
        backdropElement.className = "modal-backdrop"
        backdropElement.style.display = "none"
        document.body.appendChild(backdropElement)
    }

    if (editProfileBtn) {
        editProfileBtn.addEventListener("click", () => {
            // El formulario aparece con los datos actuales del usuario
            const editNombreElement = document.getElementById("editNombre")
            if (editNombreElement) editNombreElement.value = user.nombre || ""

            const editApellidoElement = document.getElementById("editApellido")
            if (editApellidoElement) editApellidoElement.value = user.apellido || ""

            const editEmailElement = document.getElementById("editEmail")
            if (editEmailElement) editEmailElement.value = user.email || ""

            const editTelefonoElement = document.getElementById("editTelefono")
            if (editTelefonoElement) editTelefonoElement.value = user.telefono || ""

            const currentPasswordElement = document.getElementById("currentPassword")
            if (currentPasswordElement) currentPasswordElement.value = ""

            const editPasswordElement = document.getElementById("editPassword")
            if (editPasswordElement) editPasswordElement.value = ""

            const passwordErrorElement = document.getElementById("password-error")
            if (passwordErrorElement) passwordErrorElement.style.display = "none"

            if (editProfileModal) editProfileModal.style.display = "block"

            const backdropElement = document.getElementById("modalBackdrop")
            if (backdropElement) backdropElement.style.display = "block"
        })
    }

    if (closeEditModal) {
        closeEditModal.addEventListener("click", () => {
            if (editProfileModal) editProfileModal.style.display = "none"

            const backdropElement = document.getElementById("modalBackdrop")
            if (backdropElement) backdropElement.style.display = "none"
        })
    }

    window.addEventListener("click", (event) => {
        const backdropElement = document.getElementById("modalBackdrop")
        if (event.target === backdropElement) {
            if (editProfileModal) editProfileModal.style.display = "none"

            const passwordVerificationModal = document.getElementById("passwordVerificationModal")
            if (passwordVerificationModal) passwordVerificationModal.style.display = "none"

            const deleteConfirmationModal = document.getElementById("deleteConfirmationModal")
            if (deleteConfirmationModal) deleteConfirmationModal.style.display = "none"

            if (backdropElement) backdropElement.style.display = "none"
        }
    })

    // Configurar el formulario de edición de perfil
    if (editProfileForm) {
        editProfileForm.addEventListener("submit", (event) => {
            event.preventDefault()

            const editNombreElement = document.getElementById("editNombre")
            const editApellidoElement = document.getElementById("editApellido")
            const editEmailElement = document.getElementById("editEmail")
            const editTelefonoElement = document.getElementById("editTelefono")
            const currentPasswordElement = document.getElementById("currentPassword")
            const editPasswordElement = document.getElementById("editPassword")
            const updateProfileBtn = document.getElementById("updateProfileBtn")
            const passwordError = document.getElementById("password-error")

            const nombre = editNombreElement ? editNombreElement.value : ""
            const apellido = editApellidoElement ? editApellidoElement.value : ""
            const email = editEmailElement ? editEmailElement.value : ""
            const telefono = editTelefonoElement ? editTelefonoElement.value : ""
            const currentPassword = currentPasswordElement ? currentPasswordElement.value : ""
            const newPassword = editPasswordElement ? editPasswordElement.value : ""

            if (passwordError) passwordError.style.display = "none"

            // Verificar si hay cambios
            if (
                nombre === user.nombre &&
                apellido === user.apellido &&
                email === user.email &&
                telefono === user.telefono &&
                !newPassword
            ) {
                if (typeof Swal !== "undefined") {
                    Swal.fire({
                        title: "Sin cambios",
                        text: "No se detectaron cambios para actualizar.",
                        icon: "info",
                        confirmButtonColor: "#f8345c",
                    })
                } else {
                    showToast("No se detectaron cambios para actualizar.", "info")
                }

                // Cerrar el modal
                if (editProfileModal) editProfileModal.style.display = "none"
                if (modalBackdrop) modalBackdrop.style.display = "none"

                return
            }

            if (!nombre || !apellido || !email) {
                if (typeof Swal !== "undefined") {
                    Swal.fire({
                        title: "Campos requeridos",
                        text: "Por favor, completa los campos requeridos: nombre, apellido y email.",
                        icon: "warning",
                        confirmButtonColor: "#f8345c",
                    })
                } else {
                    showToast("Por favor, completa los campos requeridos: nombre, apellido y email.", "warning")
                }
                return
            }

            // Deshabilitar el botón mientras se procesa
            if (updateProfileBtn) {
                updateProfileBtn.disabled = true
                updateProfileBtn.innerHTML =
                    '<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> Actualizando...'
            }

            // Solo verificar la contraseña actual si el usuario está intentando cambiar su contraseña
            if (newPassword) {
                if (!currentPassword) {
                    if (passwordError) {
                        passwordError.textContent = "Debes ingresar tu contraseña actual para cambiarla."
                        passwordError.style.display = "block"
                    }

                    if (updateProfileBtn) {
                        updateProfileBtn.disabled = false
                        updateProfileBtn.innerHTML = "Actualizar"
                    }

                    return
                }

                verifyCurrentPassword(user.id, currentPassword, (isValid) => {
                    if (isValid) {
                        updateUserProfile()
                    } else {
                        if (passwordError) {
                            passwordError.textContent = "La contraseña actual es incorrecta."
                            passwordError.style.display = "block"
                        }

                        if (updateProfileBtn) {
                            updateProfileBtn.disabled = false
                            updateProfileBtn.innerHTML = "Actualizar"
                        }
                    }
                })
            } else {
                updateUserProfile()
            }

            function updateUserProfile() {
                if (newPassword && !validatePassword(newPassword)) {
                    if (passwordError) {
                        passwordError.textContent =
                            "La contraseña debe tener al menos 8 caracteres, incluyendo al menos 2 números y 2 letras."
                        passwordError.style.display = "block"
                    }

                    if (updateProfileBtn) {
                        updateProfileBtn.disabled = false
                        updateProfileBtn.innerHTML = "Actualizar"
                    }

                    return
                }

                const updatedUser = {
                    id: user.id,
                    nombre: nombre,
                    apellido: apellido,
                    email: email,
                    telefono: telefono,
                    passwordActual: currentPassword || null,
                    passwordNueva: newPassword || null,
                }

                fetch("/Usuarios/ActualizarUsuario", {
                    method: "PUT",
                    headers: {
                        "Content-Type": "application/json",
                    },
                    body: JSON.stringify(updatedUser),
                })
                    .then((response) => {
                        if (!response.ok) {
                            throw new Error(`Error de servidor: ${response.status}`)
                        }
                        return response.json()
                    })
                    .then((data) => {
                        if (updateProfileBtn) {
                            updateProfileBtn.innerHTML = "Actualizar"
                            updateProfileBtn.disabled = false
                        }

                        if (data.success) {
                            // Actualizar los datos del usuario en memoria
                            user.nombre = nombre
                            user.apellido = apellido
                            user.email = email
                            user.telefono = telefono

                            // Cerrar el modal
                            if (editProfileModal) editProfileModal.style.display = "none"
                            if (modalBackdrop) modalBackdrop.style.display = "none"

                            // Mostrar mensaje de éxito
                            if (typeof Swal !== "undefined") {
                                Swal.fire({
                                    title: "¡Perfil actualizado!",
                                    text: "Tu perfil ha sido actualizado correctamente.",
                                    icon: "success",
                                    confirmButtonColor: "#f8345c",
                                }).then(() => {
                                    // Recargar la página para mostrar los cambios
                                    window.location.reload()
                                })
                            } else {
                                showToast("Tu perfil ha sido actualizado correctamente.", "success")
                                setTimeout(() => {
                                    window.location.reload()
                                }, 1500)
                            }
                        } else {
                            throw new Error(data.message || "Error al actualizar el perfil")
                        }
                    })
                    .catch((error) => {
                        console.error("Error:", error)

                        if (updateProfileBtn) {
                            updateProfileBtn.innerHTML = "Actualizar"
                            updateProfileBtn.disabled = false
                        }

                        if (error.message && error.message.includes("contraseña") && passwordError) {
                            passwordError.textContent = error.message
                            passwordError.style.display = "block"
                        } else {
                            if (typeof Swal !== "undefined") {
                                Swal.fire({
                                    title: "Error",
                                    text: "No se pudo actualizar el perfil. Por favor, verifica los datos e intenta nuevamente.",
                                    icon: "error",
                                    confirmButtonColor: "#f8345c",
                                })
                            } else {
                                showToast("Error al actualizar el perfil: " + error.message, "error")
                            }
                        }
                    })
            }
        })
    }

    // Botón de eliminar perfil y modales
    const deleteProfileBtn = document.getElementById("deleteProfileBtn")
    const passwordVerificationModal = document.getElementById("passwordVerificationModal")
    const deleteConfirmationModal = document.getElementById("deleteConfirmationModal")
    const confirmDeleteBtn = document.getElementById("confirmDeleteBtn")
    const cancelDeleteBtn = document.getElementById("cancelDeleteBtn")
    const verifyPasswordBtn = document.getElementById("verifyPasswordBtn")
    const cancelPasswordBtn = document.getElementById("cancelPasswordBtn")
    const deletePasswordInput = document.getElementById("deletePassword")
    const passwordError = document.getElementById("passwordError")

    if (passwordVerificationModal) {
        passwordVerificationModal.addEventListener("click", (event) => {
            event.stopPropagation()
        })
    }

    if (deleteConfirmationModal) {
        deleteConfirmationModal.addEventListener("click", (event) => {
            event.stopPropagation()
        })
    }

    if (deleteProfileBtn) {
        deleteProfileBtn.addEventListener("click", () => {
            if (passwordVerificationModal) passwordVerificationModal.style.display = "block"

            const backdropElement = document.getElementById("modalBackdrop")
            if (backdropElement) backdropElement.style.display = "block"

            if (deletePasswordInput) deletePasswordInput.value = ""
            if (passwordError) passwordError.style.display = "none"
            if (deletePasswordInput) deletePasswordInput.classList.remove("shake")
        })
    }

    if (cancelPasswordBtn) {
        cancelPasswordBtn.addEventListener("click", () => {
            if (passwordVerificationModal) passwordVerificationModal.style.display = "none"

            const backdropElement = document.getElementById("modalBackdrop")
            if (backdropElement) backdropElement.style.display = "none"
        })
    }

    if (verifyPasswordBtn) {
        verifyPasswordBtn.addEventListener("click", () => {
            if (deletePasswordInput) {
                verifyPassword(user.id, deletePasswordInput.value)
            }
        })
    }

    if (deletePasswordInput) {
        deletePasswordInput.addEventListener("keypress", (e) => {
            if (e.key === "Enter") {
                e.preventDefault()
                verifyPassword(user.id, deletePasswordInput.value)
            }
        })
    }

    if (cancelDeleteBtn) {
        cancelDeleteBtn.addEventListener("click", () => {
            if (deleteConfirmationModal) deleteConfirmationModal.style.display = "none"

            const backdropElement = document.getElementById("modalBackdrop")
            if (backdropElement) backdropElement.style.display = "none"
        })
    }

    if (confirmDeleteBtn) {
        confirmDeleteBtn.addEventListener("click", () => {
            requestAccountDeletion()
        })
    }
}

function verifyCurrentPassword(userId, password, callback) {
    const verifyDto = {
        usuarioId: userId,
        password: password,
    }

    fetch("/Usuarios/VerificarContrasena", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(verifyDto),
    })
        .then((response) => response.json())
        .then((data) => {
            callback(data.success)
        })
        .catch((error) => {
            console.error("Error:", error)
            callback(false)
        })
}

function verifyPassword(userId, password) {
    if (!password) {
        const passwordInput = document.getElementById("deletePassword")
        const passwordError = document.getElementById("passwordError")

        if (passwordError) {
            passwordError.textContent = "Por favor, ingresa tu contraseña."
            passwordError.style.display = "block"
        }

        // Animación de error
        if (passwordInput) {
            passwordInput.classList.remove("shake")
            void passwordInput.offsetWidth
            passwordInput.classList.add("shake")
        }

        return
    }

    const verifyPasswordBtn = document.getElementById("verifyPasswordBtn")
    if (verifyPasswordBtn) {
        verifyPasswordBtn.disabled = true
        verifyPasswordBtn.innerHTML =
            '<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> Verificando...'
    }

    const verifyDto = {
        usuarioId: userId,
        password: password,
    }

    fetch("/Usuarios/VerificarContrasena", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(verifyDto),
    })
        .then((response) => response.json())
        .then((data) => {
            if (verifyPasswordBtn) {
                verifyPasswordBtn.innerHTML = "Verificar"
                verifyPasswordBtn.disabled = false
            }

            if (data.success) {
                const passwordVerificationModal = document.getElementById("passwordVerificationModal")
                if (passwordVerificationModal) passwordVerificationModal.style.display = "none"

                const deleteConfirmationModal = document.getElementById("deleteConfirmationModal")
                if (deleteConfirmationModal) deleteConfirmationModal.style.display = "block"
            } else {
                const passwordInput = document.getElementById("deletePassword")
                const passwordError = document.getElementById("passwordError")

                if (passwordError) {
                    passwordError.textContent = data.message || "Contraseña incorrecta. Por favor, intenta nuevamente."
                    passwordError.style.display = "block"
                }

                if (passwordInput) {
                    passwordInput.classList.remove("shake")
                    void passwordInput.offsetWidth
                    passwordInput.classList.add("shake")
                }
            }
        })
        .catch((error) => {
            console.error("Error:", error)

            if (verifyPasswordBtn) {
                verifyPasswordBtn.innerHTML = "Verificar"
                verifyPasswordBtn.disabled = false
            }

            const passwordError = document.getElementById("passwordError")
            if (passwordError) {
                passwordError.textContent = "Error al verificar la contraseña. Por favor, intenta nuevamente."
                passwordError.style.display = "block"
            }

            const passwordInput = document.getElementById("deletePassword")
            if (passwordInput) {
                passwordInput.classList.remove("shake")
                void passwordInput.offsetWidth // Forzar reflow
                passwordInput.classList.add("shake")
            }
        })
}

function requestAccountDeletion() {
    const confirmDeleteBtn = document.getElementById("confirmDeleteBtn")
    if (confirmDeleteBtn) {
        confirmDeleteBtn.disabled = true
        confirmDeleteBtn.innerHTML =
            '<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> Procesando...'
    }

    fetch("/Usuarios/SolicitarEliminacion", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
    })
        .then((response) => {
            if (!response.ok) {
                if (response.status === 404) {
                    throw new Error("La ruta para eliminar la cuenta no fue encontrada. Contacta al administrador.")
                }
                return response.text().then((text) => {
                    try {
                        const data = JSON.parse(text)
                        throw new Error(data.message || `Error ${response.status}: ${response.statusText}`)
                    } catch (e) {
                        throw new Error(`Error ${response.status}: ${response.statusText}`)
                    }
                })
            }
            return response.text().then((text) => {
                try {
                    return JSON.parse(text)
                } catch (e) {
                    return { success: true, message: "Solicitud procesada correctamente" }
                }
            })
        })
        .then((data) => {
            const deleteConfirmationModal = document.getElementById("deleteConfirmationModal")
            if (deleteConfirmationModal) deleteConfirmationModal.style.display = "none"

            const modalBackdrop = document.getElementById("modalBackdrop")
            if (modalBackdrop) modalBackdrop.style.display = "none"

            // Mostrar mensaje de éxito con SweetAlert2
            if (typeof Swal !== "undefined") {
                Swal.fire({
                    title: "Solicitud enviada",
                    html: `
            <div class="text-center">
              <div class="mb-4">
                <svg xmlns="http://www.w3.org/2000/svg" width="80" height="80" fill="#f8345c" class="bi bi-envelope-check" viewBox="0 0 16 16">
                  <path d="M2 2a2 2 0 0 0-2 2v8.01A2 2 0 0 0 2 14h5.5a.5.5 0 0 0 0-1H2a1 1 0 0 1-.966-.741l5.64-3.471L8 9.583l7-4.2V8.5a.5 0 0 0 1 0V4a2 2 0 0 0-2-2H2Zm3.708 6.208L1 11.105V5.383l4.708 2.825ZM1 4.217V4a1 1 0 0 1 1-1h12a1 1 0 0 1 1 1v.217l-7 4.2-7-4.2Z"/>
                  <path d="M16 12.5a3.5 3.5 0 1 1-7 0 3.5 3.5 0 0 1 7 0Zm-1.993-1.679a.5.5 0 0 0-.686.172l-1.17 1.95-.547-.547a.5.5 0 0 0-.708.708l.774.773a.75.75 0 0 0 1.174-.144l1.335-2.226a.5.5 0 0 0-.172-.686Z"/>
                </svg>
              </div>
              <p>Se ha enviado un correo electrónico con instrucciones para confirmar la eliminación de tu cuenta.</p>
              <p class="text-muted mt-2">Revisa tu bandeja de entrada para completar el proceso.</p>
            </div>
          `,
                    icon: false,
                    confirmButtonColor: "#f8345c",
                    allowOutsideClick: false,
                }).then(() => {
                    // Cerrar sesión y redirigir a la página principal
                    window.location.href = "/"
                })
            } else {
                alert("Se ha enviado un correo electrónico con instrucciones para confirmar la eliminación de tu cuenta.")
                window.location.href = "/"
            }
        })
        .catch((error) => {
            console.error("Error:", error)

            if (typeof Swal !== "undefined") {
                Swal.fire({
                    title: "Error",
                    text:
                        error.message ||
                        "No se pudo procesar la solicitud de eliminación. Por favor, intenta nuevamente más tarde.",
                    icon: "error",
                    confirmButtonColor: "#f8345c",
                })
            } else {
                showToast("Error al procesar la solicitud de eliminación: " + error.message, "error")
            }

            if (confirmDeleteBtn) {
                confirmDeleteBtn.innerHTML = "Eliminar"
                confirmDeleteBtn.disabled = false
            }

            const deleteConfirmationModal = document.getElementById("deleteConfirmationModal")
            if (deleteConfirmationModal) deleteConfirmationModal.style.display = "none"

            const modalBackdrop = document.getElementById("modalBackdrop")
            if (modalBackdrop) modalBackdrop.style.display = "none"
        })
}

function requestEmailVerification() {
    const verificationBtn = document.getElementById("verification-btn")
    if (verificationBtn) {
        verificationBtn.disabled = true
        verificationBtn.innerHTML =
            '<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> Enviando...'
    }

    fetch("/Usuarios/VerificarEmail", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
    })
        .then((response) => {
            if (!response.ok) {
                throw new Error(`Error ${response.status}: ${response.statusText}`)
            }
            return response.json()
        })
        .then((data) => {
            if (verificationBtn) {
                verificationBtn.disabled = false
                verificationBtn.innerHTML = "Verificación"
            }

            if (data.success) {
                if (typeof Swal !== "undefined") {
                    Swal.fire({
                        title: "Correo enviado",
                        html: `
              <div class="text-center">
                <div class="mb-4">
                  <svg xmlns="http://www.w3.org/2000/svg" width="80" height="80" fill="#4CAF50" class="bi bi-envelope-check" viewBox="0 0 16 16">
                    <path d="M2 2a2 2 0 0 0-2 2v8.01A2 2 0 0 0 2 14h5.5a.5.5 0 0 0 0-1H2a1 1 0 0 1-.966-.741l5.64-3.471L8 9.583l7-4.2V8.5a.5 0 0 0 1 0V4a2 2 0 0 0-2-2H2Zm3.708 6.208L1 11.105V5.383l4.708 2.825ZM1 4.217V4a1 1 0 0 1 1-1h12a1 1 0 0 1 1 1v.217l-7 4.2-7-4.2Z"/>
                    <path d="M16 12.5a3.5 3.5 0 1 1-7 0 3.5 3.5 0 0 1 7 0Zm-1.993-1.679a.5.5 0 0 0-.686.172l-1.17 1.95-.547-.547a.5.5 0 0 0-.708.708l.774.773a.75.75 0 0 0 1.174-.144l1.335-2.226a.5.5 0 0 0-.172-.686Z"/>
                  </svg>
                </div>
                <p>Se ha enviado un correo electrónico con instrucciones para verificar tu cuenta.</p>
                <p class="text-muted mt-2">Revisa tu bandeja de entrada y sigue las instrucciones para completar la verificación.</p>
              </div>
            `,
                        icon: false,
                        confirmButtonColor: "#4CAF50",
                    })
                } else {
                    showToast("Se ha enviado un correo electrónico con instrucciones para verificar tu cuenta.", "success")
                }
            } else {
                throw new Error(data.message || "Error al enviar el correo de verificación")
            }
        })
        .catch((error) => {
            console.error("Error:", error)

            if (verificationBtn) {
                verificationBtn.disabled = false
                verificationBtn.innerHTML = "Verificación"
            }

            if (typeof Swal !== "undefined") {
                Swal.fire({
                    title: "Error",
                    text:
                        error.message || "No se pudo enviar el correo de verificación. Por favor, intenta nuevamente más tarde.",
                    icon: "error",
                    confirmButtonColor: "#f8345c",
                })
            } else {
                showToast("Error al enviar el correo de verificación: " + error.message, "error")
            }
        })
}

function validatePassword(password) {
    if (!password) return false

    const hasMinLength = password.length >= 8
    const hasNumbers = (password.match(/\d/g) || []).length >= 2
    const hasLetters = (password.match(/[a-zA-Z]/g) || []).length >= 2

    return hasMinLength && hasNumbers && hasLetters
}

function showToast(message, type = "info") {
    // Primero intentamos usar SweetAlert2 si está disponible
    if (typeof Swal !== "undefined") {
        const Toast = Swal.mixin({
            toast: true,
            position: "top-end",
            showConfirmButton: false,
            timer: 3000,
            timerProgressBar: true,
        })

        Toast.fire({
            icon: type,
            title: message,
        })
        return
    }

    // Si no hay un contenedor de toast, lo creamos
    let toastContainer = document.getElementById("toast-container")

    if (!toastContainer) {
        toastContainer = document.createElement("div")
        toastContainer.id = "toast-container"
        toastContainer.style.position = "fixed"
        toastContainer.style.top = "20px"
        toastContainer.style.right = "20px"
        toastContainer.style.zIndex = "9999"
        document.body.appendChild(toastContainer)
    }

    const toast = document.createElement("div")
    toast.style.minWidth = "250px"
    toast.style.margin = "10px 0"
    toast.style.padding = "15px"
    toast.style.borderRadius = "4px"
    toast.style.boxShadow = "0 2px 10px rgba(0,0,0,0.2)"
    toast.style.opacity = "0"
    toast.style.transition = "opacity 0.3s ease"

    // Establecer color según el tipo
    if (type === "success") {
        toast.style.backgroundColor = "#4CAF50"
        toast.style.color = "white"
    } else if (type === "error") {
        toast.style.backgroundColor = "#F44336"
        toast.style.color = "white"
    } else if (type === "warning") {
        toast.style.backgroundColor = "#FF9800"
        toast.style.color = "white"
    } else {
        toast.style.backgroundColor = "#2196F3"
        toast.style.color = "white"
    }

    toast.textContent = message
    toastContainer.appendChild(toast)

    // Mostrar el toast
    setTimeout(() => {
        toast.style.opacity = "1"
    }, 10)

    // Ocultar y eliminar el toast después de un tiempo
    setTimeout(() => {
        toast.style.opacity = "0"
        setTimeout(() => {
            toast.remove()
        }, 300)
    }, 3000)
}
