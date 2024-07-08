using Chat;
using Chat.Middlewares;

var builder = WebApplication.CreateBuilder(args);

ServicesRegistration.ConfigureServices(builder);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("CorsPolicy");
app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();
app.MapControllers();


app.MapHub<ChatHub>("/chat");


app.Run();
