using Microsoft.AspNetCore.Mvc.Filters;

[AttributeUsage(AttributeTargets.Class)]
public class ApiAuth : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext filterContext)
    {

    }
}