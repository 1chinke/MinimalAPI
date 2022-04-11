using MinimalAPI.Mediatr.Commands.PersonCommands;
using MinimalAPI.Infrastructure.Database;
using FluentValidation;

namespace MinimalAPI.Validators;

public class InsertPersonCmdValidator : AbstractValidator<InsertPersonCmd>
{
    private readonly IPersonRepo _repo;

    public InsertPersonCmdValidator(IPersonRepo repo)
    {
        _repo = repo;

        RuleFor(p => p)
            .MustAsync((x, cancellationToken) => BeUnique(x))
            .WithMessage("Bu kayıt zaten tanımlı");
    }


    protected async Task<bool> BeUnique(InsertPersonCmd cmd)
    {
        var result = await _repo.GetByFirstNameAndLastName(cmd.FirstName, cmd.LastName);
        return result == null;
    }

}
