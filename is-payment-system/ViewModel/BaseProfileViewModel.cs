using System;
using System.Windows.Input;
using is_payment_system.Model;
using is_payment_system.Model.Enums;

public class BaseProfileViewModel
{
    public string Username { get; set; }
    public UserRole Role { get; set; }

    public ICommand LogoutCommand { get; set; }

    public BaseProfileViewModel(User currentUser)
    {
        Role = currentUser.Role;
        LogoutCommand = new RelayCommand(ExecuteLogout);
    }

    private void ExecuteLogout()
    {
        Console.WriteLine($"{Username} излезе от системата.");
        // По-късно ще добавя логика за Login екран.
    }
}