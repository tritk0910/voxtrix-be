using Application.Core;
using Application.DTOs.Message;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class MessagesController(IMessageRepository messageRepository) : BaseApiController
{
    [HttpGet("direct")]
    public async Task<ActionResult<Result<List<DirectMessageDto>>>> GetConversation([FromQuery] string user1Id, [FromQuery] string user2Id, [FromQuery] DefaultParams defaultParams)
    {
        var messages = await messageRepository.GetConversationAsync(user1Id, user2Id, defaultParams);

        if (messages == null) return NotFound("Messages not found");
        var pagedMessage = await PagedList<DirectMessageDto>.CreateAsync(messages, defaultParams.PageNumber, defaultParams.PageSize);

        var result = new PagedResult<DirectMessageDto>
        {
            Items = pagedMessage,
            CurrentPage = pagedMessage.CurrentPage,
            TotalPages = pagedMessage.TotalPages
        };

        return Ok(Result<PagedResult<DirectMessageDto>>.SuccessResult(result));
    }

    [HttpGet("server")]
    public async Task<ActionResult<Result<List<MessageDto>>>> GetChannelMessages([FromQuery] string channelId, [FromQuery] DefaultParams defaultParams)
    {
        var messages = await messageRepository.GetChannelMessagesAsync(channelId, defaultParams);

        if (messages == null) return NotFound("Messages not found");
        var pagedMessage = await PagedList<MessageDto>.CreateAsync(messages, defaultParams.PageNumber, defaultParams.PageSize);

        var result = new PagedResult<MessageDto>
        {
            Items = pagedMessage,
            CurrentPage = pagedMessage.CurrentPage,
            TotalPages = pagedMessage.TotalPages
        };

        return Ok(Result<PagedResult<MessageDto>>.SuccessResult(result));
    }
}