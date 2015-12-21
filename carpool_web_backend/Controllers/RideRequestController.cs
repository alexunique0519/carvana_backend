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
    [RoutePrefix("api/rideRequest")]
    public class RideRequestController : BaseApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize]
        [Route("create")]
        public HttpResponseMessage CreateRideRequest(RideRequestModel rideRequest)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "The ride request information is not correct.");
            }
            
            db.RideRequests.Add(rideRequest);
            int nRes = db.SaveChanges();

            if(nRes == 1)
            {
                return Request.CreateResponse(HttpStatusCode.OK, "The ride request have been created successfully");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "The ride request not created");
            }
        }

        

    }
}
