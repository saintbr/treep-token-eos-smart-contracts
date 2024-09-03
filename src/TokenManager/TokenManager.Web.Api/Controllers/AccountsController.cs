using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TokenManager.Repository.Data;
using TokenManager.Repository.Models;
using TokenManager.Web.Api.Models.Authorities;
using TokenManager.Web.Api.Models.Request.API;
using TokenManager.Web.Api.Models.Response.API;
using TokenManager.Web.Api.Services.EOS;

namespace TokenManager.Web.Api.Controllers
{
    [ApiController, Route("api/v1/accounts")]
    public class AccountsController : ControllerBase
    {
        private readonly BaseAuthority _account_authority;
        private readonly BaseAuthority _token_authority;
        private readonly EosBlockchainService _eosBlockchainService;

        public AccountsController(EosBlockchainService eosBlockchainService)
        {
            _eosBlockchainService = eosBlockchainService;
            _account_authority = _eosBlockchainService.GetAccountAuthority();
            _token_authority = eosBlockchainService.GetTokenAuthority();
        }

        [HttpGet("balance/{address}")]
        public async Task<IActionResult> GetBalanceAsync([FromRoute] string address)
        {
            try
            {
                var result = await _eosBlockchainService.GetBalanceAsync(address, _token_authority.Name);
                return await Task.FromResult(Ok(new AccountBalanceResponse(result)));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("")]
        public async Task<IActionResult> NewAccountAsync([FromBody] NewAccountRequest request)
        {
            await _eosBlockchainService.NewAccountAsync(request.Address, _account_authority);
            return await Task.FromResult(Ok());
        }

        [HttpPost("GenerateRemnantAccount")]
        public async Task<List<string>> GenerateRemnantAccountAsync()
        {
            List<string> listAccounts = new List<string>();
            using (var ctx = new GenbitDB_Context())
            {
                var listaFiltrada = ctx.Conta.FromSqlRaw(@" SELECT distinct gc.ContaId 
                                                              FROM zero10club.dbo.Usuario zu, 
                                                                   zero10club.dbo.UsuarioDetalhe zud, 
	                                                               GenbitDB.dbo.Conta gc
                                                            WHERE zu.Email = gc.Email 
                                                              AND zud.IdUsuario = zu.IdUsuario 
                                                              AND zud.idgraduacao >= 2 
                                                              and gc.ContaId NOT in (select ct.ContaId from GenbitDB.dbo.Carteira c, GenbitDB.dbo.Conta ct, GenbitDB.dbo.Moeda m
                                                            where ct.ContaId = c.ContaId
                                                            and m.MoedaId = c.MoedaId
                                                            and m.Simbolo IN ('EOS', 'TPK'))
                                                            ORDER BY gc.ContaId").ToList();

                foreach (var item in listaFiltrada)
                {
                    try
                    {
                        var novaConta = string.Empty;
                        while (novaConta == string.Empty)
                        {
                            var str = $"gnb{new string(Enumerable.Repeat("12345", 9).Select(s => s[new Random().Next(s.Length)]).ToArray())}";
                            if (!ctx.Carteira.Any(x => x.Url.Equals(str)))
                                novaConta = str;
                        }

                        var retorno = await NewAccountAsync(new NewAccountRequest { Address = novaConta });

                        
                        if (((Microsoft.AspNetCore.Mvc.StatusCodeResult)Ok(retorno).Value).StatusCode == 200)
                        {
                            ctx.Carteira.Add(new Carteira
                            {
                                Descricao = "TPK",
                                Url = novaConta,
                                MoedaId = 6,
                                StatusCarteira = 1,
                                DataCadastro = DateTime.Now,
                                ContaId = item.ContaId,
                                Hash = Guid.NewGuid(),
                                DataConfirmacao = DateTime.Now,
                                TipoCarteira = 1,
                            });

                            listAccounts.Add(novaConta);
                            ctx.SaveChanges();

                        }
                      

                    }
                    catch (Exception)
                    {
                        throw;
                    }

                }
            }
            return listAccounts;
        }
    }
}