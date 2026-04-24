using BikeHub.DapperQuery;
using BikeHub.Service.Interface;
using BikeHub.Shared.Common;
using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using System.Data;
using System.Text.Json;

namespace BikeHub.BackgroudProcessor
{
    public class OutboxProcessor : BackgroundService
    {

        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IEmailService _emailService;

        public OutboxProcessor(IServiceScopeFactory scopeFactory, IEmailService emailService)
        {
            _scopeFactory = scopeFactory;
            _emailService = emailService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _scopeFactory.CreateScope();
                var connection = scope.ServiceProvider.GetRequiredService<IDbConnection>();

                 connection.Open();

                List<OutBoxMessage> messages;

                using (var transaction = connection.BeginTransaction())
                {
                    //lock and get messages in one step to avoid race conditions
                    messages = (await connection.QueryAsync<OutBoxMessage>(
                        EmailTemplateQuery.LockAndGetMessages, 
                        transaction: transaction)).ToList();

                    transaction.Commit();
                }

                foreach (var msg in messages)
                {
                    try
                    {
                        var data = JsonSerializer.Deserialize<OutBoxMessagePayload>(msg.Payload);

                        if (data == null) continue;

                        await _emailService.SendAsync(data.Email, data.Subject, data.TemplateContent);

                        await connection.ExecuteAsync(
                            EmailTemplateQuery.MarkProcessed,
                            new { msg.Id });
                    }
                    catch
                    {
                        await connection.ExecuteAsync(
                            EmailTemplateQuery.IncrementRetryCount,
                            new { msg.Id });
                    }
                }

                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
