﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace VoteApi
{
    public class Vote
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public int ThreadId { get; set; }
        public string UserId { get; set; }
        public bool IsUp { get; set; }
    }
}
