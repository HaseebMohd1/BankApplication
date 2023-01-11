using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Bank.Models;
using WebApplication1.DTO;
using WebApplication1.Repository;
using BCrypt.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace WebApplication1.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;
        private readonly IRepository _repository;

        private readonly IConfiguration _configuration;



        public EmployeeService(IEmployeeRepository employeeRepository, IMapper mapper, IRepository repository, IConfiguration configuration)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
            _repository = repository;
            _configuration = configuration;
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


            // password hashing using SHA512
            //CreatePasswordHash(userDetails.UserPassword, out string passwordSalt, out string passwordHash);


            // Password Hashing using SHA256
            string randomSalt = GenerateRandomSalt();

            string hashedPassword = CreatePasswordHashUsingSha256(userDetails.UserPassword+ randomSalt);


            userDetails.UserPassword = hashedPassword;
            userDetails.PasswordSalt = randomSalt;

            //userDetails.UserPassword = passwordHash;
            //userDetails.PasswordSalt = passwordSalt;

            //userDetails.UserPassword = passwordHash;

            //string hashedPassword = CreatePasswordHash2(userDetails.UserPassword);

            //string hashedPassword = CreatePasswordHashUsingBcrypt(userDetails.UserPassword);
            //userDetails.UserPassword = hashedPassword;




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


        private void CreatePasswordHash(string password, out string passwordSalt, out string passwordHash)
        {
            //byte[] key = new byte[32];

            using (var hmac = new HMACSHA512())
            {
                passwordSalt = Convert.ToBase64String(hmac.Key);
                passwordHash = Convert.ToBase64String(hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)));
            }

        }


        private string CreatePasswordHash2(string password)
        {
            StringBuilder hash = new StringBuilder();

            // input string
            string input = password;

            // defining MD5 object
            var md5provider = new MD5CryptoServiceProvider();
            // computing MD5 hash
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));
            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }

            // final output
            // Console.WriteLine(string.Format("The MD5 hash is: {0}", hash));

            return hash.ToString();
        }

        private string CreatePasswordHashUsingBcrypt(string password)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            return hashedPassword;
        }



        private string CreatePasswordHashUsingSha256(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var resBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

               // Console.WriteLine(resBytes + "---");

                var hashedPassword = BitConverter.ToString(resBytes).Replace("-", "").ToLower();

                //Console.WriteLine(resString + "\n");

                return hashedPassword;

            }
        }


        private string GenerateRandomSalt()
        {
            byte[] bytes = new byte[128 / 8];
            using (var keyGenerator = RandomNumberGenerator.Create())
            {
                keyGenerator.GetBytes(bytes);
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }


        

        public string EmployeeLogin(string employeeEmail, string employeePassword)
        {
            Employee employeeDetails = _employeeRepository.GetEmployeeByEmail(employeeEmail);

            if(employeeDetails == null)
            {
                throw new Exception("Employee with this Email doesn't exist! Please enter valid details");
            }

            string hashedPassword = employeeDetails.PasswordHash;
            string passwordSalt = employeeDetails.PasswordSalt;

            

            if (!VerifyPasswordSha256(employeePassword, hashedPassword, passwordSalt))
            {
                throw new Exception("Entered Details are Incorrect. Please Enter valid Email & Password");
            }

            string token = CreateToken(employeeDetails);

            return token;

        }


        private bool VerifyPasswordSha256(string password, string hashedPassword, string salt)
        {
            string userEnteredHashedPassword = CreatePasswordHashUsingSha256(password + salt);

            return userEnteredHashedPassword.Equals(hashedPassword);
        }

        

        private string CreateToken(Employee employeeDetails)
        {

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, employeeDetails.EmployeeEmail),
                new Claim(ClaimTypes.Role, employeeDetails.Role),
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            // definig PAYLOAD of our JSON Web Token    
            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: credentials
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }


        public string CreateEmployee(string employeeName, string employeeEmail, string employeePassword)
        {

            string randomSalt = GenerateRandomSalt();
            string hashedPassword = CreatePasswordHashUsingSha256(employeePassword + randomSalt);

            Employee newEmployee = new Employee()
            {
                EmployeeName = employeeName,
                EmployeeEmail = employeeEmail,
                Role = "Admin",
                PasswordHash = hashedPassword,
                PasswordSalt = randomSalt
            };

            Employee newEmployeeDetails = _employeeRepository.SaveEmployee(newEmployee);

            string message = $"Employee ${newEmployeeDetails.EmployeeName} with Email : {newEmployeeDetails.EmployeeEmail}";

            return message;
        }


        


    }
}
