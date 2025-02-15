using Options.DbContext.Models;
using Options.Repositories.Models;

namespace Options.Repositories.Contracts
{
    public interface ISettingsRepository
    {
        Task<Response<Settings>> CreateSettingAsync(Settings option);

        Task<Response<Settings>> UpdateSettingAsync(Settings option);

        Task<Response> DeleteSettingAsync(Guid optionId);

        Task<Response<Settings>> GetSettingByUserIdAsync(Guid userId);
    }
}
