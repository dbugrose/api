using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using api.Services;
using api.Services.Context;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;


namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FriendsRequestController : ControllerBase
    {
        private readonly FriendRequestsService _service;

        public FriendsRequestController(FriendRequestsService service)
        {
            _service = service;
        }
        [HttpPost("SendRequest")]
        public async Task<IActionResult> SendRequest(int senderId, int receiverId)
        {
            var sentRequest = await _service.SendRequest(senderId, receiverId);
            if (sentRequest != null) return Ok(sentRequest);

            return NotFound(new { Message = "No sent Request " });
        }

        [HttpGet("GetRequestById/{id}")]

        public async Task<IActionResult> GetRequest(int id)
        { 
            var requests =  await _service.GetRequest(id);
            if (requests != null) return Ok(requests);
            return NotFound(new { Message = "No sent Request " });

        }
        [HttpGet("GetIncomingRequests/{userId}")]
        public async Task<IActionResult> GetIncomingRequests(int userId)
        {
            var requests =  await _service.GetIncomingRequests(userId);
            if (requests != null) return Ok(requests);
            return NotFound(new { Message = "No sent Request " });

        }
        [HttpGet("GetSentRequests/{userId}")]

        public async Task<IActionResult> GetSentRequests(int userId)
        {
            var requests =  await _service.GetSentRequests(userId);
            if (requests != null) return Ok(requests);
            return NotFound(new { Message = "No sent Request " });

        }
        [HttpPut("AcceptRequest")]
        public async Task<IActionResult> AcceptRequest(int requestId)
        {
            var requests =  await _service.AcceptRequest(requestId);
            if (requests != false) return Ok(requests);
            return NotFound(new { Message = "No sent Request " });

        }
        [HttpPut("DeclineRequest")]
        public async Task<IActionResult> DeclineRequest(int requestId)
        {
            var requests =  await _service.DeclineRequest(requestId);
            if (requests != false) return Ok(requests);
            return NotFound(new { Message = "No sent Request " });

        }
        [HttpDelete("CancelRequest")]

        public async Task<IActionResult> CancelRequest(int requestId)
        {
            var requests =  await _service.CancelRequest(requestId);
            if (requests != false) return Ok(requests);
            return NotFound(new { Message = "No sent Request " });

        }
        [HttpGet("GetFriends/{userId}")]

        public async Task<IActionResult> GetFriends(int userId)
        {
            var friends =  await _service.GetFriends(userId);
            if (friends != null) return Ok(friends);
            return NotFound(new { Message = "No sent Request " });

        }

        [HttpGet("GetFriendStats/{userId}")]

        public async Task<IActionResult> GetFriendsStats(int userId)
        {
            var friends =  await _service.GetFriendsStats(userId);
            if (friends != null) return Ok(friends);
            return NotFound(new { Message = "No sent Request " });

        }
    }
}