namespace BLL.DTO.Book;

public class UpdateBookDto
{
    public int Id { get; set; }
    public decimal Price { get; set; }
    public string Name { get; set; }
    public int IdPublisher { get; set; }
    
    public virtual ICollection<int> GenresId { get; set; }

    public virtual ICollection<int> AuthorsId { get; set; }
}