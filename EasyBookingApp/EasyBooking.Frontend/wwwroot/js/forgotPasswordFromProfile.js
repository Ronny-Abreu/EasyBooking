function openForgotPasswordFromProfile() {
    // Primero, se cierra el modal de edición de perfil
    const editProfileModal = document.getElementById('editProfileModal');
    if (editProfileModal) {
        editProfileModal.style.display = 'none';
    }

    // Luego, abre el modal de login
    document.getElementById("modal-overlay").classList.add("active");
    document.getElementById("login-modal").classList.add("active");

    // Finalmente, muestra el formulario de recuperación de contraseña después de un breve retraso
    setTimeout(function () {
        document.getElementById('login-form-container').style.display = 'none';
        document.getElementById('forgot-password-container').style.display = 'block';
        document.getElementById('reset-password-container').style.display = 'none';
        document.getElementById('forgot-password-message').style.display = 'none';
        document.getElementById('forgot-email').value = '';
    }, 100);
}

document.addEventListener('DOMContentLoaded', function () {
    //  Abrir modal y formulario desde el perfil
    const forgotPasswordLinkProfile = document.getElementById('forgot-password-link-profile');
    if (forgotPasswordLinkProfile) {
        forgotPasswordLinkProfile.addEventListener('click', function (e) {
            e.preventDefault();
            openForgotPasswordFromProfile();
        });
    }
});