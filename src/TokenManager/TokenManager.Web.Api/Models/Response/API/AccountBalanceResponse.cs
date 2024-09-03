using System;
using System.Collections.Generic;
using System.Globalization;
using EosSharp.Core.Api.v1;
using Newtonsoft.Json.Linq;
using TokenManager.Web.Api.Models.Tokens;

namespace TokenManager.Web.Api.Models.Response.API
{
    public class AccountBalanceResponse : List<AccountBalanceInfo>
    {
        public AccountBalanceResponse(GetTableRowsResponse result)
        {
            foreach (var row in result.rows)
            {
                var symbol = ((JObject)row).Value<string>("balance").Split(" ")[1];
                var balance = ((JObject)row).Value<string>("balance").Split(" ")[0];
                var staked = ((JObject)row).Value<string>("staked").Split(" ")[0];
                this.Add(new AccountBalanceInfo
                {
                    Balance = decimal.Parse(balance, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture),
                    Staked = decimal.Parse(staked.Replace("-", ""), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture),
                    Symbol = symbol
                });
            }
        }
    }
}