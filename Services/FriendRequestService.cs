using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using api.Models;
using api.Services.Context;

namespace api.Services
{
    public class FriendRequestsService
    {
        private readonly DataContext _context;

        public FriendRequestsService(DataContext context)
        {
            _context = context;
        }

        public async Task<FriendRequestModel> SendRequest(string senderId, string receiverId)
        {
            var request = new FriendRequestModel
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };

            _context.FriendRequestInfo.Add(request);
            await _context.SaveChangesAsync();

            return request;
        }

        public async Task<FriendRequestModel?> GetRequest(int id)
        {
            return await _context.FriendRequestInfo
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<List<FriendRequestModel>> GetIncomingRequests(string userId)
        {
            return await _context.FriendRequestInfo
                .Where(r => r.ReceiverId == userId && r.Status == "Pending")
                .ToListAsync();
        }

        public async Task<List<FriendRequestModel>> GetSentRequests(string userId)
        {
            return await _context.FriendRequestInfo
                .Where(r => r.SenderId == userId && r.Status == "Pending")
                .ToListAsync();
        }

        public async Task<bool> AcceptRequest(int requestId)
        {
            var request = await _context.FriendRequestInfo.FindAsync(requestId);

            if (request == null)
                return false;

            request.Status = "Accepted";

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeclineRequest(int requestId)
        {
            var request = await _context.FriendRequestInfo.FindAsync(requestId);

            if (request == null)
                return false;

            request.Status = "Declined";

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CancelRequest(int requestId)
        {
            var request = await _context.FriendRequestInfo.FindAsync(requestId);

            if (request == null)
                return false;

            _context.FriendRequestInfo.Remove(request);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<FriendRequestModel>> GetFriends(string userId)
        {
            return await _context.FriendRequestInfo
                .Where(r =>
                    (r.SenderId == userId || r.ReceiverId == userId)
                    && r.Status == "Accepted")
                .ToListAsync();
        }
    }
}