using System.Text;
using Api.Identity;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<UserManagerDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddDatabaseManagerServices();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddMediatR(options => 
	options.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(cfg => cfg.TokenValidationParameters = new TokenValidationParameters()
	{
		ValidIssuers = builder.Configuration.GetSection("JwtSettings:Issuers")
			.GetChildren().Select(i => i.Value),
		ValidAudiences = builder.Configuration.GetSection("JwtSettings:Audiences")
			.GetChildren().Select(a => a.Value),
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]!)),
		ValidateAudience = true,
		ValidateIssuer = true,
		ValidateLifetime = true,
		ValidateIssuerSigningKey = true
	});

builder.Services.AddAuthorization(options =>
{
	options.AddPolicy(IdentityData.CubanAdminUserPolicyName, p =>
	{
		p.RequireClaim(IdentityData.AdminUserClaimName, "true");
		p.RequireClaim(IdentityData.CountryUserClaimName, "CU");
	});
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
