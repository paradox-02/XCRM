using Microsoft.EntityFrameworkCore;
using XCRM.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// 注册 Controller API
builder.Services.AddControllers();

// OpenAPI 文档
builder.Services.AddOpenApi();

// 注册 EF Core
builder.Services.AddDbContext<XCrmDbContext>(options =>
{
    var connectionString =
        builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException(
            "未找到连接字符串 DefaultConnection");

    options.UseSqlServer(connectionString);
});

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