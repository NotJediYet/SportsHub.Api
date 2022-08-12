﻿using FluentValidation;
using SportsHub.Business.Services;
using SportsHub.Shared.Models;
using SportsHub.Shared.Resources;

namespace SportsHub.Web.Validators
{
    public class CreateTeamModelValidator : AbstractValidator<CreateTeamModel>
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
                .NotEmpty().WithMessage(Errors.TeamNameCannotBeEmpty)
                .MustAsync((name, cancellation) => DoesTeamNameIsUniqueAsync(name))
                .WithMessage(Errors.TeamNameIsNotUnique);

            RuleFor(team => team.SubcategoryId)
                .NotEmpty().WithMessage(Errors.SubcategoryIdCannotBeEmpty)
                .MustAsync((id, cancellation) => _subcategoryService.DoesSubcategoryAlredyExistByIdAsync(id))
                .WithMessage(Errors.SubcategoryDoesNotExist);
        }

        private async Task<bool> DoesTeamNameIsUniqueAsync(string teamName)
        {
            var result = await _teamService.DoesTeamAlreadyExistByNameAsync(teamName);

            return !result;
        }
    }
}
