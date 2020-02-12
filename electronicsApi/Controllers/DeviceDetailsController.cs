using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ElectronicsAPI.Commands;
using ElectronicsAPI.Services;
using ElectronicsAPI.Models;
using ElectronicsAPI.Services.Interface;
using ElectronicsAPI.Commands.Interface;

namespace ElectronicsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceDetailsController : ControllerBase
    {
        private readonly IDeviceDetailsService _deviceService;
        private readonly IDeviceDetailsCommands _detailsCommands;

        public DeviceDetailsController(IDeviceDetailsCommands detailsCommands, IDeviceDetailsService deviceDetailsService)
        {
            this._deviceService = deviceDetailsService ?? throw new ArgumentNullException(nameof(deviceDetailsService));
            this._detailsCommands = detailsCommands ?? throw new ArgumentNullException(nameof(deviceDetailsService));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get()
        {
            var queryCollectin = this.Request.Query;

            if (queryCollectin.Count() == 0)
            {
                var deviceinfos = _deviceService.GetAll() as IEnumerable<object>;

                if (deviceinfos == null || deviceinfos.Count() <= 0)
                    return NotFound();

                return Ok(deviceinfos);
            } else if (queryCollectin.Count() == 1)
            {
                return GetQueries(queryCollectin);
            } else
            {
                return NotFound();
            }
        }

        [HttpGet("information")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetInformation()
        {
            var queryColection = this.Request.Query;
            if(queryColection.Count() == 0)
            {
                var deviceinfos = _deviceService.GetAllInformation() as IEnumerable<object>;

                if (deviceinfos == null || deviceinfos.Count() <= 0)
                    return NotFound();

                return Ok(deviceinfos);
            } else if(queryColection.Count() == 1)
            {
                return GetQueries(queryColection);
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
            object deviceinfo = null;
            //var error = new ErrorNotFound();
            switch (queries.First().Key.ToLower())
            {
                case "brand":
                    {
                        if (!int.TryParse(queries["controlNumber"], out int id))
                            return NotFound();
                        if (id <= 0)
                            return NotFound();
                        if (!_deviceService.GetByControlNumber(id, out deviceinfo))
                            return NotFound();
                    }
                    break;
                default:
                    return NotFound();
            }

            return Ok(deviceinfo);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get(int id)
        {
            if (id <= 0)
                return NotFound();
            if (!_deviceService.GetByControlNumber(id, out var deviceinfos))
                return NotFound();
            return Ok(deviceinfos);
        }

        [HttpPost]
        public void Post([FromBody]DeviceDetails deviceDetails)
        {
            _detailsCommands.Post(deviceDetails);

            //return CreatedAtRoute("GetDevice", new { controlNumber = deviceDetails.controlNumber.ToString() }, deviceDetails);
        }

        [HttpPut("{controlNumber}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult PutLatestVersion(int controlNumber, [FromBody]DeviceDetails latestInfo)
        {
            var saveCount = 0;

            try
            {
                saveCount = _detailsCommands.PutLatestVersionAll(controlNumber, latestInfo);
                if (saveCount < 1)
                    return BadRequest();
            } catch(InvalidOperationException)
            {
                return NotFound();
            } catch(ArgumentNullException)
            {
                return Ok();
            }
            return Ok();
        }


        [HttpDelete("{Id}")]
        public IActionResult Delete(int id)
        {
            var device = _deviceService.getById(id);
            if(device == null)
            {
                return NotFound();
            }

            _detailsCommands.DeleteById(device.Id);

            return NoContent();
        }

    }
}
