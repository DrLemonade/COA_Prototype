class patient{
    private int patientID;
    private string name;
    private string username;
    private string password;
    private string email;
    private string phone;

    public patient(string n, string u, string p, string e){
        name = n;
        username = u;
        password = p;
        email = e;
    }
    
    public patient(string n, string u, string p, string e, string ph){
        name = n;
        username = u;
        password = p;
        email = e;
        phone = ph;
    }
}