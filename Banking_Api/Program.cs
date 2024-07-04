using Banking_Api.Data;
using Banking_Api.Jwt;
using Banking_Api.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<BankingDbContext>(options =>
{
  options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//able to inject jwt services inside our controller
builder.Services.AddScoped<JwtServices>();

//defining the identity core services
builder.Services.AddIdentityCore<User>(options =>
{
    options.Password.RequiredLength = 6;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = true;
    options.SignIn.RequireConfirmedEmail = false;

})

    .AddRoles<IdentityRole>() // able to add role
    .AddRoleManager<RoleManager<IdentityRole>>() // ableus to use manager role
    .AddEntityFrameworkStores<BankingDbContext>() //providing our context
    .AddSignInManager<SignInManager<User>>() //signin manager
    .AddUserManager<UserManager<User>>()//for creating users
    .AddDefaultTokenProviders();//will be  use for email tokens



//this will authecate the user using jwt
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    
    //uisng  jwt baear
    .AddJwtBearer(Options =>
    {
        Options.TokenValidationParameters = new TokenValidationParameters
        {

            //valiadte the token base on the key that we have provided the user inside appsettings.json jwt key
            ValidateIssuerSigningKey = true,

            //sign in key base on jwt key inside appsettings.json 
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
            
            //the issues is this api url projects
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidateIssuer = true,

            ValidateAudience = false
        };

    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


//Authentication we varify the identity of a user of services
app.UseAuthentication();

//dtermines the users acces right
app.UseAuthorization();

app.MapControllers();

app.Run();
