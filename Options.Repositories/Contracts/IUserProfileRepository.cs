using Options.DbContext.Models;
using Options.Repositories.Models;

namespace Options.Repositories.Contracts
{
    public interface IUserProfileRepository
    {
        Task<Response<UserProfile>> GetUserProfileByIdAsync(Guid userId);
        Task<Response<UserProfile>> CreateUserProfileAsync(UserProfile userProfile);
        Task<Response<UserProfile>> UpdateUserProfileAsync(UserProfile userProfile);
        Task<Response> DeleteUserProfileAsync(Guid userId);
    }
}
