using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GP_Report_Request.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2","Welcome" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values


        [HttpPost]

        //public string Get_driver_report(double longitude, double latitude, string report_type)
        //{

        //    //range of 10 km

        //    //retrieve the database records then check

        //    if (longitude <= maxlong && longitude >= minlong && latitude <= maxlat && latitude >= minlat)
        //    {

        //        //insert location details to database table2  


        //    }
        //    else
        //    {
        //        double Longitude_Positive_Range = longitude + 0.1;
        //        double Longitude_Negative_Range = longitude - 0.1;


        //        double Latitude_Positive_Range = latitude + 0.1;
        //        double Latitude_Negative_Range = latitude - 0.1;

        //        //insert table1 maxlong=long_positive
        //        //insert table1 minlong=long-negative

        //        //insert table1 maxlat=lat_positive
        //        //insert table1 minlat=lat_negtive


        //        //insert table2 longitude
        //        //insert table2 latitude
        //        //insert table2 address


        //    }























        //    return "Type is : " + report_type;






        //}


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