using DAL.Data;
using DAL.Repositories.Interfaces;
using DAL.Repositories;
using DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using BLL.Services.Interfaces;
using BLL.Services;
using AutoMapper;


namespace Chat;

public static class ServicesRegistration
{
    public static void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddSignalR();

        builder.Services.AddDbContext<SimpleChatContext>(options => options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")));


        builder.Services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
            builder.WithOrigins("http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
        });


        var mapperConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new Mappings.AutoMapper());
        });

        IMapper mapper = mapperConfig.CreateMapper();
        builder.Services.AddSingleton(mapper);


        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IChatRepository, ChatRepository>();
        builder.Services.AddScoped<IMessageRepository, MessageRepository>();

        builder.Services.AddScoped<IChatService, ChatService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IMessageService, MessageService>();
    }
}
