using System.Security.Cryptography;
using AutoMapper;
using Bank.Models;
using WebApplication1.DTO;
using WebApplication1.Repository;

namespace WebApplication1.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;
        private readonly IRepository _repository;

        public EmployeeService(IEmployeeRepository employeeRepository, IMapper mapper, IRepository repository)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
            _repository = repository;
        }


        public Task<List<UserDto>> GetUsers()
        {
            var response = _employeeRepository.GetUsers();

            return response;
        }


        public UserDto GetUserById(int userId)
        {
            try
            {
                if (!this.isUserActive(userId))
                {
                    throw new Exception("User Doesn't Exits!!!");
                }


                var userDetails = _employeeRepository.GetUserDetails(userId);

                var response = _mapper.Map<UserDto>(userDetails);

                return response;

            }
            catch
            {
                throw new Exception("Error : Couldn't GET user details by given ID");
            }
        }



        public bool isUserActive(int userId)
        {

            var userDetails = _employeeRepository.GetUserDetails(userId);

            if(userDetails == null)
            {
                return false;

            }

            if(userDetails != null && userDetails.IsActive==0)
            {
                return false;
            }

            return true;
        }


        public UserDto CreateUser(User userDetails)
        {
            var user = _repository.GetUserByEmail(userDetails.UserEmail);

            if(user != null )
            {
                throw new Exception("User already exists with this Email. Please try with other Email.");
            }


            // password hashing 
            CreatePasswordHash(userDetails.UserPassword, out byte[] passwordSalt, out string passwordHash);
            userDetails.UserPassword = passwordHash;

            var res = _employeeRepository.CreateUser(userDetails);

            if (res == Task.FromResult(0))
            {
                throw new Exception("User Registration Failed. Please try again!!");
            }

            var newUser = new UserDto
            {
                UserName = userDetails.UserName,
                UserEmail = userDetails.UserEmail,
                UserPhone = userDetails.UserPhone,
                BankCode = userDetails.BankCode,
                BankName = userDetails.BankName,
                AccountNumber = userDetails.AccountNumber
            };

            return newUser;

        }


        private void CreatePasswordHash(string password, out byte[] passwordSalt, out string passwordHash)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = Convert.ToBase64String(hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)));
            }
        }


    }
}
