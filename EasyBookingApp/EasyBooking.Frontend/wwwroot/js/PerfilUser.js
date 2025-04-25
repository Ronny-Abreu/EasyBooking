/**
 * Inicialización cuando el DOM está cargado
 */
document.addEventListener("DOMContentLoaded", () => {
    const userJson = localStorage.getItem("user");

    if (userJson) {
        try {
            const user = JSON.parse(userJson);


            // Actualiza la información del perfil
            document.getElementById("profile-name").textContent = user.nombre;
            document.getElementById("confirmed-info-title").textContent = `Información confirmada de ${user.nombre}`;

            // Inicial del nombre en el avatar
            const avatarElement = document.getElementById("profile-avatar");
            if (avatarElement && user.nombre) {
                avatarElement.textContent = user.nombre.charAt(0).toUpperCase();
            }

            // Actualizar datos personales
            document.getElementById("user-fullname").textContent = `${user.nombre} ${user.apellido}`;
            document.getElementById("user-email").textContent = user.email;
            document.getElementById("user-phone").textContent = user.telefono;
            // Configurar botón de verificación
            const verificationBtn = document.getElementById("verification-btn");
            if (verificationBtn) {
                verificationBtn.addEventListener("click", () => {
                    sendVerificationEmail(user.email, user.id);
                });
            }

            // Configurar botones de acción
            setupActionButtons(user);
        } catch (e) {
            console.error("Error al parsear los datos del usuario:", e);
        }
    } else {
        // Si no hay usuario autenticado, redirigir a la página principal
        // window.location.href = "/";
    }
});


/**
 * Configurar los botones de acción del perfil
 * @param {Object} user - Datos del usuario
 */
function setupActionButtons(user) {
    // Botón de editar perfil
    const editProfileBtn = document.getElementById("editProfileBtn");
    const editProfileModal = document.getElementById("editProfileModal");
    const closeEditModal = document.getElementById("closeEditModal");
    const editProfileForm = document.getElementById("editProfileForm");
    const modalBackdrop = document.getElementById("modalBackdrop");
    document.getElementById("editEmail").value = user.email || "";


    // Configurar modal de edición
    if (editProfileBtn) {
        editProfileBtn.addEventListener("click", () => {
            // El formulario aparece con los datos actuales del usuario
            document.getElementById("editNombre").value = user.nombre || "";
            document.getElementById("editApellido").value = user.apellido || "";
            document.getElementById("currentPassword").value = "";
            document.getElementById("editPassword").value = "";
            document.getElementById("password-error").style.display = "none";

            editProfileModal.style.display = "block";
            modalBackdrop.style.display = "block";
        });
    }

    if (closeEditModal) {
        closeEditModal.addEventListener("click", () => {
            editProfileModal.style.display = "none";
            modalBackdrop.style.display = "none";
        });
    }

    // Cerrar modal al hacer clic fuera
    window.addEventListener("click", (event) => {
        if (event.target === modalBackdrop) {
            editProfileModal.style.display = "none";
            modalBackdrop.style.display = "none";
        }
    });

    // Manejar envío del formulario de edición
    if (editProfileForm) {
        editProfileForm.addEventListener("submit", (event) => {
            event.preventDefault();

            // Valores del formulario
            const nombre = document.getElementById("editNombre").value;
            const apellido = document.getElementById("editApellido").value;
            const username = document.getElementById("editUsername").value;
            const currentPassword = document.getElementById("currentPassword").value;
            const newPassword = document.getElementById("editPassword").value;
            const passwordError = document.getElementById("password-error");

            passwordError.style.display = "none";

            // Validar campos obligatorios
            if (!nombre || !apellido || !username) {
                alert("Por favor, completa todos los campos requeridos.");
                return;
            }

            // Solo verificar la contraseña actual si el usuario está intentando cambiar su contraseña
            if (newPassword) {
                if (!currentPassword) {
                    passwordError.textContent = "Debes ingresar tu contraseña actual para cambiarla.";
                    passwordError.style.display = "block";
                    return;
                }

                verifyCurrentPassword(user.id, currentPassword, (isValid) => {
                    if (isValid) {
                        updateUserProfile();
                    } else {
                        passwordError.textContent = "La contraseña actual es incorrecta.";
                        passwordError.style.display = "block";
                    }
                });
            } else {
                updateUserProfile();
            }

            function updateUserProfile() {
                if (newPassword) {
                    if (!validatePassword(newPassword)) {
                        passwordError.textContent =
                            "La contraseña debe tener al menos 8 caracteres, incluyendo al menos 2 números y 2 letras.";
                        passwordError.style.display = "block";
                        return;
                    }
                }

                const updatedUser = {
                    id: user.id,
                    nombre: nombre,
                    apellido: apellido,
                    email: user.email,
                    telefono: user.telefono,

                    isEmailVerified: user.isEmailVerified,
                };

                if (newPassword) {
                    updatedUser.password = newPassword;
                    updatedUser.currentPassword = currentPassword;
                }

                updateProfile(updatedUser);
            }
        });
    }

    // Botón de eliminar perfil y modales
    const deleteProfileBtn = document.getElementById("deleteProfileBtn");
    const passwordVerificationModal = document.getElementById("passwordVerificationModal");
    const deleteConfirmationModal = document.getElementById("deleteConfirmationModal");
    const confirmDeleteBtn = document.getElementById("confirmDeleteBtn");
    const cancelDeleteBtn = document.getElementById("cancelDeleteBtn");
    const verifyPasswordBtn = document.getElementById("verifyPasswordBtn");
    const cancelPasswordBtn = document.getElementById("cancelPasswordBtn");
    const deletePasswordInput = document.getElementById("deletePassword");
    const passwordError = document.getElementById("passwordError");

    // Evitar propagación de clics en modales
    passwordVerificationModal.addEventListener("click", (event) => {
        event.stopPropagation();
    });

    deleteConfirmationModal.addEventListener("click", (event) => {
        event.stopPropagation();
    });

    // Modal de verificación
    deleteProfileBtn.addEventListener("click", () => {
        passwordVerificationModal.style.display = "block";
        modalBackdrop.style.display = "block";
        deletePasswordInput.value = "";
        passwordError.style.display = "none";
        deletePasswordInput.classList.remove("shake");
    });

    cancelPasswordBtn.addEventListener("click", () => {
        passwordVerificationModal.style.display = "none";
        modalBackdrop.style.display = "none";
    });

    verifyPasswordBtn.addEventListener("click", () => {
        verifyPassword(user.id, deletePasswordInput.value);
    });

    deletePasswordInput.addEventListener("keypress", (e) => {
        if (e.key === "Enter") {
            e.preventDefault();
            verifyPassword(user.id, deletePasswordInput.value);
        }
    });

    cancelDeleteBtn.addEventListener("click", () => {
        deleteConfirmationModal.style.display = "none";
        modalBackdrop.style.display = "none";
    });

    confirmDeleteBtn.addEventListener("click", () => {
        requestAccountDeletion(user.id, deletePasswordInput.value);
    });

    // Cerrar modales al hacer clic en el fondo
    modalBackdrop.addEventListener("click", (event) => {
        if (event.target === modalBackdrop) {
            if (passwordVerificationModal.style.display === "block") {
                passwordVerificationModal.style.display = "none";
            }
            if (deleteConfirmationModal.style.display === "block") {
                deleteConfirmationModal.style.display = "none";
            }
            if (editProfileModal.style.display === "block") {
                editProfileModal.style.display = "none";
            }
            modalBackdrop.style.display = "none";
        }
    });
}

/**
 * Función para verificar la contraseña actual
 * @param {string} userId - ID del usuario
 * @param {string} password - Contraseña a verificar
 * @param {Function} callback - Función de callback con el resultado
 */
function verifyCurrentPassword(userId, password, callback) {
    // URL de la API
    const apiUrl = "https://localhost:7191/api/ApiUsuario/VerifyPassword";

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
                throw new Error("Error al verificar la contraseña");
            }
            return response.json();
        })
        .then((data) => {
            callback(data.success);
        })
        .catch((error) => {
            console.error("Error:", error);
            callback(false);
        });
}

/**
 * Función para verificar la contraseña para eliminación de cuenta
 * @param {string} userId - ID del usuario
 * @param {string} password - Contraseña a verificar
 */
function verifyPassword(userId, password) {
    if (!password) {
        const passwordInput = document.getElementById("deletePassword");
        const passwordError = document.getElementById("passwordError");

        passwordError.textContent = "Por favor, ingresa tu contraseña.";
        passwordError.style.display = "block";

        // Animación de error
        passwordInput.classList.remove("shake");
        void passwordInput.offsetWidth; // Forzar reflow
        passwordInput.classList.add("shake");

        return;
    }

    const verifyPasswordBtn = document.getElementById("verifyPasswordBtn");
    verifyPasswordBtn.disabled = true;
    verifyPasswordBtn.innerHTML =
        '<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> Verificando...';

    // URL de la API
    const apiUrl = "https://localhost:7191/api/ApiUsuario/VerifyPassword";

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
                throw new Error("Error al verificar la contraseña");
            }
            return response.json();
        })
        .then((data) => {
            verifyPasswordBtn.innerHTML = "Verificar";
            verifyPasswordBtn.disabled = false;

            if (data.success) {
                document.getElementById("passwordVerificationModal").style.display = "none";
                document.getElementById("deleteConfirmationModal").style.display = "block";
            } else {
                const passwordInput = document.getElementById("deletePassword");
                const passwordError = document.getElementById("passwordError");

                passwordError.textContent = data.message || "Contraseña incorrecta. Por favor, intenta nuevamente.";
                passwordError.style.display = "block";

                // Animación de error
                passwordInput.classList.remove("shake");
                void passwordInput.offsetWidth; // Forzar reflow
                passwordInput.classList.add("shake");
            }
        })
        .catch((error) => {
            console.error("Error:", error);

            verifyPasswordBtn.innerHTML = "Verificar";
            verifyPasswordBtn.disabled = false;

            const passwordError = document.getElementById("passwordError");
            passwordError.textContent = "Error al verificar la contraseña. Por favor, intenta nuevamente.";
            passwordError.style.display = "block";

            const passwordInput = document.getElementById("deletePassword");
            passwordInput.classList.remove("shake");
            void passwordInput.offsetWidth; // Forzar reflow
            passwordInput.classList.add("shake");
        });
}

/**
 * Función para solicitar la eliminación de la cuenta
 * @param {string} userId - ID del usuario
 * @param {string} password - Contraseña del usuario
 */
function requestAccountDeletion(userId, password) {
    const confirmDeleteBtn = document.getElementById("confirmDeleteBtn");
    confirmDeleteBtn.disabled = true;
    confirmDeleteBtn.innerHTML =
        '<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> Procesando...';

    // URL de la API
    const apiUrl = "https://localhost:7191/api/ApiUsuario/RequestAccountDeletion";

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
                throw new Error("Error al procesar la solicitud");
            }
            return response.json();
        })
        .then((data) => {
            document.getElementById("deleteConfirmationModal").style.display = "none";
            document.getElementById("modalBackdrop").style.display = "none";

            if (data.success) {
                if (typeof Swal !== "undefined") {
                    Swal.fire({
                        title: "Solicitud enviada",
                        text: "Se ha enviado un correo electrónico con instrucciones para confirmar la eliminación de tu cuenta.",
                        icon: "info",
                        confirmButtonText: "Aceptar",
                    }).then(() => {
                        localStorage.removeItem("user");
                        window.location.href = "/";
                    });
                } else {
                    alert("Se ha enviado un correo electrónico con instrucciones para confirmar la eliminación de tu cuenta.");
                    localStorage.removeItem("user");
                    window.location.href = "/";
                }
            } else {
                throw new Error(data.message || "Error al procesar la solicitud de eliminación de cuenta.");
            }

            confirmDeleteBtn.innerHTML = "Eliminar";
            confirmDeleteBtn.disabled = false;
        })
        .catch((error) => {
            console.error("Error:", error);

            if (typeof Swal !== "undefined") {
                Swal.fire({
                    title: "Error",
                    text: "Ocurrió un error al procesar tu solicitud. Por favor, intenta nuevamente más tarde.",
                    icon: "error",
                    confirmButtonText: "Aceptar",
                });
            } else {
                alert("Ocurrió un error al procesar tu solicitud. Por favor, intenta nuevamente más tarde.");
            }

            confirmDeleteBtn.innerHTML = "Eliminar";
            confirmDeleteBtn.disabled = false;

            // Ocultar el modal de confirmación
            document.getElementById("deleteConfirmationModal").style.display = "none";
            document.getElementById("modalBackdrop").style.display = "none";
        });
}

/**
 * Validación de contraseña segura
 * @param {string} password - Contraseña a validar
 * @returns {boolean} - Resultado de la validación
 */
function validatePassword(password) {
    if (!password) return false;

    const hasMinLength = password.length >= 8;
    const hasNumbers = (password.match(/\d/g) || []).length >= 2;
    const hasLetters = (password.match(/[a-zA-Z]/g) || []).length >= 2;

    return hasMinLength && hasNumbers && hasLetters;
}

/**
 * Actualizar perfil de usuario
 * @param {Object} updatedUser - Datos actualizados del usuario
 */
function updateProfile(updatedUser) {
    const updateProfileBtn = document.getElementById("updateProfileBtn");
    const passwordError = document.getElementById("password-error");

    const userJson = localStorage.getItem("user");
    if (userJson) {
        const user = JSON.parse(userJson);

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
                });
            } else {
                alert("No se detectaron cambios para actualizar.");
            }

            return;
        }
    }

    updateProfileBtn.disabled = true;
    updateProfileBtn.innerHTML =
        '<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> Actualizando...';

    // URL de la API
    const apiUrl = "https://localhost:7191/api/ApiUsuario/UpdateProfile";

    // Enviar solicitud para actualizar el perfil
    fetch(apiUrl, {
        method: "PUT",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(updatedUser),
    })
        .then((response) => {
            if (!response.ok) {
                return response.json().then((data) => {
                    throw new Error(data.message || "Error al actualizar el perfil");
                });
            }
            return response.json();
        })
        .then((data) => {
            if (data.success) {
                const userJson = localStorage.getItem("user");
                if (userJson) {
                    const user = JSON.parse(userJson);
                    user.nombre = updatedUser.nombre;
                    user.apellido = updatedUser.apellido;
                    user.username = updatedUser.username;
                    localStorage.setItem("user", JSON.stringify(user));
                }

                if (typeof Swal !== "undefined") {
                    Swal.fire({
                        title: "¡Perfil actualizado!",
                        text: "Tu perfil ha sido actualizado correctamente.",
                        icon: "success",
                        confirmButtonText: "Aceptar",
                    }).then(() => {
                        window.location.reload();
                    });
                } else {
                    alert("Tu perfil ha sido actualizado correctamente.");
                    window.location.reload();
                }
            } else {
                throw new Error(data.message || "Error al actualizar el perfil");
            }
        })
        .catch((error) => {
            console.error("Error:", error);

            if (error.message && error.message.includes("contraseña")) {
                passwordError.textContent = error.message;
                passwordError.style.display = "block";
                passwordError.scrollIntoView({ behavior: "smooth", block: "center" });
            } else {
                if (typeof Swal !== "undefined") {
                    Swal.fire({
                        title: "Error al actualizar el perfil",
                        text: error.message || "Error al actualizar el perfil.",
                        icon: "error",
                        confirmButtonText: "Aceptar",
                    });
                } else {
                    alert("Ocurrió un error al actualizar el perfil: " + (error.message || "Error al actualizar el perfil."));
                }
            }

            updateProfileBtn.innerHTML = "Actualizar";
            updateProfileBtn.disabled = false;
        });
}

/**
 * Enviar correo de verificación
 * @param {string} email - Correo electrónico del usuario
 * @param {string} userId - ID del usuario
 */
function sendVerificationEmail(email, userId) {
    const verificationBtn = document.getElementById("verification-btn");
    verificationBtn.disabled = true;
    verificationBtn.innerHTML =
        '<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> Enviando...';

    const apiUrl = "https://localhost:7191/api/ApiUsuario/SendVerificationEmail";

    // Solicitud para enviar correo de verificación
    fetch(`${apiUrl}?email=${encodeURIComponent(email)}&userId=${userId}`, {
        method: "POST",
    })
        .then((response) => {
            if (!response.ok) {
                throw new Error("Error al enviar el correo de verificación");
            }
            return response.json();
        })
        .then((data) => {
            if (typeof Swal !== "undefined") {
                Swal.fire({
                    title: "Correo enviado",
                    text: "Se ha enviado un correo de verificación a tu dirección de correo electrónico. Por favor, revisa tu bandeja de entrada y sigue las instrucciones para verificar tu cuenta.",
                    icon: "success",
                    confirmButtonText: "Aceptar",
                });
            } else {
                alert("Se ha enviado un correo de verificación a tu dirección de correo electrónico. Por favor, revisa tu bandeja de entrada y sigue las instrucciones para verificar tu cuenta.");
            }
            verificationBtn.innerHTML = "Verificación";
            verificationBtn.disabled = false;
        })
        .catch((error) => {
            console.error("Error:", error);
            if (typeof Swal !== "undefined") {
                Swal.fire({
                    title: "Error",
                    text: "Ocurrió un error al enviar el correo de verificación. Por favor, intenta nuevamente más tarde.",
                    icon: "error",
                    confirmButtonText: "Aceptar",
                });
            } else {
                alert("Ocurrió un error al enviar el correo de verificación. Por favor, intenta nuevamente más tarde.");
            }
            verificationBtn.innerHTML = "Verificación";
            verificationBtn.disabled = false;
        });
}