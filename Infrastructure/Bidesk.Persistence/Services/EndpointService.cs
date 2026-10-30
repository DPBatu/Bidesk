using Bidesk.Application.Abstraction.Services;
using Bidesk.Application.Custom_Attributes;
using Bidesk.Application.DTOs.Endpoint;
using Bidesk.Application.Enums.User;
using Bidesk.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Reflection;

namespace Bidesk.Persistence.Services
{
    public class EndpointService : IEndpointService
    {
        public List<EndpointDto> GetAPIEndpoints(Type type)
        {
            Assembly? assembly = Assembly.GetAssembly(type);
            if (assembly == null) throw new BaseException("Program bilgileri alınamadı.", "Program alınamadı!");

            List<EndpointDto> results = new();

            List<Type> controllers = assembly.GetTypes().Where(t => t.IsAssignableTo(typeof(ControllerBase))).ToList();

            foreach (var controller in controllers)
            {
                List<MethodInfo> actions = controller.GetMethods().Where(m => m.IsDefined(typeof(AuthorizeDefinitionAttribute))).ToList();

                foreach (var action in actions)
                {
                    var attributes = action.GetCustomAttributes(true);
                    if (attributes == null) continue;

                    AuthorizeDefinitionAttribute? authorizeDefinitionAttribute = attributes.FirstOrDefault(a => a.GetType() == typeof(AuthorizeDefinitionAttribute)) as AuthorizeDefinitionAttribute;
                    if (authorizeDefinitionAttribute == null) continue;

                    string? actionType = Enum.GetName(typeof(ActionType), authorizeDefinitionAttribute.ActionType);
                    string? portalType = Enum.GetName(typeof(PortalType), authorizeDefinitionAttribute.Portal);
                    string? menuType = Enum.GetName(typeof(MenuType), authorizeDefinitionAttribute.Menu);

                    if (actionType == null || portalType == null || menuType == null) continue;

                    EndpointDto endpoint = new()
                    {
                        ActionType = actionType,
                        Definition = authorizeDefinitionAttribute.Definition,
                        PortalType = portalType,
                        MenuType = menuType
                    };

                    HttpMethodAttribute? httpAttribute = attributes.FirstOrDefault(a => a.GetType().IsAssignableTo(typeof(HttpMethodAttribute))) as HttpMethodAttribute;
                    endpoint.HttpType = httpAttribute == null ? HttpMethods.Get : httpAttribute.HttpMethods.First();

                    endpoint.Code = $"{endpoint.PortalType}.{endpoint.HttpType}.{endpoint.ActionType}.{endpoint.Definition.Replace(" ", "")}";
                    results.Add(endpoint);
                }
            }

            return results;
        }
    }
}
