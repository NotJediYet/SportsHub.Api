﻿using Microsoft.AspNetCore.Http;

namespace SportsHub.Shared.Models
{
    public class EditTeamModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public Guid SubcategoryId { get; set; }

        public bool IsHidden { get; set; } = false;

        public int OrderIndex { get; set; }

        public IFormFile TeamLogo { get; set; }
    }
}
