using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TokenManager.Web.Api.Models.Request.API;
using TokenManager.Web.Api.Models.Tokens;
using TokenManager.Web.Api.Services;
using TokenManager.Web.Api.Services.EOS;

namespace TokenManager.Web.Api.Controllers
{
    [Route("api/v1/tokens/treeptoken")]
    public class TreepTokenController : TokenBaseController<TreepToken>
    {
        public TreepTokenController(EosBlockchainService eosBlockchainService) : base(eosBlockchainService) { }

        [HttpPost("transfer")]
        public override async Task<IActionResult> TransferAsync([FromBody] TransferRequest request)
        {
            var result = await _eosBlockchainService.TransferAsync("euroexchange", tokenDefinition.Symbol, tokenDefinition.Decimals, request.FromAddress, request.ToAddress, request.Quantity, request.Unstaked, request.Memo);
            if (result.Item1) 
            {
                return await Task.FromResult(Ok(result.Item2));
            }
            return await Task.FromResult(BadRequest(result.Item2));
        }

        [HttpPost("unstake")]
        public override async Task<IActionResult> UnstakeAsync([FromBody] UnstakeRequest request)
        {
            await _eosBlockchainService.UnstakeAsync(request.Name, request.Quantity, tokenDefinition.Symbol, tokenDefinition.Decimals);
            return await Task.FromResult(Ok());
        }
    }
}