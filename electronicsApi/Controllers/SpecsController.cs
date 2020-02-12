using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ElectronicsAPI.Models;
using ElectronicsAPI.Services.Interface;
using ElectronicsAPI.Commands.Interface;


namespace ElectronicsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecsController : ControllerBase
    {

        private readonly ISpecificationService _service;
        private readonly ISpecsCommands _commands;
        public SpecsController(ISpecificationService service, ISpecsCommands commands)
        {
            this._service = service ?? throw new ArgumentNullException(nameof(service));
            this._commands = commands;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get()
        {
            var queryCollection = this.Request.Query;

            if(queryCollection.Count() == 0)
            {
                var specsInfos = _service.GetAll() as IEnumerable<object>;

                if (specsInfos == null || specsInfos.Count() <= 0)
                    return NotFound();

                return Ok(specsInfos);
                     
            } else if(queryCollection.Count() == 1)
            {
                return GetQueries(queryCollection);
            }
            else
            {
                return NotFound();
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetQueries(IQueryCollection queries)
        {
            object specsInfo = null;

            switch (queries.First().Key.ToLower())
            {
                case "ram":
                    {
                        if (!int.TryParse(queries["deviceId"], out int id))
                            return NotFound();
                        if (id <= 0)
                            return NotFound();
                        if (!_service.GetByDeviceId(id, out specsInfo))
                            return NotFound();
                    }
                    break;
                default:
                    return NotFound();
            }
            return Ok(specsInfo);
        }


        [HttpGet("{deviceId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Get(int deviceId)
        {
            if (deviceId <= 0)
                return NotFound();
            if (!_service.GetByDeviceId(deviceId, out var deviceInfos))
                return NotFound();

            return Ok(deviceInfos);

            //_service.GetTest(deviceId);

            //return deviceId;

        }

        [HttpPost]
        public void Post([FromBody]Specification specs)
        {
            _commands.Post(specs);
        }


        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult PutLatestVersion(int deviceId, [FromBody]Specification specs)
        {
            var saveCount = 0;

            try
            {
                saveCount = _commands.UpdateLatest(deviceId, specs);
                if (saveCount < 1)
                    return BadRequest();
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
            catch (ArgumentNullException)
            {
                return Ok();
            }

            return Ok();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var spec = _service.gettingById(id);
            if(spec == null)
            {
                return NotFound();
            }
            _commands.DeletingById(spec.Id);

            return NoContent();
        }
    }
}
