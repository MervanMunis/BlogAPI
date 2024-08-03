using BlogAPI.Entities.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BlogAPI.DTOs.Response
{
    public class LikeResponse
    {
        public long PostId { get; set; }

        public string? AuthorId { get; set; }

        public int NumberOfLikes { get; set; }

        public string? AuthorUsername { get; set; }
    }
}
