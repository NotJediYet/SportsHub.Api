﻿using FluentValidation;
using SportsHub.Business.Services;
using SportsHub.Shared.Models;

namespace SportsHub.Web.Validators
{
    internal class CreateTeamModelValidator : AbstractValidator<CreateTeamModel>
    {
        private readonly ISubcategoryService _subcategoryService;
        private readonly ITeamService _teamService;

        public CreateTeamModelValidator(
            ISubcategoryService subcategoryService,
            ITeamService teamService)
        {
            _subcategoryService = subcategoryService ?? throw new ArgumentNullException(nameof(subcategoryService));
            _teamService = teamService ?? throw new ArgumentNullException(nameof(teamService));

            RuleFor(team => team.Name)
                .NotEmpty().WithMessage("Team name cannot be empty!")
                .MustAsync((name, cancellation) => DoesTeamNameIsUniqueAsync(name)).WithMessage("Team with that name already exists!");

            RuleFor(team => team.SubcategoryId)
                .NotEmpty().WithMessage("Subcategory id cannot be empty!")
                .MustAsync((id, cancellation) => _subcategoryService.DoesSubcategoryAlredyExistByIdAsync(id))
                .WithMessage("Subcategory with that id does not exist!");
        }

        private async Task<bool> DoesTeamNameIsUniqueAsync(string teamName)
        {
            var result = await _teamService.DoesTeamAlreadyExistByNameAsync(teamName);

            return !result;
        }
    }
}
