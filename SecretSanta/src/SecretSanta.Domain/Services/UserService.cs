using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SecretSanta.Domain.Models;

namespace SecretSanta.Domain.Services
{
    //Need to be able to add and update a user, not worried about deleting
    public class UserService
    {
        private ApplicationDbContext DbContext { get; }
    }
}
