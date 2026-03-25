using Microsoft.AspNetCore.Mvc;
using api.Models;
using api.Services;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoOpController : ControllerBase
    {
        private readonly CoOpService _coOpService;

        public CoOpController(CoOpService coOpService)
        {
            _coOpService = coOpService;
        }

        [HttpPost("Send")]
        public async Task<ActionResult<CoopModel>> SendRequest(int userId, int teamMemberId)
        {
            var request = await _coOpService.SendRequest(userId, teamMemberId);
            return Ok(request);
        }

        [HttpGet("GetRequest/{id}")]
        public async Task<ActionResult<CoopModel>> GetRequest(int id)
        {
            var request = await _coOpService.GetRequest(id);
            if (request == null) return NotFound();
            return Ok(request);
        }

        [HttpGet("IncomingRequests/{userId}")]
        public async Task<ActionResult<List<CoopModel>>> GetIncomingRequests(int userId)
        {
            var requests = await _coOpService.GetIncomingRequests(userId);
            return Ok(requests);
        }

        [HttpGet("Sent/{userId}")]
        public async Task<ActionResult<List<CoopModel>>> GetSentRequests(int userId)
        {
            var requests = await _coOpService.GetSentRequests(userId);
            return Ok(requests);
        }

        [HttpPut("Accept/{requestId}")]
        public async Task<ActionResult> AcceptRequest(int requestId)
        {
            var success = await _coOpService.AcceptRequest(requestId);
            if (!success) return NotFound();
            return Ok(new { message = "Request accepted" });
        }

        [HttpPut("Decline/{requestId}")]
        public async Task<ActionResult> DeclineRequest(int requestId)
        {
            var success = await _coOpService.DeclineRequest(requestId);
            if (!success) return NotFound();
            return Ok(new { message = "Request declined" });
        }

        [HttpDelete("Cancel/{requestId}")]
        public async Task<ActionResult> CancelRequest(int requestId)
        {
            var success = await _coOpService.CancelRequest(requestId);
            if (!success) return NotFound();
            return Ok(new { message = "Request canceled" });
        }

    }
}