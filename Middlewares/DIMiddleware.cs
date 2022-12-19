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

        public async Task Invoke(HttpContext context,ILogger<DIMiddleware> logme)
        {

            if (context.Request.Path == "/dimiddleware")
                await _format.Format(context, "Dependency Injected Successfully");

            logme.LogDebug("DI middleware is called!");

            await _next(context);
        }
    }
}
