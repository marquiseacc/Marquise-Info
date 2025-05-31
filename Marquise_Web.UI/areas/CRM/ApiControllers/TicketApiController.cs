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
        [Authorize] // اطمینان از نیاز به احراز هویت JWT
        public async Task<IHttpActionResult> NewTicket(NewTicketVM newTicket)
        {
            var claimsPrincipal = User as ClaimsPrincipal;

            if (claimsPrincipal == null || !claimsPrincipal.HasClaim(c => c.Type == "OtpVerified" && c.Value == "True"))
            {
                return Json(new OperationResult<object>
                {
                    IsSuccess = false,
                    Message = "دسترسی غیرمجاز",
                    Data = null
                });
            }

            var crmId = ((ClaimsIdentity)User.Identity).FindFirst("CrmAccountId")?.Value;

            if (string.IsNullOrEmpty(crmId))
            {
                return Json(new OperationResult<object>
                {
                    IsSuccess = false,
                    Message = "شناسه حساب CRM یافت نشد.",
                    Data = null
                });
            }

            var dto = UIDataMapper.Mapper.Map<NewTicketDto>(newTicket);
            dto.CrmId = crmId;

            var result = await unitOfWork.TicketService.CreateTicketAsync(dto);

            return Json(new OperationResult<object>
            {
                IsSuccess = result.IsSuccess,
                Message = result.Message
            });
        }



        [HttpPost]
        [Authorize]
        [System.Web.Http.Route("api/CRM/TicketApi/NewAnswer")]
        public async Task<IHttpActionResult> NewAnswer(AnswerVM answer)
        {
            var claimsPrincipal = User as ClaimsPrincipal;

            if (claimsPrincipal == null || !claimsPrincipal.HasClaim(c => c.Type == "OtpVerified" && c.Value == "True"))
            {
                return Json(new
                {
                    success = false,
                    message = "دسترسی غیرمجاز"
                });
            }

            var dto = UIDataMapper.Mapper.Map<NewAnswerDto>(answer);
            var result = await unitOfWork.TicketService.AddAnswerAsync(dto);

            return Json(new
            {
                success = result.IsSuccess,
                message = result.Message
            });
        }



        [HttpPost]
        [Authorize]
        [System.Web.Http.Route("api/CRM/TicketApi/CloseTicket")]
        public async Task<IHttpActionResult> CloseTicket(CloseTicket ticket)
        {
            var claimsPrincipal = User as ClaimsPrincipal;

            if (claimsPrincipal == null || !claimsPrincipal.HasClaim(c => c.Type == "OtpVerified" && c.Value == "True"))
            {
                return Json(new
                {
                    success = false,
                    message = "دسترسی غیرمجاز"
                });
            }

            var dto = UIDataMapper.Mapper.Map<CloseTicketDto>(ticket);
            var result = await unitOfWork.TicketService.CloseTicketAsync(dto);

            return Json(new
            {
                success = result.IsSuccess,
                message = result.Message,
                redirectUrl = Url.Link("CRM_Area", new
                {
                    controller = "Ticket",
                    action = "Index",
                    area = "CRM"
                })
            });
        }

    }
}
