﻿using Projekt.Models;
using Projekt.Data;
using Microsoft.EntityFrameworkCore;

namespace Projekt.Services
{
    public class PollService : IService
    {
        private readonly P4ProjektDbContext _context;
        private static readonly PollService _instance = new PollService();

        public PollService()
        {
            _context = AppService.DbContext;
        }

        public static PollService Instance
        {
            get { return _instance; }
        }

        public static bool IsValid()
        {
            return _instance != null;
        }

        public PollModel[] GetPolls()
        {
            return [.. _context.Polls
                .Include(p => p.Options)
                .Include(p => p.Author)];
        }

        public PollModel? GetPollById(int id)
        {
            return _context.Polls.Include(p => p.Options)
                               .Include(p => p.Author)
                               .FirstOrDefault(p => p.Id == id);
        }

        public async Task<PollModel> AddPoll(PollModel poll)
        {
            _context.Options.AddRange(poll.Options);
            _context.Polls.Add(poll);
            await _context.SaveChangesAsync();
            return poll;
        }

        public async Task UpdatePoll(PollModel poll)
        {
            _context.Polls.Update(poll);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePoll(int id)
        {
            var poll = await _context.Polls.FindAsync(id);
            if (poll != null)
            {
                _context.Polls.Remove(poll);
                await _context.SaveChangesAsync();
            }
        }

        public PollModel[] GetUserPolls(int userId)
        {
            return [.. _context.Polls.Include(p => p.Options)
                                   .Include(p => p.Author)
                                   .Where(p => p.AuthorId == userId)];
        }

        public PollModel[] GetPublicPolls()
        {
            return [.. _context.Polls.Include(p => p.Options)
                                   .Include(p => p.Author)
                                   .Where(p => p.Public)];
        }

        public CategoryModel[] GetCategories()
        {
            return _context.Categories.ToArray();
        }

        public async Task AddVote(VoteModel vote)
        {
            _context.Votes.Add(vote);
            await _context.SaveChangesAsync();
        }

        public async Task AddVotes(VoteModel[] votes)
        {
            _context.Votes.AddRange(votes);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveVote(VoteModel vote)
        {
            _context.Votes.Remove(vote);
            await _context.SaveChangesAsync();
        }

        public VoteModel[] GetPollVotes(int pollId)
        {
            return [.. _context.Votes.Include(v => v.Option)
                                   .Include(v => v.User)
                                   .Where(v => v.PollId == pollId)];
        }

        public CategoryModel[] getPollCategories(int pollId)
        {
            return [.. _context.Categories.Include(c => c.Polls)
                                         .Where(c => c.Polls.Any(p => p.Id == pollId))];
        }

        public bool HasUserVoted(int pollId, int userId)
        {
            return _context.Votes.Any(v => v.PollId == pollId && v.UserId == userId);
        }
    }
}
