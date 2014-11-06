using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AccountCreateApi.Models;
using Messages;

namespace AccountCreateApi.Controllers
{
      [RoutePrefix("api/Account")]
    public class AccountCreateController : ApiController
    {
          [HttpPost]
          [Route("create")]
          public IHttpActionResult CreateAccount(RegisterBindingModel model)
          {
              if (!ModelState.IsValid)
              {
                  return BadRequest(ModelState);
              }

              var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };

              
              WebApiApplication.Bus.Send(new CreateAccount { Email = model.Email, Password = model.Password });



              return Ok();
          }
          [HttpPost]
          [Route("confirm")]

          public IHttpActionResult ConfirmAccount(ConfirmBindingModel model)
          {
              

              var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };

            
              WebApiApplication.Bus.Send(new ConfirmAccount { EmailAddress = model.Email, ConfirmationCode = model.ConfirmationCode, Nonce = model.Nonce });


              return Ok();
              
          }
    }
}
