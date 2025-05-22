using Marquise_Web.Model.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Marquise_Web.Model.DTOs.CRM
{
    public class AccountDto
    {
        public string AccountId { get; set; }
        public string UserId { get; set; }
        public string CrmAccountId { get; set; }
        public string Name { get; set; }
    }
}
