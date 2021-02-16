using notes.application.Extensions;
using NUnit.Framework;

namespace notes.application.tests.ExtensionTests
{
    [TestFixture(Category = "Unit")]
    public class DataExtensionsTests
    {
        [Test]
        public void DataExtensions_CreatePassword_Passwords_Match()
        {
            string password1 = "password1";
            string password2 = "password1";

            DataExtensions.CreatePasswordHash(password1, out byte[] hash, out byte[] salt);
            bool result = DataExtensions.IsPasswordCorrect(password2, hash, salt);

            Assert.IsTrue(result);
        }

        [Test]
        public void DataExtensions_CreatePassword_Passwords_Dont_Natch()
        {
            string password1 = "password1";
            string password2 = "password2";

            DataExtensions.CreatePasswordHash(password1, out byte[] hash, out byte[] salt);
            bool result = DataExtensions.IsPasswordCorrect(password2, hash, salt);

            Assert.IsFalse(result);
        }
    }
}
