using Options.DbContext.Models;
using Options.Repositories.Models;

namespace Options.Repositories.Contracts
{
    public interface ISettingsRepository
    {
        Task<Response<Setting>> CreateSettingAsync(Setting option);

        Task<Response<Setting>> UpdateSettingAsync(Setting option);

        Task<Response> DeleteSettingAsync(Guid optionId);

        Task<Response<Setting>> GetSettingByUserIdAsync(Guid userId);
    }
}
