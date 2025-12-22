using SambhramaBook.Infrastructure;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(type => type.ToString());
});

builder.Host.UseDefaultServiceProvider((context, options) =>
{
    options.ValidateScopes = !context.HostingEnvironment.IsProduction();
    options.ValidateOnBuild = !context.HostingEnvironment.IsProduction();
});

builder.Services.RegisterSambhramaBookServices(builder.Configuration);

builder.Services.AddHealthChecks();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/healthz");
app.Run();
