using Microsoft.EntityFrameworkCore;
using XCRM.Infrastructure.Data;
using XCRM.Domain.Repositories;
using XCRM.Infrastructure.Repositories;
using XCRM.Infrastructure;
using XCRM.Application;

var builder = WebApplication.CreateBuilder(args);

// 注册 Controller API
builder.Services.AddControllers();

// OpenAPI 文档
builder.Services.AddOpenApi();

builder.Services.AddApplication();

builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// 映射 Controller，不再使用 app.MapGet(...)
app.MapControllers();

app.Run();