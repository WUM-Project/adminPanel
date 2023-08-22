using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Admin_Panel.Services;
using Admin_Panel.Interfaces;
using Admin_Panel.Models;
namespace Admin_Panel.Controllers
{
    public class AttributesController : Controller
    {
       private readonly IServiceManager _serviceManager;

        public AttributesController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken = default)
        {  
          
          var test =await _serviceManager.AttributeServices.GetAllAsync(cancellationToken);
            Console.WriteLine("====================================");
            Console.WriteLine(test);
            Console.WriteLine("================================");
            
            return View(await _serviceManager.AttributeServices.GetAllAsync());
        }

        public async Task<IActionResult> Details(int? id,CancellationToken cancellationToken = default)
        {
            if (id == null)
            {
                return NotFound();
            }
              Console.WriteLine("================================");
            Console.WriteLine(id.Value);
              Console.WriteLine("================================");
            var attribute = await _serviceManager.AttributeServices.GetByIdAsync(id.Value,cancellationToken);
             Console.WriteLine("================================");
            Console.WriteLine(attribute);
              Console.WriteLine("================================");
            if (attribute == null)
            {
                return NotFound();
            }

            return View(attribute);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Models.Attribute attribute)
        {
            if (ModelState.IsValid)
            {
                await _serviceManager.AttributeServices.Create(attribute);
                return RedirectToAction(nameof(Index));
            }
            return View(attribute);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attribute = await _serviceManager.AttributeServices.GetByIdAsync(id.Value);
            if (attribute == null)
            {
                return NotFound();
            }
            return View(attribute);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Models.Attribute attribute)
        {
            if (id != attribute.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _serviceManager.AttributeServices.Update(attribute);
                return RedirectToAction(nameof(Index));
            }
            return View(attribute);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attribute = await _serviceManager.AttributeServices.GetByIdAsync(id.Value);
            if (attribute == null)
            {
                return NotFound();
            }

            return View(attribute);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _serviceManager.AttributeServices.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}