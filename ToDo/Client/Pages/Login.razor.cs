using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using AntDesign;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using ToDo.Shared;

namespace ToDo.Client.Pages
{
    public partial class Login
    {
        private bool isLoading;

        private LoginDto model = new();
        [Inject] public HttpClient Http { get; set; }
        [Inject] public MessageService MsgSvr { get; set; }
        [Inject] public AuthenticationStateProvider AuthProvider { get; set; }

        protected override void OnInitialized()
        {
            model.UserName = "Admin";
            model.Password = "Admin";
            base.OnInitialized();
            this.OnLogin();
        }


        private async void OnLogin()
        {
            isLoading = true;

            var httpResponse = await Http.PostAsJsonAsync("api/Auth/Login", model);
            var result = await httpResponse.Content.ReadFromJsonAsync<UserDto>();

            if (string.IsNullOrWhiteSpace(result?.Token) == false)
            {
                await MsgSvr.Success("登录成功");
                Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.Token);
                ((AuthProvider) AuthProvider).MarkUserAsAuthenticated(result);
            }
            else
            {
                await MsgSvr.Error("用户名或密码错误");
            }

            isLoading = false;
            await InvokeAsync(StateHasChanged);
        }
    }
}