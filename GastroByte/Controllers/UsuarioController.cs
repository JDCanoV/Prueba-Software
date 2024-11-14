using GastroByte.Dtos;
using GastroByte.Services;
using System;
using System.Web.Mvc;

namespace GastroByte.Controllers
{
    public class UsuarioController : Controller
    {
        // Vista principal de Usuario
        // Esta acción devuelve la vista principal del usuario
        public ActionResult Index()
        {
            return View();
        }

        // Vista de Login (GET)
        // Esta acción se encarga de mostrar el formulario de inicio de sesión
        public ActionResult Login(string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl; // Almacena la URL de retorno para redirigir después del login
            return View();
        }

        // Procesamiento del Login (POST)
        // Esta acción maneja la lógica cuando el usuario envía el formulario de login
        [HttpPost]
        public ActionResult Login(UsuarioDto loginUser, string returnUrl = null)
        {
            // Verifica si el modelo de usuario está vacío, y si es así, asigna un mensaje de error
            if (loginUser == null)
            {
                loginUser = new UsuarioDto { Message = "El modelo de usuario no se envió correctamente." };
                return View(loginUser); // Devuelve la vista con el mensaje de error
            }

            try
            {
                // Llama al servicio de usuario para autenticar al usuario
                var userService = new UsuarioService();
                var userResponse = userService.LoginUser(loginUser);

                // Si la respuesta indica que el login fue exitoso (Response == 1)
                if (userResponse.Response == 1)
                {
                    Session["UserID"] = userResponse.id_usuario;
                    Session["UserName"] = userResponse.nombre;
                    Session["UserRole"] = userResponse.id_rol;
                    // Redirige a diferentes páginas según el rol del usuario
                    switch (userResponse.id_rol)
                    {
                        case 1:
                            return RedirectToAction("Index", "Administrador");
                        case 2:
                            return RedirectToAction("Index", "Asistente");
                        case 3:
                            return RedirectToAction("Index", "Home");
                        default:
                            // Si el rol no coincide, redirige a una página genérica o de error
                            return RedirectToAction("Login", "Usuario");
                    
                    Session["UserDocumento"] = userResponse.numero_documento;
                    Session["UserTelefono"] = userResponse.telefono;
                    Session["UserCorreo"] = userResponse.correo_electronico;

                // Redirige al returnUrl si está definido, o al home en caso contrario
                if (!string.IsNullOrEmpty(returnUrl))
                        return Redirect(returnUrl); // Si hay una URL de retorno, redirige a esa

                    // Si no hay returnUrl, redirige al home
                    return RedirectToAction("Index", "Home");
                }
            }
                else
                {
                    // Si las credenciales son incorrectas, muestra un mensaje de error
                    loginUser.Message = "Credenciales incorrectas. Inténtalo nuevamente.";
                    return View(loginUser); // Devuelve la vista con el mensaje de error
                }
            }
            catch (Exception ex)
            {
                // Si ocurre una excepción, muestra el mensaje de error correspondiente
                loginUser.Message = "Ocurrió un error inesperado: " + ex.Message;
                return View(loginUser); // Devuelve la vista con el mensaje de error
            }
        }
       


        // GET: Usuario/Logout
        public ActionResult Logout()
        {
            Session.Clear();  // Limpia todas las variables de sesión
            return RedirectToAction("Login", "Usuario");
        }

        // Método para cerrar sesión
        // Esta acción limpia la sesión del usuario y redirige a la página de login
        public ActionResult Logout()
        {
            Session.Clear(); // Limpia toda la sesión del usuario
            return RedirectToAction("Login", "Usuario"); // Redirige a la página de login
        }

        // Vista de creación de usuario (GET)
        // Muestra el formulario para crear un nuevo usuario
        public ActionResult Create()
        {
            // Inicializa un objeto UsuarioDto vacío para pasarlo a la vista
            return View(new UsuarioDto { Response = 0, Message = string.Empty });
        }

        // Procesamiento de creación de usuario (POST)
        // Esta acción maneja la lógica cuando el formulario de creación de usuario es enviado
        [HttpPost]
        public ActionResult Create(UsuarioDto newUser)
        {
            // Verifica si el modelo de usuario está vacío, y si es así, asigna un mensaje de error
            if (newUser == null)
            {
                newUser = new UsuarioDto { Message = "El wo de usuario no se envió correctamente." };
                return View(newUser); // Devuelve la vista con el mensaje de error
            }

            try
            {
                // Llama al servicio de usuario para crear el nuevo usuario
                var userService = new UsuarioService();
                var userResponse = userService.CreateUser(newUser);

                // Si la respuesta indica que la creación fue exitosa (Response == 1)
                if (userResponse.Response == 1)
                {
                    return RedirectToAction("Index"); // Redirige al index de usuario
                }
                else
                {
                    // Si ocurrió un error en la creación, muestra el mensaje de error
                    if (userResponse.Message == null)
                    {
                        userResponse.Message = "Error al crear el usuario. Por favor, inténtalo nuevamente.";
                    }
                    return View(userResponse); // Devuelve la vista con el mensaje de error
                }
            }
            catch (Exception ex)
            {
                // Si ocurre una excepción, muestra el mensaje de error correspondiente
                newUser.Message = "Ocurrió un error inesperado: " + ex.Message;
                return View(newUser); // Devuelve la vista con el mensaje de error
            }
        }
    }
}
