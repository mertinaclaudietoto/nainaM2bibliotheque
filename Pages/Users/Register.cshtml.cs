using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Client.Repositorys;
public class RegisterModel : PageModel
{
    private readonly UserRepository _userRepo;

    public RegisterModel(UserRepository userRepo)
    {
        _userRepo = userRepo;
    }

    [BindProperty]
    public string Nom { get; set; }
    [BindProperty]
    public string Prenom { get; set; }
    [BindProperty]
    public string Email { get; set; }
    [BindProperty]
    public string Login { get; set; }
    [BindProperty]
    public string Motdepasse { get; set; }
    [BindProperty]
    public DateTime? DateNaissance { get; set; }

    public string ErrorMessage { get; set; }
    public string SuccessMessage { get; set; }

    public void OnGet() { }

    public void OnPost()
    {
        if (_userRepo.LoginExists(Login))
        {
            ErrorMessage = "Ce login existe déjà !";
            return;
        }

        _userRepo.Register(Nom, Prenom, Email, Login, Motdepasse, DateNaissance);
        SuccessMessage = "Inscription réussie !";
    }
}
