using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjetBiere.Entity;

namespace ProjetBiere.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    public class StyleController : ControllerBase
    {


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(Enum.GetValues(typeof(Style)));
            }
            catch (Exception ex)
            {
                //return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
                //Logging            
                return BadRequest(ex);
            }
        }
    }
}