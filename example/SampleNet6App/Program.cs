using kr.bbon.AspNetCore.DependencyInjection;
using kr.bbon.AspNetCore.Filters;
using Microsoft.AspNetCore.Mvc;

var version = new ApiVersion(1, 0);

var builder = WebApplication.CreateBuilder(args);

// AppOptions
builder.Services.ConfigureAppOptions();

// Override OpenApiInfo
//builder.Services.ConfigureOpenApiInfo(options =>
//{
//    options.Title = "Awesome API";
//    options.Description = "Awesome API :)";
//    options.Version = version.ToString();
//    options.License = new Microsoft.OpenApi.Models.OpenApiLicense
//    {
//        Name = "MIT",
//        Url = new Uri("https://localhost:5001/license"),
//    };
//    options.Contact = new Microsoft.OpenApi.Models.OpenApiContact
//    {
//        Name = "Pon Cheol Ku",
//        Email = "-",
//        Url = new Uri("https://bbon.kr"),
//    };
//});

builder.Services.ConfigureDefaultJsonOptions();

// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ApiExceptionHandlerFilter>();
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApiVersioningAndSwaggerGen(version);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUIWithApiVersioning();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
