using AdvertApi.Models;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;


namespace WebAdvert.Web.ServiceClient
{
    public class AdvertApiClient : IAdvertApiClient
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public AdvertApiClient(IConfiguration configuration,HttpClient client,IMapper mapper)
        {
            _configuration = configuration;
            _client = client;
            _mapper = mapper;

            var createUrl = _configuration.GetSection(key: "AdvertApi").GetValue<string>("CreateUrl");
            _client.BaseAddress = new Uri(createUrl);
            _client.DefaultRequestHeaders.Add("Content-type", "application/json");
        }

        public async Task<bool> Confirm(ConfirmAdvertRequest model)
        {
            var advertModel = _mapper.Map<ConfirmAdvertModel>(model);
            var jsonModel = JsonConvert.SerializeObject(advertModel);

            var response = await _client.PutAsync(new Uri($"{_client.BaseAddress}/confirm"), new StringContent(jsonModel));

            return response.StatusCode == HttpStatusCode.OK;


        }

        public async Task<AdvertResponse> Create(CreateAdvertModel model)
        {
            var advertApiModel = _mapper.Map<AdvertModel>(model);

            var jsonModel = JsonConvert.SerializeObject(advertApiModel);
            var response = await _client.PostAsync(new Uri($"{_client.BaseAddress}/create"), new StringContent(jsonModel));
            var responseJson = await response.Content.ReadAsStringAsync();
            var createAdvertResponse = JsonConvert.DeserializeObject<CreateAdvertResponse>(responseJson);
            var advertResponse = _mapper.Map<AdvertResponse>(createAdvertResponse);

            return advertResponse;
        }
    }
}
