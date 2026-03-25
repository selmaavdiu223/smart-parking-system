using System.IO;

public class FileRepository<T> : IRepository<T>
{
    private string filePath = "data.csv";
    private List<T> items = new List<T>();

    public List<T> GetAll()
    {
        return items;
    }

    public T GetById(int id)
    {
        return items.FirstOrDefault();
    }

    public void Add(T entity)
    {
        items.Add(entity);
    }

    public void Save()
    {
        File.WriteAllText(filePath, "data saved");
    }
}