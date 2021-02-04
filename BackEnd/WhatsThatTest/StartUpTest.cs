using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Moq;
using System;
using System.Collections.Generic;
using WhatsThatSong;
using Xunit;

namespace WhatsThatTest
{
    public class StartUpTest
    {
        [Fact]
        public void StartupTest()
        {
            var serviceProvider = new Mock<IServiceProvider>();
            var ap = new ApplicationBuilder(serviceProvider.Object).New();
            var webHost = Microsoft.AspNetCore.WebHost.CreateDefaultBuilder().UseStartup<Startup>().Build();
            Assert.NotNull(webHost);
        }
    }
}
