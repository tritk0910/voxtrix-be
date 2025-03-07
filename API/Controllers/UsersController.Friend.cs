using Application.Core;
using Application.DTOs.Users;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public partial class UsersController
{
    /// <summary>
    /// Retrieves a paginated list of friends for a given user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="defaultParams">Pagination parameters.</param>
    /// <returns>A paginated list of friends.</returns>
    [HttpGet("friends")]
    public async Task<ActionResult<Result<PagedResult<UserBasicDto>>>> GetFriendsAsync([FromQuery] string userId, [FromQuery] DefaultParams defaultParams)
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

    /// <summary>
    /// Retrieves a paginated list of pending friend requests for a given user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="defaultParams">Pagination parameters.</param>
    /// <returns>A paginated list of pending friend requests.</returns>
    [HttpGet("friends/pending")]
    public async Task<ActionResult<Result<PagedResult<FriendDto>>>> GetPendingFriendRequestsAsync([FromQuery] string userId, [FromQuery] DefaultParams defaultParams)
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

    /// <summary>
    /// Retrieves a paginated list of incoming friend requests for a given user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="defaultParams">Pagination parameters.</param>
    /// <returns>A paginated list of incoming friend requests.</returns>
    [HttpGet("friends/incoming")]
    public async Task<ActionResult<Result<PagedResult<FriendDto>>>> GetIncomingFriendRequestsAsync([FromQuery] string userId, [FromQuery] DefaultParams defaultParams)
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

    /// <summary>
    /// Sends a friend request to a target user.
    /// </summary>
    /// <param name="userId">The ID of the user sending the request.</param>
    /// <param name="targetUsername">The username of the target user.</param>
    /// <returns>The result of the friend request operation.</returns>
    [HttpPost("friends/request")]
    public async Task<ActionResult<Result<FriendResponseDto>>> SendFriendRequest([FromQuery] string userId, string targetUsername)
    {
        var result = await userRepository.SendFriendRequestAsync(userId, targetUsername);
        if (!result.Success) return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Ignores a friend request.
    /// </summary>
    /// <param name="requestId">The ID of the friend request to ignore.</param>
    /// <returns>The result of the ignore operation.</returns>
    [HttpPut("friends/request")]
    public async Task<ActionResult<Result<FriendResponseDto>>> IgnoreFriendRequest([FromQuery] string requestId)
    {
        var result = await userRepository.IgnoreFriendRequestAsync(requestId);
        if (!result.Success) return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Cancels a friend request.
    /// </summary>
    /// <param name="requestId">The ID of the friend request to cancel.</param>
    /// <returns>The result of the cancel operation.</returns>
    [HttpDelete("friends/request")]
    public async Task<ActionResult<Result<bool>>> CancelFriendRequest([FromQuery] string requestId)
    {
        var result = await userRepository.RemoveFriendRequestAsync(requestId);
        if (!result.Success) return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Accepts a friend request.
    /// </summary>
    /// <param name="requestId">The ID of the friend request to accept.</param>
    /// <returns>The result of the accept operation.</returns>
    [HttpPost("friends")]
    public async Task<ActionResult<Result<FriendResponseDto>>> AcceptFriendRequest([FromQuery] string requestId)
    {
        var result = await userRepository.AcceptFriendRequestAsync(requestId);
        if (!result.Success) return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Removes a friend.
    /// </summary>
    /// <param name="userId">The ID of the user removing the friend.</param>
    /// <param name="targetId">The ID of the friend to remove.</param>
    /// <returns>The result of the remove operation.</returns>
    [HttpDelete("friends")]
    public async Task<ActionResult<Result<bool>>> RemoveFriend([FromQuery] string userId, string targetId)
    {
        var result = await userRepository.RemoveFriendAsync(userId, targetId);
        if (!result.Success) return BadRequest(result);

        return Ok(result);
    }
}