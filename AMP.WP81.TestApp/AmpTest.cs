using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Threading.Tasks;
using Anfema.Amp;

namespace AMP.WP81.TestApp
{
    [TestClass]
    public class AmpTest
    {
        [TestMethod]
        public async Task TestAmp()
        {
            var loginSucceeded = await Amp.Instance.LoginAsync("admin@anfe.ma", "test");
            Assert.AreEqual(true, loginSucceeded);

            await Amp.Instance.LoadDataAsync();

            var translations = Amp.Instance.GetPageTranslations();
            Assert.AreEqual(0, translations.Count);
        }
    }
}
