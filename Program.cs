using Admin_Panel.Data;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Admin_Panel.Services;
using Admin_Panel.Interfaces;
using Interfaces;
using Admin_Panel;

var builder = WebApplication.CreateBuilder(args);
string connection = builder.Configuration.GetConnectionString("DefualtConnection");



builder.Services.AddHttpClient<IUserService, UserServiceClient>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5001/");

});
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(connection);
});





//builder.Services.AddAuthentication()
//    .AddMicrosoftAccount(microsoftOptions => {
//        microsoftOptions.ClientId = "<Microsoft-Client-ID>";
//        microsoftOptions.ClientSecret = "<Microsoft-Client-Secret>";
//    })
//    .AddGoogle(googleOptions => {
//        googleOptions.ClientId = "<Google-Client-ID>";
//        googleOptions.ClientSecret = "<Google-Client-Secret>";
//    })
//    .AddFacebook(facebookOptions => {
//        facebookOptions.AppId = "<Facebook-App-ID>";
//        facebookOptions.AppSecret = "<Facebook-App-Secret>";
//    });

builder.Services.AddScoped<IServiceManager, ServiceManager>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
 
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Admin_Panel", Version = "v1" });

    //Add authorize to swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Admin_panel v1"));
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
