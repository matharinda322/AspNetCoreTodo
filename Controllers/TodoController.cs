using System;
using System.Threading.Tasks;
using AspNetCoreTodo.Models;
using AspNetCoreTodo.Services;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreTodo.Controllers
{
    public class TodoController:Controller
    {
        private readonly ITodoItemService _todoItemservice;

        public TodoController(ITodoItemService todoItemservice)
        {
            this._todoItemservice = todoItemservice;
        }
        public async Task<IActionResult> Index()
        {
            var itemServices = await _todoItemservice.GetIncompleteItemsAsync();

            var model = new TodoViewModel()
            {
                Items = itemServices
            };

            return View(model);
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItem(TodoItem newItem)
        {
            if(!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            var successful = await this._todoItemservice.AddItemAsync(newItem);

            if(!successful)
            {
                return BadRequest("Could not add item");
            }

            return RedirectToAction("Index");
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkDone(Guid id)
        {
            if(id == Guid.Empty)
            {
                return RedirectToAction("Index");
            }

            var successful = await _todoItemservice.MarkDoneAsync(id);

            if(!successful)
            {
                return BadRequest("Could not mark item as done");
            }
            return RedirectToAction("Index");
        }
    }
}