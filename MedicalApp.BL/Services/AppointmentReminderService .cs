using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedicalApp.DA.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace MedicalApp.BL.Services
{
    public class AppointmentReminderService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        public AppointmentReminderService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                    var now = DateTime.Now;
                    var next24Hours = now.AddHours(24);

                    // تجيب المواعيد الجاية خلال الـ 24 ساعة
                    var upcomingAppointments = await context.Appointments
                        .Include(a => a.Patient)
                        .Include(a => a.Doctor)
                        .Where(a => a.AppointmentDate >= now && a.AppointmentDate <= next24Hours
                                    && a.Status == DA.Enums.Status.Scheduled)
                        .ToListAsync(stoppingToken);

                    foreach (var appt in upcomingAppointments)
                    {
                        // هنا بنسجل لوج بدل ما نبعت إيميل حقيقي
                        Log.Information("Reminder: Appointment {AppointmentId} for Patient {PatientEmail} with Doctor {DoctorEmail} at {Date}",
                            appt.Id, appt.Patient.User.Email, appt.Doctor.User.Email, appt.AppointmentDate);
                    }
                }

                // ينتظر ساعة قبل المرة الجاية
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }
    }
}

