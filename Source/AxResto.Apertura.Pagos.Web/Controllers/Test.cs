using log4net;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace AxResto.Apertura.Pagos.Web.Controllers
{
    [Route("api/[controller]")]
    //[ApiController]
    public class Test : ControllerBase
    {
        private readonly ILog _logger;

        #region constructor
        public Test(ILog log)
        {
            _logger = log;
        }
        #endregion


        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            _logger.Debug("[START] GET");
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        //// POST api/values
        //[HttpPost]
        //[ActionName("Post")]
        //public string Post([FromBody] string value)
        //{
        //    return $"Mi {value}";
        //}

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}

        // POST api/values
        [HttpPost("Post3")]
        public string Post3([FromBody]IDictionary<string, string> value)
        {
            return $"cantidad de items {value.Count}";
        }

        //[HttpPost]
        //[ActionName("Post4")]
        //public string Post4([FromBody]string value)
        //{
        //    return $"cantidad de items {value}";
        //}

        //[HttpPost("Pagos2")]
        ////[ActionName("Pagos2")]
        //public string Pagos()
        //{
        //    var value = "a";
        //    return $"cantidad de pagos {value}";
        //}
    }
}
