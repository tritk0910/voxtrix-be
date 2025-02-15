using Application.Core;
using Application.DTOs.Users;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public partial class UsersController
{
    [HttpGet("friends")]
    public async Task<ActionResult<Result<PagedResult<UserBasicDto>>>> GetFriendsAsync([FromQuery] string userId, [FromBody] DefaultParams defaultParams)
    {
        var friends = await userRepository.GetFriendsAsync(userId, defaultParams);
        if (friends == null) return NotFound(Result<PagedResult<UserBasicDto>>.FailureResult("User not found"));

        var pagedFriends = await PagedList<UserBasicDto>.CreateAsync(friends, defaultParams.PageNumber, defaultParams.PageSize);

        var result = new PagedResult<UserBasicDto>
        {
            Items = pagedFriends,
            CurrentPage = pagedFriends.CurrentPage,
            TotalPages = pagedFriends.TotalPages
        };

        return Ok(Result<PagedResult<UserBasicDto>>.SuccessResult(result));
    }

    [HttpGet("friends/pending")]
    public async Task<ActionResult<Result<PagedResult<FriendDto>>>> GetPendingFriendRequestsAsync([FromQuery] string userId, [FromBody] DefaultParams defaultParams)
    {
        var pendingRequests = await userRepository.GetPendingFriendRequestsAsync(userId, defaultParams);
        if (pendingRequests == null) return NotFound(Result<PagedResult<FriendDto>>.FailureResult("User not found"));

        var pagedRequests = await PagedList<FriendDto>.CreateAsync(pendingRequests, defaultParams.PageNumber, defaultParams.PageSize);

        var result = new PagedResult<FriendDto>
        {
            Items = pagedRequests,
            CurrentPage = pagedRequests.CurrentPage,
            TotalPages = pagedRequests.TotalPages
        };

        return Ok(Result<PagedResult<FriendDto>>.SuccessResult(result));
    }

    [HttpGet("friends/incoming")]
    public async Task<ActionResult<Result<PagedResult<FriendDto>>>> GetIncomingFriendRequestsAsync([FromQuery] string userId, [FromBody] DefaultParams defaultParams)
    {
        var incomingRequests = await userRepository.GetIncomingFriendRequestsAsync(userId, defaultParams);
        if (incomingRequests == null) return NotFound(Result<PagedResult<FriendDto>>.FailureResult("User not found"));

        var pagedRequests = await PagedList<FriendDto>.CreateAsync(incomingRequests, defaultParams.PageNumber, defaultParams.PageSize);

        var result = new PagedResult<FriendDto>
        {
            Items = pagedRequests,
            CurrentPage = pagedRequests.CurrentPage,
            TotalPages = pagedRequests.TotalPages
        };

        return Ok(Result<PagedResult<FriendDto>>.SuccessResult(result));
    }

    [HttpPost("friends/request")]
    public async Task<ActionResult<Result<FriendResponseDto>>> SendFriendRequest([FromQuery] string userId, string targetUsername)
    {
        var result = await userRepository.SendFriendRequestAsync(userId, targetUsername);
        if (!result.Success) return BadRequest(result);

        return Ok(result);
    }

    [HttpPut("friends/request")]
    public async Task<ActionResult<Result<FriendResponseDto>>> IgnoreFriendRequest([FromQuery] string requestId)
    {
        var result = await userRepository.IgnoreFriendRequestAsync(requestId);
        if (!result.Success) return BadRequest(result);

        return Ok(result);
    }

    [HttpDelete("friends/request")]
    public async Task<ActionResult<Result<bool>>> CancelFriendRequest([FromQuery] string requestId)
    {
        var result = await userRepository.RemoveFriendRequestAsync(requestId);
        if (!result.Success) return BadRequest(result);

        return Ok(result);
    }

    [HttpPost("friends")]
    public async Task<ActionResult<Result<FriendResponseDto>>> AcceptFriendRequest([FromQuery] string requestId)
    {
        var result = await userRepository.AcceptFriendRequestAsync(requestId);
        if (!result.Success) return BadRequest(result);

        return Ok(result);
    }

    [HttpDelete("friends")]
    public async Task<ActionResult<Result<bool>>> RemoveFriend([FromQuery] string userId, string targetId)
    {
        var result = await userRepository.RemoveFriendAsync(userId, targetId);
        if (!result.Success) return BadRequest(result);

        return Ok(result);
    }
}