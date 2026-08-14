using Microsoft.EntityFrameworkCore;
using XCRM.Infrastructure.Data;
using XCRM.Domain.Repositories;
using XCRM.Infrastructure.Repositories;
using XCRM.Infrastructure;
using XCRM.Application;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using XCRM.Api;
using XCRM.Application.Common.Identity;
using XCRM.Api.Identity;
using XCRM.Api.ExceptionHandling;

var builder = WebApplication.CreateBuilder(args);

// 注册 Controller API
builder.Services.AddControllers();

builder.Services.AddProblemDetails();

builder.Services.AddExceptionHandler<ConflictExceptionHandler>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUser, CurrentUser>();

// OpenAPI 文档
builder.Services.AddOpenApi(n =>
{
    n.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
});

builder.Services.AddApplication();

builder.Services.AddInfrastructure(builder.Configuration);


// Jwt
var jwtIssuer = builder.Configuration["Jwt:Issuer"]
    ?? throw new InvalidOperationException(
        "未配置Jwt:Issuer");

var jwtAudience = builder.Configuration["Jwt:Audience"]
    ?? throw new InvalidOperationException(
        "未配置Jwt:Audience");

var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException(
        "未配置Jwt:Key");

builder.Services.AddAuthentication(
    JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtIssuer,

            ValidateAudience = true,
            ValidAudience = jwtAudience,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(jwtKey)),

            ValidateLifetime = true,

            ClockSkew = TimeSpan.FromSeconds(30)
        };
    });

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

// 映射 Controller，不再使用 app.MapGet(...)
app.MapControllers();

app.Run();