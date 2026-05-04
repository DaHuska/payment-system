using System.Collections.Generic;
using System.Linq;
using System;
using is_payment_system.Model;

public class ClientProfileViewModel : BaseProfileViewModel
{
    public string ClientName;
    public decimal TotalBalance;
    public ClientProfileViewModel(User currentUser)
        : base(currentUser)
    {
        //TODO: fix logic
    }
}