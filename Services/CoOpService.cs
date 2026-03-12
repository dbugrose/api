using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using api.Models;
using api.Services.Context;

namespace api.Services
{
    public class CoOpService
    {
        private readonly DataContext _context;

        public CoOpService(DataContext context)
        {
            _context = context;
        }

        public async Task<CoopModel> SendRequest(string senderId, string receiverId)
        {
            var request = new CoopModel
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Status = "Pending",
            };

            _context.CoopInfo.Add(request);
            await _context.SaveChangesAsync();

            return request;
        }

        public async Task<CoopModel?> GetRequest(int id)
        {
            return await _context.CoopInfo
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<List<CoopModel>> GetIncomingRequests(string senderId)
        {
            return await _context.CoopInfo
                .Where(r => r.ReceiverId == senderId && r.Status == "Pending")
                .ToListAsync();


        }
        public async Task<List<CoopModel>> GetSentRequests(string senderId)
        {
            return await _context.CoopInfo
                .Where(r => r.SenderId == senderId && r.Status == "Pending")
                .ToListAsync();
        }

        public async Task<bool> AcceptRequest(int receiverId)
        {
            var request = await _context.CoopInfo.FindAsync(receiverId);

            if (request == null)
                return false;

            request.Status = "Accepted";

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeclineRequest(int receiverId)
        {
            var request = await _context.CoopInfo.FindAsync(receiverId);

            if (request == null)
                return false;

            request.Status = "Declined";

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CancelRequest(int receiverId)
        {
            var request = await _context.CoopInfo.FindAsync(receiverId);

            if (request == null)
                return false;

            _context.CoopInfo.Remove(request);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<CoopModel>> GetFriends(string senderId)
        {
            return await _context.CoopInfo
                .Where(r =>
                    (r.SenderId == senderId || r.ReceiverId == senderId)
                    && r.Status == "Accepted")
                .ToListAsync();
        }
    }
}