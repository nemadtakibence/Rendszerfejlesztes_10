using System.Buffers;
using Moodle.Data;
using Moodle.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<MoodleDbContext>();

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

app.UseMiddleware<TokenMiddleware>(builder.Configuration);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


/*using(MoodleDbContext context = new MoodleDbContext()){
    int lectId = context.Degrees.SingleOrDefault(x => x.Name == "Oktató").Id;  
            foreach(var user in context.Users){
                if(user.Degree_Id==lectId){                    
                    ACL.AddUser(user.Username);
                    ACL.AddPermission(user.Username, Roles.Teacher);
                }
                else{
                    ACL.AddUser(user.Username);
                    ACL.AddPermission(user.Username, Roles.Student);
                }
                Console.WriteLine(user.Username);
            }
}*/

app.Run();