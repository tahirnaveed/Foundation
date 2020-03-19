using EPiServer.Core;
using System;
using System.Threading.Tasks;
using System.Web;

namespace Foundation.Demo.ProfileStore
{
    public interface IProfileStoreService
    {
        Task<ProfileStoreItems> GetAllProfilesAsync(ProfileStoreFilterOptions profileStoreFilterOptions);
        Task<ProfileStoreItems> GetProfilesAsync(string queryString);
        Task<ProfileStoreModel> GetProfileByIdAsync(string scope, Guid profileId);
        Task EditOrCreateProfileAsync(string scope, ProfileStoreModel model);
        void LoadCountry(ProfileStoreModel profileModel);
        Task<ScopeItems> GetAllScopesAsync();
        Task<ScopeItems> GetScopesByIdAsync(Guid scopeId);
        Task<SegmentItems> GetAllSegmentsAsync();
        Task<SegmentItems> GetSegmentByIdAsync(Guid scopeId);
        Task EditOrCreateSegmentAsync(SegmentModel model);
        Task<BlacklistItems> GetAllBlacklistAsync();
        Task<BlacklistItems> GetBlacklistByIdAsync(Guid blacklistId);
        Task<VisualizationItems> GetVisualizationItemsAsync(string queryString);
        Task<TrackEventItems> GetAllTrackEventsAsync(ProfileStoreFilterOptions profileStoreFilterOptions);
    }
}
