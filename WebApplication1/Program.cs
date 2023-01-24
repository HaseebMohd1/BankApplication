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
using Microsoft.AspNetCore.HttpOverrides;
using Serilog;
using Serilog.Events;
using Microsoft.ApplicationInsights;
using Serilog.Sinks.ApplicationInsights.TelemetryConverters;

// Adding Logging even before the Start of the Application
Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            
            .CreateBootstrapLogger();

try
{
    Log.Information("Started the Application");

    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddApplicationInsightsTelemetry();



    // setting up Serilog for our application
    // This is for reading the Configuration and Enabling Serilog
    builder.Host.UseSerilog((context, services, configuration) => configuration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .WriteTo.ApplicationInsights(services.GetRequiredService<TelemetryClient>(), new TraceTelemetryConverter())
            .Enrich.FromLogContext()
    );


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



    var app = builder.Build();

    // Exchange asp .net loggin with Serilog Loggin
        // Logging all HTTP requestss
    // This is for Logging the requests. It is a middleware for logging every request
    app.UseSerilogRequestLogging(configure =>
    {
        configure.MessageTemplate = "HTTP {RequestMethod} {RequestPath} ({UserId}) responded {StatusCode} in {Elapsed:0.0000}ms";
    });



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


    // adding custom middleware for Exception Handling - 1
    //app.UseMiddleware<ExceptionHandlingMiddleware>();

    app.UseAuthentication();

    app.UseCors("CorsPolicy");

    app.UseAuthorization();

    app.MapControllers();

    // for serilog
    // IDiagnosticContext is something provided by Serilog
    // This is used to add Additional Disgnostic Meta data to our API REQUEST
    app.MapGet("/request-context", (IDiagnosticContext diagnosticContext) =>
    {
        diagnosticContext.Set("UserId", "Someone");
    });

    // Custom Middleware - 2
    app.AddGlobalErrorHandler();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}

return 0;

