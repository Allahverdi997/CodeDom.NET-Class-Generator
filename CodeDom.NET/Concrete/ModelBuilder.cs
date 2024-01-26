using CodeDom.NET.Abstract;
using CodeDom.NET.Models;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace CodeDom.NET.Concrete
{
    public class ModelBuilder : IModelBuilder
    {
        private BaseModel Model { get; set; }
        public bool CreatedModel { get ; set ; }

        public IModelBuilder CreateConfiguration(BaseEntity ConfigurationModel)
        {
            if (!CreatedModel)
                throw new ArgumentNullException("Please, use SetModel methow previosly");

            if (string.IsNullOrEmpty(ConfigurationModel.Namespace))
                throw new ArgumentNullException("Please, enter Namespace into model");

            CodeCompileUnit compileUnit = new CodeCompileUnit();

            CodeNamespace samples = new CodeNamespace(ConfigurationModel.Namespace);
            compileUnit.Namespaces.Add(samples);

            string _name = Model.ModelName + "Configuration";

            CodeTypeDeclaration _class = new CodeTypeDeclaration(_name);
            _class.IsClass = true;

            _class.TypeAttributes =
                TypeAttributes.Public | TypeAttributes.Sealed;

            samples.Types.Add(_class);

            if(!string.IsNullOrEmpty(ConfigurationModel.BaseType))
               _class.BaseTypes.Add(ConfigurationModel.BaseType);
            else
                _class.BaseTypes.Add("IEntityTypeConfiguration<" + Model.ModelName+">");

            CodeDomProvider _codeDomProvider = CodeDomProvider.CreateProvider("CSharp");
            IndentedTextWriter tw = new IndentedTextWriter(new System.IO.StreamWriter(_name, true), "    ");
            _codeDomProvider.GenerateCodeFromCompileUnit(compileUnit, tw, new CodeGeneratorOptions());
            tw.Close();

            return this;
        }

        public IModelBuilder CreateEntity()
        {
            if (!CreatedModel)
                throw new ArgumentNullException("Please, use SetModel methow previosly");

            string _name = Model.ModelName;
            CodeCompileUnit compileUnit = new CodeCompileUnit();

            CodeNamespace samples = new CodeNamespace(Model.Namespace);
            compileUnit.Namespaces.Add(samples);

            CodeTypeDeclaration class1 = new CodeTypeDeclaration(_name);

            if (Model.Properties.Count > 0)
            {
                foreach (var item in Model.Properties)
                {
                    string comment = string.Empty;
                    string accessor = "public";
                    string property= string.Empty;

                    CodeSnippetTypeMember snippet = new CodeSnippetTypeMember();
                    if(!string.IsNullOrEmpty(item.Comment) != null)
                       snippet.Comments.Add(new CodeCommentStatement(item.Comment, true));

                    if(!string.IsNullOrEmpty(item.Accessor))
                        accessor= item.Accessor;

                    if (string.IsNullOrEmpty(item.PropertyType) && string.IsNullOrEmpty(item.PropertyName))
                        throw new Exception("Please, enter PropertyName and PropertyType");

                    if(item.IsGet)
                      property=accessor + " " + item.PropertyType + " " + item.PropertyName+" { get; }";

                    if (item.IsSet)
                        property = accessor + " " + item.PropertyType + " " + item.PropertyName + " { set; }";

                    if(item.IsGet & item.IsSet)
                        property = accessor + " " + item.PropertyType + " " + item.PropertyName + " { get; set; }";

                    snippet.Text = property;
                    class1.Members.Add(snippet);
                }
            }

            if(!string.IsNullOrEmpty(Model.BaseType))
               class1.BaseTypes.Add(new CodeTypeReference(Model.BaseType));

            class1.IsClass = true;
            class1.TypeAttributes =
                TypeAttributes.Public | TypeAttributes.Sealed;

            samples.Types.Add(class1);

            CodeDomProvider codeDomProvider = CodeDomProvider.CreateProvider("CSharp");
            IndentedTextWriter tw = new IndentedTextWriter(new System.IO.StreamWriter(Model.ModelName, true), "    ");
            codeDomProvider.GenerateCodeFromCompileUnit(compileUnit, tw, new CodeGeneratorOptions());
            tw.Close();

            return this;
        }

        public IModelBuilder CreateRepository(BaseEntity InterfaceRepositoryModel, BaseEntity RepositoryMoodel, string DbContextType)
        {
            if (!CreatedModel)
                throw new ArgumentNullException("Please, use SetModel methow previosly");

            if (string.IsNullOrEmpty(InterfaceRepositoryModel.Namespace))
                throw new ArgumentNullException("Please, enter Namespace into model");

            if (string.IsNullOrEmpty(RepositoryMoodel.Namespace))
                throw new ArgumentNullException("Please, enter Namespace into model");

            CodeCompileUnit compileUnit = new CodeCompileUnit();

            CodeNamespace samples = new CodeNamespace(InterfaceRepositoryModel.Namespace);
            compileUnit.Namespaces.Add(samples);

            #region Repository Interface
            string _interfacename = "I" + Model.ModelName + "Repository";

            CodeTypeDeclaration interfaceclass = new CodeTypeDeclaration(_interfacename);
            interfaceclass.IsInterface = true;
            samples.Types.Add(interfaceclass);

            interfaceclass.BaseTypes.Add(InterfaceRepositoryModel.BaseType+ "<" + Model.ModelName + ">");
            interfaceclass.Name = _interfacename;

            CodeDomProvider codeDomProvider = CodeDomProvider.CreateProvider("CSharp");
            IndentedTextWriter tw = new IndentedTextWriter(new System.IO.StreamWriter(_interfacename, true), "    ");
            codeDomProvider.GenerateCodeFromCompileUnit(compileUnit, tw, new CodeGeneratorOptions());
            tw.Close();
            #endregion

            #region Repository Class
            string _name = Model.ModelName + "Repository";

            CodeTypeDeclaration _class = new CodeTypeDeclaration(_name);
            _class.IsClass = true;

            if(!string.IsNullOrEmpty(DbContextType))
            {
                CodeConstructor baseStringConstructor = new CodeConstructor();
                baseStringConstructor.Attributes = MemberAttributes.Public;
                // Declares a parameter of type string named "TestStringParameter".
                baseStringConstructor.Parameters.Add(new CodeParameterDeclarationExpression(DbContextType, "_context"));
                // Calls a base class constructor with the TestStringParameter parameter.
                baseStringConstructor.BaseConstructorArgs.Add(new CodeVariableReferenceExpression("_context"));
                // Adds the constructor to the Members collection of the DerivedType.
                _class.Members.Add(baseStringConstructor);
            }
            
            samples.Types.Add(_class);

            _class.BaseTypes.Add(RepositoryMoodel.BaseType + "<" + Model.ModelName + ">");
            _class.BaseTypes.Add(_interfacename);


            _class.TypeAttributes =
                TypeAttributes.Public | TypeAttributes.Sealed;

            CodeDomProvider _codeDomProvider = CodeDomProvider.CreateProvider("CSharp");
            IndentedTextWriter _tw = new IndentedTextWriter(new System.IO.StreamWriter(_name, true), "    ");
            _codeDomProvider.GenerateCodeFromCompileUnit(compileUnit, _tw, new CodeGeneratorOptions());
            _tw.Close();
            #endregion

            return this;
        }

        public IModelBuilder CreateService(BaseEntity InterfaceServiceModel, BaseEntity ManagerModel)
        {
            if (!CreatedModel)
                throw new ArgumentNullException("Please, use SetModel methow previosly");

            if (string.IsNullOrEmpty(InterfaceServiceModel.Namespace))
                throw new ArgumentNullException("Please, enter Namespace into model");

            CodeCompileUnit compileUnit = new CodeCompileUnit();

            CodeNamespace samples = new CodeNamespace(InterfaceServiceModel.Namespace);
            compileUnit.Namespaces.Add(samples);

            #region Service Interface
            string _interfacename = "I" + Model.ModelName + "Service";

            CodeTypeDeclaration interfaceclass = new CodeTypeDeclaration();
            interfaceclass.IsInterface = true;
            samples.Types.Add(interfaceclass);

            if(!string.IsNullOrEmpty(InterfaceServiceModel.BaseType))
                interfaceclass.BaseTypes.Add(InterfaceServiceModel.BaseType);

            interfaceclass.Name = _interfacename;

            CodeDomProvider codeDomProvider = CodeDomProvider.CreateProvider("CSharp");
            IndentedTextWriter tw = new IndentedTextWriter(new System.IO.StreamWriter(_interfacename, true), "    ");
            codeDomProvider.GenerateCodeFromCompileUnit(compileUnit, tw, new CodeGeneratorOptions());
            tw.Close();
            #endregion

            #region Manager Class
            if (string.IsNullOrEmpty(ManagerModel.Namespace))
                throw new ArgumentNullException("Please, enter Namespace into model");

            string _name = Model.ModelName + "Manager";

            CodeTypeDeclaration _class = new CodeTypeDeclaration(_name);
            _class.IsClass = true;

            CodeSnippetTypeMember snippet = new CodeSnippetTypeMember();

            var property ="public "+ "I" + Model.ModelName + "Repository"+" Repository { get; set; }";

            snippet.Text = property;
            _class.Members.Add(snippet);

            CodeConstructor baseStringConstructor = new CodeConstructor();
            baseStringConstructor.Attributes = MemberAttributes.Public;
            // Declares a parameter of type string named "TestStringParameter".
            baseStringConstructor.Parameters.Add(new CodeParameterDeclarationExpression("I"+Model.ModelName+"Repository", "repository"));
            // Adds the constructor to the Members collection of the DerivedType.

            CodeFieldReferenceExpression widthReference =
                new CodeFieldReferenceExpression(
                new CodeThisReferenceExpression(), "Repository");
            baseStringConstructor.Statements.Add(new CodeAssignStatement(widthReference,
                new CodeArgumentReferenceExpression("repository")));

            _class.Members.Add(baseStringConstructor);

            _class.TypeAttributes =
                TypeAttributes.Public | TypeAttributes.Sealed;

            samples.Types.Add(_class);

            if (!string.IsNullOrEmpty(ManagerModel.BaseType))
                _class.BaseTypes.Add(ManagerModel.BaseType);

            _class.BaseTypes.Add(_interfacename);

            _class.Name = _name;

            CodeDomProvider _codeDomProvider = CodeDomProvider.CreateProvider("CSharp");
            IndentedTextWriter _tw = new IndentedTextWriter(new System.IO.StreamWriter(_name, true), "    ");
            _codeDomProvider.GenerateCodeFromCompileUnit(compileUnit, _tw, new CodeGeneratorOptions());
            _tw.Close();
            #endregion

            return this;
        }

        public IModelBuilder SetModel(BaseModel entityModel)
        {
            if(string.IsNullOrEmpty(entityModel.ModelName))
                throw new ArgumentNullException("Please, enter ModelName into model");

            if(string.IsNullOrEmpty(entityModel.Namespace))
                throw new ArgumentNullException("Please, enter Namespace into model");

            Model = entityModel;

            this.CreatedModel = true;
            return this;
        }
    }
}
