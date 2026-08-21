using PindahWebsite3.Services;

namespace PindahWebsite3.Routing;

public sealed class SeoSlugRouteConstraint : IRouteConstraint
{
    public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
    {
        if (!values.TryGetValue(routeKey, out var raw) || raw is not string slug)
        {
            return false;
        }

        return SeoLandingCatalog.TryGet(slug, out _);
    }
}
