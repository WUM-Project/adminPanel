using global::Interfaces.Models;
using global::Interfaces;


namespace Admin_Panel
{
 

    public class UserServiceClient : IUserService
    {
        private readonly HttpClient _httpClient;

        public UserServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<bool> CheckCredentialsAsync(string login, string password)
        {
            try
            {

                var response = await _httpClient.GetAsync($"api/user/credentials?login={login}&password={password}");
                response.EnsureSuccessStatusCode();


                var isValidCredentials = await response.Content.ReadFromJsonAsync<bool>();
                return isValidCredentials;
            }
            catch (HttpRequestException)
            {

                return false;
            }
        }



        public async Task<UserDTO> GetLoginedUser(string login, string password)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/user/logined?login={login}&password={password}");
                response.EnsureSuccessStatusCode();

                var userDTO = await response.Content.ReadFromJsonAsync<UserDTO>();
                return userDTO;
            }
            catch (HttpRequestException)
            {

                throw;
            }
        }
    }
}
