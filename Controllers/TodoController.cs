using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UpscaleTechnicalTest.Models;
using UpscaleTechnicalTest.Services;

namespace UpscaleTechnicalTest.Controllers;

[Authorize]
public class TodoController : Controller
{
    private readonly ITodoService _service;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    public TodoController(ITodoService service, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
    {
        _service = service;
        _signInManager = signInManager;
        _userManager = userManager;
    }

    // GET: Todo
    public async Task<IActionResult> Index()
    {
        var getList = await _service.List();
        if (getList.Error.status)
            return Problem(getList.Error.error);

        return View(getList.Successful.data);
    }

    // GET: Todo/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var get = await _service.Get(id);
        if (get.Error.status)
            if (get.Error.error == "Data not found")
                return NotFound();
            else
                return Problem(get.Error.error);

        return View(get.Successful.data);
    }

    // GET: Todo/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Todo/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Title,Description,Scope,Priority,Deadline,IsCompleted")] TodoModel todoModel)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByNameAsync(User.Identity?.Name!);
            todoModel.EmailNotification = user?.Email!;

            var save = await _service.Save(todoModel);
            if (save.Error.status)
                if (save.Error.error == "Data not found")
                    return NotFound();
                else
                    return Problem(save.Error.error);

            return RedirectToAction(nameof(Index));
        }
        return View(todoModel);
    }

    // GET: Todo/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var get = await _service.Get(id);
        if (get.Error.status)
            if (get.Error.error == "Data not found")
                return NotFound();
            else
                return Problem(get.Error.error);

        return View(get.Successful.data);
    }

    // POST: Todo/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Scope,Priority,Deadline,IsCompleted")] TodoModel todoModel)
    {
        if (id != todoModel.Id)
            return NotFound();

        if (ModelState.IsValid)
        {
            var update = await _service.Update(todoModel);
            if (update.Error.status)
                if (update.Error.error == "Data not found")
                    return NotFound();
                else
                    return Problem(update.Error.error);
            return RedirectToAction(nameof(Index));
        }
        return View(todoModel);
    }

    // GET: Todo/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var get = await _service.Get(id);
        if (get.Error.status)
            if (get.Error.error == "Data not found")
                return NotFound();
            else
                return Problem(get.Error.error);

        return View(get.Successful.data);
    }

    // POST: Todo/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var delete = await _service.Delete(id);
        if (delete.Error.status)
            if (delete.Error.error == "Data not found")
                return NotFound();
            else
                return Problem(delete.Error.error);

        return RedirectToAction(nameof(Index));
    }

    // GET: Todo/MarkAsCompleted/5
    public async Task<IActionResult> MarkAsCompleted(int? id)
    {
        if (id == null)
            return NotFound();

        var get = await _service.Get(id);
        if (get.Error.status)
            if (get.Error.error == "Data not found")
                return NotFound();
            else
                return Problem(get.Error.error);

        return View(get.Successful.data);
    }

    // POST: Todo/Delete/5
    [HttpPost, ActionName("MarkAsCompleted")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MarkAsCompletedConfirmed(int id)
    {
        var markAsCompleted = await _service.MarkAsCompleted(id);
        if (markAsCompleted.Error.status)
            if (markAsCompleted.Error.error == "Data not found")
                return NotFound();
            else
                return Problem(markAsCompleted.Error.error);

        return RedirectToAction(nameof(Index));
    }
}