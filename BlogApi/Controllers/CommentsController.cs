using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlogApi.Data;
using BlogApi.Models;
using BlogApi.DTOs;

namespace BlogApi.Controllers;

[ApiController]
[Route("api/posts/{postId}/[controller]")]
public class CommentsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<CommentsController> _logger;

    public CommentsController(AppDbContext context, ILogger<CommentsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: api/posts/1/comments
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CommentResponseDto>>> GetComments(int postId)
    {
        var post = await _context.Posts.FindAsync(postId);
        if (post == null)
        {
            return NotFound(new { message = $"Post with id {postId} not found" });
        }

        var comments = await _context.Comments
            .Where(c => c.PostId == postId)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();

        return Ok(comments.Select(c => MapToCommentResponseDto(c)));
    }

    // GET: api/posts/1/comments/5
    [HttpGet("{id}")]
    public async Task<ActionResult<CommentResponseDto>> GetComment(int postId, int id)
    {
        var comment = await _context.Comments
            .FirstOrDefaultAsync(c => c.Id == id && c.PostId == postId);

        if (comment == null)
        {
            return NotFound(new { message = $"Comment with id {id} not found for post {postId}" });
        }

        return Ok(MapToCommentResponseDto(comment));
    }

    // POST: api/posts/1/comments
    [HttpPost]
    public async Task<ActionResult<CommentResponseDto>> CreateComment(int postId, CreateCommentDto createDto)
    {
        var post = await _context.Posts.FindAsync(postId);
        if (post == null)
        {
            return NotFound(new { message = $"Post with id {postId} not found" });
        }

        var comment = new Comment
        {
            Content = createDto.Content,
            Author = createDto.Author,
            CreatedAt = DateTime.UtcNow,
            PostId = postId
        };

        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();

        _logger.LogInformation($"Created new comment with id {comment.Id} for post {postId}");

        return CreatedAtAction(nameof(GetComment), 
            new { postId = postId, id = comment.Id }, 
            MapToCommentResponseDto(comment));
    }

    // PUT: api/posts/1/comments/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateComment(int postId, int id, UpdateCommentDto updateDto)
    {
        var comment = await _context.Comments
            .FirstOrDefaultAsync(c => c.Id == id && c.PostId == postId);

        if (comment == null)
        {
            return NotFound(new { message = $"Comment with id {id} not found for post {postId}" });
        }

        if (!string.IsNullOrEmpty(updateDto.Content))
        {
            comment.Content = updateDto.Content;
        }

        await _context.SaveChangesAsync();
        _logger.LogInformation($"Updated comment with id {id} for post {postId}");

        return NoContent();
    }

    // DELETE: api/posts/1/comments/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteComment(int postId, int id)
    {
        var comment = await _context.Comments
            .FirstOrDefaultAsync(c => c.Id == id && c.PostId == postId);

        if (comment == null)
        {
            return NotFound(new { message = $"Comment with id {id} not found for post {postId}" });
        }

        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();
        
        _logger.LogInformation($"Deleted comment with id {id} for post {postId}");

        return NoContent();
    }

    private CommentResponseDto MapToCommentResponseDto(Comment comment)
    {
        return new CommentResponseDto
        {
            Content = comment.Content,
        };
    }
}