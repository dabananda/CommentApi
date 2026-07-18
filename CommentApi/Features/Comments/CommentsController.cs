using CommentApi.Common.Abstraction;
using CommentApi.Controllers;
using CommentApi.Features.Comments.CreateComment;
using Microsoft.AspNetCore.Mvc;

namespace CommentApi.Features.Comments
{
    public class CommentsController(ISender sender) : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> CreateComment([FromBody] CreateCommentCommand command)
        {
            var result = await sender.Send(command);
            return Handle(result);
        }
    }
}
