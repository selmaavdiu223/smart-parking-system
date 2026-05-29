using SmartParkingSystem.Repositories;
using SmartParkingSystem.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<FileRepository>();
builder.Services.AddScoped<ParkingService>();
builder.Services.AddScoped<ParkingSessionService>();
builder.Services.AddSingleton(sp => new AuthService(sp.GetRequiredService<IConfiguration>()));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();
