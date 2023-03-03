using Microsoft.EntityFrameworkCore;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net;
using System.Net.Mail;
using UpscaleTechnicalTest.Core;
using UpscaleTechnicalTest.Data;
using UpscaleTechnicalTest.Models;

namespace UpscaleTechnicalTest.Services;

public class TodoService : ITodoService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<BackgroundService> _logger;
    private readonly IConfiguration _configuration;

    public TodoService(ApplicationDbContext context, ILogger<BackgroundService> logger, IConfiguration configuration)
    {
        _context = context;
        _logger = logger;
        _configuration = configuration;
    }

    public async Task<Result<TodoModel, string>> Get(int? id)
    {
        if (id == null)
            return new Result<TodoModel, string>("Data not found");

        var todoModel = await _context.Todos
            .FirstOrDefaultAsync(m => m.Id == id);
        if (todoModel == null)
        {
            return new Result<TodoModel, string>("Data not found");
        }

        return new Result<TodoModel, string>(todoModel);
    }

    public async Task<Result<IEnumerable<TodoModel>, string>> List()
    {
        var todoModel = await _context.Todos.ToListAsync();
        return new Result<IEnumerable<TodoModel>, string>(todoModel);
    }

    public async Task<Result<TodoModel, string>> Save(TodoModel todoModel)
    {
        todoModel.CreatedDate = DateTime.Now;
        _context.Add(todoModel);
        await _context.SaveChangesAsync();
        return new Result<TodoModel, string>(todoModel);
    }

    public async Task<Result<TodoModel, string>> Update(TodoModel todoModel)
    {
        try
        {
            var exItem = await _context.Todos.AsNoTracking().FirstOrDefaultAsync(w => w.Id.Equals(todoModel.Id));
            if (exItem is null)
                return new Result<TodoModel, string>("Data not found");

            todoModel.UpdatedDate = DateTime.Now;
            _context.Update(todoModel);
            _context.Entry(todoModel).State = EntityState.Modified;
            _context.Entry(todoModel).Property(x => x.CreatedDate).IsModified = false;
            _context.Entry(todoModel).Property(x => x.EmailNotification).IsModified = false;
            _context.Entry(todoModel).Property(x => x.IsNotificationSend).IsModified = false;

            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return new Result<TodoModel, string>(ex.Message);
        }
        return new Result<TodoModel, string>(todoModel);
    }

    public async Task<Result<bool, string>> Delete(int id)
    {
        var exItem = await _context.Todos.AsNoTracking().FirstOrDefaultAsync(w => w.Id.Equals(id));
        if (exItem is null)
            return new Result<bool, string>("Data not found");

        _context.Todos.Remove(exItem);
        await _context.SaveChangesAsync();
        return new Result<bool, string>(true);
    }

    public async Task<Result<TodoModel, string>> MarkAsCompleted(int id)
    {
        var todoModel = await _context.Todos.FindAsync(id);
        if (todoModel is null)
            return new Result<TodoModel, string>("Data not found");

        todoModel.IsCompleted = true;
        _context.Todos.Update(todoModel);
        await _context.SaveChangesAsync();

        return new Result<TodoModel, string>(todoModel);
    }

    public async Task<Result<bool, string>> SendExpiredItemNotification()
    {
        var senderEmailProvider = _configuration.GetSection("SenderEmailProvider").Value;
        var sendgridAPIKey = _configuration.GetSection("SendgridAPIKey").Value;
        var sendGridClient = new SendGridClient(sendgridAPIKey);
        var sendgridFrom = new EmailAddress(_configuration.GetSection("SendgridEmailFrom").Value, _configuration.GetSection("SendgridEmailName").Value);
        var googleSMTPEmailAddress = _configuration.GetSection("GoogleSMTPEmailAddress").Value;
        var googleSMTPEmailPassword = _configuration.GetSection("GoogleSMTPEmailPassword").Value;
        var googleSMTPClient = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            Credentials = new NetworkCredential(googleSMTPEmailAddress, googleSMTPEmailPassword),
            EnableSsl = true,
        };

        //get expired or nearly expired items
        IQueryable<TodoModel> query = _context.Set<TodoModel>();
        var items = await query.Where(w => w.Deadline <= DateTime.Now.AddDays(1) && (w.IsCompleted == null || !w.IsCompleted.Value) && !w.IsNotificationSend).ToListAsync();

        foreach (var item in items)
        {
            var subject = "Please check your task";
            var plainTextContent = $"Your task {item.Title} will expire at {item.Deadline:dd-MMM-yyyy HH:mm}";
            var htmlContent = $"<strong>Your task {item.Title} will expire at {item.Deadline:dd-MMM-yyyy HH:mm}</strong>";

            //send notification email
            if (!string.IsNullOrEmpty(item.EmailNotification))
            {
                _logger.LogInformation("Trying to send email notification to {email}", item.EmailNotification);

                if (senderEmailProvider.Equals("GoogleSMTP", StringComparison.InvariantCultureIgnoreCase))
                {
                    try
                    {
                        var mailMessage = new MailMessage
                        {
                            From = new MailAddress(googleSMTPEmailAddress),
                            To = { item.EmailNotification },
                            Subject = subject,
                            Body = htmlContent,
                            IsBodyHtml = true,
                        };

                        googleSMTPClient.Send(mailMessage);
                        _logger.LogInformation("Email successfully send at: {time}", DateTimeOffset.Now);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("Error while sending email: {error}", ex.Message);
                    }
                }
                else if (senderEmailProvider.Equals("Sendgrid", StringComparison.InvariantCultureIgnoreCase))
                {
                    var msg = MailHelper.CreateSingleEmail(sendgridFrom, new EmailAddress(item.EmailNotification, item.EmailNotification), subject, plainTextContent, htmlContent);
                    var response = await sendGridClient.SendEmailAsync(msg);

                    if (response.IsSuccessStatusCode)
                        _logger.LogInformation("Email successfully send at: {time}", DateTimeOffset.Now);
                    else
                        _logger.LogError("Error while sending email: {error}", await response.Body.ReadAsStringAsync());
                }
            }

            //update item
            item.IsNotificationSend = true;
            _context.Todos.Update(item);
            await _context.SaveChangesAsync();
        }

        return new Result<bool, string>(true);
    }

}