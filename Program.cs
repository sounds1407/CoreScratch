using CoreScratch;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<FruitOptions>(options =>
{
    options.Name = "Watermelon";
}); //configuring options to be used by the middleware


//Middleware Configurations and diffferent usages
//Middlewares execute in the same order in which it is given here below

var app = builder.Build();

app.MapGet("/fruit", async (HttpContext context,IOptions<FruitOptions> fruits) =>
{
    FruitOptions options = fruits.Value;
    await context.Response.WriteAsync($"{options.Name}, {options.Color}");
});

((IApplicationBuilder)app).Map("/branch", branch =>
{
    branch.Run(new Middleware().Invoke);
}
); //Invoking middleware from class with the branch

((IApplicationBuilder) app).Map("/branch",branch =>
{
    branch.Use(async (HttpContext context, Func<Task> next) =>
    {
        await context.Response.WriteAsync("Branch Middleware from Program.cs file");
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
