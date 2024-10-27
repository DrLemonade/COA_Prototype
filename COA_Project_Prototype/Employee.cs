using System;
using System.Globalization;
using System.Threading;

public static class Program
{
    Employee[] employees;
    public void main()
    {
        employees = {new Executive("executive", "username1", "password"), 
            new DeptChair("department chair", "username2", "password"), 
            new ProviderManager("provider manager", "username3", "password"), 
            new Provider("provider", "username4", "password"),
            new Analyst("analyst", "username5", "password"),
            new SiteAdministrator("site administrator", "username6", "password"),
            new DataAdministrator("data administrator", "username7", "password"),
            new TenantAdministrator("tenant administrator", "username8", "password")};
    }
    
    public Employee logIn(string username)
    {
        int min = 0;
        int max = employees.length - 1;
        while(max > min)
        {
            mid = (min + max) / 2;
            if(employees[mid].getUsername().Equals(username))
                return employees[i];
            else if(String.Compare(username, employees[i].getUsername(), comparisonType: StringComparison.OrdinalIgnoreCase) < 0)
                max = mid - 1;
            else if(String.Compare(username, employees[i].getUsername(), comparisonType: StringComparison.OrdinalIgnoreCase) > 0)
                min = mid + 1;
        }
        return null;
    }
}

public abstract class Employee
{
    private string name;
    private string username;
    private string password;

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

