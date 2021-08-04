using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using ProjetBiere.Entity;
using ProjetBiere.Modeles;
using ProjetBiere.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjetBiere.Controllers
{
    //[EnableCors]
    [ApiController]
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    //-Forces all actions within the controller to return JSON-formatted responses.
    //-If other formatters are configured and the client specifies a different format, JSON is returned.
    public class BiereController : ControllerBase
    {
        private readonly IBiereService _biereService;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public BiereController(IBiereService biereService, LinkGenerator linkGenerator, IMapper mapper, ILogger<BiereController> logger)
        {
            this._biereService = biereService;
            this._linkGenerator = linkGenerator;
            this._mapper = mapper;
            this._logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BiereModele>>> Get()
        {
            try
            {
                var bieres = await _biereService.GetBiere();
                if (bieres == null) { return NotFound(); } //Peut-être pas nécessaire
                return Ok(_mapper.Map<IEnumerable<BiereModele>>(bieres));
            }
            catch (Exception ex)
            {
                //return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
                _logger.LogInformation("BiereController.Get()", ex);
                return BadRequest(ex);
            }
        }

        [HttpGet("{biereId:int}")]
        public async Task<ActionResult<BiereModele>> Get(int biereId)
        {
            try
            {
                var biere = await _biereService.GetBiere(biereId);
                if (biere == null) return NotFound();
                return Ok(_mapper.Map<BiereModele>(biere));
            }
            catch (Exception ex)
            {
                //return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
                //Logging
                return BadRequest(ex);
            }
        }

        [HttpPost]
        public async Task<ActionResult<BiereModele>> Post(BiereModele biereModele)
        {
            try
            {
                var biere = _mapper.Map<Biere>(biereModele);
                var biereEnBD = await _biereService.GetBiere(biere.Id);
                if (biereEnBD != null) { return BadRequest("Biere déjà sauvegardée"); }
                var bierePost = await _biereService.Post(biere);
                var location = _linkGenerator.GetPathByAction("Get", "Biere", new { biereId = biere.Id });

                if (string.IsNullOrWhiteSpace(location))
                {
                    return BadRequest("Mauvais URL");
                }
                return Created(location, _mapper.Map<BiereModele>(bierePost));
            }
            catch (Exception ex)
            {
                //return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
                //Logging            
                return BadRequest(ex);
            }
        }

        [HttpPut]
        public async Task<ActionResult<BiereModele>> Put(BiereModele biereModele)
        {
            try
            {
                var biere = _mapper.Map<Biere>(biereModele);
                var biereEnBD = await _biereService.GetBiere(biere.Id);
                if (biereEnBD == null) { return BadRequest("Biere pas déjà sauvegardée"); }
                var nouvelleBiere = await _biereService.Update(biere, biereEnBD);
                return Ok(_mapper.Map<BiereModele>(nouvelleBiere));
            }
            catch (Exception ex)
            {
                //return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
                //Logging            
                return BadRequest(ex);
            }
        }

        //[AllowAnonymous]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var biere = await _biereService.GetBiere(id);
                if (biere == null) { return NotFound(); }
                var deleteSucces = await _biereService.Delete(biere);
                if (deleteSucces)
                {
                    return Ok("Deleted");
                }
                else
                {
                    return BadRequest("Erreur lors du delete");
                }
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
