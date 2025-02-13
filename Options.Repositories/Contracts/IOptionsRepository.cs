using Options.DbContext.Models;
using Options.Repositories.Models;

namespace Options.Repositories.Contracts
{
    public interface IOptionsRepository
    {
        Task<Response<Option>> CreateOptionAsync(Option option);

        Task<Response<Option>> UpdateOptionAsync(Option option);

        Task<Response<Option>> DeleteOptionAsync(Guid optionId);

        Task<Response<List<Option>>> GetOptionsAsync(OptionsFilter filter);

    }
}
