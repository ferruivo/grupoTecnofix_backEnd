namespace GrupoTecnofix_Api.Auth
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.Extensions.Options;

    public class PermissionPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        public PermissionPolicyProvider(IOptions<AuthorizationOptions> options) : base(options) { }

        public override Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            // cria policy "on the fly" pelo nome
            var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .AddRequirements(new PermissionRequirement(policyName))
                .Build();

            return Task.FromResult<AuthorizationPolicy?>(policy);
        }
    }

    public class PermissionRequirement : IAuthorizationRequirement
    {
        public string Permission { get; }
        public PermissionRequirement(string permission) => Permission = permission;
    }

    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var ok = context.User.Claims.Any(c => c.Type == "permission" && c.Value == requirement.Permission);
            if (ok) context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}