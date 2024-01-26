// See https://aka.ms/new-console-template for more information

using CodeDom.NET.Concrete;
using CodeDom.NET.Models;

ModelBuilder modelBuilder = new ModelBuilder();
BaseModel model = new BaseModel()
{
    ModelName = "Test",
    Namespace = "ConsoleApp1.Domain",
    Properties = new List<PropertyModel>()
       {
           new PropertyModel()
           {
                 PropertyName="Id",
                  IsGet=true,
                   IsSet=true,
                    PropertyType="int"
           },
           new PropertyModel()
           {
                 PropertyName="Name",
                  IsGet=true,
                   IsSet=true,
                    PropertyType="string"
           }
       }
};

var result=modelBuilder.SetModel(model).CreateEntity().CreateConfiguration(new BaseEntity() { BaseType = null, Namespace = "ConsoleApp1.Configurations" })
    .CreateRepository(
    new BaseEntity() { BaseType = "IRepository", Namespace = "ConsoleApp1.Repositories.Abstract" },
    new BaseEntity() { BaseType = "GenericRepository", Namespace = "ConsoleApp1.Repositories.Concrete" },
    "AppDbContext"
    ).CreateService(
    new BaseEntity() { BaseType =string.Empty, Namespace = "ConsoleApp1.Services.Abstract" },
    new BaseEntity() { BaseType = string.Empty, Namespace = "ConsoleApp1.Services.Concrete" });

Console.WriteLine("Hello, World!");
