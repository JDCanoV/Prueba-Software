document.addEventListener("DOMContentLoaded", function () {
    // Función para quitar un producto del carrito
    function quitarDelCarrito(id, url) {
        $.ajax({
            url: url,
            type: 'POST',
            data: JSON.stringify({ id_platillo: id }),
            contentType: 'application/json; charset=utf-8',
            success: function (response) {
                if (response.success) {
                    mostrarModal(response.message); // Mostrar mensaje en el modal
                    setTimeout(() => {
                        $("#confirmationModal").modal("hide"); // Cerrar el modal
                        window.location.reload(); // Recargar la página
                    }, 1500);
                } else {
                    mostrarModal("Hubo un problema al quitar el producto. Por favor, inténtalo de nuevo.");
                }
            },
            error: function () {
                mostrarModal("Ocurrió un error en la solicitud. Verifica tu conexión e inténtalo nuevamente.");
            }
        });
    }

    // Función para mostrar el modal con un mensaje
    function mostrarModal(mensaje) {
        const modalMessageElement = document.getElementById("confirmationModalMessage");
        modalMessageElement.textContent = mensaje; // Establecer el mensaje del modal
        $("#confirmationModal").modal("show"); // Mostrar el modal
    }

    // Registrar los eventos en los botones de la tabla dinámicamente
    document.querySelectorAll(".btn-danger").forEach((button) => {
        button.addEventListener("click", function () {
            const id = this.getAttribute("data-id");
            const url = this.getAttribute("data-url");
            quitarDelCarrito(id, url);
        });
    });
});
document.addEventListener("DOMContentLoaded", function () {
    // Mostrar mensaje en el modal
    function mostrarModal(mensaje) {
        const modalMessageElement = document.getElementById("confirmationModalMessage");
        modalMessageElement.textContent = mensaje;
        $("#confirmationModal").modal("show");
    }

    // Función para agregar un producto al carrito
    function agregarAlCarrito(id, url) {
        const producto = {
            id_platillo: id
        };

        $.ajax({
            url: url,
            type: 'POST',
            data: JSON.stringify(producto),
            contentType: 'application/json; charset=utf-8',
            success: function (response) {
                if (response.success) {
                    mostrarModal(response.message);
                } else {
                    mostrarModal("Error al agregar el producto al carrito.");
                }
            },
            error: function () {
                mostrarModal("Error en la solicitud para agregar el producto al carrito.");
            }
        });
    }

    // Función para limpiar el carrito
    function limpiarCarrito(url) {
        $.ajax({
            url: url,
            type: 'POST',
            success: function (response) {
                if (response.success) {
                    mostrarModal("Carrito limpiado");
                    setTimeout(() => window.location.reload(), 1500);
                } else {
                    mostrarModal("Error al limpiar el carrito.");
                }
            },
            error: function () {
                mostrarModal("Error en la solicitud para limpiar el carrito.");
            }
        });
    }

    // Registrar eventos para los botones de "Agregar al Carrito"
    document.querySelectorAll(".btn-agregar").forEach(button => {
        button.addEventListener("click", function () {
            const id = this.getAttribute("data-id");
            const url = this.getAttribute("data-url");
            agregarAlCarrito(id, url);
        });
    });

    // Registrar el evento para el botón de "Limpiar Carrito"
    const limpiarCarritoButton = document.getElementById("btn-limpiar-carrito");
    if (limpiarCarritoButton) {
        limpiarCarritoButton.addEventListener("click", function () {
            const url = this.getAttribute("data-url");
            limpiarCarrito(url);
        });
    }
});




