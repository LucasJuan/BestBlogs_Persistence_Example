using System;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    public record Comment
    {
        public Guid Id { get; set; }

        public Guid PostId { get; set; }

        [MaxLength(120, ErrorMessage = "Maximum {1} content characters exceded.")]
        public string Content { get; set; }

        [MaxLength(30, ErrorMessage = "Maximum {1} author characters exceded.")]
        public string Author { get; set; }

        public DateTime CreationDate { get; set; }
    }
}