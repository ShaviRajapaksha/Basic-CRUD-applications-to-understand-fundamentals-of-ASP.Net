using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlogApi.Data;
using BlogApi.Models;
using BlogApi.DTOs;

namespace BlogApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<PostsController> _logger;

    public PostsController(AppDbContext context, ILogger<PostsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: api/posts
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PostResponseDto>>> GetPosts()
    {
        var posts = await _context.Posts
            .Include(p => p.Comments)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();

        return Ok(posts.Select(p => MapToPostResponseDto(p)));
    }

    // GET: api/posts/5
    [HttpGet("{id}")]
    public async Task<ActionResult<PostResponseDto>> GetPost(int id)
    {
        var post = await _context.Posts
            .Include(p => p.Comments)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (post == null)
        {
            return NotFound(new { message = $"Post with id {id} not found" });
        }

        return Ok(MapToPostResponseDto(post));
    }

    // POST: api/posts
    [HttpPost]
    public async Task<ActionResult<PostResponseDto>> CreatePost(CreatePostDto createDto)
    {
        var post = new Post
        {
            Title = createDto.Title,
            Content = createDto.Content,
            Author = createDto.Author,
            CreatedAt = DateTime.UtcNow
        };

        _context.Posts.Add(post);
        await _context.SaveChangesAsync();

        _logger.LogInformation($"Created new post with id {post.Id}");
        
        return CreatedAtAction(nameof(GetPost), new { id = post.Id }, MapToPostResponseDto(post));
    }

    // PUT: api/posts/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePost(int id, UpdatePostDto updateDto)
    {
        var post = await _context.Posts.FindAsync(id);
        
        if (post == null)
        {
            return NotFound(new { message = $"Post with id {id} not found" });
        }

        if (!string.IsNullOrEmpty(updateDto.Title))
        {
            post.Title = updateDto.Title;
        }
        
        if (!string.IsNullOrEmpty(updateDto.Content))
        {
            post.Content = updateDto.Content;
        }
        
        post.UpdatedAt = DateTime.UtcNow;

        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Updated post with id {id}");
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!PostExists(id))
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }

    // DELETE: api/posts/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePost(int id)
    {
        var post = await _context.Posts
            .Include(p => p.Comments)
            .FirstOrDefaultAsync(p => p.Id == id);
            
        if (post == null)
        {
            return NotFound(new { message = $"Post with id {id} not found" });
        }

        _context.Posts.Remove(post);
        await _context.SaveChangesAsync();
        
        _logger.LogInformation($"Deleted post with id {id} and its {post.Comments.Count} comments");

        return NoContent();
    }

    private bool PostExists(int id)
    {
        return _context.Posts.Any(e => e.Id == id);
    }

    private PostResponseDto MapToPostResponseDto(Post post)
    {
        return new PostResponseDto
        {
            Id = post.Id,
            Title = post.Title,
            Content = post.Content,
            Author = post.Author,
            CreatedAt = post.CreatedAt,
            UpdatedAt = post.UpdatedAt,
            Comments = post.Comments.Select(c => new CommentResponseDto
            {
                Content = c.Content,
            }).ToList()
        };
    }
}