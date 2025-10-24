using Graduation_Project.Application.DTOs.NotificationDTOs;
using Graduation_Project.Application.Interfaces.IUnitOfWorks;
using Graduation_Project.Domain.Entities;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Application.Services
{
    public class SessionReminderService : BackgroundService
    {
        IEmailSender _emailSender;
        IServiceProvider _serviceProvider;
        public SessionReminderService(IEmailSender emailSender , IServiceProvider serviceProvider)
        {
            _emailSender = emailSender;
            _serviceProvider = serviceProvider;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
            
                    using var scope = _serviceProvider.CreateScope();
                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                    var now = DateTime.UtcNow;
                    var upcomingSessions = await unitOfWork.SessionRepo.GetAllAsQuerable()
                        .Include(s => s.Reservation.Client)
                        .Where(s =>
                            s.StartDateTime > now &&
                            s.StartDateTime <= now.AddHours(24) &&
                            s.ReservationId != null &&
                            !s.Reminded)
                        .ToListAsync(stoppingToken);

                    foreach (var session in upcomingSessions)
                    {
                        var email = session.Reservation.Client.User.Email;
                        var name = session.Reservation.Client.FirstName;
                        var time = session.StartDateTime.ToString("f");
                        var body = $"Dear {name},<br><br>This is a reminder for your session on <b>{time}</b>.<br>Thank you.";
                        await _emailSender.SendEmailAsync(email, "Session Reminder", body);
                    var notification = new CreateNotificationDTO
                    {
                        Title = "Upcoming Session Reminder",
                        Message = $"You have a session scheduled on {time}.",
                        Type = "Reminder",
                        Routing = "/client-profile/client-session", 
                        SenderId = session.Reservation.Client.Id,
                        Readed = false,
                    };
                    session.Reminded = true;
                        await unitOfWork.SaveChangesAsync();
                    }
       
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }


    }
}
