using System.ComponentModel.DataAnnotations;

namespace BlogApi.DTOs;

public class CreateCommentDto
{
    [Required]
    public string Content { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Author { get; set; } = string.Empty;

}

public class CommentResponseDto
{
    public string Content { get; set; } = string.Empty;
}

public class CommentDetailResponseDto
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public int PostId { get; set; }
}