// Forgot Password Variables
let forgotPasswordEmail = '';
let verificationCodeValid = false;

// Event Listeners para Forgot Password
document.addEventListener('DOMContentLoaded', function () {
    // Forgot Password Link
    const forgotPasswordLink = document.getElementById('forgot-password-link');
    if (forgotPasswordLink) {
        forgotPasswordLink.addEventListener('click', function (e) {
            e.preventDefault();
            showForgotPasswordForm();
        });
    }

    // volver al formulario del Login
    const backToLoginBtn = document.getElementById('back-to-login-btn');
    if (backToLoginBtn) {
        backToLoginBtn.addEventListener('click', function () {
            showLoginForm();
        });
    }

    // Ir al formulario Forgot Password
    const backToForgotBtn = document.getElementById('back-to-forgot-btn');
    if (backToForgotBtn) {
        backToForgotBtn.addEventListener('click', function () {
            showForgotPasswordForm();
        });
    }

    // Forgot Password Form
    const forgotPasswordForm = document.getElementById('forgot-password-form');
    if (forgotPasswordForm) {
        forgotPasswordForm.addEventListener('submit', function (e) {
            e.preventDefault();
            requestPasswordResetCode();
        });
    }

    // Verificación Codigo Input
    const verificationCodeInput = document.getElementById('verification-code');
    if (verificationCodeInput) {
        verificationCodeInput.addEventListener('input', function () {
            
            this.value = this.value.replace(/[^0-9]/g, '');

            // Verificación en tiempo real longuitud maxima de (6 digitos) para el codigo de verif.
            if (this.value.length === 6) {
                verifyResetCode(this.value);
            } else {
                
                resetCodeValidationStatus();
            }
        });
    }

    // Reset Password Form
    const resetPasswordForm = document.getElementById('reset-password-form');
    if (resetPasswordForm) {
        resetPasswordForm.addEventListener('submit', function (e) {
            e.preventDefault();
            resetPassword();
        });
    }
});


function showForgotPasswordForm() {
    document.getElementById('login-form-container').style.display = 'none';
    document.getElementById('forgot-password-container').style.display = 'block';
    document.getElementById('reset-password-container').style.display = 'none';

    document.getElementById('forgot-password-message').style.display = 'none';
    document.getElementById('forgot-email').value = '';
}


function showResetPasswordForm() {
    document.getElementById('login-form-container').style.display = 'none';
    document.getElementById('forgot-password-container').style.display = 'none';
    document.getElementById('reset-password-container').style.display = 'block';

    document.getElementById('reset-password-message').style.display = 'none';
    document.getElementById('verification-code').value = '';
    document.getElementById('new-password').value = '';
    document.getElementById('new-password').disabled = true;
    document.getElementById('reset-password-btn').disabled = true;

    resetCodeValidationStatus();
}

// Show Login Form
function showLoginForm() {
    document.getElementById('login-form-container').style.display = 'block';
    document.getElementById('forgot-password-container').style.display = 'none';
    document.getElementById('reset-password-container').style.display = 'none';
}

// Pide el correo para enviar el Password Reset Code
function requestPasswordResetCode() {
    const email = document.getElementById('forgot-email').value;
    if (!email) {
        showMessage('forgot-password-message', 'Por favor, ingresa tu correo electrónico.', 'danger');
        return;
    }

    forgotPasswordEmail = email;

    const submitButton = document.querySelector('#forgot-password-form .login-button');
    const originalText = submitButton.value;
    submitButton.disabled = true;
    submitButton.value = 'Enviando...';

    // API URL
    const apiUrl = 'https://localhost:7191/api/ApiUsuario/SendPasswordResetCode';

    // Send request
    fetch(apiUrl, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ email: email })
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Error al enviar el código de verificación');
            }
            return response.json();
        })
        .then(data => {

            submitButton.disabled = false;
            submitButton.value = originalText;

            if (data.success) {
                showMessage('forgot-password-message', 'Hemos enviado un código de verificación a tu correo electrónico.', 'info');

                setTimeout(() => {
                    showResetPasswordForm();
                }, 2000);
            } else {
                showMessage('forgot-password-message', data.message || 'Error al enviar el código de verificación.', 'danger');
            }
        })
        .catch(error => {
            console.error('Error:', error);

            submitButton.disabled = false;
            submitButton.value = originalText;

            showMessage('forgot-password-message', 'Error al enviar el código de verificación. Por favor, intenta nuevamente.', 'danger');
        });
}

// Verify Reset Code
function verifyResetCode(code) {
    if (!code || code.length !== 6) {
        resetCodeValidationStatus();
        return;
    }

    // API URL
    const apiUrl = 'https://localhost:7191/api/ApiUsuario/VerifyResetCode';

    // Send request
    fetch(apiUrl, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            email: forgotPasswordEmail,
            code: code
        })
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Error al verificar el código');
            }
            return response.json();
        })
        .then(data => {
            const codeInput = document.getElementById('verification-code');

            //codigo de verificacion valido
            if (data.success) {
                verificationCodeValid = true;

                // ver estatus
                codeInput.classList.remove('invalid', 'shake');
                codeInput.classList.add('valid');
                document.getElementById('code-valid').style.display = 'block';
                document.getElementById('code-invalid').style.display = 'none';

                // Habilita la entrada de contraseña si el codigo de verif. es corrrecto
                document.getElementById('new-password').disabled = false;
                document.getElementById('reset-password-btn').disabled = false;

                document.getElementById('new-password').focus();
            } else {
                //codigo de verificacion invalido
                verificationCodeValid = false;

                // Animación shake si el codigo ingresado llegó a los 6 digitos y es incorrecto
                codeInput.classList.remove('valid');
                codeInput.classList.add('invalid');
                document.getElementById('code-valid').style.display = 'none';
                document.getElementById('code-invalid').style.display = 'block';

                // shake animation
                codeInput.classList.remove('shake');
                void codeInput.offsetWidth; // Trigger reflow to restart animation
                codeInput.classList.add('shake');

                // Desactiva el input del ingreso de contraseña
                document.getElementById('new-password').disabled = true;
                document.getElementById('reset-password-btn').disabled = true;
            }
        })
        .catch(error => {
            console.error('Error:', error);
            resetCodeValidationStatus();
            showMessage('reset-password-message', 'Error al verificar el código. Por favor, intenta nuevamente.', 'danger');
        });
}

// Reset Code Validation Status
function resetCodeValidationStatus() {
    verificationCodeValid = false;
    const codeInput = document.getElementById('verification-code');
    codeInput.classList.remove('valid', 'invalid', 'shake');
    document.getElementById('code-valid').style.display = 'none';
    document.getElementById('code-invalid').style.display = 'none';
    document.getElementById('new-password').disabled = true;
    document.getElementById('reset-password-btn').disabled = true;
}

// Reset Password
function resetPassword() {
    const code = document.getElementById('verification-code').value;
    const newPassword = document.getElementById('new-password').value;

    if (!code || code.length !== 6) {
        showMessage('reset-password-message', 'Por favor, ingresa el código de verificación.', 'danger');
        return;
    }

    if (!newPassword) {
        showMessage('reset-password-message', 'Por favor, ingresa tu nueva contraseña.', 'danger');
        return;
    }

    if (!verificationCodeValid) {
        showMessage('reset-password-message', 'El código de verificación no es válido.', 'danger');
        return;
    }

    const submitButton = document.getElementById('reset-password-btn');
    const originalText = submitButton.value;
    submitButton.disabled = true;
    submitButton.value = 'Procesando...';

    // API URL
    const apiUrl = 'https://localhost:7191/api/ApiUsuario/ResetPassword';

    // Send request
    fetch(apiUrl, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            email: forgotPasswordEmail,
            code: code,
            newPassword: newPassword
        })
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Error al restablecer la contraseña');
            }
            return response.json();
        })
        .then(data => {
            
            submitButton.disabled = false;
            submitButton.value = originalText;

            if (data.success) {
                // mensaje de exito
                showMessage('reset-password-message', 'Tu contraseña ha sido restablecida correctamente. Ahora puedes iniciar sesión con tu nueva contraseña.', 'success');

                setTimeout(() => {
                    showLoginForm();
                }, 3000);
            } else {
                showMessage('reset-password-message', data.message || 'Error al restablecer la contraseña.', 'danger');
            }
        })
        .catch(error => {
            console.error('Error:', error);

            submitButton.disabled = false;
            submitButton.value = originalText;

            showMessage('reset-password-message', 'Error al restablecer la contraseña. Por favor, intenta nuevamente.', 'danger');
        });
}

// Show Message
function showMessage(elementId, message, type) {
    const messageElement = document.getElementById(elementId);
    messageElement.textContent = message;
    messageElement.className = `alert alert-${type}`;
    messageElement.style.display = 'block';
}