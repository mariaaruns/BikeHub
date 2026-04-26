using BikeHub.Repository;
using BikeHub.Repository.IRepository;
using BikeHub.Service.Interface;
using BikeHub.Shared.Common;
using BikeHub.Shared.Dto.Request;
using BikeHub.Shared.Dto.Request.ServiceReq;
using BikeHub.Shared.Dto.Response;
using BikeHub.Shared.Dto.Response.ServiceRes;
using Carter;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Ocsp;



namespace BikeHub.Features
{
    public class BikeServiceModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
{
            app.MapGet("/api/services/mechanic/live-stats", async (IServiceRepository _serviceRepository) =>
            {

                try
                {
                    var result = await _serviceRepository.GetLiveMechanicStatsAsync();
                    return Results.Ok(ApiResponse<MechanicLiveStatsDto>.Success(result));
                }
                catch (Exception)
                {
                    return Results.InternalServerError(ApiResponse<MechanicLiveStatsDto>.Fail("Internal server error..!"));
                }
            })
               .WithName("mechanic-live-stats")
               .WithTags("Bike-Services")
               .RequireAuthorization("SERVICE_DASHBOARD");

            app.MapGet("/api/services/mechanic/status", async (IServiceRepository _serviceRepository) =>
            {

                try
                {
                    //if (mechanicId is 0)
                    //    return Results.BadRequest(ApiResponse<MechanicCurrentStatus>.Fail("Invalid Mechanic"));

                    var result = await _serviceRepository.GetMechanicStatusAsync();
                    return Results.Ok(ApiResponse<IEnumerable<MechanicCurrentStatus>>.Success(result));
                }
                catch (Exception)
                {
                    return Results.InternalServerError(ApiResponse<IEnumerable<MechanicCurrentStatus>>.Fail("Internal server error..!"));
                }
            })
               .WithName("mechanic-status")
               .WithTags("Bike-Services");

            app.MapPatch("/api/services/start-job/{jobId:int}", async (int jobId, IServiceRepository _serviceRepository) =>
            {
                try
                {
                    if (jobId is 0)
                        return Results.BadRequest(ApiResponse<string>.Fail("Invalid job"));

                    await _serviceRepository.StartJobAsync(jobId);
                    return Results.Ok(ApiResponse<string>.Success("job started."));
                }
                catch (Exception)
                {
                    return Results.InternalServerError(ApiResponse<string>.Fail("Internal server error..!"));
                }
            })
               .WithName("move-start-job")
               .WithTags("Bike-Services");

            app.MapPatch("/api/services/complete-job/{jobId:int}", async (int jobId, 
                IServiceRepository _serviceRepository,IEmailRepository _emailRepository,IEmailService _emailService) =>
            {
                try
                {
                    if (jobId is 0)
                        return Results.BadRequest(ApiResponse<string>.Fail("Invalid job"));

                    await _serviceRepository.CompleteJobAsync(jobId);
                    return Results.Ok(ApiResponse<string>.Success("job completed."));
                }
                catch (Exception)
                {
                    return Results.InternalServerError(ApiResponse<string>.Fail("Internal server error..!"));
                }
            })
               .WithName("move-complete-job")
               .WithTags("Bike-Services");

            app.MapPatch("/api/services/update-job-status/{jobId:int}", async (int jobId, [FromBody] int statusId, [FromServices] IServiceRepository _serviceRepository) =>
            {
                try
                {
                    if (jobId is 0)
                        return Results.BadRequest(ApiResponse<string>.Fail("Invalid job"));

                    await _serviceRepository.CompleteJobAsync(jobId);
                    return Results.Ok(ApiResponse<string>.Success("job completed."));
                }
                catch (Exception)
                {
                    return Results.InternalServerError(ApiResponse<string>.Fail("Internal server error..!"));
                }
            })
               .WithName("update-service-status")
               .WithTags("Bike-Services");

            app.MapGet("/api/services/mechanic/summary/{mechanicId:int}", async (int mechanicId, IServiceRepository _serviceRepository) =>
            {
                try
                {
                    if (mechanicId is 0)
                        return Results.BadRequest(ApiResponse<MechanicTaskSummayDto>.Fail("Invalid mechanic"));

                    var result = await _serviceRepository.GetMechanicWorkSummaryAsync(mechanicId);
                    return Results.Ok(ApiResponse<MechanicTaskSummayDto>.Success(result));
                }
                catch (Exception)
                {
                    return Results.InternalServerError(ApiResponse<MechanicTaskSummayDto>.Fail("Internal server error..!"));
                }
            })
               .WithName("mechanic-summary")
               .WithTags("Bike-Services").RequireAuthorization("");

            app.MapGet("/api/services/mechanic/assigned-jobs/{mechanicId:int}", async (int mechanicId, IServiceRepository _serviceRepository) =>
            {
                try
                {
                    if (mechanicId is 0)
                        return Results.BadRequest(ApiResponse<IEnumerable<AssignedJobDto>>.Fail("Invalid mechanic"));

                    var result = await _serviceRepository.GetMechanicAssignedJobsAsync(mechanicId);
                    return Results.Ok(ApiResponse<IEnumerable<AssignedJobDto>>.Success(result));
                }
                catch (Exception)
                {
                    return Results.InternalServerError(ApiResponse<IEnumerable<AssignedJobDto>>.Fail("Internal server error..!"));
                }
            })
               .WithName("Mechanic-Assigned-Jobs")
               .WithTags("Bike-Services");

            app.MapGet("/api/services/daily-jobs", async (int? serviceStatus, IServiceRepository _serviceRepository) =>
            {

                try
                {

                    var result = await _serviceRepository.GetDailyJobsByStatusAsync(null, serviceStatus);
                    return Results.Ok(ApiResponse<IEnumerable<TodayJobFeed>>.Success(result));
                }
                catch (Exception)
                {
                    return Results.InternalServerError(ApiResponse<IEnumerable<TodayJobFeed>>.Fail("Internal server error..!"));
                }
            })
               .WithName("daily-job-list")
               .WithTags("Bike-Services");
            
            app.MapGet("/api/services/job/{jobId:int}", async (int jobId, IServiceRepository _serviceRepository) =>
            {
                try
                {
                    var result = await _serviceRepository.GetJobByIdAsync(jobId);
                    return Results.Ok(ApiResponse<ServiceJobDetailDto>.Success(result));
                }
                catch (Exception)
                {
                    return Results.InternalServerError(ApiResponse<ServiceJobDetailDto>.Fail("Internal server error..!"));
                }
            })
               .WithName("job-details")
               .WithTags("Bike-Services");

            

            app.MapGet("/api/services/items/{jobId:int}", async (int jobId, IServiceRepository _serviceRepository) =>
            {
                try
                {
                    var result = await _serviceRepository.GetServiceItemsAsync(jobId);
                    return Results.Ok(ApiResponse<IEnumerable<ServiceItemDto>>.Success(result));
                }
                catch (Exception)
                {
                    return Results.InternalServerError(ApiResponse<ServicePartsDto>.Fail("Internal server error..!"));
                }
            })
               .WithName("service-items-list")
               .WithTags("Bike-Services");

            app.MapPost("/api/services/items-add", async ([FromBody] AddServiceItemsDto req, [FromServices] IServiceRepository _serviceRepository) =>
            {
                try
                {
                    var IsValid = ModelValidator.Validate(req);

                    if (!IsValid.IsValid)
                    {
                        return Results.BadRequest(ApiResponse<int>.Fail("Invalid Request", IsValid.Errors));
                    }
                    var result=  await _serviceRepository.AddServiceItemsAsync(req);
                    return Results.Ok(ApiResponse<bool>.Success(result));
                }
                catch (Exception)
                {
                    return Results.InternalServerError(ApiResponse<bool>.Fail("Internal server error..!"));
                }
            });



            app.MapPost("/api/services/new-job", async (CreateJobAssignmentDto dto, IServiceRepository _serviceRepository) =>
            {
                try
                {
                    await _serviceRepository.AssignNewJobAsync(dto);
                    return Results.Ok(ApiResponse<string>.Success("Job created and Assigned Successfully"));
                }
                catch (Exception)
                {
                    return Results.InternalServerError(ApiResponse<string>.Fail("Internal server error..!"));
                }
            })
               .WithName("new-job")
               .WithTags("Bike-Services");

          
            app.MapGet("/api/services/parts", async ([FromServices] IServiceRepository _serviceRepository ) =>
            {

                try
                {
                    var result = await _serviceRepository.GetServiceParts();
                    return Results.Ok(ApiResponse<IEnumerable<PartsDto>>.Success(result));
                }
                catch (Exception)
                {
                    return Results.InternalServerError(ApiResponse<PartsDto>.Fail("Internal server error..!"));
                }
            }).WithTags("Bike-Services"); ;
            
            
        }
    }
}
