using GastroByte.Dtos;
using GastroByte.Services;
using System;
using System.Web.Mvc;

namespace GastroByte.Controllers
{
    public class UsuarioController : Controller
    {
        // Vista principal de Usuario
        public ActionResult Index()
        {
            return View();
        }

        // Vista de Login (GET)
        public ActionResult Login(string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl; // Almacena la URL de retorno para redirigir después del login
            return View();
        }

        // Procesamiento del Login (POST)
        [HttpPost]
        public ActionResult Login(UsuarioDto loginUser, string returnUrl = null)
        {
            if (loginUser == null)
            {
                loginUser = new UsuarioDto { Message = "El modelo de usuario no se envió correctamente." };
                return View(loginUser);
            }

            try
            {
                var userService = new UsuarioService();
                var userResponse = userService.LoginUser(loginUser);

                if (userResponse.Response == 1)
                {
                    // Establece la sesión con los datos del usuario
                    Session["UserID"] = userResponse.id_usuario;
                    Session["UserName"] = userResponse.nombre;
                    Session["UserRole"] = userResponse.id_rol;

                    // Redirige al returnUrl si está definido, o a la página principal
                    if (!string.IsNullOrEmpty(returnUrl))
                        return Redirect(returnUrl);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    loginUser.Message = "Credenciales incorrectas. Inténtalo nuevamente.";
                    return View(loginUser);
                }
            }
            catch (Exception ex)
            {
                loginUser.Message = "Ocurrió un error inesperado: " + ex.Message;
                return View(loginUser);
            }
        }

        // Método para cerrar sesión
        public ActionResult Logout()
        {
            Session.Clear(); // Limpia toda la sesión del usuario
            return RedirectToAction("Login", "Usuario");
        }

        // Vista de creación de usuario (GET)
        public ActionResult Create()
        {
            return View(new UsuarioDto { Response = 0, Message = string.Empty });
        }

        // Procesamiento de creación de usuario (POST)
        [HttpPost]
        public ActionResult Create(UsuarioDto newUser)
        {
            if (newUser == null)
            {
                newUser = new UsuarioDto { Message = "El modelo de usuario no se envió correctamente." };
                return View(newUser);
            }

            try
            {
                var userService = new UsuarioService();
                var userResponse = userService.CreateUser(newUser);

                if (userResponse.Response == 1)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    if (userResponse.Message == null)
                    {
                        userResponse.Message = "Error al crear el usuario. Por favor, inténtalo nuevamente.";
                    }
                    return View(userResponse);
                }
            }
            catch (Exception ex)
            {
                newUser.Message = "Ocurrió un error inesperado: " + ex.Message;
                return View(newUser);
            }
        }
    }
}
