using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using GP_Report_Request.Models;
using System.Web.Helpers;

namespace GP_Report_Request.Controllers
{
    public class AccidentController : ApiController
    {
        private TrafficEntities db = new TrafficEntities();
        
        // GET api/Accident
        public IEnumerable<area_range> Getarea_range()
        {
            
            
            //TODO:return the real_accident table rows to the emergency_backend


            var area_range = db.area_range.Include(a => a.city).Include(a => a.country);
       
            return  area_range.AsEnumerable();

        
        }

        // GET api/Accident/5
        public area_range Getarea_range(int id)
        {
            area_range area_range = db.area_range.Find(id);
            if (area_range == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return area_range;
        }

        // PUT api/Accident/5
        public HttpResponseMessage Putarea_range(int id, area_range area_range)
        {
            if (ModelState.IsValid && id == area_range.area_id)
            {
                db.Entry(area_range).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {   
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }


        // POST api/Accident
        //The report details coming from driver
        public HttpResponseMessage Postarea_range(accident_requests accident_requests)
        {
          




         

            if (ModelState.IsValid)
            {
                //TODO:check if area_id is repeated for 10 times , if true add the accident location and time in real_accident table , if false keep checking 


                // accident_requests.time = Convert.ToDateTime("2017-05-25 09:16:00.000");

                //accident_requests groupBy Area_id then count if Area_id >=9

                string AR_ID = "", status = "";

                //Accident classification by it's area_id column , then from the new generated table we get (id,count,address)
                var real_accident = db.accident_requests.GroupBy(a => a.area_id).
                 Select(g => new { Area_ID = g.Key, count = g.Count(), address = g.Select(a => a.address) }).ToList();

                //select all accidents area_id from the real_accident table and store it in a var variable
                var verified_accident2 = db.real_accident.Select(a=>a.area_id).ToList();

                //Verify_Accident
                #region Real_Accident
                foreach (var item in real_accident)
                {
                    Models.real_accident verified_accident = new real_accident() { area_id = item.Area_ID, happened_time = Convert.ToDateTime("2017-05-25 09:16:00.000") };

                    AR_ID += "</br>" + "Area (" + item.Area_ID + ") Count is :" + item.count;

                    //Max repetions for specific Area_Id to be added to real_accident table
                    if (item.count >= 3)
                    {
                        status += "count is reached to max and Area_ID is (" + item.Area_ID + ") </br>";
                        //insert Area_Id to real_accident table  instead 


                        if (!verified_accident2.Contains(verified_accident.area_id))
                        {
                            db.real_accident.Add(verified_accident);
                            db.SaveChanges();




                            // delete the accident_requests from accident_request table to be not added again in the real_accidents
                        }

                    }
                    else
                    {
                        //else logic
                        status += "count is less than predicted for Area(" + item.Area_ID + ") </br>";

                    }

                }  
                #endregion
                

                //Monitoring The Road

                #region Area_Range && Accident_Requests
                IList<area_range> AR_List = db.area_range.ToList();
                
                if (AR_List.Count>0)
                    {
                foreach (var item in AR_List)
                {

                    //if coming location request in the range 
                   
                        if (db.area_range.Any(a => a.max_latitude >= accident_requests.latitude && a.min_latitude <= accident_requests.latitude && a.max_longitude >= accident_requests.longitude && a.min_longitude <= accident_requests.longitude))
                    {
                        //  longitude <= maxlong && longitude >= minlong && latitude <= maxlat && latitude >= minlat
                        accident_requests.area_id = db.area_range.Where(a => a.max_latitude >= accident_requests.latitude &&
                                                     a.min_latitude <= accident_requests.latitude
                                                     && a.max_longitude >= accident_requests.longitude &&
                                                     a.min_longitude <= accident_requests.longitude).Select(a => a.area_id).First();

                        db.accident_requests.Add(accident_requests);
                        db.SaveChanges();
                        //save the new location in the accident_requests table
                        return Request.CreateErrorResponse(HttpStatusCode.OK, "Location In Range And Added to Accident_request Table Successfully!</br>" + status);



                    }
                    
                    

                        //if coming request doesn't in range
                    //Apply The Formula then add the new range
                    //then add this request to the accident requests table
                    else
                    {


                        decimal Longitude_Positive_Range = accident_requests.longitude + (decimal)0.1;
                        decimal Longitude_Negative_Range = accident_requests.longitude - (decimal)0.1;


                        decimal Latitude_Positive_Range = accident_requests.latitude + (decimal)0.1;
                        decimal Latitude_Negative_Range = accident_requests.latitude - (decimal)0.1;



                        Models.area_range AR = new Models.area_range()
                        {

                            max_longitude = Longitude_Positive_Range,
                            min_longitude = Longitude_Negative_Range
                            ,
                            max_latitude = Latitude_Positive_Range,
                            min_latitude = Latitude_Negative_Range,
                            country_id = 1,
                            city_id = 1,

                            time = accident_requests.time

                            //lat , long , country , cit ,time


                        };

                        db.area_range.Add(AR);
                        db.SaveChanges();


                        accident_requests.area_id = db.area_range.Where(a => a.max_longitude == Longitude_Positive_Range && a.min_longitude == Longitude_Negative_Range).Select(a => a.area_id).First();

                        db.accident_requests.Add(accident_requests);
                        db.SaveChanges();
                        return Request.CreateErrorResponse(HttpStatusCode.OK, "New Range Added !");


                    }

                } // foreach end

                   


                    }//inner if end

                    //Excutes just one time when area_range table doesn't have rows
                else
                {

                    decimal Longitude_Positive_Range = accident_requests.longitude + (decimal)0.1;
                    decimal Longitude_Negative_Range = accident_requests.longitude - (decimal)0.1;


                    decimal Latitude_Positive_Range = accident_requests.latitude + (decimal)0.1;
                    decimal Latitude_Negative_Range = accident_requests.latitude - (decimal)0.1;



                    Models.area_range AR = new Models.area_range()
                    {

                        max_longitude = Longitude_Positive_Range,
                        min_longitude = Longitude_Negative_Range
                        ,
                        max_latitude = Latitude_Positive_Range,
                        min_latitude = Latitude_Negative_Range,
                        country_id = 1,
                        city_id = 1,

                        time = accident_requests.time

                        //lat , long , country , cit ,time


                    };

                    db.area_range.Add(AR);
                    db.SaveChanges();


                    accident_requests.area_id = db.area_range.Where(a => a.max_longitude == Longitude_Positive_Range && a.min_longitude == Longitude_Negative_Range).Select(a => a.area_id).First();

                    db.accident_requests.Add(accident_requests);
                    db.SaveChanges();
                    return Request.CreateErrorResponse(HttpStatusCode.OK, "New Range Added !");

                }


                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, accident_requests);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = accident_requests.area_id })); 
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            } 
                #endregion
        }

        // DELETE api/Accident/5
        public HttpResponseMessage Deletearea_range(int id)
        {
            area_range area_range = db.area_range.Find(id);
            if (area_range == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.area_range.Remove(area_range);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, area_range);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}