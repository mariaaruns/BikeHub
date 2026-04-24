using BikeHub.DapperQuery;
using BikeHub.Repository.IRepository;
using BikeHub.Shared.Common;
using BikeHub.Shared.Dto.Response;
using BikeHub.Shared.Dto.Response.ServiceRes;
using Dapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;
using System.Net.NetworkInformation;


namespace BikeHub.Repository
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly IDbConnection _dbConnection;
        public ServiceRepository(IDbConnection dbConnection)
        {
            this._dbConnection = dbConnection;
        }
        public async Task AssignNewJobAsync(CreateJobAssignmentDto dto)
        {
            try
            {
                var sql = ServiceQuery.CreateAndAssignJob;
                var parameters = new
                {
                    @CustomerId = dto.CustomerId,
                    @StoreId = dto.StoreId,
                    @BikeModel = dto.BikeModel,
                    @BikeNumber = dto.BikeNumber,
                    @ProblemDescription = dto.ProblemDescription,
                    @ServiceStatus = dto.ServiceStatus,
                    @EstimatedCost = dto.EstimatedCost,
                    @FinalCost = dto.FinalCost,
                    @CreatedBy = dto.CreatedBy,
                    @CreatedDate = dto.CreatedDate,
                    @MechanicId = dto.MechanicId,
                    @AssignedDate = dto.AssignedDate,
                    @EstimatedDuration = dto.EstimatedDuration,
                    @AssignmentStatus = dto.AssignmentStatus,
                    @AssignedBy = dto.CreatedBy,
                    @StartTime = dto.StartTime
                };
                using (var connection = new SqlConnection(_dbConnection.ConnectionString))
                {
                    await connection.ExecuteAsync(sql, parameters);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task CompleteJobAsync(int jobId)
        {
            var customer = (string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
            try
            {
                var sql = ServiceQuery.UpdateInprogressToCompletedStatus;
                var EmailTemplateSql = EmailTemplateQuery.EmailTemplate;
                var outboxSql = EmailTemplateQuery.InsertOutBoxMsg;

                var parameters = new
                {
                    @serviceJobId = jobId
                };
                using (var connection = new SqlConnection(_dbConnection.ConnectionString))
                {
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            customer = await connection.ExecuteScalarAsync<(string, string, string, string, string)>(sql, parameters, transaction: transaction);

                            if (!string.IsNullOrEmpty(customer.Item2))
                            {
                                var emailTemplate = await connection.QueryFirstOrDefaultAsync<(string Subject, string HtmlBody)>(EmailTemplateSql, new { @slugName = "Bike-Repair-Delivery" }, transaction);


                                var htmlBody = emailTemplate.HtmlBody.Replace("{BikeModel}", customer.Item4)
                                                                                  .Replace("{JobCardNumber}", customer.Item3);

                                //Insert into outbox for email notification

                                await connection.ExecuteAsync(outboxSql, new
                                {
                                    @eventType = "ServiceJobCompleted",
                                    @payLoad = System.Text.Json.JsonSerializer.Serialize(new OutBoxMessagePayload
                                                {

                                                    Email = customer.Item2,
                                                    CustomerName = customer.Item1,
                                                    Subject = emailTemplate.Subject,
                                                    TemplateContent = htmlBody

                                                })
                                }, transaction);
                            }
                            await transaction.CommitAsync();
                        }
                        catch
                        {
                            await transaction.RollbackAsync();
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<IEnumerable<TodayJobFeed>> GetDailyJobsByStatusAsync(DateTime? date, int? serviceStatus)
        {
            try
            {
                var sql = ServiceQuery.GetTodayJobFeed;

                var parameters = new
                {
                    @serviceStatus = serviceStatus
                };

                using (var connection = new SqlConnection(_dbConnection.ConnectionString))
                {
                    return await connection.QueryAsync<TodayJobFeed>(sql, parameters);
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ServiceJobDetailDto> GetJobByIdAsync(int jobId)
        {
            try
            {
                var sql = ServiceQuery.GetJobDetails;
                var parameters = new
                {

                    @serviceJobId = jobId
                };
                using (var connection = new SqlConnection(_dbConnection.ConnectionString))
                {
                    return await connection.QueryFirstOrDefaultAsync<ServiceJobDetailDto>(sql, parameters);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<MechanicLiveStatsDto> GetLiveMechanicStatsAsync()
        {
            try
            {
                var sql = ServiceQuery.GetMechanicLiveStats;
                using (var connection = new SqlConnection(_dbConnection.ConnectionString))
                {
                    return await connection.QueryFirstOrDefaultAsync<MechanicLiveStatsDto>(sql);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<AssignedJobDto>> GetMechanicAssignedJobsAsync(int mechanicId)
        {
            try
            {
                var sql = ServiceQuery.GetMechanicAssignedJobsQuery;
                var parameters = new
                {

                    @mechanicId = mechanicId
                };
                using (var connection = new SqlConnection(_dbConnection.ConnectionString))
                {
                    return await connection.QueryAsync<AssignedJobDto>(sql, parameters);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<MechanicCurrentStatus>> GetMechanicStatusAsync()
        {
            try
            {
                var sql = ServiceQuery.GetMechanicCurrentStatus;

                using (var connection = new SqlConnection(_dbConnection.ConnectionString))
                {
                    return await connection.QueryAsync<MechanicCurrentStatus>(sql);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<MechanicTaskSummayDto> GetMechanicWorkSummaryAsync(int mechanicId)
        {
            try
            {
                var sql = ServiceQuery.GetMechanicTaskSummary;
                var parameters = new
                {

                    @mechanicId = mechanicId
                };
                using (var connection = new SqlConnection(_dbConnection.ConnectionString))
                {
                    return await connection.QueryFirstOrDefaultAsync<MechanicTaskSummayDto>(sql, parameters);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ServiceItemDto> GetServiceItemsAsync(int jobId)
        {
            try
            {
                var sql = ServiceQuery.GetServiceItemsByServiceJobId;
                var parameters = new
                {
                    @jobId = jobId
                };
                using (var connection = new SqlConnection(_dbConnection.ConnectionString))
                {
                    return (await connection.QueryFirstOrDefaultAsync<ServiceItemDto>(sql, parameters));
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<DropdownDto>> GetServiceStatusDropdownAsync()
        {
            try
            {
                var sql = ServiceQuery.ServiceStatusDropdown;
                using (var connection = new SqlConnection(_dbConnection.ConnectionString))
                {
                    return (await connection.QueryAsync<DropdownDto>(sql));
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task StartJobAsync(int jobId)
        {
            try
            {
                var sql = ServiceQuery.UpdatePendingToInProgressStatus;
                var parameters = new
                {
                    @serviceJobId = jobId
                };
                using (var connection = new SqlConnection(_dbConnection.ConnectionString))
                {
                    await connection.ExecuteAsync(sql, parameters);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task UpdateJobStatusAsync(int status, int jobId)
        {
            try
            {
                var sql = ServiceQuery.UpdateServiceStatus;

                var parameters = new
                {
                    @serviceStatus = status,
                    @serviceJobId = jobId
                };

                using (var connection = new SqlConnection(_dbConnection.ConnectionString))
                {
                    await connection.ExecuteAsync(sql, parameters);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
