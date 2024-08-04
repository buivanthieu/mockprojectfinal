using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebMVC.Services;
using Programme = WebMVC.Models.Programme;

namespace WebMVC.Controllers;
public class ProgrammeController : Controller
{
    private readonly IProgrammeService _programmeService;

    public ProgrammeController(IProgrammeService programmeService)
    {
        _programmeService = programmeService;
    }

    public async Task<IActionResult> Index()
    {
        var programmes = await _programmeService.GetAllProgrammesAsync();
        return View(programmes);
    }

    public async Task<IActionResult> Details(int id)
    {
        var programme = await _programmeService.GetProgrammeByIdAsync(id);
        return View(programme);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        try
        {
            var contacts = await _programmeService.GetAllContactsAsync();
            ViewBag.Contacts = new SelectList(contacts, "Id", "Firstname");
            return View();
        }
        catch (HttpRequestException ex)
        {
            // Log the exception or handle it as needed
            Console.WriteLine($"Error retrieving contacts: {ex.Message}");
            ModelState.AddModelError(string.Empty, "Error retrieving contacts. Please try again later.");
            return View();
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Programme programme)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await _programmeService.CreateProgrammeAsync(programme);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                ModelState.AddModelError(string.Empty, "An error occurred while creating the programme. Please try again later.");
            }
        }

        var contacts = await _programmeService.GetAllContactsAsync();
        ViewBag.Contacts = new SelectList(contacts, "Id", "Firstname");
        return View(programme);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var programme = await _programmeService.GetProgrammeByIdAsync(id);
        if (programme == null)
        {
            return NotFound();
        }

        var contacts = await _programmeService.GetAllContactsAsync();
        ViewBag.Contacts = new SelectList(contacts, "Id", "Firstname");
        return View(programme);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Programme programme)
    {
        if (id != programme.Id)
        {
            return BadRequest();
        }

        if (ModelState.IsValid)
        {
            try
            {
                await _programmeService.UpdateProgrammeAsync(id, programme);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while updating the programme. Please try again later.");
            }
        }

        var contacts = await _programmeService.GetAllContactsAsync();
        ViewBag.Contacts = new SelectList(contacts, "Id", "Firstname");
        return View(programme);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var programme = await _programmeService.GetProgrammeByIdAsync(id);
        if (programme == null)
        {
            return NotFound();
        }
        return View(programme);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _programmeService.DeleteProgrammeAsync(id);
        return RedirectToAction(nameof(Index));
    }
}