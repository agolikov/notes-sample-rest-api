using AutoMapper;
using notes.application.Extensions;
using notes.application.Interfaces;
using notes.application.Models.User;
using notes.data.Entities;
using notes.data.Exceptions;
using notes.data.Interfaces;
using System;
using System.Threading.Tasks;

namespace notes.application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IMapper mapper,
            IUserRepository userRepository)
        {
            (_userRepository, _mapper) = (userRepository, mapper);
        }

        public async Task<UserModel> GetUserAsync(Guid userId)
        {
            var user = await _userRepository.FindOneAsync(t => t.Id == userId);
            if (user == null)
                throw userId.EntityNotFoundException();

            return _mapper.Map<User, UserModel>(user);
        }

        public async Task<UserModel> ChangeUserPasswordAsync(ChangePasswordModel model)
        {
            var user = await _userRepository.FindOneAsync(t => t.Email == model.Email);
            if (user == null)
            {
                throw model.Email.EmailNotFoundException();
            }

            if (!DataExtensions.IsPasswordCorrect(model.OldPassword, user.PasswordHash, user.PasswordSalt))
            {
                throw user.PasswordIsIncorrectException();
            }

            DataExtensions.CreatePasswordHash(model.NewPassword, out byte[] newPassHash, out byte[] newSaltHash);

            user.PasswordHash = newPassHash;
            user.PasswordSalt = newSaltHash;

            var updatedUser = await _userRepository.InsertOrUpdateAsync(user, user.CreatedBy);

            return _mapper.Map<User, UserModel>(updatedUser);
        }
    }
}