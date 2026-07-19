using CommentApi.Common.Abstraction;
using CommentApi.Controllers;
using CommentApi.Features.Comments.Create;
using CommentApi.Features.Comments.GetAll;
using Microsoft.AspNetCore.Mvc;

namespace CommentApi.Features.Comments
{
    public class CommentsController(ISender sender) : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> CreateComment([FromBody] CreateCommand command)
        {
            var result = await sender.Send(command);
            return Handle(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetComments([FromQuery] GetAllQuery query)
        {
            var result = await sender.Send(query);
            return Handle(result);
        }
    }
}
