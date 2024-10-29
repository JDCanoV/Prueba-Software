using GastroByte.Dtos;
using GastroByte.Services;
using System.Web.Mvc;

namespace GastroByte.Controllers
{
    public class MenuController : Controller
    {
        private readonly MenuService _menuService;

        public MenuController()
        {
            _menuService = new MenuService();
        }

        // Acción para mostrar la vista principal con la lista de menús
        public ActionResult Index()
        {
            var menu = _menuService.GetAllMenus();
            return View(menu);
        }

        // Acción GET para mostrar el formulario de creación
        public ActionResult Create()
        {
            return View();
        }

        // Acción POST para crear un nuevo platillo
        [HttpPost]
        [ValidateAntiForgeryToken] // Atributo para validar el token
        public ActionResult Create(MenuDto menu)
        {
            if (ModelState.IsValid)
            {
                var createdMenu = _menuService.CreateMenu(menu);
                if (createdMenu.Response == 1)
                {
                    TempData["SuccessMessage"] = "Platillo creado correctamente.";
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", createdMenu.Message);
            }
            return View(menu);
        }


        // Acción para cargar los datos de un platillo específico para editar
        public ActionResult Edit(int id)
        {
            var menu = _menuService.GetMenuById(id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            return View(menu);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MenuDto menu)
        {
            if (!ModelState.IsValid)
            {
                return View(menu);
            }

            var updatedMenu = _menuService.UpdateMenu(menu);

            if (updatedMenu != null)
            {
                TempData["SuccessMessage"] = "Menu actualizado correctamente.";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Error al actualizar el menu.");
            return View(menu);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var isDeleted = _menuService.DeleteMenu(id);
            if (isDeleted)
            {
                TempData["SuccessMessage"] = "Platillo eliminado correctamente.";
            }
            else
            {
                TempData["ErrorMessage"] = "Error al eliminar el platillo.";
            }
            return RedirectToAction("Index");
        }

    }
}
