using AcmeLib;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AcmeBankApi.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AcmeBankApiController : ControllerBase
    {
        [HttpGet, Route("getaccounts")]
        public async Task<IActionResult> GetAllAccounts(string email, IBankService svc)
        {
            try
            {
                return Ok(await svc.GetAccounts(email));
            }
            catch (Exception)
            {
                return NotFound();
            }
          
        }
    }
}
