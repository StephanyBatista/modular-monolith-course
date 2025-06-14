using System.Reflection;
using System.Text;
using EGeek.Api;
using EGeek.Catalog.Config;
using EGeek.Id.Config;
using EGeek.Order.Config;
using EGeek.Purchase.Config;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddExceptionHandler<GlobalException>();
builder.Services.AddProblemDetails();
builder.Services.AddHttpClient();

List<Assembly> mediatoRAssembly = [typeof(Program).Assembly];

IdModularExtension.Apply(builder.Services, builder.Configuration, mediatoRAssembly);
CatalogModularExtension.Apply(builder.Services, builder.Configuration, mediatoRAssembly);
PurchaseModularExtension.Apply(builder.Services, builder.Configuration, mediatoRAssembly);
OrderModularExtension.Apply(builder.Services, builder.Configuration, mediatoRAssembly);

builder.Services.AddMediatR(config => 
    config.RegisterServicesFromAssemblies(mediatoRAssembly.ToArray()));

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateActor = true,
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateLifetime = true,    
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

IdConfigApp.Apply(app);
CatalogConfigApp.Apply(app);
PurchaseConfigApp.Apply(app);
OrderConfigApp.Apply(app);

app.UseExceptionHandler();

app.Run();