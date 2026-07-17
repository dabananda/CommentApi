using CommentApi.Entities;

namespace CommentApi.Repositories
{
    public interface ICommentRepository
    {
        Task CreateCommentAsync(Comment comment, CancellationToken cancellationToken);
        Task<Comment?> GetCommentByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<IQueryable<Comment>> GetCommentsAsync(CancellationToken cancellationToken);
        Task UpdateCommentAsync(Comment comment, CancellationToken cancellationToken);
        Task DeleteCommentAsync(Comment comment, CancellationToken cancellationToken);
    }
}
