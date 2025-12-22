using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Client.Repositorys;

public class LoginModel : PageModel
{
    private readonly UserRepository _userRepo;

    public LoginModel(UserRepository userRepo)
    {
        _userRepo = userRepo;
    }

    [BindProperty]
    public string Login { get; set; }
    [BindProperty]
    public string Motdepasse { get; set; }

    public string ErrorMessage { get; set; }

    public void OnGet() { }

    public IActionResult OnPost()
    {
        if (_userRepo.ValidateUser(Login, Motdepasse))
        {
            // Ici tu peux gérer la session ou un cookie
            return RedirectToPage("/Home/Index");
        }
        else
        {
            ErrorMessage = "Login ou mot de passe incorrect !";
            return Page();
        }
    }
}
