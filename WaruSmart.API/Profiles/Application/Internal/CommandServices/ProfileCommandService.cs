using WaruSmart.API.IAM.Interfaces.ACL;
using WaruSmart.API.IAM.Interfaces.ACL.Services;
using WaruSmart.API.Profiles.Domain.Model.Aggregates;
using WaruSmart.API.Profiles.Domain.Model.Commands;
using WaruSmart.API.Profiles.Domain.Repositories;
using WaruSmart.API.Profiles.Domain.Services;
using WaruSmart.API.Shared.Domain.Repositories;

namespace WaruSmart.API.Profiles.Application.Internal.CommandServices;

public class ProfileCommandService(
    IProfileRepository profileRepository,
    IUnitOfWork unitOfWork,
    IIamContextFacade iamContextFacade)
    : IProfileCommandService
{
    public async Task<Profile?> Handle(CreateProfileCommand command)
    {
        var userId = await ExternalUserService(command.UserId);
        
        var profile = new Profile(command, userId);
        try
        {
            await profileRepository.AddAsync(profile);
            await unitOfWork.CompleteAsync();
            return profile;
        } catch (Exception e)
        {
            Console.WriteLine($"An error occurred while creating the profile: {e.Message}");
            return null;
        }
    }
    
    /*
     * This method call the external user service to fetch the user id by userId
     * TODO: Implement the external user service at other file 
     */
    private async Task<int> ExternalUserService(int userId)
    {
        var result = await iamContextFacade.FetchUserIdByUserId(userId);
        if (result == 0)
        {
            Console.WriteLine("!!!!!!!User not found");
            throw new Exception("User not found");
            //Imprimir un mensaje de error
            
        }
        return result;
    }

    public async Task<Profile?> Handle(UpdateProfileCommand command)
    {
        var profile = await profileRepository.GetProfileByIdAsync(command.ProfileId);
        if (profile == null) return null;

        profile.UpdateProfile(
            command.FullName,
            command.EmailAddress,
            command.CountryId,
            command.CityId
        );

        await profileRepository.UpdateProfile(profile);
        return profile;
    }
}