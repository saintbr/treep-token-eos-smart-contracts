using EosSharp;
using EosSharp.Core;
using EosSharp.Core.Api.v1;
using EosSharp.Core.Exceptions;
using EosSharp.Core.Providers;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TokenManager.Web.Api.Models.Authorities;
using TokenManager.Web.Api.Models.Request.EOSIO;
using TokenManager.Web.Api.Models.Tokens;

namespace TokenManager.Web.Api.Services.EOS
{
    public class EosBlockchainService : IBlockchainService
    {
        private readonly EOSConfig _config;
        protected readonly Eos _eos_token;
        protected readonly Eos _eos_account;

        public EosBlockchainService(IOptions<EOSConfig> config)
        {
            _config = config.Value;

            _eos_token = new Eos(new EosConfigurator()
            {
                HttpEndpoint = _config.HttpEndpoint,
                ChainId = _config.ChainId,
                ExpireSeconds = 60,
                SignProvider = new DefaultSignProvider(_config.TokenIssuer.PrivateKey)
            });

            _eos_account = new Eos(new EosConfigurator()
            {
                HttpEndpoint = _config.HttpEndpoint,
                ChainId = _config.ChainId,
                ExpireSeconds = 60,
                SignProvider = new DefaultSignProvider(_config.AccountIssuer.PrivateKey)
            });
        }

        public async Task AdjustAccountHealth(string from, string to, string net_weight, string cpu_weight)
        {
            try
            {
                var transaction = new Transaction()
                {
                    actions = new List<Action>
                    {
                        new Action()
                        {
                            account = "eosio",
                            authorization = new List<PermissionLevel>()
                            {
                                new PermissionLevel() { actor = from, permission = "active" }
                            },
                            name = "undelegatebw",
                            data = new { from = from, receiver = to, unstake_net_quantity = net_weight, unstake_cpu_quantity = cpu_weight }
                        },

                        //new Action()
                        //{
                        //    account = "eosio",
                        //    authorization = new List<PermissionLevel>()
                        //    {
                        //        new PermissionLevel() { actor = from, permission = "active" }
                        //    },
                        //    name = "delegatebw",
                        //    data = new { from = from, receiver = to, stake_net_quantity = "0.0500 EOS", stake_cpu_quantity = "0.1500 EOS", transfer = false }
                        //}
                    }
                };
                await System.Console.Out.WriteLineAsync(JsonConvert.SerializeObject(new { transaction }, Formatting.Indented));
                var result_newaccount = await _eos_account.CreateTransaction(transaction);
            }
            catch (ApiErrorException ex)
            {
                var error = ex.error.details;
            }
        }

        public async Task<string> GetAccountAsync(string accountName)
        {
            var acc = await _eos_account.GetAccount(accountName);
            return string.Empty;
        }

        public async Task<List<T>> GetTableDataAsync<T>(string code, string table, string scope, string lower_bound) where T : class
        {
            var data = await _eos_account.GetTableRows<T>(new GetTableRowsRequest 
            {
                json = true,
                code = code,
                scope = scope,
                table = table,
                limit = 1000,
                lower_bound = lower_bound
            });
            return await Task.FromResult(data.rows);
        }

        public async Task UnstakeAsync(string name, decimal quantity, TokenSymbol symbol, int decimals)
        {
            try
            {
                var _quantity = $"{System.Math.Round(quantity, decimals)} {symbol}".Replace(",", ".");

                var result_transfer = await _eos_account.CreateTransaction(new Transaction
                {
                    actions = new List<Action>
                    {
                        new Action
                        {
                            account = "euroexchange",
                            authorization = new List<PermissionLevel>()
                            {
                                new PermissionLevel() { actor = name, permission = "active" }
                            },
                            name = "unstake",
                            data = new { owner = name, value = _quantity }
                        }
                    }
                });
                await Task.CompletedTask;
            }
            catch (ApiErrorException ex)
            {
                var error = ex.error.details;
            }
        }

        internal BaseAuthority GetTokenAuthority()
        {
            return _config.TokenIssuer;
        }

        internal BaseAuthority GetAccountAuthority()
        {
            return _config.AccountIssuer;
        }

        public async Task NewAccountAsync(string address, BaseAuthority issuer)
        {
            try
            {
                var result_newaccount = await _eos_account.CreateTransaction(new Transaction()
                {
                    actions = new List<Action>
                    {
                        new Action
                        {
                            account = "eosio",
                            authorization = new List<PermissionLevel>()
                            {
                                new PermissionLevel() { actor = issuer.Name, permission = "active" }
                            },
                            name = "newaccount",
                            data = new NewAccountRequest
                            {
                                creator = issuer.Name,
                                name = address,
                                active = new NewAccountAutority
                                {
                                    accounts = new List<object> { },
                                    keys = new List<NewAccountAutorityKey>
                                    {
                                        new NewAccountAutorityKey { key = issuer.PublicKey, weight = 1 }
                                    },
                                    threshold = 1,
                                    waits = new List<object> { }
                                },
                                owner = new NewAccountAutority
                                {
                                    accounts = new List<object> { },
                                    keys = new List<NewAccountAutorityKey>
                                    {
                                        new NewAccountAutorityKey { key = issuer.PublicKey, weight = 1 }
                                    },
                                    threshold = 1,
                                    waits = new List<object> { }
                                }
                            },
                        },
                        new Action()
                        {
                            account = "eosio",
                            authorization = new List<PermissionLevel>()
                            {
                                new PermissionLevel() { actor = issuer.Name, permission = "active" }
                            },
                            name = "buyrambytes",
                            data = new { payer = issuer.Name, receiver = address, bytes = 3072 }
                        },
                        new Action()
                        {
                            account = "eosio",
                            authorization = new List<PermissionLevel>()
                            {
                                new PermissionLevel() { actor = issuer.Name, permission = "active" }
                            },
                            name = "delegatebw",
                            data = new { from = issuer.Name, receiver = address, stake_net_quantity = "0.0500 EOS", stake_cpu_quantity = "0.1500 EOS", transfer = false }
                        }
                    }
                });
            }
            catch (ApiErrorException ex)
            {
                var error = ex.error.details;
            }
        }

        public async Task<bool> CheckTransactionAsync(string transactionId)
        {
            try
            {
                //TODO: Check Tx on EOS
                //await _eos_account.GetTransaction(transactionId);
                return true;
            }
            catch (ApiErrorException ex)
            {
                var error = ex.error.details;
                return false;
            }
        }

        public async Task<System.Tuple<bool, string>> TransferAsync(string contract, TokenSymbol symbol, int decimals, string from, string to, decimal quantity, decimal unstaked, string memo) 
        {
            try
            {
                //format to EOS symbol pattern
                var _quantity = $"{System.Math.Round(quantity, decimals)} {symbol}".Replace(",", ".");

                var transaction = new Transaction
                {
                    actions = new List<Action>
                    {
                        new Action
                        {
                            account = "eosio",
                            authorization = new List<PermissionLevel>()
                            {
                                new PermissionLevel() { actor = "treepstaked3", permission = "active" }
                            },
                            name = "delegatebw",
                            data = new { from = "treepstaked3", receiver = from, stake_net_quantity = "0.0500 EOS", stake_cpu_quantity = "2.5000 EOS", transfer = false }
                        },
                        new Action
                        {
                            account = "euroexchange",
                            authorization = new List<PermissionLevel>()
                                {
                                    new PermissionLevel() { actor = from, permission = "active" }
                                },
                            name = "unstake",
                            data = new { owner = from, value = _quantity }
                        },
                        new Action
                        {
                            account = contract,
                            authorization = new List<PermissionLevel>()
                            {
                                new PermissionLevel() { actor = from, permission = "active" }
                            },
                            name = "transfer",
                            data = new { from, to, quantity = _quantity, memo }
                        },
                        new Action
                        {
                            account = "eosio",
                            authorization = new List<PermissionLevel>()
                            {
                                new PermissionLevel() { actor = "treepstaked3", permission = "active" }
                            },
                            name = "undelegatebw",
                            data = new { from = "treepstaked3", receiver = from, unstake_net_quantity = "0.0500 EOS", unstake_cpu_quantity = "2.5000 EOS" }
                        },
                    }
                };
                await System.Console.Out.WriteLineAsync(JsonConvert.SerializeObject(new { transaction }, Formatting.Indented));
                var result = await _eos_account.CreateTransaction(transaction);
                //var result = System.Guid.NewGuid().ToString();
                return new System.Tuple<bool, string>(true, result);
            }
            catch (ApiErrorException ex)
            {
                var error = ex.error.details;
                return new System.Tuple<bool, string>(false, error.FirstOrDefault().message);
            }
        }

        public bool AccountExists(string address) 
        {
            try
            {
                var exists = _eos_account.GetAccount(address).Result;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<GetTableRowsResponse> GetBalanceAsync(string address, string token)
        {
            try
            {
                var info = await _eos_token.GetTableRows(new GetTableRowsRequest()
                {
                    json = true,
                    code = token,
                    scope = address,
                    table = "accounts"
                });
                return await Task.FromResult(info);
            }
            catch (ApiErrorException ex)
            {
                var error = ex.error.details;
                return null;
            }
        }

        internal string GenerateEosAccount()
        {

            StringBuilder sb = new StringBuilder("genbit");
            var numbers = new int[] { 1, 2, 3, 4, 5 };
            for (int i = 0; i < 6; i++)
            {
                System.Random r = new System.Random();
                int randomIndex = r.Next(0, numbers.Length);
                sb.Append(numbers[randomIndex]);
            }
            return sb.ToString();
        }
    }
}