using Marquise_Web.Service.IService;
using Marquise_Web.UI.areas.CRM.Models;
using Marquise_Web.Model.DTOs.CRM;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using Utilities.Map;
using Marquise_Web.Model.Utilities;

namespace Marquise_Web.UI.areas.CRM.ApiControllers
{
    public class TicketApiController : ApiController
    {
        private readonly IUnitOfWorkService unitOfWork;

        public TicketApiController(IUnitOfWorkService unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpPost]
        [System.Web.Http.Route("api/CRM/TicketApi/NewTicket")]
        public async Task<IHttpActionResult> NewTicket(NewTicketVM newTicket)
        {
            var claimsPrincipal = User as ClaimsPrincipal;

            if (claimsPrincipal == null || !claimsPrincipal.HasClaim(c => c.Type == "OtpVerified" && c.Value == "True"))
            {
                return Json(new { success = false });
            }

            var crmId = ((ClaimsIdentity)User.Identity).FindFirst("CRMId")?.Value;

            var dto = UIDataMapper.Mapper.Map<NewTicketDto>(newTicket);
            dto.CrmId = crmId;

            var result = await unitOfWork.TicketService.CreateTicketAsync(dto);
            if (result.IsSuccess)
            {
                return Json(new OperationResult<object>
                {
                    IsSuccess = true,
                    Message = result.Message,
                    Data = new
                    {
                        redirectUrl = Url.Link("DefaultApi", new { controller = "Ticket", action = "Index", area = "CRM" })
                    }
                });
            }
            else
            {
                return Json(new OperationResult<object>
                {
                    IsSuccess = false,
                    Message = result.Message,
                    Data = new
                    {
                        redirectUrl = Url.Link("DefaultApi", new { controller = "Ticket", action = "Index", area = "CRM" })
                    }
                });
            }
        }


        [HttpPost]
        [System.Web.Http.Route("api/CRM/TicketApi/NewAnswer")]
        public async Task<IHttpActionResult> NewAnswer(AnswerVM answer)
        {
            var claimsPrincipal = User as ClaimsPrincipal;

            if (claimsPrincipal == null || !claimsPrincipal.HasClaim(c => c.Type == "OtpVerified" && c.Value == "True"))
            {
                return Json(new { success = false });
            }

            var dto = UIDataMapper.Mapper.Map<NewAnswerDto>(answer);
            var result = await unitOfWork.TicketService.AddAnswerAsync(dto);

            if (result.IsSuccess)
            {
                return Json(new OperationResult<object>
                {
                    IsSuccess = true,
                    Message = result.Message,
                    Data = new
                    {
                        redirectUrl = Url.Link("DefaultApi", new { controller = "Ticket", action = "Index", area = "CRM", ticketId= answer.TicketId })
                    }
                });
            }
            else
            {
                return Json(new OperationResult<object>
                {
                    IsSuccess = false,
                    Message = result.Message,
                    Data = new
                    {
                        redirectUrl = Url.Link("DefaultApi", new { controller = "Ticket", action = "Index", area = "CRM" })
                    }
                });
            }
        }


        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/CRM/TicketApi/CloseTicket")]
        public async Task<IHttpActionResult> CloseTicket(CloseTicket ticket)
        {
            var claimsPrincipal = User as ClaimsPrincipal;

            if (claimsPrincipal == null || !claimsPrincipal.HasClaim(c => c.Type == "OtpVerified" && c.Value == "True"))
            {
                return Json(new { success = false });
            }

            var dto = UIDataMapper.Mapper.Map<CloseTicketDto>(ticket);
            var result = await unitOfWork.TicketService.CloseTicketAsync(dto);
            if (result.IsSuccess)
            {
                return Json(new OperationResult<object>
                {
                    IsSuccess = true,
                    Message = result.Message,
                    Data = new
                    {
                        redirectUrl = Url.Link("DefaultApi", new { controller = "Ticket", action = "Index", area = "CRM" })
                    }
                });
            }
            else
            {
                return Json(new OperationResult<object>
                {
                    IsSuccess = false,
                    Message = result.Message,
                    Data = new
                    {
                        redirectUrl = Url.Link("DefaultApi", new { controller = "Ticket", action = "Index", area = "CRM" })
                    }
                });
            }

        }
    }
}
