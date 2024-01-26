using CodeDom.NET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeDom.NET.Abstract
{
    public interface IModelBuilder
    {
        protected bool CreatedModel { get; set; }
        IModelBuilder SetModel(BaseModel model);
        IModelBuilder CreateEntity();
        IModelBuilder CreateConfiguration(BaseEntity configurationModel);
        IModelBuilder CreateRepository(BaseEntity InterfaceRepositoryModel, BaseEntity RepositoryMoodel, string DbContextType);
        IModelBuilder CreateService(BaseEntity InterfaceServiceModel, BaseEntity ManagerModel);
    }
}
