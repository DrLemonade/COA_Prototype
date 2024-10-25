using System;

public class Patient
{
    private string name;
    private string email;
    private string phone;

    public Patient(string name, string email)
    {
        this.name = name;
        this.email = email;
    }
    
    public Patient(string name, string email, string phone)
    {
        this.name = name;
        this.email = email;
        this.phone = phone;
    }
}