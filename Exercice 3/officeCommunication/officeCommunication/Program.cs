using officeCommunication;
using Microsoft.OpenApi.Models;
using officeCommunication.Models;
using MySqlConnector;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


var builder = WebApplication.CreateBuilder(args);


var securityScheme = new OpenApiSecurityScheme()
{
    Name = "Authorization",
    Type = SecuritySchemeType.ApiKey,
    Scheme = "Bearer",
    BearerFormat = "JWT",
    In = ParameterLocation.Header,
    Description = "JSON Web Token based security",
};


var securityReq = new OpenApiSecurityRequirement()
{
    {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        },
        new string[] {}
    }
};


var contact = new OpenApiContact()
{
    Name = "Mohammad Ayoub",
    Email = "mouhammad.ayoub@outlook.com",
    Url = new Uri("https://www.linkedin.com/in/mouhammad-ayoub")
};


var license = new OpenApiLicense()
{
    Name = "Free License",
    Url = new Uri("https://www.linkedin.com/in/mouhammad-ayoub")
};


var info = new OpenApiInfo()
{
    Version = "v1",
    Title = "Office Communication API V1",
    Description = "Implementing Upload and Download in Minimal API",
    Contact = contact,
    TermsOfService = new Uri("http://www.example.com"),
    License = license
};


builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen(c =>
{
    c.OperationFilter<FileUploadOperationFilter>();
    c.SwaggerDoc("v1", info);
    c.AddSecurityDefinition("Bearer", securityScheme);
    c.AddSecurityRequirement(securityReq);
});


builder.Services.AddAuthentication(o =>
{
    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey
            (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true
    };
});


builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Office Communication API V1");
    });
}


app.UseHttpsRedirection();


app.UseAuthentication();
app.UseAuthorization();


app.MapPost("/Security/GetToken", [AllowAnonymous] (UserDto user) =>
{

    if (user.UserName == "admin@mohammadayoub.com" && user.Password == "P@ssword")
    {
        var issuer = builder.Configuration["Jwt:Issuer"];
        var audience = builder.Configuration["Jwt:Audience"];
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var jwtTokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("Id", "1"),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            }),
            Expires = DateTime.UtcNow.AddHours(6),
            Audience = audience,
            Issuer = issuer,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
        };
        var token = jwtTokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = jwtTokenHandler.WriteToken(token);
        return Results.Ok(jwtToken);
    }
    else
    {
        return Results.Unauthorized();
    }
});


app.MapPost("/Files/UploadFiles", [Authorize] (HttpRequest request) =>
{
    if (!request.Form.Files.Any())
        return Results.BadRequest("At least one file is needed.");

    foreach (var file in request.Form.Files)
    {
        try
        {
            MySqlConnection con = new MySqlConnection("SERVER=127.0.0.1;" + "DATABASE=testdatabase;" + "UID=root;" + "PASSWORD=Ayoub123?;");
            con.Open();

            MySqlCommand icmmd = new MySqlCommand("INSERT INTO logs(datetime,action_type,filename)VALUES(@a,@b,@c)", con);
            icmmd.Parameters.AddWithValue("a", DateTime.Now);
            icmmd.Parameters.AddWithValue("b", 0);
            icmmd.Parameters.AddWithValue("c", file.FileName);
            icmmd.ExecuteNonQuery();
            
            con.Close();
        }
        catch (MySqlException)
        {
            return Results.BadRequest("Error while trying to save the file to the database.");
        }

        using (var stream = new FileStream(@"C:\Users\user\Desktop\Mohammad Ayoub\Part 1\Exercice 3\officeCommunication\Uploaded Files\" + file.FileName, FileMode.Create))
        {
            file.CopyTo(stream);
        }
    }

    return Results.Ok("File Upload Successful.");

})
.WithName("PostUploadFiles")
.Accepts<List<IFormFile>>("multipart/form-data");


app.MapGet("/Files/DownloadFiles/{requiredFile}", [Authorize] (string requiredFile) =>
{

    int fileExist = 0;

    try
    {
        MySqlConnection con = new MySqlConnection("SERVER=127.0.0.1;" + "DATABASE=testdatabase;" + "UID=root;" + "PASSWORD=Ayoub123?;");
        con.Open();

        MySqlCommand icmmd = new MySqlCommand("SELECT COUNT(*) FROM logs where filename = '" + requiredFile + "'", con);

        fileExist = (int)(long)icmmd.ExecuteScalar();

        con.Close();
    }
    catch (MySqlException)
    {

        return Results.NotFound("The current file name does not exist.");
    }

    if(fileExist > 0)
    {
        try
        {
            MySqlConnection con = new MySqlConnection("SERVER=127.0.0.1;" + "DATABASE=testdatabase;" + "UID=root;" + "PASSWORD=Ayoub123?;");
            con.Open();

            MySqlCommand icmmd = new MySqlCommand("INSERT INTO logs(datetime,action_type,filename)VALUES(@a,@b,@c)", con);
            icmmd.Parameters.AddWithValue("a", DateTime.Now);
            icmmd.Parameters.AddWithValue("b", 1);
            icmmd.Parameters.AddWithValue("c", requiredFile);
            icmmd.ExecuteNonQuery();
            con.Close();
        }
        catch (MySqlException)
        {

            return Results.BadRequest("Error while trying to save the file to the database.");
        }

        using (FileStream selectedFile = File.Open(@"C:\Users\user\Desktop\Mohammad Ayoub\Part 1\Exercice 3\officeCommunication\Uploaded Files\" + requiredFile, FileMode.Open, FileAccess.Read))
        {
            using (var stream = new FileStream(@"C:\Users\user\Desktop\Mohammad Ayoub\Part 1\Exercice 3\officeCommunication\Downloaded Files\" + requiredFile, FileMode.Create))
            {
                selectedFile.CopyTo(stream);
            }

        }

    }
    else
    {
        return Results.NotFound("File Not Found.");
    }

    return Results.Ok("File Download Successful.");

});

app.Run();
