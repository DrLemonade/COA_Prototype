using System;

public class Patient
{
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string Phone { get; private set; }

    public Patient(string name, string email)
    {
        this.Name = name;
        this.Email = email;
    }
    
    public Patient(string name, string email, string phone)
    {
        this.Name = name;
        this.Email = email;
        this.Phone = phone;
    }
}