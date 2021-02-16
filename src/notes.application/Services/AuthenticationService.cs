using AutoMapper;
using notes.application.Extensions;
using notes.application.Interfaces;
using notes.application.Models;
using notes.application.Models.User;
using notes.data.Entities;
using notes.data.Interfaces;
using System.Threading.Tasks;

namespace notes.application.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly Settings.Settings _settings;

        public AuthenticationService(IMapper mapper, IUserRepository userRepository, Settings.Settings settings)
        {
            (_userRepository, _mapper, _settings) = (userRepository, mapper, settings);
        }

        public async Task<UserModel> SignUpAsync(SignUpModel model)
        {
            var userByEmail = await _userRepository.FindOneAsync(r => r.Email == model.Email);
            if (userByEmail != null)
                throw userByEmail.EmailAlreadyTakenException();

            var newUser = _mapper.Map<SignUpModel, User>(model);
            DataExtensions.CreatePasswordHash(model.Password, out var hash, out var salt);
            newUser.PasswordHash = hash;
            newUser.PasswordSalt = salt;

            await _userRepository.InsertOrUpdateAsync(newUser, newUser.Id);

            var createdUserModel = _mapper.Map<User, UserModel>(newUser);
            return createdUserModel;
        }

        public async Task<LogInResult> SignInAsync(SignInModel model)
        {
            var user = await _userRepository.FindOneAsync(r => r.Email == model.Email);

            if (user == null)
                throw model.Email.EmailNotFoundException();

            if (!DataExtensions.IsPasswordCorrect(model.Password, user.PasswordHash, user.PasswordSalt))
                throw user.PasswordIsIncorrectException();

            var userModel = _mapper.Map<User, UserModel>(user);

            return new LogInResult
            {
                User = userModel,
                Token = DataExtensions.GenerateToken(user, _settings.SecretKey)
            };
        }
    }
}