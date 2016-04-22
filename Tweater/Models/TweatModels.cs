using System;
using System.ComponentModel.DataAnnotations;

namespace Tweater.Models
{
    public class Tweat
    {
        public int Id { get; set; }
        public TweaterUser Author { get; set; }
        [StringLength(140, ErrorMessage = "The Tweat can be at most 140 characters long.")]
        public string Body { get; set; }
        public DateTime CreateDate { get; set; }

    }

    public class TweatVM
    {
        public int Id { get; set; }
        public string AuthorHandle { get; set; }
        public string Body { get; set; }
        public DateTime CreateDate { get; set; }

    }

    public class CreateTweatVM
    {
        [Required]
        [StringLength(140, ErrorMessage = "The Tweat cannot be more than 140 characters long.")]
        public string Body { get; set; }
    }
}