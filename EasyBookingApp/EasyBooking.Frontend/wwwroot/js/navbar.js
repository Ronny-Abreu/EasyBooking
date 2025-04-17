document.addEventListener("DOMContentLoaded", () => {
    // Profile dropdown toggle
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

    // Back to top button
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

    // Theme toggle
    const themeToggleButton = document.getElementById("theme-toggle-button")

    if (themeToggleButton) {
        // Check for saved theme preference or use device preference
        const savedTheme = localStorage.getItem("theme")
        const prefersDark = window.matchMedia("(prefers-color-scheme: dark)").matches

        if (savedTheme === "dark" || (!savedTheme && prefersDark)) {
            document.body.classList.add("dark-theme")
        }

        themeToggleButton.addEventListener("click", () => {
            document.body.classList.toggle("dark-theme")

            // Save preference
            if (document.body.classList.contains("dark-theme")) {
                localStorage.setItem("theme", "dark")
            } else {
                localStorage.setItem("theme", "light")
            }
        })
    }

    // Navbar hide on scroll down, show on scroll up
    let lastScrollTop = 0
    const navbar = document.querySelector(".EasyBooking-navbar")

    if (navbar) {
        window.addEventListener("scroll", () => {
            const scrollTop = window.pageYOffset || document.documentElement.scrollTop

            if (scrollTop > lastScrollTop && scrollTop > 100) {
                // Scroll down
                navbar.classList.add("navbar-hidden")
            } else {
                // Scroll up
                navbar.classList.remove("navbar-hidden")
            }

            lastScrollTop = scrollTop
        })
    }

    // Modal functions
    window.openLoginModal = () => {
        document.getElementById("modal-overlay").classList.add("active")
        document.getElementById("login-modal").classList.add("active")
        document.getElementById("login-form-container").style.display = "block"
        document.getElementById("forgot-password-container").style.display = "none"
        document.getElementById("reset-password-container").style.display = "none"
    }

    window.closeLoginModal = () => {
        document.getElementById("modal-overlay").classList.remove("active")
        document.getElementById("login-modal").classList.remove("active")
    }

    window.openRegisterModal = () => {
        document.getElementById("modal-overlay").classList.add("active")
        document.getElementById("register-modal").classList.add("active")
    }

    window.closeRegisterModal = () => {
        document.getElementById("modal-overlay").classList.remove("active")
        document.getElementById("register-modal").classList.remove("active")
    }

    window.switchToRegister = (event) => {
        event.preventDefault()
        document.getElementById("login-modal").classList.add("slide-out")

        setTimeout(() => {
            document.getElementById("login-modal").classList.remove("active")
            document.getElementById("login-modal").classList.remove("slide-out")
            document.getElementById("register-modal").classList.add("active")
            document.getElementById("register-modal").classList.add("slide-in")

            setTimeout(() => {
                document.getElementById("register-modal").classList.remove("slide-in")
            }, 300)
        }, 300)
    }

    window.switchToLogin = (event) => {
        event.preventDefault()
        document.getElementById("register-modal").classList.add("slide-out")

        setTimeout(() => {
            document.getElementById("register-modal").classList.remove("active")
            document.getElementById("register-modal").classList.remove("slide-out")
            document.getElementById("login-modal").classList.add("active")
            document.getElementById("login-modal").classList.add("slide-in")

            setTimeout(() => {
                document.getElementById("login-modal").classList.remove("slide-in")
            }, 300)
        }, 300)
    }

    // Forgot password link
    const forgotPasswordLink = document.getElementById("forgot-password-link")
    if (forgotPasswordLink) {
        forgotPasswordLink.addEventListener("click", (e) => {
            e.preventDefault()
            document.getElementById("login-form-container").style.display = "none"
            document.getElementById("forgot-password-container").style.display = "block"
        })
    }

    // Back to login button
    const backToLoginBtn = document.getElementById("back-to-login-btn")
    if (backToLoginBtn) {
        backToLoginBtn.addEventListener("click", () => {
            document.getElementById("login-form-container").style.display = "block"
            document.getElementById("forgot-password-container").style.display = "none"
        })
    }

    // Back to forgot password button
    const backToForgotBtn = document.getElementById("back-to-forgot-btn")
    if (backToForgotBtn) {
        backToForgotBtn.addEventListener("click", () => {
            document.getElementById("reset-password-container").style.display = "none"
            document.getElementById("forgot-password-container").style.display = "block"
        })
    }
})
