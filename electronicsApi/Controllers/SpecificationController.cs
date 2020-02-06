using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ElectronicsAPI.Models;
using ElectronicsAPI.Commands;
using ElectronicsAPI.Services;
using ElectronicsAPI.Services.Interface;
using ElectronicsAPI.Commands.Interface;

namespace ElectronicsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecificationController : ControllerBase
    {
        private readonly ISpecificationService _spec;
        private readonly ISpecificationCommands _specsCommands;

        public SpecificationController(ISpecificationService spec)
        {
            this._spec = spec ?? throw new ArgumentNullException(nameof(spec));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get()
        {
            var queryCollection = this.Request.Query;

            if(queryCollection.Count() == 0)
            {
                var deviceinfos = _spec.GetAll() as IEnumerable<object>;
                if (deviceinfos == null || deviceinfos.Count() <= 0)
                    return NotFound();

                return Ok(deviceinfos);
            } else if(queryCollection.Count() == 1)
            {
                return GetQueries(queryCollection);
            } else
            {
                return NotFound();
            }
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetQueries(IQueryCollection queries)
        {

            object deviceinfo = null;

            switch (queries.First().Key.ToLower())
            {
                case "deviceId" :
                    {
                        if (queries["Id"] == "")
                            return NotFound();

                        if (!_spec.GetById(queries["Id"], out deviceinfo))
                            return NotFound();
                    }
                    break;
                default:
                    return NotFound();
            }

            return Ok(deviceinfo);
        }

        [HttpGet("{deviceId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IEnumerable<object> GetAllInformation(int id) {
            return new string[] { "this", "is", "hard", "coded" };
        }

        [HttpGet("information")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetAllInformation()
        {
            var queryCollection = this.Request.Query;

            if (queryCollection.Count() == 0)
            {
                var deviceinfos = _spec.GetAllInformation() as IEnumerable<object>;
                if (deviceinfos == null || deviceinfos.Count() <= 0)
                    return NotFound();

                return Ok(deviceinfos);
            }
            else if (queryCollection.Count() == 1)
            {
                return GetQueries(queryCollection);
            }
            else
            {
                return NotFound();
            }

        }

       [HttpPost]
       [ProducesResponseType(StatusCodes.Status200OK)]
       [ProducesResponseType(StatusCodes.Status404NotFound)]
       public void Post()
        {
            Specification specs = new Specification();
            DeviceDetails deviceDetails = new DeviceDetails();

            _specsCommands.Post(deviceDetails, specs);

            //return new string[] { "lsnce", "jared" };
        }

        [HttpPost("Test")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public void PostSample([FromBody]DeviceDetails dev)
        {
            _specsCommands.PostTest(dev);

        }


        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public void Delete()
        {
            
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public void Put()
        {

        }




    }
}
