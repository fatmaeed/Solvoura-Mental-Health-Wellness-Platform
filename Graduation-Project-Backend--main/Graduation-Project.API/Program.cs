using Graduation_Project.Application.ConfigrationMapper;
using Graduation_Project.Application.Hubs;
using Graduation_Project.Application.Interfaces.IRepositories;
using Graduation_Project.Application.Interfaces.IServices;
using Graduation_Project.Application.Interfaces.IUnitOfWorks;
using Graduation_Project.Application.Services;
using Graduation_Project.Application.Utils;
using Graduation_Project.Domain.Entities;
using Graduation_Project.Infrastructure.Data;
using Graduation_Project.Infrastructure.Repositories;
using Graduation_Project.Infrastructure.UnitOfWorks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Graduation_Project.API {

    public class Program {

        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);
            var openAiKey = builder.Configuration["OpenAI:ApiKey"];
            var dbConnectionString = builder.Configuration.GetConnectionString("MentalHealthDB");

            // Register services
            builder.Services.AddSingleton<AIService>(new AIService(openAiKey, dbConnectionString));
            builder.Services.AddSingleton<AIDatabaseService>(new AIDatabaseService(dbConnectionString));

            builder.Services.AddCors(setup => {
                setup.AddPolicy("default", (options) => {
                    options.WithOrigins(ConstantData.forntEndServerName).AllowAnyHeader().AllowAnyMethod().AllowCredentials();
                });
            });
            //service
            builder.Services.AddScoped<IOurService, OurService>();
            builder.Services.AddScoped<IReservationService, ReservationService>();
            builder.Services.AddScoped<IClientService, ClientService>();
            builder.Services.AddScoped<INotifiService,NotificationService>();
            builder.Services.AddControllers();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<ISessionUserConnectionRepo, SessionUserConnectionRepo>();
          
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));
            builder.Services.AddTransient<IEmailSender, EmailHandler>();
            builder.Services.AddScoped<IIllnessService, IllnessService>();
            builder.Services.AddScoped<IServiceProviderService, ServiceProviderService>();
            builder.Services.AddScoped<ISessionService, SessionService>();
            builder.Services.AddScoped<IPostService, PostService>();
            builder.Services.AddScoped<ICommentService, CommentService>();
            builder.Services.AddScoped<IClientService, ClientService>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();
            builder.Services.AddScoped<IMeetingSessionService, MeetingSessionService>();
            builder.Services.AddScoped<IAppHubService, AppHubService>();
            builder.Services.AddScoped<INotificationService, NotificationHandler>();
            builder.Services.AddSignalR();
            builder.Services.AddScoped<IFeedBackService,FeedBackService>();
            builder.Services.AddScoped<IUserLikesService,UserLikesService>();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddAutoMapper(cfg => { }, typeof(MapperConfigH), typeof(MapperConfigR)
                , typeof(MapperConfigAB), typeof(MapperConfigAM), typeof(MapperConfigF)
                );
            builder.Services.AddDbContext<MentalDbContext>(options => options.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("MentalHealthDb")));

            builder.Services.AddOpenApi();
            builder.Services.AddDataProtection();
            builder.Services.AddIdentityCore<UserEntity>(
                (options) => {
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireNonAlphanumeric = true;
                    options.Password.RequiredLength = 8;
                }
                ).AddRoles<IdentityRole<int>>().
                AddEntityFrameworkStores<MentalDbContext>().AddDefaultTokenProviders();

            builder.Services.AddHostedService<SessionReminderService>();

            builder.Services.AddAuthentication(
                op
                => {
                    op.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    op.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }
                ).AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters {
                    IssuerSigningKey = MyTokenHandler.GetSecurityKey(),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment()) {
                app.MapOpenApi();
                app.UseSwaggerUI(op => op.SwaggerEndpoint("/openapi/v1.json", "v1"));
            }
            app.UseCors("default");
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.MapHub<MeetingSessionHub>("/hubs/video");
            app.MapHub<AppHub>("/hubs/system");

            app.Run();
        }
    }
}