using System;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreTodo.Data;
using AspNetCoreTodo.Models;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreTodo.Services
{
    public class TodoItemService : ITodoItemService
    {
        private readonly ApplicationDbContext context;

        public TodoItemService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> AddItemAsync(TodoItem newItem)
        {
            newItem.Id = Guid.NewGuid();
            newItem.IsDone = false;
            newItem.DueAt = DateTimeOffset.Now.AddDays(3);

            this.context.Items.Add(newItem);
            var SaveResult = await this.context.SaveChangesAsync();
            return SaveResult == 1;
        }

        public async Task<TodoItem[]> GetIncompleteItemsAsync()
        {
            return await this.context.Items.Where(c => c.IsDone == false).ToArrayAsync();
        }

        public async Task<bool> MarkDoneAsync(Guid id)
        {
            var item = await this.context.Items.Where(x => x.Id == id).SingleOrDefaultAsync();

            if(item == null)return false;

            item.IsDone = true;

            var SaveResult =await this.context.SaveChangesAsync();

            return SaveResult == 1;
        }
    }
}