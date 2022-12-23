namespace BLL.DTO.Book;

public class CreateBookDto
{
    public decimal Price { get; set; }
    public string Name { get; set; }
    public int IdPublisher { get; set; }
    
    public virtual ICollection<int> IdGenres { get; set; }

    public virtual ICollection<int> IdAuthors { get; set; }
}