using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Client.Repositorys;
using System.ComponentModel.DataAnnotations;


public class LoginModel : PageModel
{
    private readonly UserRepository _userRepo;

    public LoginModel(UserRepository userRepo)
    {
        _userRepo = userRepo;
    }

    [BindProperty]
    [Required(ErrorMessage = "Le login est obligatoire")]
    // [EmailAddress(ErrorMessage = "Email invalide")]
    public string Login { get; set; }
    [BindProperty]
    [Required(ErrorMessage = "Le mot de passe est obligatoire")]
    public string Motdepasse { get; set; }

    public string ErrorMessage { get; set; }

    public void OnGet() { }

  
        public IActionResult OnPost()
    {
        int? userId = _userRepo.ValidateUser(Login, Motdepasse);

        if (userId != null)
        {
            // ✅ Enregistrer dans la session
            Console.WriteLine( userId.Value);
            HttpContext.Session.SetInt32("UserId", userId.Value);

            return RedirectToPage("/Home/UserHome");
        }

        ModelState.AddModelError(string.Empty, "Login ou mot de passe incorrect");
        return Page();
    }

}
