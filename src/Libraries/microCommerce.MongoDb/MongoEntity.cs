﻿using microCommerce.Domain;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Runtime.Serialization;

namespace microCommerce.MongoDb
{
    //[DataContract]
    //[Serializable]
    //[BsonIgnoreExtraElements(Inherited = false)]
    public abstract class MongoEntity : BaseEntityTypeId<ObjectId>
    {
        //[DataMember]
        //[BsonRepresentation(BsonType.ObjectId)]
        public override ObjectId Id { get; set; }
    }
}