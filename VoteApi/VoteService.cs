using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace VoteApi
{
    public class VoteService
    {
        private readonly IMongoCollection<Vote> _votes;
        private readonly BsonDocument _groupByThreadIdAndCountVotes = new("$group", new BsonDocument
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
                });
        public VoteService(VoteDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _votes = database.GetCollection<Vote>(settings.VoteCollectionName);
        }

        public Dictionary<string, (int upvotes, int downvotes)> GetUpvotesAndDownvotesForThreads(List<string> threadIds)
        {

            var pipeline = new List<BsonDocument>
            {
                // Match documents with the specified threadIds
                new BsonDocument("$match", new BsonDocument("ThreadId", new BsonDocument("$in", new BsonArray(threadIds)))),
                _groupByThreadIdAndCountVotes

            };

            var cursor = _votes.Aggregate<BsonDocument>(pipeline).ToList();

            var resultDict = new Dictionary<string, (int upvotes, int downvotes)>();
            foreach (var document in cursor)
            {
                var threadId = document["_id"].AsString;
                var upvotes = document["upvotes"].AsInt32;
                var downvotes = document["downvotes"].AsInt32;
                resultDict[threadId] = (upvotes, downvotes);
            }

            return resultDict;
        }

        public (int UpVotes, int DownVotes) GetUpvotesAndDownvotesForThread(int threadId)
        {
            var pipeline = new BsonDocument[]
            {
                // Match documents with the specified threadId
                new BsonDocument("$match", new BsonDocument("ThreadId", threadId)),
                _groupByThreadIdAndCountVotes
            };

            var result = _votes.Aggregate<BsonDocument>(pipeline).FirstOrDefault();

            if (result != null && result.Contains("upvotes") && result.Contains("downvotes"))
            {
                var upvotes = result["upvotes"].AsInt32;
                var downvotes = result["downvotes"].AsInt32;
                return (upvotes, downvotes);
            }

            return (0, 0);
        }

        public Vote GetByThreadIdAndUserId(int threadId, string userId) =>
            _votes.Find<Vote>(Vote => Vote.ThreadId == threadId && Vote.UserId == userId).FirstOrDefault();

        public Vote Create(Vote Vote)
        {
            _votes.InsertOne(Vote);
            return Vote;
        }
        public void UpdateByThreadIdAndUserId(int threadId, string userId, bool isUp)
        {
            var update = Builders<Vote>.Update.Set(x => x.IsUp, isUp);
            _votes.UpdateOne(Vote => Vote.ThreadId == threadId && Vote.UserId == userId, update);
        }

        public void RemoveByThreadIdAndUserId(int threadId, string userId) =>
            _votes.DeleteOne(Vote => Vote.ThreadId == threadId && Vote.UserId == userId);
    }
}