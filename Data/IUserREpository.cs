namespace DotnetAPI.Data;

public interface IUserRepository
{
    public bool RemoveEntity<T>(T entityToAdd);
    public bool AddEntity<T>(T entityToAdd);
    public bool SaveChanges();   
}