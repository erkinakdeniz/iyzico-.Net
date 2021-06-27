using Business.Handlers.Authorizations.Commands;
using Business.Handlers.Authorizations.Queries;
using Business.Handlers.Users.Commands;
using Business.Services.Authentication.Model;
using Business.Services.iyzico;
using Core.Utilities.Results;
using Core.Utilities.Security.Jwt;
using Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    /// <summary>
    /// Make it Authorization operations
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class IyzicoController : BaseApiController
    {
        private readonly IConfiguration _configuration;
        private readonly IIyzicoService _iyzicoService;

        /// <summary>
        /// Dependency injection is provided by constructor injection.
        /// </summary>
        /// <param name="configuration"></param>
        public IyzicoController(IConfiguration configuration,IIyzicoService ıyzicoService)
        {
            _configuration = configuration;
            _iyzicoService = ıyzicoService;
        }



        /// <summary>
        ///  Make it User Register operations
        /// </summary>
        /// <param name="iyzicoModel"></param>
        /// <returns></returns>        
        [AllowAnonymous]
        [Consumes("application/json")]
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IResult))]
        [HttpPost("pay")]
        public async Task<IActionResult> Payment(Iyzico iyzicoModel)
        {
            var result = await _iyzicoService.PayWithIyzico(iyzicoModel);

            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

    }
}