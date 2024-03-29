﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsHub.Shared.Entities
{
    public class ArticleImage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "ArticleId is required.")]
        public Guid ArticleId { get; set; }

        public byte[] Bytes { get; set; }

        [Required(ErrorMessage = "File extension name is required.")]
        public string FileExtension { get; set; }

        public ArticleImage(byte[] bytes, string fileExtension, Guid articleId)
        {
            Bytes = bytes;
            FileExtension = fileExtension;
            ArticleId = articleId;
        }
    }
}