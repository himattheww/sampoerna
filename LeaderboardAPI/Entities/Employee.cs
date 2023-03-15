using System;
using System.Collections.Generic;

#nullable disable

namespace LeaderboardAPI.Entities
{
    public partial class Employee
    {
        public string EmployeeCode { get; set; }
        public string RoleType { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeEmail { get; set; }
        public string EmployeePhone { get; set; }
        public string EmployeeArea { get; set; }
        public string EmployeePassword { get; set; }
        public ulong FirstLogin { get; set; }
        public ulong PasswordIssued { get; set; }
        public string ChangeToken { get; set; }
        public DateTime? TokenExpired { get; set; }
    }
}
