﻿using System;
using System.Collections.Generic;
using Lockbox.Api.Requests;
using Lockbox.Api.Services;
using Nancy;
using Nancy.Security;

namespace Lockbox.Api.Modules
{
    public class UserModule : ModuleBase
    {
        public UserModule(IUserService userService) : base("users")
        {
            this.RequiresAuthentication();

            Get("", async args =>
            {
                RequiresAdmin();
                var usernames = await userService.GetUsernamesAsync();

                return usernames ?? new List<string>();
            });

            Get("{username}", async args =>
            {
                var username = (string) args.username;
                if (!username.Equals(CurrentUsername, StringComparison.CurrentCultureIgnoreCase))
                    RequiresAdmin();

                var user = await userService.GetAsync(username);
                if (user == null)
                    return HttpStatusCode.NotFound;

                return new
                {
                    username = user.Username,
                    createdAt = user.CreatedAt.ToString("g"),
                    updatedAt = user.UpdatedAt.ToString("g"),
                    role = user.Role.ToString().ToLowerInvariant(),
                    isActive = user.IsActive,
                    apiKeys = user.ApiKeys
                };
            });

            Post("", async args =>
            {
                RequiresAdmin();
                var request = BindRequest<CreateUser>();
                await userService.CreateAsync(request.Username, request.Password, request.Role);

                return Created($"users/{request.Username}");
            });

            Delete("{username}", async args =>
            {
                RequiresAdmin();
                await userService.DeleteAsync((string) args.username);

                return HttpStatusCode.NoContent;
            });
        }
    }
}