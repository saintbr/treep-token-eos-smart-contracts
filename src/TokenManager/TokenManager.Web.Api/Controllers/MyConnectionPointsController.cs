using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TokenManager.Web.Api.Models.Request.API;
using TokenManager.Web.Api.Models.Tokens;
using TokenManager.Web.Api.Services.EOS;

namespace TokenManager.Web.Api.Controllers
{
    [Route("api/v1/tokens/myconnectionpoints")]
    public class MyConnectionPointsController : TokenBaseController<MyConnectionPoints>
    {
        public MyConnectionPointsController(EosBlockchainService eosBlockchainService) : base(eosBlockchainService) { }

        [HttpPost("transfer")]
        public override async Task<IActionResult> TransferAsync([FromBody] TransferRequest request)
        {
            var result = await _eosBlockchainService.TransferAsync("euroexchange", tokenDefinition.Symbol, tokenDefinition.Decimals, request.FromAddress, request.ToAddress, request.Quantity, request.Unstaked, request.Memo);
            if (result.Item1)
            {
                return await Task.FromResult(Ok(result.Item2)).ConfigureAwait(false);
            }
            return await Task.FromResult(BadRequest(result.Item2)).ConfigureAwait(false);
        }

        [HttpPost("unstake")]
        public override async Task<IActionResult> UnstakeAsync([FromBody] UnstakeRequest request)
        {
            await _eosBlockchainService.UnstakeAsync(request.Name, request.Quantity, tokenDefinition.Symbol, tokenDefinition.Decimals);
            return await Task.FromResult(Ok()).ConfigureAwait(false);
        }
    }
}