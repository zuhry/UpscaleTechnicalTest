using UpscaleTechnicalTest.Core;
using UpscaleTechnicalTest.Models;

namespace UpscaleTechnicalTest.Services;

public interface ITodoService
{
    Task<Result<TodoModel, string>> Get(int? id);
    Task<Result<IEnumerable<TodoModel>, string>> List();
    Task<Result<TodoModel, string>> Save(TodoModel todoModel);
    Task<Result<TodoModel, string>> Update(TodoModel todoModel);
    Task<Result<bool, string>> Delete(int id);
    Task<Result<TodoModel, string>> MarkAsCompleted(int id);
    Task<Result<bool, string>> SendExpiredItemNotification();
}