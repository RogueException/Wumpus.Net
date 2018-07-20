﻿#pragma warning disable CS1998

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wumpus.Entities;
using Wumpus.Requests;

namespace Wumpus.Server.Controllers
{
    public class UserController : ControllerBase
    {
        [HttpGet("users/@me")]
        public async Task<IActionResult> GetCurrentUserAsync()
        {
            return Ok(new User());
        }
        [HttpGet("users/{userId}")]
        public async Task<IActionResult> GetUserAsync(Snowflake userId)
        {
            return Ok(new User
            {
                Id = userId
            });
        }
        [HttpPatch("users/@me")]
        public async Task<IActionResult> ModifyCurrentUserAsync([FromBody] ModifyCurrentUserParams args)
        {
            args.Validate();

            var user = new User();
            if (args.Avatar.IsSpecified)
                user.Avatar = args.Avatar;
            if (args.Username.IsSpecified)
                user.Username = args.Username;

            return Ok(user);
        }

        [HttpGet("users/@me/guilds")]
        public async Task<IActionResult> GetCurrentUserGuildsAsync([FromQuery] Dictionary<string, string> queryMap)
        {
            var args = new GetCurrentUserGuildsParams();
            args.LoadQueryMap(queryMap);
            args.Validate();

            var guild = new UserGuild();
            if (args.Before.IsSpecified)
                guild.Id = args.Before.Value;
            if (args.After.IsSpecified)
                guild.Id = args.After.Value;

            return Ok(new[] { guild });
        }
        [HttpDelete("users/@me/guilds/{guildId}")]
        public async Task<IActionResult> LeaveGuildAsync(Snowflake guildId)
        {
            return NoContent();
        }

        [HttpGet("users/@me/channels")]
        public async Task<IActionResult> GetPrivateChannelsAsync()
        {
            return Ok(new[] { new Channel() });
        }
        [HttpPost("users/@me/channels")]
        public async Task<IActionResult> CreateDMChannelAsync([FromBody] CreateDMChannelParams args)
        {
            args.Validate();

            if (args.RecipientId.IsSpecified)
            {
                return Ok(new Channel
                {
                    Type = ChannelType.Dm,
                    Recipients = new User[] { new User
                    {
                        Id = args.RecipientId.Value
                    }}
                });
            }
            else
            {
                return Ok(new Channel
                {
                    Type = ChannelType.GroupDm
                });
            }
        }

        [HttpGet("users/@me/connections")]
        public async Task<IActionResult> GetUserConnectionsAsync()
        {
            return Ok(new[] { new Connection() });
        }
    }
}
