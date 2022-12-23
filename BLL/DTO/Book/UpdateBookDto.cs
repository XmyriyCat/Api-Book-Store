namespace BLL.DTO.Book
{
    public class UpdateBookDto
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public string Name { get; set; }
        public int IdPublisher { get; set; }
    }
}
