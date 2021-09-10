using QuorumTravelAgency.API.RabbitMQPublishers;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
// Add services to the container.

services.AddControllers();
services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "QuorumTravelAgency.API", Version = "v1" });
});

services.AddRabbitMQConnection();
services.AddSingleton<AvailableFlightsPublisher>();
services.AddSingleton<AvailableFlightsV2Publisher>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "QuorumTravelAgency.API v1"));
}

app.UseHttpsRedirection();


app.MapControllers();

app.Run();
