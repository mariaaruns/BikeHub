using BikeHub.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;
using System.Reflection.Metadata;
using System.Security.Claims;

namespace BikeHub.AuthHandlers.PolicyHandler
{
    public class PolicyHandler : AuthorizationHandler<PolicyRequirement>
    {

      //  private readonly IMemoryCache _cache;
        
        private readonly IAuthService _authService;

        public PolicyHandler( IAuthService authService)
        {
            //_cache = cache;
            _authService = authService;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PolicyRequirement requirement)
        {

            // Get the user ID from the claims
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            // If user ID is not found, fail the authorization
            if (userId == null)
            {
                context.Fail();
                return;
            }
            
            //Admin has all policies, so if the user is in the Admin role, succeed directly without checking policies
            if (context.User.IsInRole("Admin"))
            {
                context.Succeed(requirement);
                return;
            }

            // Check if the policies for the user are already cached
            HashSet<string> policies;
            try
            {
                policies = await _authService.GetPoliciesAsync(long.Parse(userId)) ?? new HashSet<string>();
            }
            catch
            {
                context.Fail();
                return;
            }
            // Check if the required policy is in the user's policies
            if (policies.Contains(requirement.PolicyName))
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
            return;

        }
    }
}
