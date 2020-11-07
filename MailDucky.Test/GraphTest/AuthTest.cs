using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MailDucky.Test.GraphTest
{
    class AuthTest : MailDuckyBaseTest
    {
        [Test]
        public async Task AuthenticateDiego()
        {
            var graphService = new GraphConnector.GraphAuthenticationService(config);

            var user = await graphService.GetUser("DiegoS@devbrett.onmicrosoft.com");
            Assert.AreEqual(user.DisplayName, "Diego Siciliani");
        }
    }
}
