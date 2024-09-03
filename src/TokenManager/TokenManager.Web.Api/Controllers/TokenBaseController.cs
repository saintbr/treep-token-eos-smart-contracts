using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TokenManager.Web.Api.Models.Request.API;
using TokenManager.Web.Api.Models.Tokens;
using TokenManager.Web.Api.Services;
using TokenManager.Web.Api.Services.EOS;

namespace TokenManager.Web.Api.Controllers
{
    [ApiController]
    public abstract class TokenBaseController<T> : ControllerBase where T : TokenDefinition, new()
    {
        protected readonly TokenDefinition tokenDefinition;
        protected readonly EosBlockchainService _eosBlockchainService;

        public TokenBaseController(EosBlockchainService eosBlockchainService)
        {
            tokenDefinition = new T();
            _eosBlockchainService = eosBlockchainService;
        }

        public abstract Task<IActionResult> TransferAsync([FromBody] TransferRequest request);

        public abstract Task<IActionResult> UnstakeAsync([FromBody] UnstakeRequest request);
    }
}