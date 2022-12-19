using CoreScratch;
using CoreScratch.Middlewares;
using CoreScratch.Services;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

//configuring options to be used by the middleware
builder.Services.Configure<FruitOptions>(options =>
{
    options.Name = "Watermelon";
}); 

//Service class Instance for dependency Injection
builder.Services.AddSingleton<IResponseFormatter,TextResponseFormatter>(); 

//Middleware Configurations and diffferent usages
//Middlewares execute in the same order in which it is given here below
var app = builder.Build();

app.Logger.LogDebug("Pipeline configuration starts");

app.MapGet("/fruit", async (HttpContext context,IOptions<FruitOptions> fruits) =>
{
    FruitOptions options = fruits.Value;
    await context.Response.WriteAsync($"{options.Name}, {options.Color}");
});

//Invoking middleware from class with the branch
((IApplicationBuilder)app).Map("/branch", branch =>
{
    branch.Run(new Middleware().Invoke);
}
); 

//Middleware branching, and there is no next() invoked
((IApplicationBuilder) app).Map("/branch",branch =>
{
    branch.Use(async (HttpContext context, Func<Task> next) =>
    {
        await context.Response.WriteAsync("Branch Middleware from Program.cs file");
    });
}
); 

//Use is the custom middleware
app.Use(async(context, next) =>
{
    if (context.Request.Method == HttpMethods.Get && context.Request.Query["custom"] == "true")
    {
        context.Response.ContentType= "text/plain";
        await context.Response.WriteAsync("Custom middleware from Use \n");
    }
    await next();
}
);

//Middleware using class file
app.UseMiddleware<Middleware>();

app.UseMiddleware<DIMiddleware>();

//endpoint navigation
app.MapGet("/", () => "Hello World!");

//Usage of service Instance
app.MapGet("/formatter1",async (HttpContext context, IResponseFormatter formatter) =>{
    await formatter.Format(context,"Formatter One");
});

app.MapGet("/formatter2", async (HttpContext context, IResponseFormatter formatter) => {
    await formatter.Format(context, "Formatter Two");
});

app.Logger.LogDebug("Pipeline configuration Ends");

//this listens the request, "Run" is the terminal middleware.
app.Run(); 


