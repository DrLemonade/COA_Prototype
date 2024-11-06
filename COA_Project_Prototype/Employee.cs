using System;
using System.Globalization;
using System.Threading;


public class EmployeeArray
{
    private Employee[] employees;
    private int employeeCount;

    public EmployeeArray()
    {
        employees = new Employee[10];
        employeeCount = 0;
    }

    public void add(Employee employee)
    {
        insert(employee, employeeCount);
    }

    public void insert(Employee employee, int index)
    {
        if(index < 0 || index > employeeCount)
            throw new IndexOutOfRangeException();
        if(elementCount == employees.length) 
			doubleBackingArray();
        for(int i = employeeCount; i > index; i--)
            employees[i+1] = employees[1];
        employees[index] = employee;
        employeeCount++;
    }

    private void doubleBackingArray()
    {
        Employee newArray = new Employee[employees.length * 2];
        for(int i = 0; i < employees.length; i++){
            newArray[i] = employees[i];
        }
        employees = newArray;
    }

    public void remove(int index)
    {
        if(index < 0 || index > employeeCount)
            throw new IndexOutOfRangeException();

        for(int i = index + 1; i < elementCount; i++)
            employees[i - 1] = employees[i];

        employees[employeeCount] = null;
        employeeCount--;
    }

    public void sort(bool byName)
    {
        sort(employees, 0, employeeCount - 1, byName);
    }

    private void sort(int startIndex, int pivotIndex, bool byName)
    {
        if(startIndex >= pivotIndex)
            return;

        int i = startIndex - 1;
        int j = startIndex;
        while(j <= pivotIndex)
        {
            if(employees[j].compareTo(employees[pivotIndex], byName) <= 0)
            {
                i++;
                Employee temp = employees[i];
                employees[i] = employees[j];
                employees[j] = temp;
            }
            j++;
        }

        sort(startIndex, i - 1, byName);
        sort(i + 1, pivotIndex, byName);
    }

    public Employee logIn(string username)
    {
        int min = 0;
        int max = employees.length - 1;
        while(max > min)
        {
            mid = (min + max) / 2;
            else if(employees[mid] == null ||  String.Compare(username, employees[i].getUsername(), comparisonType: StringComparison.OrdinalIgnoreCase)< 0)
                max = mid - 1;
            else if(String.Compare(username, employees[i].getUsername(), comparisonType: StringComparison.OrdinalIgnoreCase) > 0)
                min = mid + 1;
            else if(employees[mid].getUsername().Equals(username))
                return employees[i];
        }
        return null;
    }

    public void getEmployee(int index) { return employees[index]; }
    public void getSize() { return employeeCount; }

    public string toString()
    {
        string result = "";
        for(int i = 0; i < employeeCount; i++)
            result += " " + employees[i];
            
        return result;
    }
}

public abstract class Employee
{
    private string name;
    private string username;
    private string password;
    private int index;

    public Employee(string name, string username, string password, int index)
    {
        this.name = name;
        this.username = username;
        this.password = password;
        this.index = index;
    }

    public string getName() { return name; }

    public string getUsername() { return name; }

    public string getPassowrd() { return name; }
    public double compareTo(Employee other, bool byName)
    {
        if(byName)
            return String.Compare(username, other.getUsername(), comparisonType: StringComparison.OrdinalIgnoreCase);
        else
            return String.Compare(name, other.getName(), comparisonType: StringComparison.OrdinalIgnoreCase);
    }
}

public class Executive: Employee
{
    public Executive(string name, string username, string password, int index) : base(name, username, password, index)
    {
        
    }

    public string toString()
    {
        return getName() + ": " + getUsername() + ", " + getPassowrd();
    }
}

public class DeptChair: Employee
{
    public DeptChair(string name, string username, string password, int index) : base(name, username, password, index)
    {
        
    }

    public string toString()
    {
        return getName() + ": " + getUsername() + ", " + getPassowrd();
    }
}

public class ProviderManager: Employee
{
    public ProviderManager(string name, string username, string password, int index) : base(name, username, password, index)
    {
        
    }

    public string toString()
    {
        return getName() + ": " + getUsername() + ", " + getPassowrd();
    }
}


public class Provider: Employee
{
    public Provider(string name, string username, string password, int index) : base(name, username, password, index)
    {
        
    }

    public string toString()
    {
        return getName() + ": " + getUsername() + ", " + getPassowrd();
    }
}

public class Analyst: Employee
{
    public Analyst(string name, string username, string password, int index) : base(name, username, password, index)
    {
        
    }

    public string toString()
    {
        return getName() + ": " + getUsername() + ", " + getPassowrd();
    }
}

public class SiteAdministrator: Employee
{
    public SiteAdministrator(string name, string username, string password, int index) : base(name, username, password, index)
    {
        
    }

    public string toString()
    {
        return getName() + ": " + getUsername() + ", " + getPassowrd();
    }
}

public class DataAdministrator: Employee
{
    public DataAdministrator(string name, string username, string password, int index) : base(name, username, password, index)
    {
        
    }

    public string toString()
    {
        return getName() + ": " + getUsername() + ", " + getPassowrd();
    }
}

public class TenantAdministrator: Employee
{
    public TenantAdministrator(string name, string username, string password, int index) : base(name, username, password, index)
    {
        
    }

    public string toString()
    {
        return getName() + ": " + getUsername() + ", " + getPassowrd();
    }
}

