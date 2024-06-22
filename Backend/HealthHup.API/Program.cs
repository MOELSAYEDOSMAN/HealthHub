using HealthHup.API.Service.AccountService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;
using HealthHup.API.Hubs.ChatHubFolder;
using Hangfire;
using HealthHup.API.Service.MlService;
using HealthHup.API.Service.ModelService.HospitalService.DrugModelService;
using HealthHup.API.Service.ModelService.HospitalService.DoctorDates;
using HealthHup.API.Service.ModelService.Admin.Alerts;
using HealthHup.API.Service.BackGroundJobs.Dates;
using HealthHup.API.Service.Notification;
using HealthHup.API.Service.MlService.MLCT;
using Microsoft.Extensions.Options;
using HealthHup.API.Service.ModelService.HospitalService.Chat;




var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Connect With Db
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicatoinDataBaseContext>(option =>option.UseSqlServer(connectionString,
    sqlServerOption => sqlServerOption.CommandTimeout(115000)));

//Add Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedEmail = true;
    options.Password.RequiredLength = 8;
    options.User.RequireUniqueEmail = true;
}).AddEntityFrameworkStores<ApplicatoinDataBaseContext>()
.AddDefaultTokenProviders();

//Add JWT 
builder.Services.AddScoped<Jwt, Jwt>();
builder.Services.Configure<Jwt>(builder.Configuration.GetSection("JWT"));
builder.Services.AddAuthentication(o =>
{
    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JWT:IssUser"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]??""))
    };
    o.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];

            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) &&
                (path.StartsWithSegments("/chat")))
            {
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
});

////Add SeriaLog
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);
//HangeFire
//DataBase
builder.Services.AddHangfire(x => x.UseSqlServerStorage(connectionString));
builder.Services.AddHangfireServer();

//Def Service
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
}).ConfigureApiBehaviorOptions(option=>option.SuppressModelStateInvalidFilter=true);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(s =>
{
    s.AddSecurityDefinition("Bearer",
        securityScheme: new Microsoft.OpenApi.Models.OpenApiSecurityScheme()
        {
            In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            Description = "Enter Token",
            Name = "Authorization",
            Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
            Scheme= "bearer",
            BearerFormat="JWT"
        });
    
    s.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new()
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

//Add Cours
builder.Services.AddCors(options =>
options.AddPolicy("MyPolicy",
bui => bui.SetIsOriginAllowed(d=>d!="").AllowCredentials()
        .AllowAnyMethod()
        .AllowAnyHeader())
);




//RealeTime
builder.Services.AddSignalR();


//Start Inject Service
    //Image
builder.Services.AddTransient<ISaveImage, ImageService>();
    //Message
builder.Services.AddSingleton<ISendMessageService, SendMEssageService>();
//Chta
builder.Services.AddTransient<IChatService, ChatService>();

//Account
builder.Services.AddTransient<IAuthService, AuthService>();
//Start DataBase
builder.Services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));
//Address
builder.Services.AddTransient<IGovermentService, GovermentService>();
builder.Services.AddTransient<IAreaService, AreaService>();
//Doctors
builder.Services.AddTransient<IDoctorService, DoctorService>();
builder.Services.AddScoped<IMedicalSessionService, MedicalSessionService>();
builder.Services.AddTransient<IDoctorDateService, DoctorDateService>();
//Patient
builder.Services.AddTransient<IPatientDatesService, PatientDatesService>();
builder.Services.AddTransient<IPatientInfoService, PatientInfoService>();
builder.Services.AddTransient<IRateService, RateService>();
//Drug
builder.Services.AddScoped<IDrugModelApiService, DrugModelApiService>();
//Logs
builder.Services.AddTransient<IAdminLogs,AdminLogsService>();
//Alert
builder.Services.AddScoped<IAlertService, AlertService>();
//BackGroundJobs
builder.Services.AddTransient<IBDataPeients, BDataPeients>();
//Notifactions
builder.Services.AddTransient<INotifiyService, NotifiyService>();
//CTV
builder.Services.AddTransient<ICTService, CTService>();

//End DataBase

//Api Ml
string UriModel = builder.Configuration.GetSection("Flask").GetValue<string>("uri");
builder.Services.AddHttpClient<IMLApiService, MLApiService>
    (client => client.BaseAddress = new Uri(UriModel));
//End Inject Service

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("MyPolicy");
/pp.UseHttpsRedirection();
app.UseStaticFiles();


app.UseAuthentication();
app.UseAuthorization();


//HangFire(BackGroundJobs):Dashboard
//app.UseHangfireDashboard("/Dashboard");
//Add Hubs
app.MapHub<ChatHub>("chat");

app.MapControllers();

app.Run();
