using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.ModelBuilder;
using Q1_de4.Models;

var builder = WebApplication.CreateBuilder(args);
var modelBuilder = new ODataConventionModelBuilder();

// Add services to the container.
builder.Services.AddCors();
//modelBuilder.EntitySet<Product>("Product");

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.AddDbContext<PRN_Sum22_B1Context>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers()
           .AddOData(options => options
           .AddRouteComponents("odata", modelBuilder.GetEdmModel())
           .Select()
           .Filter()
           .OrderBy()
           .SetMaxTop(20)
           .Count()
           .Expand()
           );



var app = builder.Build();
app.UseCors
(policy =>
policy.AllowAnyOrigin()
.AllowAnyHeader()
.AllowAnyMethod());
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();
app.UseODataBatching();


app.MapControllers();

app.Run();