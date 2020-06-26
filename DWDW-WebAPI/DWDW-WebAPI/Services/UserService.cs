﻿using DWDW_WebAPI.Models;
using DWDW_WebAPI.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Transactions;
using System.Threading.Tasks;

namespace DWDW_WebAPI.Services
{
    public interface IUserService : IDisposable
    {
        List<UserViewModel> GetUsers();
        User GetUserById(int userId);
        bool InsertUser(User user);
        bool UpdateUser(User user);
        bool DeactiveUser(User user);
        void Save();
        bool UserExists(int userId);
        Task<UserViewModel> LoginAsync(string username, string password);

    }
       
    public class UserService : IUserService, IDisposable
    {
        private DWDBContext context;
        private bool disposed = false;
        public UserService()
        {
            context = new DWDBContext();
        }
        public bool DeactiveUser(User user)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    user.isActive = false;
                    context.Entry(user).State = EntityState.Modified;
                    context.SaveChanges();
                    scope.Complete();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public User GetUserById(int userId)
        {
            return context.Users.Find(userId);
        }

        public List<UserViewModel> GetUsers()
        {
            var list = context.Users
                .Select(u => new UserViewModel()
                {
                    userId = u.userId,
                    userName = u.userName,
                    password = u.password,
                    phone = u.phone,
                    dateOfBirth = u.dateOfBirth,
                    gender = u.gender,
                    deviceToken = u.deviceToken,
                    roleId = u.roleId,
                    isActive = u.isActive
                }).ToList();
            return list;
        }

        public bool InsertUser(User user)
        {
            if (context.Roles.Find(user.roleId) == null) return false;
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    context.Users.Add(user);
                    context.SaveChanges();
                    scope.Complete();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public bool UpdateUser(User user)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    context.Entry(user).State = EntityState.Modified;
                    context.SaveChanges();
                    scope.Complete();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        public bool UserExists(int userId)
        {
            return context.Users.Count(e => e.userId == userId) > 0;
        }

        public async Task<UserViewModel> LoginAsync(string username, string password)
        {
            //check validate fields

            UserViewModel result = null;
            //get User by username password
            var user = await context.Users.FirstOrDefaultAsync(x => x.userName.Equals(username)
            && x.password.Equals(password));

            if(user != null)
            {
                result = new UserViewModel
                {
                    userId = user.userId,
                    userName = user.userName,
                    roleId = user.roleId,
                };
            }
            return result;
        }
    }
}