using MillionTestApi.Settings;

using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuracion de MongoDB
var mongoDbSettings = builder.Configuration.GetSection("MongoDB").Get<MongoDbSettings>()!;
var client = new MongoClient(mongoDbSettings.ConnectionString);
var database = client.GetDatabase(mongoDbSettings.DatabaseName);

// Se registra el servicio de MongoDB como singleton
builder.Services.AddSingleton(database);

var myAllowSpecificOrigins = "_million-test-nextjsappp";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:3000") // Esta es la URL de tu nuestra app Next.js
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Aplicamos la politica cors que creamos en la linea 25
app.UseCors(myAllowSpecificOrigins); 

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
