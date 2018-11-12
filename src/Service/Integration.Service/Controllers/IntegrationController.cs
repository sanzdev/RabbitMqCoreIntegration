using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Integration.Service.Broker;
using Integration.Service.Common;
using Integration.Service.Model;
using Microsoft.AspNetCore.Mvc;

namespace Integration.Service.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class IntegrationController : ControllerBase
    {
        // POST api/v1/integration
        /// <summary>
        ///     Send a message to the integration service.
        /// </summary>
        [HttpPost(Name = "SendMessage")]
        [Consumes("application/xml", "application/json", "text/xml")]
        public IActionResult Post()
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                Request.Body.CopyTo(ms);
                ms.Position = 0;
                byte[] buffer = new byte[ms.Length];
                ms.Read(buffer, 0, buffer.Length);
                var broker = new IntegrationBroker();
                broker.SendMessage("IntegrationR", Encoding.UTF8.GetString(buffer));
            }
            catch (Exception)
            {
                return StatusCode(400);
            }
            return Ok();
        }      
    }
}