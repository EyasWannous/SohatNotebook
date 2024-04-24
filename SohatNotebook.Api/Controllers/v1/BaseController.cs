using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SohatNotebook.DataService.IConfiguration;
using SohatNotebook.Entities.DTOs.Errors;

namespace SohatNotebook.Api.Controllers.v1;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class BaseController(IUnitOfWork unitOfWork,
    UserManager<IdentityUser> userManager, IMapper mapper) : ControllerBase
{
    protected readonly IUnitOfWork _unitOfWork = unitOfWork;
    protected readonly UserManager<IdentityUser> _userManager = userManager;
    protected readonly IMapper _mapper = mapper;

    internal Error PopulateError(int code, string message, string type)
    {
        return new Error
        {
            Code = code,
            Message = message,
            Type = type
        };
    }
}