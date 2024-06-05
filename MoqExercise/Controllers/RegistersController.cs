using Microsoft.AspNetCore.Mvc;
using MoqExercise.Models;
using MoqExercise.Repositories;

namespace MoqExercise.Controllers;

public class RegistersController(IRegisterRepository repo) : Controller
{
    private readonly IRegisterRepository _repo = repo;

    public IActionResult Create() =>
        View();

    public async Task<IActionResult> Create(Register register)
    {
        if (!ModelState.IsValid)
            return View();

        await _repo.CreateAsync(register);
        return RedirectToAction("Read");
    }

    public async Task<IActionResult> Read()
    {
        var registers = await _repo.ListAsync();
        return View(registers);
    }

    public async Task<IActionResult> Update(int id)
    {
        var register = await _repo.GetByIdAsync(id);
        return View(register);
    }

    [HttpPost]
    public async Task<IActionResult> Update(Register register)
    {
        if (ModelState.IsValid)
        {
            await _repo.UpdateAsync(register);
            ViewBag.Result = "Success";
        }
        return View(register);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        await _repo.DeleteAsync(id);
        return RedirectToAction("Read");
    }
}
