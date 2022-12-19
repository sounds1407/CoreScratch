﻿namespace CoreScratch.Middlewares
{
    public class Middleware
    {
        private RequestDelegate _next;

        public Middleware()
        {
        }

        public Middleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Method == HttpMethods.Get && context.Request.Query["custom"] == "one")
            {
                if (!context.Response.HasStarted)
                {
                    context.Response.ContentType = "text/plain";
                }

                await context.Response.WriteAsync("Class based middleware \n");
            }

            if (_next != null)
                await _next(context);
        }
    }
}
