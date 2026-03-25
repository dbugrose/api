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

        public async Task<FriendRequestModel> SendRequest(int senderId, int receiverId)
        {
            UserModel? sender = await GetUserInfoByUserIdAsync(senderId);
            UserModel? receiver = await GetUserInfoByUserIdAsync(receiverId);
            var request = new FriendRequestModel
            {
                SenderId = senderId,
                SenderUser = sender.Username,
                ReceiverUser = receiver.Username,
                ReceiverId = receiverId,
                Status = "Pending",
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

        public async Task<List<FriendRequestModel>> GetIncomingRequests(int userId)
        {
            return await _context.FriendRequestInfo
                .Where(r => r.ReceiverId == userId && r.Status == "Pending")
                .ToListAsync();
        }

        public async Task<List<FriendRequestModel>> GetSentRequests(int userId)
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

        public async Task<List<FriendRequestModel>> GetFriends(int userId)
        {
            return await _context.FriendRequestInfo
                .Where(r =>
                    (r.SenderId == userId || r.ReceiverId == userId)
                    && r.Status == "Accepted")
                .ToListAsync();
        }

        public async Task<UserModel?> GetUserInfoByUserIdAsync(int userId) => await _context.UserInfo.SingleOrDefaultAsync(user => user.Id == userId);

        public async Task<List<StatsModel>> GetFriendsStats(int userId)
        {
            var friends = await _context.FriendRequestInfo
                .Where(r =>
                    (r.SenderId == userId || r.ReceiverId == userId)
                    && r.Status == "Accepted")
                .ToListAsync();

            var friendIds = friends
                .Select(f => f.SenderId == userId ? f.ReceiverId : f.SenderId)
                .Distinct()
                .ToList();

            var friendStats = await _context.StatsInfo
                .Where(s => friendIds.Contains(s.Id))
                .ToListAsync();

            return friendStats;
        }

    }
}