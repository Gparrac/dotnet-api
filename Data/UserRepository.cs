namespace DotnetAPI.Data;


public class UserRepository(IConfiguration configuration) : IUserRepository
{
    private readonly DataContextEF _entityFramework = new(configuration);
    public bool SaveChanges()
    {

        return _entityFramework.SaveChanges() > 0;
    }

    public bool AddEntity<T>(T entityToAdd)
    {
        if (entityToAdd != null)
        {
            _entityFramework.Add(entityToAdd);
            return true;
        }
        else
            return false;
    }

    public bool RemoveEntity<T>(T entityToAdd)
    {
        if (entityToAdd != null)
        {
            _entityFramework.Remove(entityToAdd);
            return true;
        }
        else
            return false;
    }
}