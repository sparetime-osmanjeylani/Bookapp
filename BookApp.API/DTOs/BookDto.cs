namespace BookApp.API.DTOs;

public class BookDto
{
	public string Title { get; set; } = string.Empty;
	public string Author { get; set; } = string.Empty;
	public DateTime PublishedDate { get; set; }
}