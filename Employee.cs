using System;

public abstract class Employee
{
    private string name;
    private string username;
    private string password;
    private int employeeID;

    public Employee(string name, string username, string password)
    {
        this.name = name;
        this.username = username;
        this.password = password;
    }

    public string getName() { return name; }

    public string getUsername() { return name; }

    public string getPassowrd() { return name; }
}

public class Executive: Employee
{
    public Executive(string name, string username, string password) : base(name, username, password)
    {
        
    }

    public string toString()
    {
        return getName() + ": " + getUsername() + ", " + getPassowrd();
    }
}

public class DeptChair: Employee
{
    public DeptChair(string name, string username, string password) : base(name, username, password)
    {
        
    }

    public string toString()
    {
        return getName() + ": " + getUsername() + ", " + getPassowrd();
    }
}

public class ProviderManager: Employee
{
    public ProviderManager(string name, string username, string password) : base(name, username, password)
    {
        
    }

    public string toString()
    {
        return getName() + ": " + getUsername() + ", " + getPassowrd();
    }
}


public class Provider: Employee
{
    public Provider(string name, string username, string password) : base(name, username, password)
    {
        
    }

    public string toString()
    {
        return getName() + ": " + getUsername() + ", " + getPassowrd();
    }
}

public class Analyst: Employee
{
    public Analyst(string name, string username, string password) : base(name, username, password)
    {
        
    }

    public string toString()
    {
        return getName() + ": " + getUsername() + ", " + getPassowrd();
    }
}

public class SiteAdministrator: Employee
{
    public SiteAdministrator(string name, string username, string password) : base(name, username, password)
    {
        
    }

    public string toString()
    {
        return getName() + ": " + getUsername() + ", " + getPassowrd();
    }
}

public class DataAdministrator: Employee
{
    public DataAdministrator(string name, string username, string password) : base(name, username, password)
    {
        
    }

    public string toString()
    {
        return getName() + ": " + getUsername() + ", " + getPassowrd();
    }
}

public class TenantAdministrator: Employee
{
    public TenantAdministrator(string name, string username, string password) : base(name, username, password)
    {
        
    }

    public string toString()
    {
        return getName() + ": " + getUsername() + ", " + getPassowrd();
    }
}

