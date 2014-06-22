using Moq;
using Nancy;
using Nancy.Testing;
using NUnit.Framework;
using PayMoreApi.Models;
using PayMoreApi.Modules;

namespace PayMoreApi.Specs
{
    [TestFixture]
    public class PayMoreIntegrationTests
    {

        [Test]
        public void Status_should_be_ok()
        {
            var browser = new Browser(with => with.Module<StatusModule>());

            var response = browser.Get("paymore/status");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Status endpoint is not happy");
        }
        [Test]
        public void Should_be_able_to_add_a_valid_card()
        {                       
            var browser = new Browser(with => with.Module<PayMoreModule>()
                                               .Dependency(new Card(Mock.Of<IStoreCards>())));
            var response = browser.Post("paymore/card",
                                      with =>
                                      {
                                          with.HttpRequest();
                                          with.FormValue("cardtype", "PayMore");
                                          with.FormValue("cardnumber", "4543454345434543");
                                          with.FormValue("cardholdername", "MR DAN SWAIN");
                                          with.FormValue("startdate", "1/14");
                                          with.FormValue("expirydate", "1/15");
                                          with.FormValue("CVV", "123");
                                          with.FormValue("issuenumber", "1");
                                      }
                );

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        } 
    }
}