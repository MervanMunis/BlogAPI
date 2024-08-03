using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BlogAPI.Entities.Models
{
	public class PostSubCategory
	{
        public short? SubCategoriesId { get; set; }

        [ForeignKey(nameof(SubCategoriesId))]
        public SubCategory? SubCategory { get; set; }

        public long? PostId { get; set; }

        [ForeignKey(nameof(PostId))]
        public Post? Post { get; set; }
    }
}

