using MinimalAPI.Mediatr.Commands.KullaniciCommands;
using MinimalAPI.Repository;
using FluentValidation;

namespace MinimalAPI.Validators;

public class InsertKullaniciCmdValidator : AbstractValidator<InsertKullanici>
{
    private readonly IKullaniciRepo _repo;

    public InsertKullaniciCmdValidator(IKullaniciRepo repo)
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
