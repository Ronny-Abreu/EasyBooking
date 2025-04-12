// Variables globales
let userSessionEmail = ""
let userSessionId = 0

// Función que abre el modal de recuperación de contraseña desde el perfil
function openForgotPasswordFromProfile() {
    const editProfileModal = document.getElementById("editProfileModal")
    if (editProfileModal) {
        editProfileModal.style.display = "none"
    }

    document.getElementById("modal-overlay").classList.add("active")
    document.getElementById("login-modal").classList.add("active")

    setTimeout(() => {
        document.getElementById("login-form-container").style.display = "none"
        document.getElementById("forgot-password-container").style.display = "block"
        document.getElementById("reset-password-container").style.display = "none"
        document.getElementById("forgot-password-message").style.display = "none"

        // Obtener datos del usuario en sesión
        const userJson = localStorage.getItem("user")
        if (userJson) {
            try {
                const user = JSON.parse(userJson)
                userSessionEmail = user.email || ""
                userSessionId = user.id || 0

                // Establece el correo del usuario en sesión y deshabilitar el campo
                const emailInput = document.getElementById("forgot-email")
                if (emailInput) {
                    emailInput.value = userSessionEmail
                    emailInput.readOnly = true
                    emailInput.disabled = true
                    emailInput.style.backgroundColor = "#f0f0f0"
                    emailInput.style.cursor = "not-allowed"
                }

                if (!document.getElementById("from-profile-flag")) {
                    const hiddenField = document.createElement("input")
                    hiddenField.type = "hidden"
                    hiddenField.id = "from-profile-flag"
                    hiddenField.name = "from-profile-flag"
                    hiddenField.value = "true"
                    document.getElementById("forgot-password-form").appendChild(hiddenField)
                } else {
                    document.getElementById("from-profile-flag").value = "true"
                }

                showMessage(
                    "forgot-password-message",
                    "Enviaremos un código de verificación a tu correo electrónico registrado.",
                    "info",
                )
            } catch (e) {
                console.error("Error al parsear los datos del usuario:", e)
                document.getElementById("forgot-email").value = ""
            }
        } else {
            document.getElementById("forgot-email").value = ""
        }
    }, 100)
}

document.addEventListener("DOMContentLoaded", () => {
    // Obtener datos del usuario en sesión
    const userJson = localStorage.getItem("user")
    if (userJson) {
        try {
            const user = JSON.parse(userJson)
            userSessionEmail = user.email || ""
            userSessionId = user.id || 0
        } catch (e) {
            console.error("Error al parsear los datos del usuario:", e)
        }
    }

    // Abrir modal y formulario desde el perfil
    const forgotPasswordLinkProfile = document.getElementById("forgot-password-link-profile")
    if (forgotPasswordLinkProfile) {
        forgotPasswordLinkProfile.addEventListener("click", (e) => {
            e.preventDefault()
            openForgotPasswordFromProfile()
        })
    }

    // Modificar el formulario de recuperación de contraseña cuando se abre desde el perfil
    const forgotPasswordForm = document.getElementById("forgot-password-form")
    if (forgotPasswordForm) {
        forgotPasswordForm.addEventListener("submit", (e) => {
            if (
                document.getElementById("from-profile-flag") &&
                document.getElementById("from-profile-flag").value === "true"
            ) {
                e.preventDefault()

                // Verificar que el correo en el campo sea exactamente el mismo que el de la sesión
                const emailInput = document.getElementById("forgot-email")
                if (emailInput && emailInput.value !== userSessionEmail) {
                    // Si alguien modificó el valor del campo (a pesar de ser readonly), restaurarlo
                    emailInput.value = userSessionEmail
                    showMessage("forgot-password-message", "Solo puedes restablecer la contraseña de tu propia cuenta.", "danger")
                    return
                }

                requestPasswordResetCodeFromProfile()
            }
        })
    }
})

function requestPasswordResetCodeFromProfile() {
    // Verificar que el correo coincida con el de la sesión
    const email = userSessionEmail // Usa directamente el email de la sesión

    if (!email) {
        showMessage(
            "forgot-password-message",
            "No se pudo obtener tu correo electrónico. Por favor, inicia sesión nuevamente.",
            "danger",
        )
        return
    }

    forgotPasswordEmail = email

    const submitButton = document.querySelector("#forgot-password-form .login-button")
    const originalText = submitButton.value
    submitButton.disabled = true
    submitButton.value = "Enviando..."

    // API URL
    const apiUrl = "https://localhost:7191/api/ApiUsuario/SendPasswordResetCode"

    fetch(apiUrl, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify({
            email: email,
            userId: userSessionId,
        }),
    })
        .then((response) => {
            if (!response.ok) {
                throw new Error("Error al enviar el código de verificación")
            }
            return response.json()
        })
        .then((data) => {
            submitButton.disabled = false
            submitButton.value = originalText

            if (data.success) {
                showMessage(
                    "forgot-password-message",
                    "Si tu correo está registrado, verás el código de verificación enviado a tu correo electrónico.",
                    "info",
                )

                setTimeout(() => {
                    showResetPasswordForm()
                }, 5000)
            } else {
                showMessage("forgot-password-message", data.message || "Error al enviar el código de verificación.", "danger")
            }
        })
        .catch((error) => {
            console.error("Error:", error)

            submitButton.disabled = false
            submitButton.value = originalText

            showMessage(
                "forgot-password-message",
                "Error al enviar el código de verificación. Por favor, intenta nuevamente.",
                "danger",
            )
        })
}

function showResetPasswordForm() {
    document.getElementById("login-form-container").style.display = "none"
    document.getElementById("forgot-password-container").style.display = "none"
    document.getElementById("reset-password-container").style.display = "block"
    document.getElementById("reset-password-message").style.display = "none"
}

// Función auxiliar para mostrar mensajes
function showMessage(elementId, message, type) {
    const messageElement = document.getElementById(elementId)
    if (messageElement) {
        messageElement.textContent = message
        messageElement.className = `alert alert-${type}`
        messageElement.style.display = "block"
    }
}

