using SportsHub.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddBusiness();
builder.Services.AddInfrastructure();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();
builder.Services.AddCors();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(builder => builder
        .AllowAnyHeader()
        .AllowAnyMethod()
        .SetIsOriginAllowed((host) => true)
        .AllowCredentials());
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
