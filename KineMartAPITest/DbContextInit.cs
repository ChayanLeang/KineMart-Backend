using KineMartAPI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KineMartAPITest
{
    public static class DbContextInit
    {
        public static DbContextOptions<MartDbContext> DbContextOptions()
        {
            return new DbContextOptionsBuilder<MartDbContext>()
                       .UseInMemoryDatabase(databaseName:"KineMartDbTest")
                       .Options;
        }
    }
}
