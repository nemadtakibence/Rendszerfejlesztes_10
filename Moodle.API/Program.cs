using System.Buffers;
using Moodle.Data;
using Moodle.API.Middlewares;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Moodle.Core.Roles;
using Moodle.API.WebSocketServer;
using Microsoft.AspNetCore.WebSockets;
using System.Net.WebSockets;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<MoodleDbContext>();

// Add JWT authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
    };
});

builder.Services.AddWebSockets(options =>
{
    // Configure WebSocket options if necessary
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(options => {
    options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
});

// Use authentication middleware
app.UseAuthentication();

// Use WebSocket middleware
app.UseWebSockets();

app.UseMiddleware<TokenMiddleware>(builder.Configuration);

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Use(async (context, next) =>
{
    if (context.Request.Path == "/ws")
    {
        if (context.WebSockets.IsWebSocketRequest)
        {
            using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
            Console.WriteLine("WebSocket connection established");
            await Echo(webSocket);
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            Console.WriteLine("WebSocket request denied - not a WebSocket request");
        }
    }
    else
    {
        await next();
    }
});

try
{
    Task.Run(async () =>
    {
        var webSocketServer = new WebSocketServer("127.0.0.1", 8000);
        await webSocketServer.Run();
    });
}
catch (Exception ex)
{
    Console.WriteLine($"Error starting WebSocket server: {ex.Message}");
}

using (MoodleDbContext context = new MoodleDbContext())
{
    int lectId = context.Degrees.SingleOrDefault(x => x.Name == "Oktató").Id;
    foreach (var user in context.Users)
    {
        if (user.Degree_Id == lectId)
        {
            ACL.AddUser(user.Username);
            ACL.AddPermission(user.Username, Roles.Teacher);
        }
        else
        {
            ACL.AddUser(user.Username);
            ACL.AddPermission(user.Username, Roles.Student);
        }
        Console.WriteLine(user.Username);
    }
}

app.Run();

static async Task Echo(WebSocket webSocket)
{
    var buffer = new byte[1024 * 4];
    var receiveResult = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
    Console.WriteLine("Message received from client");

    while (!receiveResult.CloseStatus.HasValue)
    {
        await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, receiveResult.Count), receiveResult.MessageType, receiveResult.EndOfMessage, CancellationToken.None);
        Console.WriteLine("Message sent to client");
        receiveResult = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        Console.WriteLine("Message received from client");
    }

    await webSocket.CloseAsync(receiveResult.CloseStatus.Value, receiveResult.CloseStatusDescription, CancellationToken.None);
    Console.WriteLine("WebSocket connection closed");
}