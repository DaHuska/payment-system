using is_payment_system.Model.Enums;

namespace is_payment_system.Model;

public class Merchant
{
    private int _id;
    private string _businessName;
    private string _email;
    private string _phone;
    private decimal _balance;
    private MerchantStatus _status;
    private DateTime _createdAt;
    
    public Merchant()
    {
    }

    public int Id
    {
        get => _id;
        set => _id = value;
    }

    public decimal Balance
    {
        get => _balance;
        set => _balance = value;
    }

    public string BusinessName
    {
        get => _businessName;
        set => _businessName = value;
    }

    public string Email
    {
        get => _email;
        set => _email = value;
    }

    public string Phone
    {
        get => _phone;
        set => _phone = value;
    }

    public MerchantStatus Status
    {
        get => _status;
        set => _status = value;
    }

    public DateTime CreatedAt
    {
        get => _createdAt;
        set => _createdAt = value;
    }
}