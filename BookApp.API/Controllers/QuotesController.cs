using System.Security.Claims;
using BookApp.API.Data;
using BookApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class QuotesController : ControllerBase
{
	private readonly AppDbContext _context;

	public QuotesController(AppDbContext context)
	{
		_context = context;
	}

	private int GetUserId() =>
		int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

	[HttpGet]
	public async Task<IActionResult> GetAll()
	{
		var quotes = await _context.Quotes
			.Where(q => q.UserId == GetUserId())
			.ToListAsync();
		return Ok(quotes);
	}

	[HttpPost]
	public async Task<IActionResult> Create(Quote dto)
	{
		var quote = new Quote
		{
			Text = dto.Text,
			Author = dto.Author,
			UserId = GetUserId()
		};

		_context.Quotes.Add(quote);
		await _context.SaveChangesAsync();
		return Ok(quote);
	}

	[HttpPut("{id}")]
	public async Task<IActionResult> Update(int id, Quote dto)
	{
		var quote = await _context.Quotes
			.FirstOrDefaultAsync(q => q.Id == id && q.UserId == GetUserId());
		if (quote == null) return NotFound();

		quote.Text = dto.Text;
		quote.Author = dto.Author;

		await _context.SaveChangesAsync();
		return Ok(quote);
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> Delete(int id)
	{
		var quote = await _context.Quotes
			.FirstOrDefaultAsync(q => q.Id == id && q.UserId == GetUserId());
		if (quote == null) return NotFound();

		_context.Quotes.Remove(quote);
		await _context.SaveChangesAsync();
		return Ok();
	}
}