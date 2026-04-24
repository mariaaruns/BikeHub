using Microsoft.Identity.Client;

namespace BikeHub.DapperQuery
{
    public class EmailTemplateQuery
    {
        //Order-Ready-Delivery
        //Bike-Repair-Delivery
        public const string EmailTemplate= @"select top 1 Subject,HtmlBody from messaging.emailTemplates 
                                             where Slug=@slugName";


        public const string InsertOutBoxMsg = @"Insert into  Messaging.OutboxMessages (EventType,Payload) 
                                                    Values(@eventType,@payLoad)";

        public const string GetOutBoxMsg = @"SELECT TOP 10 * 
                                              FROM Messaging.OutboxMessages
                                              WHERE IsProcessed = 0 AND RetryCount < 3
                                              ORDER BY CreatedDate";
        
        public const string MarkProcessed = @"UPDATE OutboxMessages
                                                SET IsProcessed = 1,
                                                    IsProcessing = 0,
                                                    ProcessedDate = GETDATE()
                                                WHERE Id = @Id";

         public const string IncrementRetryCount = @"UPDATE Messaging.OutboxMessages
                                              SET RetryCount = RetryCount + 1
                                              WHERE Id = @Id";

        public const string LockAndGetMessages = @"WITH cte AS (
                                                        SELECT TOP (10) *
                                                        FROM Messaging.OutboxMessages
                                                        WHERE IsProcessed = 0
                                                          AND RetryCount < 3
                                                          AND (
                                                                IsProcessing = 0
                                                                OR LockedAt < DATEADD(MINUTE, -10, GETDATE())
                                                              )
                                                        ORDER BY createdAt
                                                    )
                                                    UPDATE cte
                                                    SET IsProcessing = 1,
                                                        LockedAt = GETDATE()
                                                    OUTPUT INSERTED.*;";


    }
}
