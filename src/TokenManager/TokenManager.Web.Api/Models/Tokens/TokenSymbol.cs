using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TokenManager.Web.Api.Models.Tokens
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TokenSymbol
    {
        //cripto
        TPK,
        TREEP,
        MYPOINT,
        CNPOINT,

        //fiat equivalent
        UsdValue,
        EurValue,
        BrlValue,
    }
}