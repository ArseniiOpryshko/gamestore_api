using GameStore.Common;
using GameStore.Data;
using GameStore.Repositories.Store;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<GameStoreContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GameStoreContext")));

builder.Services.AddScoped<IStoreRepository, StoreRepository>();

builder.Services.AddAutoMapper(typeof(MappingProfile));



builder.Services.AddControllers().
    AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(builder => builder
   .SetIsOriginAllowed(_ => true)
   .AllowAnyMethod()
   .AllowAnyHeader()
   .AllowCredentials());


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
