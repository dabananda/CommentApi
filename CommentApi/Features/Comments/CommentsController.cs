using CommentApi.Common.Abstraction;
using CommentApi.Controllers;
using CommentApi.Features.Comments.Create;
using CommentApi.Features.Comments.Delete;
using CommentApi.Features.Comments.GetAll;
using CommentApi.Features.Comments.GetById;
using CommentApi.Features.Comments.Update;
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

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetCommentById([FromRoute] Guid id)
        {
            var result = await sender.Send(new GetByIdQuery(id));
            return Handle(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateComment([FromBody] UpdateCommand command)
        {
            var result = await sender.Send(command);
            return Handle(result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> DeleteComment([FromRoute] Guid id)
        {
            var result = await sender.Send(new DeleteCommand(id));
            return Handle(result);
        }
    }
}
