using Application.Core;
using Application.DTOs.Friends;
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
    public async Task<ActionResult<Result<PagedResult<FriendRequestDto>>>> GetPendingFriendRequestsAsync([FromQuery] string userId, [FromBody] DefaultParams defaultParams)
    {
        var pendingRequests = await userRepository.GetPendingFriendRequestsAsync(userId, defaultParams);
        if (pendingRequests == null) return NotFound(Result<PagedResult<FriendRequestDto>>.FailureResult("User not found"));

        var pagedRequests = await PagedList<FriendRequestDto>.CreateAsync(pendingRequests, defaultParams.PageNumber, defaultParams.PageSize);

        var result = new PagedResult<FriendRequestDto>
        {
            Items = pagedRequests,
            CurrentPage = pagedRequests.CurrentPage,
            TotalPages = pagedRequests.TotalPages
        };

        return Ok(Result<PagedResult<FriendRequestDto>>.SuccessResult(result));
    }

    [HttpGet("friends/incoming")]
    public async Task<ActionResult<Result<PagedResult<FriendRequestDto>>>> GetIncomingFriendRequestsAsync([FromQuery] string userId, [FromBody] DefaultParams defaultParams)
    {
        var incomingRequests = await userRepository.GetIncomingFriendRequestsAsync(userId, defaultParams);
        if (incomingRequests == null) return NotFound(Result<PagedResult<FriendRequestDto>>.FailureResult("User not found"));

        var pagedRequests = await PagedList<FriendRequestDto>.CreateAsync(incomingRequests, defaultParams.PageNumber, defaultParams.PageSize);

        var result = new PagedResult<FriendRequestDto>
        {
            Items = pagedRequests,
            CurrentPage = pagedRequests.CurrentPage,
            TotalPages = pagedRequests.TotalPages
        };

        return Ok(Result<PagedResult<FriendRequestDto>>.SuccessResult(result));
    }

    [HttpPost("friends/request")]
    public async Task<ActionResult<Result<string>>> SendFriendRequest([FromQuery] string userId, string friendId)
    {
        var result = await userRepository.SendFriendRequestAsync(userId, friendId);
        if (result == "Friend request sent") return Ok(Result<string>.SuccessResult("", result));

        return BadRequest(Result<string>.FailureResult(result));
    }

    [HttpPut("friends/request")]
    public async Task<ActionResult<Result<string>>> IgnoreFriendRequest([FromQuery] string requestId)
    {
        var result = await userRepository.IgnoreFriendRequestAsync(requestId);
        if (result == "Friend request ignored") return Ok(Result<string>.SuccessResult("", result));

        return BadRequest(Result<string>.FailureResult(result));
    }

    [HttpDelete("friends/request")]
    public async Task<ActionResult<Result<string>>> CancelFriendRequest([FromQuery] string requestId)
    {
        var result = await userRepository.RemoveFriendRequestAsync(requestId);
        if (result == "Friend request removed") return Ok(Result<string>.SuccessResult("", result));

        return BadRequest(Result<string>.FailureResult(result));
    }

    [HttpPost("friends")]
    public async Task<ActionResult<Result<string>>> AcceptFriendRequest([FromQuery] string requestId)
    {
        var result = await userRepository.AcceptFriendRequestAsync(requestId);
        if (result == "Friend request accepted") return Ok(Result<string>.SuccessResult("", result));

        return BadRequest(Result<string>.FailureResult(result));
    }

    [HttpDelete("friends")]
    public async Task<ActionResult<Result<string>>> RemoveFriend([FromQuery] string userId, string targetId)
    {
        var result = await userRepository.RemoveFriendAsync(userId, targetId);
        if (result == "Friend removed") return Ok(Result<string>.SuccessResult("", result));

        return BadRequest(Result<string>.FailureResult(result));
    }
}