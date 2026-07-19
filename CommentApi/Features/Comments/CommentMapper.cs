using CommentApi.Features.Comments.Create;

namespace CommentApi.Features.Comments
{
    public static class CommentMapper
    {
        public static CommentDto ToDto(this Comment comment)
        {
            return new CommentDto
            {
                Id = comment.Id,
                PostId = comment.PostId,
                ParentCommentId = comment.ParentCommentId,
                AuthorName = comment.AuthorName,
                AuthorEmail = comment.AuthorEmail,
                Content = comment.Content,
                CreatedAt = comment.CreatedAt
            };
        }

        public static Comment ToEntity(this CommentDto commentDto)
        {
            return new Comment
            {
                Id = commentDto.Id,
                PostId = commentDto.PostId,
                ParentCommentId = commentDto.ParentCommentId,
                AuthorName = commentDto.AuthorName,
                AuthorEmail = commentDto.AuthorEmail,
                Content = commentDto.Content,
                CreatedAt = commentDto.CreatedAt
            };
        }

        public static Comment ToEntity(this CreateCommand command)
        {
            return new Comment
            {
                PostId = command.PostId,
                ParentCommentId = command.ParentCommentId,
                AuthorName = command.AuthorName,
                AuthorEmail = command.AuthorEmail,
                Content = command.Content,
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}
