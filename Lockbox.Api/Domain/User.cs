﻿using System;
using System.Collections.Generic;
using System.Linq;
using Lockbox.Api.Extensions;

namespace Lockbox.Api.Domain
{
    public class User
    {
        private ISet<string> _apiKeys = new HashSet<string>();
        private ISet<Permission> _permissions = new HashSet<Permission>();

        public string Username { get; protected set; }
        public string Password { get; protected set; }
        public string Salt { get; protected set; }
        public Role Role { get; protected set; }

        public IEnumerable<Permission> Permissions
        {
            get { return _permissions; }
            set { _permissions = new HashSet<Permission>(value); }
        }

        public bool IsActive { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
        public DateTime UpdatedAt { get; protected set; }

        public IEnumerable<string> ApiKeys
        {
            get { return _apiKeys; }
            set { _apiKeys = new HashSet<string>(value); }
        }

        protected User()
        {
        }

        public User(string username, Role role)
        {
            if (username.Empty())
                throw new ArgumentException("Username can not be empty.", nameof(username));

            Username = username;
            Role = role;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Activate()
        {
            if (IsActive)
                return;

            IsActive = true;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Lock()
        {
            if (!IsActive)
                return;

            IsActive = false;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetPassword(string password, IEncrypter encrypter)
        {
            if (password.Empty())
                throw new ArgumentException("Password can not be empty.", nameof(password));

            var salt = encrypter.GetSalt(password);
            var hash = encrypter.GetHash(password, salt);

            Password = hash;
            Salt = salt;
            UpdatedAt = DateTime.UtcNow;
        }

        public bool ValidatePassword(string password, IEncrypter encrypter)
        {
            var hashedPassword = encrypter.GetHash(password, Salt);
            var areEqual = Password.SequenceEqual(hashedPassword);

            return areEqual;
        }

        public void AddApiKey(string apiKey)
        {
            if (apiKey.Empty())
                throw new ArgumentException("API key can not be empty.", nameof(apiKey));
            if (ApiKeys.Contains(apiKey))
                return;

            _apiKeys.Add(apiKey);
            UpdatedAt = DateTime.UtcNow;
        }

        public void DeleteApiKey(string apiKey)
        {
            if (apiKey.Empty())
                throw new ArgumentException("API key can not be empty.", nameof(apiKey));
            if (!ApiKeys.Contains(apiKey))
                return;


            _apiKeys.Remove(apiKey);
            UpdatedAt = DateTime.UtcNow;
        }

        public void AddPermission(Permission permission)
        {
            if (Permissions.Contains(permission))
                return;

            _permissions.Add(permission);
            UpdatedAt = DateTime.UtcNow;
        }

        public void DeletePermission(Permission permission)
        {
            if (!Permissions.Contains(permission))
                return;

            _permissions.Add(permission);
            UpdatedAt = DateTime.UtcNow;
        }
    }
}