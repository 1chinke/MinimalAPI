using MinimalAPI.Mediatr.Commands.PersonCommands;
using FluentValidation;
using MinimalAPI.Infrastructure.Repository.Queries;

namespace MinimalAPI.Validators;

public class InsertPersonCmdValidator : AbstractValidator<InsertPersonCmd>
{
    private readonly IPersonQryRepo _repo;

    public InsertPersonCmdValidator(IPersonQryRepo repo)
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
