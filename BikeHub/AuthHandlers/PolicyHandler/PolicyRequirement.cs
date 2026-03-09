using Microsoft.AspNetCore.Authorization;

namespace BikeHub.AuthHandlers.PolicyHandler
{
    public class PolicyRequirement: IAuthorizationRequirement
    {
        public string PolicyName { get; }

        public PolicyRequirement(string policyName)
        {
            PolicyName = policyName;
        }
    }
}
