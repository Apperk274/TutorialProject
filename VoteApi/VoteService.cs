﻿using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using VoteApi;

namespace VotesApi.Services
{
    public class VoteService
    {
        private readonly IMongoCollection<Vote> _votes;

        public VoteService(VoteDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _votes = database.GetCollection<Vote>(settings.VoteCollectionName);
        }


        public (long upvotes, long downvotes) GetUpvotesAndDownvotesForThread(string threadId)
        {
            var pipeline = new BsonDocument[]
            {
                // Match documents with the specified threadId
                new BsonDocument("$match", new BsonDocument("ThreadId", threadId)),

                // Group the documents by ThreadId and calculate the count of upvotes and downvotes
                new BsonDocument("$group", new BsonDocument
                {
                    { "_id", "$ThreadId" },
                    { "upvotes", new BsonDocument("$sum", new BsonDocument("$cond", new BsonArray
                        {
                            "$IsUp",
                            1,
                            0
                        }))
                    },
                    { "downvotes", new BsonDocument("$sum", new BsonDocument("$cond", new BsonArray
                        {
                            "$IsUp",
                            0,
                            1
                        }))
                    }
                })
              };

            var result = _votes.Aggregate<BsonDocument>(pipeline).FirstOrDefault();

            if (result != null && result.Contains("upvotes") && result.Contains("downvotes"))
            {
                var upvotes = result["upvotes"].AsInt64;
                var downvotes = result["downvotes"].AsInt64;
                return (upvotes, downvotes);
            }

            return (0, 0);
        }

        public Vote GetByUserIdAndThreadId(string threadId, string userId) =>
            _votes.Find<Vote>(Vote => Vote.ThreadId == threadId && Vote.UserId == userId).FirstOrDefault();

        public Vote Create(Vote Vote)
        {
            _votes.InsertOne(Vote);
            return Vote;
        }
        public void UpdateUserIdAndThreadId(string threadId, string userId, Vote VoteIn) =>
            _votes.ReplaceOne(Vote => Vote.ThreadId == threadId && Vote.UserId == userId, VoteIn);

        public void RemoveUserIdAndThreadId(string threadId, string userId) =>
            _votes.DeleteOne(Vote => Vote.ThreadId == threadId && Vote.UserId == userId);
    }
}