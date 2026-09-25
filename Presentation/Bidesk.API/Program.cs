using Hangfire;
using Bidesk.API.Extensions;
using Bidesk.API.Filters;
using Bidesk.Application;
using Bidesk.Application.Exceptions;
using Bidesk.Infrastructure;
using Bidesk.Infrastructure.Filters;
using Bidesk.Infrastructure.Hangfire;
using Bidesk.Persistence;
using Bidesk.SignalR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddPersistenceServices();
builder.Services.AddHangfireServices();
builder.Services.AddSignalRServices();

#if (DEBUG)
builder.Services.AddCors(options => options.AddDefaultPolicy(
    policy => policy
    .WithOrigins("http://localhost:5129", "https://localhost:5129", "http://localhost:4200", "https://localhost:4200", "http://localhost:7214", "https://localhost:7214")
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
));
#elif (RELEASE)
   builder.Services.AddCors(options => options.AddDefaultPolicy(
    policy => policy
    .WithOrigins("https://10.10.2.69:443","https://portal.dogupres.local:443", "http://portal.dogupres.local:443", "https://localhost:4200", "http://localhost:4200")
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
));
#endif

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
    options.Filters.Add<RolePermissionFilter>();
    // Taklit oturumlarinda yazma denetimi + islem izi (normal oturumlari etkilemez).
    options.Filters.Add<ImpersonationGuardFilter>();
})
    .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("Loggedin", options =>
    {
        string? securityKey = builder.Configuration["Token:SecurityKey"];

        if (string.IsNullOrEmpty(securityKey))
            throw new AppConfigInfoMissingException("Token parametresi g�venlik kodu bulunamad�!");


        options.TokenValidationParameters = new()
        {
            ValidateAudience = true, // Olu�turulacak token de�erini kimlerin/sitelerin kullan�c� belirledi�imiz de�er. www.bilmemne.com
            ValidateIssuer = true, // Olu�turulacak token de�erini kimin da��tt���n� ifade edilecek alan. www.myapi.com
            ValidateLifetime = true, // Token de�erinin s�resini kontrol edecek do�rulama
            ValidateIssuerSigningKey = true, // Token de�erinin uygulam�m�za ait oldu�unu ifade eden g�venlik anahtar� verisinin do�rulanmas�d�r.
            ValidAudience = builder.Configuration["Token:Audience"],
            ValidIssuer = builder.Configuration["Token:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"] ?? "")),
            LifetimeValidator = (notBefore, expires, securityToken, validationParameters) => expires != null ? expires > DateTime.UtcNow : false,
            NameClaimType = ClaimTypes.Name, // JWT �zerinde Name claimne kar��l�k gelen de�eri User.Identity.Name propertisinden elde edebiliriz.
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];

                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) && path.ToString().Contains("/hubs"))
                {
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    }
);

builder.Services.AddAuthorization();
builder.Services.AddMemoryCache();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureExceptionHandler<Program>(app.Services.GetRequiredService<ILogger<Program>>());

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors();
app.MapControllers();
app.UseHangfireDashboard();
app.RegisterHubs();

#if (RELEASE)
// Bildirimler kapaliysa (test modu) tekrarli isler hic kaydolmasin, varsa silinsin.
if (Bidesk.Infrastructure.Services.NotificationGate.IsEnabled(app.Configuration))
    HangfireJobQueue.CreateTasks();
else
    HangfireJobQueue.RemoveTasks();
#endif

app.Run();
