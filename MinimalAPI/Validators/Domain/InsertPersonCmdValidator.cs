using MinimalAPI.Mediatr.Commands.PersonCommands;
using MinimalAPI.Infrastructure.Database;
using FluentValidation;

namespace MinimalAPI.Validators;

public class InsertPersonCmdValidator : AbstractValidator<InsertPerson>
{
    private readonly IPersonRepo _repo;

    public InsertPersonCmdValidator(IPersonRepo repo)
    {
        _repo = repo;

        RuleFor(p => p.Model.Id).MustAsync(BeUnique).WithMessage("Bu kayıt zaten tanımlı");

    }

    protected async Task<bool> BeUnique(int id, CancellationToken cancellation)
    {
        var result = await _repo.GetById(id);
        return result == null;
    }

    
}
