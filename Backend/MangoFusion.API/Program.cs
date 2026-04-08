using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add required services.
builder.Services.AddControllers();

builder.Services.AddOpenApi();

// Use required services.
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
