using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Options.DbContext;
using Options.DbContext.Models;
using Options.Repositories.Contracts;
using Options.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Options.Repositories.Repositories
{
    public class UserProfileRepository : IUserProfileRepository
    {
        private readonly OptionsDBContext _dBContext;
        public UserProfileRepository(OptionsDBContext dBContext)
        {
            _dBContext = dBContext;
        }

        public async Task<Response<UserProfile>> CreateUserProfileAsync(UserProfile userProfile)
        {
            try
            {
                _dBContext.UserProfiles.Add(userProfile);

                await _dBContext.SaveChangesAsync();

                return new Response<UserProfile>
                {
                    StatusCode = System.Net.HttpStatusCode.Created,
                    Data = userProfile
                };
            }
            catch (Exception ex)
            {
                return new Response<UserProfile>
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    ErrorMessage = ex.Message,
                };
            }
        }

        public async Task<Response> DeleteUserProfileAsync(Guid userId)
        {
            try
            {
                var userProfileToRemove = await _dBContext.UserProfiles.FirstOrDefaultAsync(x => x.Id == userId);
                if (userProfileToRemove == null)
                {
                    return new Response
                    {
                        StatusCode = System.Net.HttpStatusCode.NotFound,
                        ErrorMessage = "Record not found."
                    };
                }

                _dBContext.Remove(userProfileToRemove);

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

        public async Task<Response<UserProfile>> GetUserProfileByIdAsync(Guid userId)
        {
            var userProfile = await _dBContext.UserProfiles.FirstOrDefaultAsync(x => x.Id == userId);
            if (userProfile == null)
            {
                return new Response<UserProfile>
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    ErrorMessage = "Record not found."
                };
            }

            return new Response<UserProfile>
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Data = userProfile
            };
        }

        public async Task<Response<UserProfile>> UpdateUserProfileAsync(UserProfile userProfile)
        {
            try
            {
                var userProfileToUpdate = await _dBContext.UserProfiles.FirstOrDefaultAsync(x => x.Id == userProfile.Id);
                if (userProfileToUpdate == null)
                {
                    return new Response<UserProfile>
                    {
                        StatusCode = System.Net.HttpStatusCode.NotFound,
                        ErrorMessage = "Record not found."
                    };
                }

                userProfileToUpdate.Name = userProfile.Name;
                userProfileToUpdate.PhoneNumber = userProfile.PhoneNumber;

                _dBContext.Update(userProfileToUpdate);

                await _dBContext.SaveChangesAsync();

                return new Response<UserProfile>
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Data = userProfileToUpdate
                };
            }
            catch (Exception ex)
            {
                return new Response<UserProfile>
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    ErrorMessage = ex.Message,
                };
            }
        }
    }
}
