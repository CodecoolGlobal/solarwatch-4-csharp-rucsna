using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using SolarWatch.Contracts;
using SolarWatch.Service.Authentication;

namespace SolarWatch.IntegrationTest;

public class AuthenticationTest : IDisposable
{
    private readonly SolarWatchFactory _factory;
    private readonly HttpClient _client;

    public AuthenticationTest()
    {
        _factory = new SolarWatchFactory();
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task Test_LoginAndRegistration()
    {
        // Login Attempt
        var loginRequest = new AuthRequest("user5@gmail.com", "password");
        var loginResponse = await _client.PostAsync("api/Auth/Login",
            new StringContent(JsonConvert.SerializeObject(loginRequest),
                Encoding.UTF8, "application/json"));

        var authResponse = JsonConvert.DeserializeObject<AuthResponse>(await loginResponse.Content.ReadAsStringAsync());

        // Assert Login
        if (loginResponse.StatusCode == HttpStatusCode.OK)
        {
            Assert.NotNull(authResponse.Token);
            Assert.Equal("user5@gmail.com", authResponse.Email);
            Assert.Equal("user5", authResponse.UserName);
        }
        else
        {
            // Registration Attempt
            var registrationRequest = new RegistrationRequest("user2@email.com", "user2", "password2");
            var registrationResponse = await _client.PostAsync("api/Auth/Register",
                new StringContent(JsonConvert.SerializeObject(registrationRequest),
                    Encoding.UTF8, "application/json"));

            // Assert Registration
            Assert.Equal(HttpStatusCode.BadRequest, registrationResponse.StatusCode);
        }
    }
    
    [Fact]
    public async Task Test_NoAuthorization()
    {
        // Step 1: Authenticate user and obtain a token
        var loginRequest = new AuthRequest("user5@gmail.com", "password");

        var loginResponse = await _client.PostAsync("api/Auth/Login",
            new StringContent(JsonConvert.SerializeObject(loginRequest),
                Encoding.UTF8, "application/json"));
        var authResponse = JsonConvert.DeserializeObject<AuthResponse>(await loginResponse.Content.ReadAsStringAsync());
        Assert.NotNull(authResponse.Token);

        // Step 2: Reset authorization header to null (no authorization)
        _client.DefaultRequestHeaders.Authorization = null;

        // Step 3: Attempt to access the protected endpoint without proper authorization
        var solarWatchResponse = await _client.GetAsync("api/SolarWatch/GetSunrise_Sunset");

        // Step 4: Assert that the response is Forbidden
        Assert.Equal(HttpStatusCode.Unauthorized, solarWatchResponse.StatusCode);
    }
    

    public void Dispose()
    {
        _factory.Dispose();
        _client.Dispose();
    }
}