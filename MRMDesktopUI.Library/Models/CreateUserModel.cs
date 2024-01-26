namespace MRMDesktopUI.Library.Models
{
    public class CreateUserModel
    {
        //[Required]
        public string FirstName { get; set; }
        //[Required]
        public string LastName { get; set; }
        //[Required]
        //[EmailAddress]
        public string EmailAddress { get; set; }
        //[Required]
        public string Password { get; set; }
        //[Required]
        //[Compare(nameof(Password), ErrorMessage = "The passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}
