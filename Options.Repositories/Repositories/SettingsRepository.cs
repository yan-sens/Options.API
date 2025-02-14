using Microsoft.EntityFrameworkCore;
using Options.DbContext;
using Options.DbContext.Models;
using Options.Repositories.Contracts;
using Options.Repositories.Models;

namespace Options.Repositories.Repositories
{
    public class SettingsRepository : ISettingsRepository
    {
        private readonly OptionsDBContext _dBContext;
        public SettingsRepository(OptionsDBContext dBContext)
        {
            _dBContext = dBContext;
        }

        public async Task<Response<Setting>> CreateSettingAsync(Setting setting)
        {
            try
            {
                _dBContext.Settings.Add(setting);

                await _dBContext.SaveChangesAsync();

                return new Response<Setting>
                {
                    StatusCode = System.Net.HttpStatusCode.Created,
                    Data = setting
                };
            }
            catch (Exception ex)
            {
                return new Response<Setting>
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    ErrorMessage = ex.Message,
                };
            }
        }

        public async Task<Response> DeleteSettingAsync(Guid settingId)
        {
            try
            {
                var settingToRemove = await _dBContext.Settings.FirstOrDefaultAsync(x => x.Id == settingId);
                if (settingToRemove == null)
                {
                    return new Response
                    {
                        StatusCode = System.Net.HttpStatusCode.NotFound,
                        ErrorMessage = "Record not found."
                    };
                }

                _dBContext.Remove(settingToRemove);

                await _dBContext.SaveChangesAsync();

                return new Response
                {
                    StatusCode = System.Net.HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    ErrorMessage = ex.Message,
                };
            }
        }

        public async Task<Response<Setting>> GetSettingByUserIdAsync(Guid userId)
        {
            var settings = await _dBContext.Settings.FirstOrDefaultAsync(x => x.UserId == userId);
            if (settings == null)
            {
                return new Response<Setting>
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    ErrorMessage = "Record not found."
                };
            }

            return new Response<Setting>
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Data = settings
            };
        }

        public async Task<Response<Setting>> UpdateSettingAsync(Setting settings)
        {
            try
            {
                var settingToUpdate = await _dBContext.Settings.FirstOrDefaultAsync(x => x.UserId == settings.UserId);
                if (settingToUpdate == null)
                {
                    return new Response<Setting>
                    {
                        StatusCode = System.Net.HttpStatusCode.NotFound,
                        ErrorMessage = "Record not found."
                    };
                }

                settingToUpdate.Tax = settings.Tax;
                settingToUpdate.RegulatoryFee = settings.RegulatoryFee;

                _dBContext.Update(settingToUpdate);

                await _dBContext.SaveChangesAsync();

                return new Response<Setting>
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Data = settingToUpdate
                };
            }
            catch (Exception ex)
            {
                return new Response<Setting>
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    ErrorMessage = ex.Message,
                };
            }
        }
    }
}
