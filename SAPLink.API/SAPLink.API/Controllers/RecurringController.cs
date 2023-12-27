using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SAPLink.API.Services;

namespace SAPLink.API.Controllers
{
    public partial class RecurringController : Controller
    {
        private readonly RecurrenceService _recurrenceService;

        public RecurringController(RecurrenceService recurrenceService)
        {
            _recurrenceService = recurrenceService;
        }

        public IActionResult AddAsync()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAsync(RecurrenceDto dto)
        {
            if (ModelState.IsValid)
            {
                await _recurrenceService.AddAsync(dto);
                return RedirectToAction("GetAll");
            }

            return View(dto);
        }

        public async Task<IActionResult> GetAllAsync()
        {
            var recurrences = await _recurrenceService.GetAllAsync();
            return View(recurrences);
        }

        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var recurrence = await _recurrenceService.GetByIdAsync(id);

            if (recurrence == null)
                return NotFound();

            return View(recurrence);
        }

        public async Task<IActionResult> UpdateAsync(int id)
        {
            var recurrence = await _recurrenceService.GetByIdAsync(id);

            if (recurrence == null)
                return NotFound();

            return View(recurrence);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateAsync(int id, Recurrence recurrence)
        {
            if (id != recurrence.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                _recurrenceService.Update(recurrence);
                return RedirectToAction("GetAll");
            }

            return View(recurrence);
        }

        public IActionResult DeleteAsync(int id)
        {
            return View(id);
        }

        [HttpPost, ActionName("DeleteAsync")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _recurrenceService.Delete(id);
            return RedirectToAction("GetAll");
        }

    }
}