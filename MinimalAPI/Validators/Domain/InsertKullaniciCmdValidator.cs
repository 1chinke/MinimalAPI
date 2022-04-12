using FluentValidation;
using MinimalAPI.Infrastructure.Repository.Queries;
using MinimalAPI.Mediatr.Commands.KullaniciCommands;

namespace MinimalAPI.Validators;

public class InsertKullaniciCmdValidator : AbstractValidator<InsertKullaniciCmd>
{
    private readonly IKullaniciQryRepo _repo;

    public InsertKullaniciCmdValidator(IKullaniciQryRepo repo)
    {
        _repo = repo;

        RuleFor(p => p.Model.Username).MustAsync(BeUnique).WithMessage("Bu kayıt zaten tanımlı.");
    }

    protected async Task<bool> BeUnique(string username, CancellationToken cancellation)
    {
        var result = await _repo.GetByUsername(username);
        return result == null;
    }


}
