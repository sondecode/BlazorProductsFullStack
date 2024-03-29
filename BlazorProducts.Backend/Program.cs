using System.Text;
using BlazorProducts.Backend.Repository;
using BlazorProducts.Server.Context;
using BlazorProducts.Server.Context.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);
//Enable CORS
builder.Services.AddCors(options =>
{
	options.AddPolicy("CorsPolicy",
		builder => builder
			.WithOrigins("https://localhost:7212/") // Replace with your client's origin
			.AllowAnyOrigin()
			.AllowAnyMethod()
			.AllowAnyHeader()
            .WithExposedHeaders("X-Pagination"));
});
// Add services to the container.
builder.Services.AddDbContext<ProductContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("sqlConnection")));
builder.Services.AddControllers();
var redisConfiguration = new RedisConfiguration();
builder.Configuration.GetSection("RedisConfiguration").Bind(redisConfiguration);
builder.Services.AddSingleton(redisConfiguration);

if (redisConfiguration.Enabled)
{
    builder.Services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(redisConfiguration.RedisConnectionString));
    builder.Services.AddStackExchangeRedisCache(option => option.Configuration = redisConfiguration.RedisConnectionString);
    builder.Services.AddSingleton<IResponseCacheService, ResponseCacheService>();
}
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IOrderProductRepository, OrderProductRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
  .AddEntityFrameworkStores<ProductContext>();

var jwtSettings = builder.Configuration.GetSection("JWTSettings");
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["validIssuer"],
        ValidAudience = jwtSettings["validAudience"],
        IssuerSigningKey =  ExtendKeyLengthIfNeeded(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["securityKey"])), 32)
    };
});

SymmetricSecurityKey ExtendKeyLengthIfNeeded(SymmetricSecurityKey key, int minLenInBytes)
{
    if (key != null && key.KeySize < (minLenInBytes * 8))
    {
        var newKey = new byte[minLenInBytes]; // zeros by default
        key.Key.CopyTo(newKey, 0);
        return new SymmetricSecurityKey(newKey);
    }
    return key;
}

builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("CorsPolicy");

app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"StaticFiles")),
    RequestPath = new PathString("/StaticFiles")
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
