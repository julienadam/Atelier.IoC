using Atelier.IoC.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// builder.Services.AddScoped<IForecasts, FakeForecasts>();
// builder.Services.AddScoped<IForecasts, SelfUpdatingForecasts>();
// builder.Services.AddSingleton<IForecasts, SelfUpdatingForecasts>();

if(builder.Configuration.GetValue<bool>("UseOpenMeteo"))
{
    builder.Services.AddSingleton<OpenMeteoForecasts>();
    builder.Services.AddSingleton<IForecasts, ThrottledForecasts<OpenMeteoForecasts>>();
}
else
{
    builder.Services.AddScoped<IForecasts, FakeForecasts>();
}


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
