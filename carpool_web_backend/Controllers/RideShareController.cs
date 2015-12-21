using carpool_web_backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace carpool_web_backend.Controllers
{
    

    [RoutePrefix("api/rideShare")]
    public class RideShareController : BaseApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize]
        [Route("create")]
        public async Task<HttpResponseMessage> CreateRideShare( RideShareModel rideShare)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "The ride share info is not valid.");
            }
            try
            {
                DateTime dt = rideShare.DepartureTime - new TimeSpan(5, 0, 0);
                rideShare.DepartureTime = dt;

                db.RideShares.Add(rideShare);
                db.SaveChanges();
            }
            catch (Exception ex)
            { 
                string sEx = ex.InnerException.Message;
                return Request.CreateResponse(HttpStatusCode.InternalServerError, sEx);
            }
          

            return Request.CreateResponse(HttpStatusCode.OK, "The ride share info has been created");

        }

        [Authorize]
        [Route("search")]
        public HttpResponseMessage Search(RideRequestModel rideRequest)
        {
            if(!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "The ride request information is not correct.");
            }

            DateTime localStartTime = rideRequest.DepartureStartTime - new TimeSpan(5, 0, 0);
            DateTime localEndTime = rideRequest.DepartureEndTime - new TimeSpan(5, 0, 0);

            var rideShares = db.RideShares.Where(r => r.DepartureTime >= localStartTime &&
                                                      r.DepartureTime <= localEndTime &&
                                                   r.FromCity == rideRequest.FromCity && r.FromProvince == rideRequest.FromProvince &&
                                                   r.FromProvince == rideRequest.FromProvince && r.FromLocation.Equals(rideRequest.FromLocation, StringComparison.InvariantCultureIgnoreCase) &&
                                                   r.ToProvince == rideRequest.ToProvince && r.ToCity == rideRequest.ToCity && r.ToLocation.Equals(rideRequest.ToLocation, StringComparison.InvariantCultureIgnoreCase));


            return Request.CreateResponse(HttpStatusCode.OK, rideShares.ToList());
        } 

    }
}
