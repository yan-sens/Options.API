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
      
        public async Task<Response<Option>> CreateOptionAsync(Option option)
        {
            try
            {
                _dBContext.Options.Add(option);

                await _dBContext.SaveChangesAsync();

                return new Response<Option>
                {
                    StatusCode = System.Net.HttpStatusCode.Created,
                    Data = option
                };
            }
            catch (Exception ex)
            {
                return new Response<Option>
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    ErrorMessage = ex.Message,
                };
            }
        }

        public async Task<Response<Option>> UpdateOptionAsync(Option option)
        {
            try
            {
                var optionToUpdate = _dBContext.Options.FirstOrDefault(x => x.Id == option.Id);
                if (optionToUpdate == null)
                {
                    return new Response<Option>
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

                return new Response<Option>
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Data = option
                };
            }
            catch (Exception ex)
            {
                return new Response<Option>
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    ErrorMessage = ex.Message,
                };
            }
        }

        public async Task<Response<Option>> DeleteOptionAsync(Guid optionId)
        {
            try
            {
                var optionToRemove = _dBContext.Options.Include(x => x.RollOvers).FirstOrDefault(x => x.Id == optionId);
                if (optionToRemove == null)
                {
                    return new Response<Option>
                    {
                        StatusCode = System.Net.HttpStatusCode.NotFound,
                        ErrorMessage = "Record not found."
                    };
                }

                if (optionToRemove.RollOvers != null && optionToRemove.RollOvers.Any())
                {
                    optionToRemove.RollOvers.ToList().ForEach(rollOver => {
                        _dBContext.Remove(rollOver);
                    });
                }

                _dBContext.Remove(optionToRemove);

                await _dBContext.SaveChangesAsync();

                return new Response<Option>
                {
                    StatusCode = System.Net.HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new Response<Option>
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    ErrorMessage = ex.Message,
                };
            }
        }

        public async Task<Response<List<Option>>> GetOptionsAsync(OptionsFilter filter)
        {
            var response = new Response<List<Option>>();

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
