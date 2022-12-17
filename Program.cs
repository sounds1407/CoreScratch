var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Use(async(context, next) =>
{
    if (context.Request.Method == HttpMethods.Get && context.Request.Query["custom"] == "true")
    {
        context.Response.ContentType= "text/plain";
        await context.Response.WriteAsync("Custom middleware from Use \n");
    }
    await next();
}
); //Use is the custom middleware

app.MapGet("/", () => "Hello World!");

app.Run(); //this listens the request