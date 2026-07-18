using CommentApi.Data;
using CommentApi.Features.Comments;
using Microsoft.EntityFrameworkCore;

namespace CommentApi.Repositories.Implementations
{
    public class CommentRepository(CommentDbContext context) : ICommentRepository
    {
        public async Task CreateCommentAsync(Comment comment, CancellationToken cancellationToken)
        {
            await context.Comments.AddAsync(comment, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteCommentAsync(Comment comment, CancellationToken cancellationToken)
        {
            comment.IsDeleted = true;
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task<Comment?> GetCommentByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await context.Comments.FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted, cancellationToken);
        }

        public async Task<IEnumerable<Comment>> GetCommentsAsync(CancellationToken cancellationToken)
        {
            return await context.Comments
                .Where(c => !c.IsDeleted)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task UpdateCommentAsync(Comment comment, CancellationToken cancellationToken)
        {
            context.Update(comment);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
