using CommentApi.Commands.CreateComment;
using CommentApi.Common.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace CommentApi.Controllers
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
