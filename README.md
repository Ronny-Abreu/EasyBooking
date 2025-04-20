# 🗺️ EasyBooking – Portal de Reservas
[![.NET](https://img.shields.io/badge/.NET-8.0-purple)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/Licencia-MIT-green)](https://opensource.org/licenses/MIT)
![image](https://github.com/user-attachments/assets/67cc205c-df54-46a2-a878-38cf8756d9d7)

**EasyBooking** es un portal web de reservas turísticas que permite a los usuarios buscar y reservar hoteles/Airbnb y paquetes turísticos de forma fácil, moderna y segura. 

Está diseñado con una arquitectura en capas (N-Capas) utilizando C#, ASP.NET MVC, Web API y SQL Server.

## 📌 Tabla de Contenidos
1. [🌐 Características Principales](#-características-principales)
2. [🏗️ Arquitectura del Proyecto](#-arquitectura-del-proyecto)
3. [🛠️ Tecnologías Utilizadas](#-tecnologías-utilizadas)
4. [⚙️ Funcionalidades por Fases](#-funcionalidades-por-fases)
5. [🖼️ Capturas de Pantalla del Proyecto](#-capturas-de-pantalla-del-proyecto)
6. [🚀 Cómo Ejecutar el Proyecto](#-cómo-ejecutar-el-proyecto)
7. [🙌 Autor](#-autor)

---

## 🌐 Características Principales

- 🔐 Autenticación y registro de usuarios
- 👤 Roles de usuario:
  - **Usuario**: Puede buscar y reservar publicaciones.
- 🏖️ Secciones del portal:
  - Paquetes turísticos
  - Hoteles y Airbnb
- 📩 Envío de correos electrónicos para confirmaciones de reservas
- 💳 Sistema de pagos (próximamente)
- 📱 Diseño responsivo

[🔼 Volver al inicio](#-easybooking--portal-de-reservas)

---

## 🏗️ Arquitectura del Proyecto

El proyecto está estructurado en una arquitectura de capas (N-Capas):

### 📡 EasyBooking.Api
Contiene la **API RESTful** encargada de exponer los endpoints que permiten la comunicación entre el frontend y el backend.

- Controladores HTTP (Endpoints para reservas, usuarios, publicaciones, etc.)
- Manejo de respuestas HTTP
- Inyección de dependencias
- Seguridad y autenticación (JWT)

📁 Estructura clave: `Controllers/`, `Program.cs`, `appsettings.json`

---

### 🧠 EasyBooking.Application
Contiene la **lógica de negocio** del sistema (servicios, validaciones, reglas, etc.).

- Interfaces de servicios (`Contracts/`)
- Servicios (`Services/`)
- DTOs (`Dtos/`)
- AutoMapper (`Mappings/`)
- Clases base (`Core/`)

📁 Estructura clave: `Contracts/`, `Services/`, `Dtos/`, `Mappings/`, `Core/`

---

### 🧱 EasyBooking.Domain
Define el **modelo de dominio** (clases y entidades centrales).

- Entidades (Usuario, Reserva, Hotel/Imágenes, PaqueteTurístico/Imágenes)
- Clases base

📁 Estructura clave: `Entities/`, `Core/`

---

### 🖥️ EasyBooking.Frontend
Es la **interfaz de usuario** basada en ASP.NET MVC.

- Vistas Razor (HTML + C#)
- Controladores MVC
- API consumption con `HttpClient` y AJAX
- Diseño visual (Bootstrap + CSS)
- Interacciones con JS (scripts.js)

📁 Estructura clave: `Views/`, `Controllers/`, `wwwroot/ └── css/ └── js/ └── scripts.js`

---

### 💾 EasyBooking.Persistence
Acceso a la **base de datos** mediante Entity Framework Core.

- `DbContext`
- Repositorios
- Excepciones personalizadas
- Interfaces

📁 Estructura clave: `Context/`, `Models/`, `Repositories/`, `Interfaces/`, `Exceptions/`

[🔼 Volver al inicio](#-easybooking--portal-de-reservas)

---

## 🛠️ Tecnologías Utilizadas

| **Capa**         | **Tecnologías**                                      |
|------------------|------------------------------------------------------|
| **Frontend**     | ASP.NET MVC, Bootstrap 5, JavaScript ES6             |
| **Backend**      | .NET 8.0, Web API, Entity Framework Core 7           |
| **Base de Datos**| SQL Server 2022, Migraciones Code-First             |
| **Seguridad**    | JWT, ASP.NET Identity, Hash con BCrypt               |

[🔼 Volver al inicio](#-easybooking--portal-de-reservas)

---

## ⚙️ Funcionalidades por Fases

### ✅ Fase 1: Configuración y funcionalidades core
- Estructura Ncapas
- Conexión a base de datos
- Autenticación inicial
- Gestión y CRUD de usuarios

### ✅ Fase 2: Publicación de Hoteles/Airbnb
- Visualización de hoteles
- Detalles de cada hotel

### ✅ Fase 3: Paquetes Turísticos y Reservas
- Visualización de paquetes
- Sistema de reservas
- Pago inicial (validación de tarjeta)

### 🚧 Fase 4: Sistema de pagos real
- [ ] Integración con Stripe/PayPal
- [ ] Gestión de transacciones
- [ ] Confirmaciones por email

[🔼 Volver al inicio](#-easybooking--portal-de-reservas)

---

## 🖼️ Capturas de Pantalla del Proyecto

### 📌 Publicaciones Disponibles
![Publicaciones](https://github.com/user-attachments/assets/3b79a2e6-7d23-4f90-b097-b6db8b3f8acd)

---

### 🔍 Detalles de una Publicación
![Detalle de Publicación](https://github.com/user-attachments/assets/d45a149a-24d3-4ee9-98d3-88fe25bb92aa)

---

### 📅 Reservaciones
![Reservaciones](https://github.com/user-attachments/assets/68029057-f225-4580-b08b-9b72bbe2eac4)

---

### 💳 Proceso de Pago
![Pago 1](https://github.com/user-attachments/assets/74004a74-bf77-4091-a798-32c69604754d)

---

### 📧 Ejemplo de Correo Electrónico Enviado
![Email Confirmación](https://github.com/user-attachments/assets/21973da8-7a61-491d-9ac5-d7bf55836018)

[🔼 Volver al inicio](#-easybooking--portal-de-reservas)

---

## 🙌 Autor

**Desarrollado por:**  
👨‍💻 **Ronny De León** – Web Developer

📧 **Email:** [dleonabreuronny@gmail.com](mailto:dleonabreuronny@gmail.com)  
🔗 **GitHub:** [github.com/Ronny-Abreu](https://github.com/Ronny-Abreu)

## 🚀 Cómo Ejecutar el Proyecto

1. Clonar el repositorio:
   ```bash
   git clone https://github.com/Ronny-Abreu/EasyBooking.git
