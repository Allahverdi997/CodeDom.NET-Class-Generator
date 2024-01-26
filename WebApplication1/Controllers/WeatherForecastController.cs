using CodeDom.NET.Concrete;
using CodeDom.NET.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection.Emit;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            ModelBuilder modelBuilder = new ModelBuilder();

            BaseModel model = new BaseModel()
            {
                ModelName = "Test",
                Namespace = "ConsoleApp1.Domain",
                Properties = new List<CodeDom.NET.Models.PropertyModel>()
                {
                    new CodeDom.NET.Models.PropertyModel()
                    {
                        PropertyName = "Id",
                        IsGet = true,
                        IsSet = true,
                        PropertyType = "int"
                    },
                    new CodeDom.NET.Models.PropertyModel()
                    {
                        PropertyName = "Name",
                        IsGet = true,
                        IsSet = true,
                        PropertyType = "string"
                    }
                }
            };

            var result = modelBuilder.SetModel(model).CreateEntity().CreateConfiguration(new BaseEntity() { BaseType = null, Namespace = "ConsoleApp1.Configurations" })
                .CreateRepository(
                new BaseEntity() { BaseType = "IRepository", Namespace = "ConsoleApp1.Repositories.Abstract" },
                new BaseEntity() { BaseType = "GenericRepository", Namespace = "ConsoleApp1.Repositories.Concrete" },
                "AppDbContext"
                ).CreateService(
                new BaseEntity() { BaseType = string.Empty, Namespace = "ConsoleApp1.Services.Abstract" },
                new BaseEntity() { BaseType = string.Empty, Namespace = "ConsoleApp1.Services.Concrete" });


            CixmaApplicationLibrary.Cixma cixma = new CixmaApplicationLibrary.Cixma();
            var b= cixma.Cix(5, 2);

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}