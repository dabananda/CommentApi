using CommentApi.Common;
using CommentApi.Common.Abstraction;
using CommentApi.Repositories;

namespace CommentApi.Features.Comments.Update
{
    public class UpdateCommandHandler(ICommentRepository commentRepository) : IRequestHandler<UpdateCommand, Result>
    {
        public async Task<Result> Handle(UpdateCommand request, CancellationToken cancellationToken)
        {
            var comment = await commentRepository.GetCommentByIdAsync(request.Id, cancellationToken);

            if (comment is null)
                return Result.Failure("No comment found");

            comment.AuthorName = request.AuthorName;
            comment.AuthorEmail = request.AuthorEmail;
            comment.Content = request.Content;

            await commentRepository.UpdateCommentAsync(comment, cancellationToken);

            return Result.Success("Comment updated successfully");
        }
    }
}
