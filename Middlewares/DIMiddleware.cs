using CoreScratch.Services;

namespace CoreScratch.Middlewares
{
    public class DIMiddleware
    {
        private RequestDelegate _next;
        private IResponseFormatter _format;


        public DIMiddleware(RequestDelegate next, IResponseFormatter format)
        {
            _next = next;
            _format = format;
        }

        public async Task Invoke(HttpContext context)
        {

            if (context.Request.Path == "/dimiddleware")
                await _format.Format(context, "Dependency Injected Successfully");

            await _next(context);
        }
    }
}
