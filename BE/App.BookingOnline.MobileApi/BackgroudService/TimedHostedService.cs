using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using App.BookingOnline.Data;
using App.BookingOnline.Data.Models;
using App.BookingOnline.Service.IService.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using static App.Core.Enums;

namespace App.BookingOnline.MobileApi.BackgroudService
{
    public class TimedHostedService : IHostedService, IDisposable
    {
        private readonly ILogger<TimedHostedService> logger;

        private readonly IServiceProvider services;

        private Timer _timer;

        private Task _executingTask;

        private CancellationTokenSource _stoppingCts;
        public IConfiguration Configuration { get; }
        public TimedHostedService(IServiceProvider services, IConfiguration configuration,
            ILogger<TimedHostedService> logger) =>
            (this.services, this.Configuration, this.logger) = (services, configuration, logger);

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _stoppingCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            var lockTeetime = Configuration.GetSection("loginTime").GetValue<int>("TimeServiceInterval");
            _timer = new Timer(FireTask, null, TimeSpan.FromSeconds(lockTeetime), TimeSpan.FromSeconds(lockTeetime));

            return Task.CompletedTask;
        }

        private void FireTask(object state)
        {
            if (_executingTask == null || _executingTask.IsCompleted)
            {
                //logger.LogInformation("[BackgroundService] No task is running, check for new job");
                _executingTask = ExecuteNextJobAsync(_stoppingCts.Token);
            }
            else
            {
                // logger.LogInformation("[BackgroundService] There is a task still running, wait for next cycle");
            }
        }

        private async Task ExecuteNextJobAsync(CancellationToken cancellationToken)
        {
            using var scope = services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<BookingOnlineDbContext>();
            ISmsHistoryService smsHistoryService = services.CreateScope().ServiceProvider.GetRequiredService<ISmsHistoryService>();
            // whatever logic to retrieve the next job
            var jobData = context.Set<NotificationQueue>().Where(x => !x.IsSend.Value && x.IsActive.Value);
            // var adfads = smsHistoryService.SendEmailBookingCourse(new Guid("CCC43B8E-1E6B-461E-BC40-6DF2290C7D3E"));
            if (jobData == null || !jobData.Any())
            {
                return;
            }
            else
            {
                foreach (var item in jobData)
                {
                    if (item.NotificationType != FcmNotifiType.SendEmailBookingCourse.ToString())
                    {
                        if (item.SendDate <= DateTime.Now)
                        {
                            var result = smsHistoryService.PushNotificationToDevice(new NotificationQueue
                            {
                                SendTo = item.SendTo,
                                Title = item.Title,
                                Content = item.Content,
                                Img_url = item.Img_url,
                                Id = item.Id
                            });
                            item.IsSuccess = result;
                            item.IsSend = true;
                            item.CompletedDate = DateTime.Now;
                            item.UpdatedDate = DateTime.Now;
                            item.UpdatedUser = "TimedHostedService";
                        }
                    }
                    else
                    {
                        var result = smsHistoryService.SendEmailBookingCourse(item.BookingId);
                        item.IsSuccess = result;
                        item.IsSend = true;
                        item.CompletedDate = DateTime.Now;
                        item.UpdatedDate = DateTime.Now;
                        item.UpdatedUser = "TimedHostedService";
                    }

                    context.Update(item);
                    context.Entry(item).State = EntityState.Modified;
                }
                context.SaveChanges();
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _timer.Change(Timeout.Infinite, 0);

            if (_executingTask == null || _executingTask.IsCompleted)
            {
                return;
            }
            try
            {
                _stoppingCts.Cancel();
            }
            finally
            {
                await Task.WhenAny(_executingTask, Task.Delay(Timeout.Infinite, cancellationToken));
            }
        }

        public void Dispose()
        {
            _timer.Dispose();
            _stoppingCts?.Cancel();
        }
    }
}