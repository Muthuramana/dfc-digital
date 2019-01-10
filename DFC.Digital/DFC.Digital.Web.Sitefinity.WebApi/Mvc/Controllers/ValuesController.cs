﻿using System;
using System.Collections.Generic;
using System.Web.Http;
using Telerik.Sitefinity.Services;

namespace DFC.Digital.Web.Sitefinity.WebApi.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            if (!SystemManager.CurrentHttpContext.Request.IsAuthenticated)
            {
                throw new UnauthorizedAccessException();
            }

            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}