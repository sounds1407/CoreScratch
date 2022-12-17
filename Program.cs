using CoreScratch;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();



((IApplicationBuilder)app).Map("/branch", branch =>
{
    branch.Run(new Middleware().Invoke);
}
); //Invoking middleware from class with the branch

((IApplicationBuilder) app).Map("/branch",branch =>
{
    branch.Use(async (HttpContext context, Func<Task> next) =>
    {
        await context.Response.WriteAsync("Branch Middleware");
    });
}
); //Middleware branching, and there is no next() invoked


app.UseMiddleware<Middleware>();


/*
 Commented out as this custom middleware is setup from the Class Middleware.cs
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
*/

app.MapGet("/", () => "Hello World!"); //endpoint 

app.Run(); //this listens the request, "Run" is the terminal middleware.
