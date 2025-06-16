using Microsoft.AspNetCore.Authentication.Negotiate;

namespace Tasks.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddOpenApi();

            builder.Services.AddTransient<ErrorMiddleware>();

            //builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
            //    .AddNegotiate();

            //builder.Services.AddAuthorization(options =>
            //{
            //    // By default, all incoming requests will be authorized according to the default policy.
            //    options.FallbackPolicy = options.DefaultPolicy;
            //});

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseMiddleware<ErrorMiddleware>();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
