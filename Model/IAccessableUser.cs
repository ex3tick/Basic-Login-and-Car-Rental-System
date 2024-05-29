namespace WebApp.Model;

public interface IAccessableUser
{
    bool Login (UserModel user);
   int Register (UserModel user);
    
    bool IsAdmin(string username);
    
    int GetUserId(string username);
    
   UserModel GetUserById(int id);
}