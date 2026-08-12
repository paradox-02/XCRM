using System;
using System.Collections.Generic;
using System.Text;

namespace XCRM.Domain.Entities
{
    public class SysUser
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long Id { get; private set; }


        /// <summary>
        /// 登录用户名
        /// </summary>
        public string Username { get; private set; } = string.Empty;


        /// <summary>
        /// 密码Hash
        /// </summary>
        public string PasswordHash { get; private set; } = string.Empty;


        /// <summary>
        /// 邮箱
        /// </summary>
        public string? Email { get; private set; }


        /// <summary>
        /// 手机号
        /// </summary>
        public string? Phone { get; private set; }


        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsActive { get; private set; }


        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; private set; }


        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UpdateTime { get; private set; }

        private SysUser()
        {

        }

        public SysUser(string username, string passwordHash)
        {
            Username = username;
            PasswordHash = passwordHash;
            IsActive = true;
            CreateTime = DateTime.UtcNow;
        }

        public void ChangePassword(string passwordHash)
        {
            PasswordHash = passwordHash;
            UpdateTime = DateTime.UtcNow;
        }

        public void Disable()
        {
            IsActive = false;
            UpdateTime = DateTime.UtcNow;
        }

        public void Enable()
        {
            IsActive = true;
            UpdateTime = DateTime.UtcNow;
        }

        public void UpdateProfile(string? email, string? phone)
        {
            Email = string.IsNullOrWhiteSpace(email) ? null : email.Trim();

            Phone = string.IsNullOrWhiteSpace(phone) ? null : phone.Trim();

            UpdateTime = DateTime.UtcNow;
        }
    }
}
