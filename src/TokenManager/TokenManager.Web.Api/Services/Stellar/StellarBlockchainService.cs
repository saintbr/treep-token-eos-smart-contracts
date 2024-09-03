using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using stellar_dotnet_sdk;
using stellar_dotnet_sdk.responses;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace TokenManager.Web.Api.Services.Stellar
{
    public class StellarBlockchainService : IBlockchainService
    {
        private readonly StellarConfig _config;
        private readonly Server server;

        public StellarBlockchainService(IOptions<StellarConfig> config)
        {
            _config = config.Value;
            Network.UsePublicNetwork();
            server = new Server(_config.HttpEndpoint);
        }

        public async Task<Tuple<bool, string, string>> CreateAccount(long contaId) 
        {
            try
            {
                //create account
                var memo = JsonConvert.SerializeObject(new { contaId }, Formatting.Indented);
                var kp = KeyPair.Random();
                var source = await server.Accounts.Account(_config.AccountIssuer.PublicKey);
                var skp = KeyPair.FromSecretSeed(_config.AccountIssuer.PrivateKey);
                var cao = new CreateAccountOperation.Builder(kp, 3.ToString()).SetSourceAccount(skp).Build();
                var transaction = new Transaction.Builder(source).AddOperation(cao).AddMemo(Memo.Text(memo)).Build();
                transaction.Sign(skp);
                var response = await server.SubmitTransaction(transaction);
                if (!response.IsSuccess())
                    return await Task.FromResult(new Tuple<bool, string, string>(false, response.SubmitTransactionResponseExtras.ResultXdr, string.Empty));

                await Console.Out.WriteLineAsync(JsonConvert.SerializeObject(new { CreateAccount = response }, Formatting.Indented));

                return await Task.FromResult(new Tuple<bool, string, string>(true, kp.AccountId, kp.SecretSeed));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new Tuple<bool, string, string>(false, ex.Message, string.Empty));
            }            
        }

        public async Task SignAccount(string account, string seed)
        {
            var acc = await GetAccount(account);
            if (acc.Signers.ToList().Any(s => s.Key.Equals(_config.AccountIssuer.PublicKey)))
                return;

            //set key from master
            var source = await server.Accounts.Account(account);
            var source_seed = KeyPair.FromSecretSeed(seed);

            var signer = KeyPair.FromSecretSeed(_config.AccountIssuer.PrivateKey);

            var soo = new SetOptionsOperation.Builder(new stellar_dotnet_sdk.xdr.SetOptionsOp
            {
                Signer = new stellar_dotnet_sdk.xdr.Signer
                {
                    Key = signer.XdrSignerKey,
                    Weight = new stellar_dotnet_sdk.xdr.Uint32(1)
                }
            }).Build();
            var transaction = new Transaction.Builder(source).AddOperation(soo).Build();
            transaction.Sign(source_seed);
            var response = await server.SubmitTransaction(transaction);
            await Console.Out.WriteLineAsync(JsonConvert.SerializeObject(new { SetOptions = response}, Formatting.Indented));
        }

        public async Task TrustToken(string account, string seed)
        {
            try
            {
                var source = await server.Accounts.Account(account);
                var source_seed = KeyPair.FromSecretSeed(seed);

                var ct = new ChangeTrustOperation.Builder(new stellar_dotnet_sdk.xdr.ChangeTrustOp
                {
                    Limit = new stellar_dotnet_sdk.xdr.Int64(150000000000000000),
                    Line = Asset.CreateNonNativeAsset(_config.TokenIssuer.Symbol, _config.TokenIssuer.PublicKey).ToXdr()
                }).Build();
                var transaction = new Transaction.Builder(source).AddOperation(ct).Build();
                transaction.Sign(source_seed);
                var response = await server.SubmitTransaction(transaction);
                if (response.IsSuccess())
                    await Console.Out.WriteLineAsync(JsonConvert.SerializeObject(new { ChangeTrust = response }, Formatting.Indented));

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }

        public async Task<Tuple<bool, string>> Payment(string source, string target, long amount, string memo)
        {
            try
            {
                var _source = await server.Accounts.Account(source);
                var _target = await server.Accounts.Account(target);

                var _source_seed = KeyPair.FromSecretSeed(_config.AccountIssuer.PrivateKey);

                var p = new PaymentOperation.Builder(new stellar_dotnet_sdk.xdr.PaymentOp
                {
                    Amount = new stellar_dotnet_sdk.xdr.Int64(amount),
                    Destination = new stellar_dotnet_sdk.xdr.AccountID(_target.KeyPair.XdrPublicKey),
                    Asset = Asset.CreateNonNativeAsset(_config.TokenIssuer.Symbol, _config.TokenIssuer.PublicKey).ToXdr()
                }).Build();

                var transaction = new Transaction.Builder(_source).AddOperation(p).Build();

                transaction.Sign(_source_seed);
                //return await Task.FromResult(new Tuple<bool, string>(true, "5c5c875997363b7946866954371d5d79b58dc61ed082d1c70da05a2270d8ac2b"));

                var response = await server.SubmitTransaction(transaction);
                if (response.IsSuccess())
                {
                    await Console.Out.WriteLineAsync(JsonConvert.SerializeObject(new { Payment = response }, Formatting.Indented));
                    return await Task.FromResult(new Tuple<bool, string>(true, response.Hash));
                }
                return await Task.FromResult(new Tuple<bool, string>(false, response.SubmitTransactionResponseExtras.ResultXdr));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new Tuple<bool, string>(false, ex.Message));
            }
        }

        public async Task<AccountResponse> GetAccount(string account)
        {
            try
            {
                return await server.Accounts.Account(account);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}