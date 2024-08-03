using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BlogAPI.Entities.Models
{
	public class PostTag
	{
        [ForeignKey(nameof(Tag))]
        public int? TagId { get; set; }
        public Tag? Tag { get; set; }

        public long? PostId { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(PostId))]
        public Post? Post { get; set; }
    }
}

