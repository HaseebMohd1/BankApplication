using System.Text;
using Bank.Data.Contracts;
using Bank.Data.Services;
using Bank.Service.Contracts;
using Bank.Service.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using WebApplication1.Models.AppDbContext;
using WebApplication1.Repository;
using WebApplication1.Services;
using ExceptionHandler;
using NLog;
using Microsoft.AspNetCore.HttpOverrides;
using Bank.LoggerService;

var builder = WebApplication.CreateBuilder(args);

LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));


// Addidng Log Feature
//builder.Logging.ClearProviders();
//builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
//builder.Logging.AddConsole();
//builder.Logging.AddDebug();
//builder.Logging.AddEventSourceLogger();

// Adding Custom Exception Middleware
builder.Services.AddTransient<ExceptionHandlingMiddleware>();




builder.Services.AddControllers();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();




// adding HttpContextAccessor
builder.Services.AddHttpContextAccessor();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization using Bearer Scheme!! (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

// To set to Authtication Scheme
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
       .AddJwtBearer(options =>
       {
           options.TokenValidationParameters = new TokenValidationParameters
           {
               ValidateIssuerSigningKey = true,
               IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
               ValidateIssuer = false,
               ValidateAudience = false
           };
       });


// adding DbContext
builder.Services.AddDbContext<EmployeeDbContext>(options => options.UseSqlServer("name=ConnectionStrings:DefaultConnection"));

// Repository
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBankRepository, BankRepository>();



// Registering Services
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IBankService, BankService>();

// for Automapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);

// for Logging
builder.Services.AddSingleton<ILog, LogNLog>();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    // test : Exceptional Handling
    //app.UseDeveloperExceptionPage();
}
else
    app.UseHsts();



app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});


// adding custom middleware for Exception Handling
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthentication();

app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
