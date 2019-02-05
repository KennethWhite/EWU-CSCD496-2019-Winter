using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Api.Models;

namespace SecretSanta.Api.Tests
{
    [TestClass]
    public class AssemblyTestConfig
    {
        [AssemblyInitialize]
        public static void ConfigureAutoMapper(TestContext context)
        {
            Mapper.Initialize(cfg => cfg.AddProfile(new AutoMapperProfileConfiguration()));
        }
    }
}