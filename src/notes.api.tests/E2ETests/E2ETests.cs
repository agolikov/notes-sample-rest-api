using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using notes.api.tests.Common;
using notes.api.tests.HelperTests;
using notes.application.Models;
using notes.application.Models.Common;
using notes.application.Models.Note;
using notes.application.Models.User;
using NUnit.Framework;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using notes.api.Controllers;

namespace notes.api.tests.E2ETests
{
    [TestFixture(Category = "EndToEnd")]
    public class E2ETests : TestsBase
    {
        private TestServer _testServer;
        private HttpClient _client;
        protected const string TestEnvironment = "development";

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var startupAssembly = typeof(AuthenticationController).GetTypeInfo().Assembly;

            var contentRoot = GetProjectPath(startupAssembly);

            _testServer = new TestServer((WebHost.CreateDefaultBuilder(null)
                .UseContentRoot(contentRoot)
                .UseEnvironment(TestEnvironment)));

            _client = _testServer.CreateClient();

            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private async Task<HttpResponseMessage> SignUp(SignUpModel signUpModel)
        {
            var response = await _client.PostAsync(@"auth/signup", signUpModel.ToJsonContent());
            response.EnsureSuccessStatusCode();
            return response;
        }
        
        private async Task<HttpResponseMessage> SignIn(SignInModel signUpModel)
        {
            var response = await _client.PostAsync(@"auth/signin", signUpModel.ToJsonContent());
            response.EnsureSuccessStatusCode();
            return response;
        }

        [Test]
        public async Task Test_SignUp()
        {
            string email = "test2@email.com";
            var signUpModel = new SignUpModel
            {
                Email = email,
                Password = "Password123!"
            };
            var response = await SignUp(signUpModel);

            var responseModel = response.ToObject<UserModel>();
            Assert.AreEqual(responseModel.Email, signUpModel.Email);
            Assert.AreEqual(responseModel.IsBlocked, false);
        }

        [Test]
        public async Task Test_SignIn()
        {
            string email = "test1@email.com";
            var signUpModel = new SignUpModel
            {
                Email = email,
                Password = "Password123!"
            };
            await SignUp(signUpModel);
            var response = await SignIn(new SignInModel
            {
                Email = signUpModel.Email,
                Password = signUpModel.Password
            });

            var responseModel = response.ToObject<LogInResult>();

            Assert.AreEqual(responseModel.User.Email, signUpModel.Email);
            Assert.IsNotEmpty(responseModel.Token);
        }


        [Test]
        public async Task Test_CreateNote()
        {
            string email = "test56@email.com";
            var signUpModel = new SignUpModel
            {
                Email = email,
                Password = "Password123!"
            };
            await SignUp(signUpModel);
            var response = await SignIn(new SignInModel
            {
                Email = signUpModel.Email,
                Password = signUpModel.Password
            });

            var responseModel = response.ToObject<LogInResult>();

            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + responseModel.Token);

            var model = new CreateNoteModel
            {
                Tags = new[] { "tag1", "tag2" },
                Text = "Text",
                Title = "Title"
            };

            var postNoteResponse = await _client.PostAsync($"notes", model.ToJsonContent());
            postNoteResponse.EnsureSuccessStatusCode();

            var responseNoteModel = postNoteResponse.ToObject<NoteModel>();

            //Assert
            var getNoteResponse = await _client.GetAsync($"notes");

            getNoteResponse.EnsureSuccessStatusCode();

            var responseGetNoteModels = getNoteResponse.ToObject<PaginatedResult<NoteModel>>();

            Assert.AreEqual(responseGetNoteModels.TotalCount, 1);

            var responseGetNoteModel = responseGetNoteModels.Items.First();
            Assert.AreEqual(model.Title, responseNoteModel.Title);
            Assert.AreEqual(model.Text, responseNoteModel.Text);
            CollectionAssert.AreEquivalent(model.Tags, responseNoteModel.Tags);
            Assert.AreEqual(responseGetNoteModel.Title, responseNoteModel.Title);
            Assert.AreEqual(responseGetNoteModel.Text, responseNoteModel.Text);
            CollectionAssert.AreEquivalent(responseGetNoteModel.Tags, responseNoteModel.Tags);
        }
    }
}
