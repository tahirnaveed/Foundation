using EPiServer;
using EPiServer.Core;
using EPiServer.Tracking.Core;
using EPiServer.Web;
using Foundation.Commerce.Customer.ViewModels;
using Mediachase.Commerce.Orders.Managers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Foundation.Demo.ProfileStore
{
    public class ProfileStoreService : IProfileStoreService
    {
        private readonly HttpClient _client = new HttpClient();
        private readonly IContentLoader _contentLoader;
        private readonly ITrackingService _trackingService;
        private readonly ISiteDefinitionResolver _siteDefinitionResolver;

        private readonly string apiBaseUrl = ConfigurationManager.AppSettings["ProfileStore.Url"];
        private readonly string subscriptionKey = ConfigurationManager.AppSettings["ProfileStore.SubscriptionKey"];
        private readonly string profilesUrl = "api/v1.0/Profiles";
        private readonly string scopesUrl = "api/v1.0/Scopes";
        private readonly string blacklistsUrl = "api/v1.0/Blacklists";
        private readonly string segmentsUrl = "api/v1.0/Segments";
        private readonly string trackEventsUrl = "api/v1.0/TrackEvents";

        public ProfileStoreService(
            ITrackingService trackingService,
            ISiteDefinitionResolver siteDefinitionResolver,
            IContentLoader contentLoader)
        {
            _trackingService = trackingService;
            _siteDefinitionResolver = siteDefinitionResolver;
            _contentLoader = contentLoader;
        }

        #region Profile Store Api

        async Task<ProfileStoreItems> IProfileStoreService.GetAllProfilesAsync(ProfileStoreFilterOptions profileStoreFilterOptions)
        {
            HttpResponseMessage response = null;
            var profileStoreItems = new ProfileStoreItems();

            if (profileStoreFilterOptions == null)
            {
                response = await RequestAsync(HttpMethod.Get, $"{apiBaseUrl}/{profilesUrl}/");
            }
            else
            {
                response = await RequestAsync(HttpMethod.Get, $"{apiBaseUrl}/{profilesUrl}/{CreateFilterOptionsUrl(profileStoreFilterOptions)}");
            }

            var getProfiles = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(getProfiles))
            {
                profileStoreItems = new ProfileStoreItems()
                {
                    ProfileStoreList = new List<ProfileStoreModel>(),
                    Total = 0,
                    Count = 0
                };
            }
            else
            {
                profileStoreItems = JsonConvert.DeserializeObject<ProfileStoreItems>(getProfiles);
            }

            return profileStoreItems;
        }

        async Task<ProfileStoreItems> IProfileStoreService.GetProfilesAsync(string queryString)
        {
            HttpResponseMessage response;
            var profileStoreItems = new ProfileStoreItems();

            var uri = $"{apiBaseUrl}/{profilesUrl}/{queryString}";
            response = await RequestAsync(HttpMethod.Get, uri);
            var getProfileStoreItems = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrEmpty(getProfileStoreItems))
            {
                profileStoreItems = new ProfileStoreItems()
                {
                    ProfileStoreList = new List<ProfileStoreModel>(),
                    Total = 0,
                    Count = 0
                };
            }
            else
            {
                profileStoreItems = JsonConvert.DeserializeObject<ProfileStoreItems>(getProfileStoreItems);
            }

            return profileStoreItems;
        }

        async Task<ProfileStoreModel> IProfileStoreService.GetProfileByIdAsync(string scope, Guid profileId)
        {
            var profileStoreModel = new ProfileStoreModel();
            var response = await RequestAsync(HttpMethod.Get, $"{apiBaseUrl}/{profilesUrl}/{scope}/{profileId.ToString()}");
            var getProfile = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(getProfile))
            {
                profileStoreModel = new ProfileStoreModel();
            }
            else
            {
                profileStoreModel = MapToProfileStore(getProfile);
            }

            return profileStoreModel;
        }

        async Task IProfileStoreService.EditOrCreateProfileAsync(string scope, ProfileStoreModel model)
        {
            var profileId = model.ProfileId == null ? Guid.NewGuid().ToString() : model.ProfileId.ToString();
            await RequestAsync(HttpMethod.Put, $"{apiBaseUrl}/{profilesUrl}/{profileId}", JsonConvert.SerializeObject(model));
        }

        #endregion

        #region Scopes Api

        async Task<ScopeItems> IProfileStoreService.GetAllScopesAsync()
        {
            var response = await RequestAsync(HttpMethod.Get, $"{apiBaseUrl}/{scopesUrl}");
            var getScopes = await response.Content.ReadAsStringAsync();
            var scopeItems = new ScopeItems();

            if (string.IsNullOrEmpty(getScopes))
            {
                scopeItems = new ScopeItems()
                {
                    ScopeList = new List<ScopeModel>(),
                    Total = 0,
                    Count = 0
                };
            }
            else
            {
                scopeItems = JsonConvert.DeserializeObject<ScopeItems>(getScopes);
            }

            return scopeItems;
        }

        async Task<ScopeItems> IProfileStoreService.GetScopesByIdAsync(Guid scopeId)
        {
            var response = await RequestAsync(HttpMethod.Get, $"{apiBaseUrl}/{scopesUrl}/{scopeId.ToString()}");
            var getScope = await response.Content.ReadAsStringAsync();
            var scopeItems = new ScopeItems();

            if (string.IsNullOrEmpty(getScope))
            {
                scopeItems = new ScopeItems()
                {
                    ScopeList = new List<ScopeModel>(),
                    Total = 0,
                    Count = 0
                };
            }
            else
            {
                scopeItems = JsonConvert.DeserializeObject<ScopeItems>(getScope);
            }

            return scopeItems;
        }

        #endregion

        #region Segment Api

        async Task<SegmentItems> IProfileStoreService.GetAllSegmentsAsync()
        {
            var response = await RequestAsync(HttpMethod.Get, $"{apiBaseUrl}/{segmentsUrl}/?$top=10000");
            var getSegments = await response.Content.ReadAsStringAsync();
            var segmentItems = new SegmentItems();

            if (string.IsNullOrEmpty(getSegments))
            {
                segmentItems = new SegmentItems()
                {
                    SegmentList = new List<SegmentModel>(),
                    Total = 0,
                    Count = 0
                };
            }
            else
            {
                segmentItems = JsonConvert.DeserializeObject<SegmentItems>(getSegments);
            }

            return segmentItems;
        }

        async Task<SegmentItems> IProfileStoreService.GetSegmentByIdAsync(Guid segmentId)
        {
            var response = await RequestAsync(HttpMethod.Get, $"{apiBaseUrl}/{segmentsUrl}/{segmentId}");
            var getSegment = await response.Content.ReadAsStringAsync();
            var segmentItems = new SegmentItems();

            if (string.IsNullOrEmpty(getSegment))
            {
                segmentItems = new SegmentItems()
                {
                    SegmentList = new List<SegmentModel>(),
                    Total = 0,
                    Count = 0
                };
            }
            else
            {
                segmentItems = JsonConvert.DeserializeObject<SegmentItems>(getSegment);
            }

            return segmentItems;
        }

        async Task IProfileStoreService.EditOrCreateSegmentAsync(SegmentModel model)
        {
            var segmentId = model.SegmentId == null ? Guid.NewGuid().ToString() : model.SegmentId.ToString();
            await RequestAsync(HttpMethod.Put, $"{apiBaseUrl}/{segmentsUrl}/{segmentId}", JsonConvert.SerializeObject(model));
        }
        #endregion

        #region Blacklist Api

        async Task<BlacklistItems> IProfileStoreService.GetAllBlacklistAsync()
        {
            var response = await RequestAsync(HttpMethod.Get, $"{apiBaseUrl}/{blacklistsUrl}");
            var getBlacklists = await response.Content.ReadAsStringAsync();
            var blacklistItems = new BlacklistItems();

            if (string.IsNullOrEmpty(getBlacklists))
            {
                blacklistItems = new BlacklistItems()
                {
                    BlacklistList = new List<BlacklistModel>(),
                    Total = 0,
                    Count = 0
                };
            }
            else
            {
                blacklistItems = JsonConvert.DeserializeObject<BlacklistItems>(getBlacklists);
            }

            return blacklistItems;
        }

        async Task<BlacklistItems> IProfileStoreService.GetBlacklistByIdAsync(Guid blaclistId)
        {
            var response = await RequestAsync(HttpMethod.Get, $"{apiBaseUrl}/{blacklistsUrl}/{blaclistId}");
            var getBlacklist = await response.Content.ReadAsStringAsync();
            var blacklistItems = new BlacklistItems();

            if (string.IsNullOrEmpty(getBlacklist))
            {
                blacklistItems = new BlacklistItems()
                {
                    BlacklistList = new List<BlacklistModel>(),
                    Total = 0,
                    Count = 0
                };
            }
            else
            {
                blacklistItems = JsonConvert.DeserializeObject<BlacklistItems>(getBlacklist);
            }

            return blacklistItems;
        }

        #endregion

        #region Track Events Api

        async Task<VisualizationItems> IProfileStoreService.GetVisualizationItemsAsync(string queryString)
        {
            HttpResponseMessage response;
            var visualizationItems = new VisualizationItems();

            var uri = $"{apiBaseUrl}/{trackEventsUrl}/{queryString}";
            response = await RequestAsync(HttpMethod.Get, uri);
            var getVisualizationItems = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrEmpty(getVisualizationItems))
            {
                visualizationItems = new VisualizationItems()
                {
                    VisualizationModels = new List<VisualizationModel>(),
                    Total = 0,
                    Count = 0
                };
            }
            else
            {
                visualizationItems = JsonConvert.DeserializeObject<VisualizationItems>(getVisualizationItems);
            }

            return visualizationItems;
        }

        async Task<TrackEventItems> IProfileStoreService.GetAllTrackEventsAsync(ProfileStoreFilterOptions profileStoreFilterOptions)
        {
            HttpResponseMessage response;
            var trackEventItems = new TrackEventItems();

            if (profileStoreFilterOptions == null)
            {
                response = await RequestAsync(HttpMethod.Get, $"{apiBaseUrl}/{trackEventsUrl}/");
            }
            else
            {
                var uri = $"{apiBaseUrl}/{trackEventsUrl}/{CreateFilterOptionsUrl(profileStoreFilterOptions)}";
                response = await RequestAsync(HttpMethod.Get, uri);
            }

            var getTrackEvents = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(getTrackEvents))
            {
                trackEventItems = new TrackEventItems()
                {
                    TrackEventList = new List<TrackEventModel>(),
                    Total = 0,
                    Count = 0
                };
            }
            else
            {
                trackEventItems = JsonConvert.DeserializeObject<TrackEventItems>(getTrackEvents);
            }

            return trackEventItems;
        }

        #endregion

        #region Other business

        /// <summary>
        /// Load all country for listing
        /// </summary>
        /// <param name="profileModel"></param>
        public void LoadCountry(ProfileStoreModel profileModel) => profileModel.CountryOptions = GetAllCountries();

        /// <summary>
        /// Makes an async HTTP Request
        /// </summary>
        /// <param name="pMethod">Those methods you know: GET, POST, HEAD, etc...</param>
        /// <param name="pUrl">Very predictable...</param>
        /// <param name="pJsonContent">String data to POST on the server.</param>
        /// <param name="pHeaders">If you use some kind of Authorization you should use this.</param>
        /// <returns></returns>
        private async Task<HttpResponseMessage> RequestAsync(HttpMethod pMethod, string pUrl, string pJsonContent = "")
        {
            var httpRequestMessage = new HttpRequestMessage
            {
                Method = pMethod,
                RequestUri = new Uri(pUrl)
            };
            httpRequestMessage.Headers.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
            //httpRequestMessage.Headers.Add("Authorization", "epi-single" + subscriptionKey);

            switch (pMethod.Method)
            {
                case "POST":
                case "PUT":
                    HttpContent httpContent = new StringContent(pJsonContent, Encoding.UTF8, "application/json");
                    httpRequestMessage.Content = httpContent;
                    break;
            }

            return await _client.SendAsync(httpRequestMessage);
        }

        /// <summary>
        /// Creates the filter option URL.
        /// </summary>
        /// <param name="profileStoreFilterOptions">The profile store filter options.</param>
        /// <returns>Part of Profile Store Api URL.</returns>
        private string CreateFilterOptionsUrl(ProfileStoreFilterOptions profileStoreFilterOptions)
        {
            var hasFilterOptions = false;
            var urlSuffix = new StringBuilder();

            urlSuffix.Append("?");
            if (!string.IsNullOrEmpty(profileStoreFilterOptions.Filter.Key))
            {
                if (hasFilterOptions == true)
                {
                    urlSuffix.Append("&");
                }
                urlSuffix.Append($"$filter={profileStoreFilterOptions.Filter.Key} eq {profileStoreFilterOptions.Filter.Value}");
                hasFilterOptions = true;
            }

            if (profileStoreFilterOptions.Skip != 0)
            {
                if (hasFilterOptions == true)
                {
                    urlSuffix.Append(" &");
                }
                urlSuffix.Append($"$skip={profileStoreFilterOptions.Skip}");
                hasFilterOptions = true;
            }

            if (profileStoreFilterOptions.Top != 0)
            {
                if (hasFilterOptions == true)
                {
                    urlSuffix.Append(" &");
                }
                urlSuffix.Append($"$top={profileStoreFilterOptions.Top}");
                hasFilterOptions = true;
            }

            if (!string.IsNullOrEmpty(profileStoreFilterOptions.OrderBy.Key))
            {
                if (hasFilterOptions == true)
                {
                    urlSuffix.Append(" &");
                }
                urlSuffix.Append($"$orderby={profileStoreFilterOptions.OrderBy.Key} {profileStoreFilterOptions.OrderBy.Value}");
                hasFilterOptions = true;
            }
            return urlSuffix.ToString();
        }

        /// <summary>
        /// Map JToken to ProfileStoreViewModel
        /// </summary>
        /// <param name="jToken">Json element contain information about profilestore</param>
        /// <returns></returns>
        private ProfileStoreModel MapToProfileStore(JToken jToken)
        {
            var profileData = JsonConvert.DeserializeObject<ProfileStoreModel>(jToken.ToString());
            if (profileData.Payload == null)
            {
                profileData.Payload = new Dictionary<string, string>();
            }

            if (profileData.Info == null)
            {
                profileData.Info = new ProfileStoreInformation();
            }
            profileData.JsonPayload = JsonConvert.SerializeObject(profileData.Payload);
            return profileData;
        }

        /// <summary>
        /// Updates profile store data by another profile store.
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="origin"></param>
        private void MergeProfileStore(ProfileStoreModel destination, ProfileStoreModel origin)
        {
            destination.Name = origin.Name;
            destination.ProfileManager = origin.ProfileManager;
            destination.Scope = origin.Scope;
            destination.Info = origin.Info;
            destination.Payload = string.IsNullOrEmpty(origin.JsonPayload) ? new Dictionary<string, string>() : JsonConvert.DeserializeObject<Dictionary<string, string>>(origin.JsonPayload);
        }

        private IEnumerable<CountryViewModel> GetAllCountries()
        {
            var allCountries = CountryManager.GetCountries().Country.Select(x => new CountryViewModel { Code = x.Code, Name = x.Name }).ToList();
            allCountries.Insert(0, new CountryViewModel { Code = string.Empty, Name = string.Empty });
            return allCountries;
        }

        private string GetMobileVersion(string userAgent, string device)
        {
            var temp = userAgent.Substring(userAgent.IndexOf(device) + device.Length).TrimStart();
            var version = string.Empty;

            foreach (var character in temp)
            {
                var validCharacter = false;

                if (int.TryParse(character.ToString(), out var test))
                {
                    version += character;
                    validCharacter = true;
                }

                if (character == '.' || character == '_')
                {
                    version += '.';
                    validCharacter = true;
                }

                if (validCharacter == false)
                    break;
            }

            return version;
        }

        #endregion
    }
}