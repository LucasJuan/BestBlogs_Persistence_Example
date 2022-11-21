using System;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    public record Post
    {
        public Guid Id { get; set; }

        [MaxLength(30, ErrorMessage = "Maximum {1} title characters exceded.")]
        public string Title { get; set; }

        [MaxLength(1200, ErrorMessage = "Maximum {1} content characters exceded.")]
        public string Content { get; set; }

        public DateTime CreationDate { get; set; }
    }
}