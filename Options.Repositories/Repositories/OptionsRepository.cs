using Azure;
using Microsoft.EntityFrameworkCore;
using Options.DbContext;
using Options.DbContext.Models;
using Options.Repositories.Contracts;
using Options.Repositories.Models;

namespace Options.Repositories.Repositories
{
    public class OptionsRepository : IOptionsRepository
    {
        private readonly OptionsDBContext _dBContext;
        public OptionsRepository(OptionsDBContext dBContext)
        {
            _dBContext = dBContext;
        }
      
        public async Task<Models.Response<Option>> CreateOptionAsync(Option option)
        {
            try
            {
                _dBContext.Options.Add(option);

                await _dBContext.SaveChangesAsync();

                return new Models.Response<Option>
                {
                    StatusCode = System.Net.HttpStatusCode.Created,
                    Data = option
                };
            }
            catch (Exception ex)
            {
                return new Models.Response<Option>
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    ErrorMessage = ex.Message,
                };
            }
        }

        public async Task<Models.Response<Option>> UpdateOptionAsync(Option option)
        {
            try
            {
                var optionToUpdate = _dBContext.Options.FirstOrDefault(x => x.Id == option.Id);
                if (optionToUpdate == null)
                {
                    return new Models.Response<Option>
                    {
                        StatusCode = System.Net.HttpStatusCode.NotFound,
                        ErrorMessage = "Record not found."
                    };
                }

                optionToUpdate.ClosedDate = option.ClosedDate;
                optionToUpdate.Completed = option.Completed;
                optionToUpdate.IsClosed = option.IsClosed;
                optionToUpdate.ReturnAmount = option.ReturnAmount;

                _dBContext.Update(optionToUpdate);

                await _dBContext.SaveChangesAsync();

                return new Models.Response<Option>
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Data = option
                };
            }
            catch (Exception ex)
            {
                return new Models.Response<Option>
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    ErrorMessage = ex.Message,
                };
            }
        }

        public async Task<Models.Response<List<Option>>> GetOptionsAsync(OptionsFilter filter)
        {
            var response = new Models.Response<List<Option>>();

            var options = await _dBContext.Options
                .Where(option => option.UserId.Equals(filter.UserId))
                .ToListAsync();

            response.Data = options
                .Where(x => x.ParentOptionId == null)
                .ToList();

            return response;
        }
    }
}
