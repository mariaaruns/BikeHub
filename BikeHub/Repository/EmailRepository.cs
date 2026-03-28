using BikeHub.DapperQuery;
using BikeHub.Repository.IRepository;
using BikeHub.Shared.Dto.Response;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BikeHub.Repository
{
    public class EmailRepository : IEmailRepository
    {
        private readonly IDbConnection _dbConnection;

        public EmailRepository(IDbConnection dbConnection)
        {
            this._dbConnection = dbConnection;
        }

        
        public async Task<(string Subject, string HtmlBody)> GetEmailTemplateBySlug(string slugName)
        {
            try
            {
                var sql = EmailTemplateQuery.EmailTemplate;
                var parameters = new
                {
                    @slugName = slugName
                };
                using (var connection = new SqlConnection(_dbConnection.ConnectionString))
                {
                    var result = await connection.QueryFirstOrDefaultAsync<(string Subject,string HtmlBody)>(sql, parameters);

                    return result;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
