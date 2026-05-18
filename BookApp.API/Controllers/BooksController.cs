using System.Security.Claims;
using BookApp.API.Data;
using BookApp.API.DTOs;
using BookApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BooksController : ControllerBase
{
	private readonly AppDbContext _context;

	public BooksController(AppDbContext context)
	{
		_context = context;
	}

	private int GetUserId() =>
		int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

	[HttpGet]
	public async Task<IActionResult> GetAll()
	{
		var books = await _context.Books
			.Where(b => b.UserId == GetUserId())
			.ToListAsync();
		return Ok(books);
	}

	[HttpPost]
	public async Task<IActionResult> Create(BookDto dto)
	{
		var book = new Book
		{
			Title = dto.Title,
			Author = dto.Author,
			PublishedDate = dto.PublishedDate,
			UserId = GetUserId()
		};

		_context.Books.Add(book);
		await _context.SaveChangesAsync();
		return Ok(book);
	}

	[HttpPut("{id}")]
	public async Task<IActionResult> Update(int id, BookDto dto)
	{
		var book = await _context.Books
			.FirstOrDefaultAsync(b => b.Id == id && b.UserId == GetUserId());
		if (book == null) return NotFound();

		book.Title = dto.Title;
		book.Author = dto.Author;
		book.PublishedDate = dto.PublishedDate;

		await _context.SaveChangesAsync();
		return Ok(book);
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> Delete(int id)
	{
		var book = await _context.Books
			.FirstOrDefaultAsync(b => b.Id == id && b.UserId == GetUserId());
		if (book == null) return NotFound();

		_context.Books.Remove(book);
		await _context.SaveChangesAsync();
		return Ok();
	}
}