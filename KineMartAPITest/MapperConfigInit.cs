using AutoMapper;
using KineMartAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KineMartAPITest
{
    public class MapperConfigInit
    {
        public static IMapper GetMapper()
        {
            return new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MapperConfig());
            }).CreateMapper();
        }
    }
}
