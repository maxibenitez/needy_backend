using needy_logic_abstraction;

namespace needy_api.Middlewares
{
    public class LoginMiddleware
    {
        private IUserContext _userContext;

        private readonly RequestDelegate _next;

        public LoginMiddleware(IUserContext userContext, RequestDelegate next)
        {
            _next = next;
            _userContext = userContext;
        }

        public async Task Invoke(HttpContext context)
        {
            var path = context.Request.Path.Value;

            if ("/api/authorization/authenticate".Equals(path))
            {
                await _next(context);
            }
            else
            {
                if (_userContext.GetUserSession() is not null)
                {
                    await _next(context);
                }
                else
                {
                    throw new Exception("First log in");
                }
            }
        }
    }
}
