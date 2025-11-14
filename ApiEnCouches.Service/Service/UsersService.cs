namespace ApiEnCouches.Service.Service;

using ApiEnCouches.DataAccess.Models;
using ApiEnCouches.DataAccess.IDataAccess;
using ApiEnCouches.Service.DataTransferObject.User;
using ApiEnCouches.Service.IService;
using BCrypt.Net;

public class UsersService : IUserService
{
    private readonly IUsers userDataAccess;

    public UsersService(IUsers userDataAccess)
    {
        this.userDataAccess = userDataAccess;
    }

    public async Task<GetUserDTO> GetById(int userId)
    {
        var user = await userDataAccess.GetById(userId);
        if (user == null) return null;

        return new GetUserDTO
        {
            UserId = user.UserId,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email
        };
    }

    public async Task<List<GetUserDTO>> GetAll()
    {
        var users = await userDataAccess.GetAll();

        return users.Select(u => new GetUserDTO
        {
            UserId = u.UserId,
            FirstName = u.FirstName,
            LastName = u.LastName,
            Email = u.Email
        }).ToList();
    }

    public async Task<GetUserDTO> Create(CreateUserDTO createUserDto)
    {
        if (await userDataAccess.EmailExists(createUserDto.Email))
        {
            throw new Exception("Un utilisateur avec cet email existe déjà");
        }

        var user = new UsersModel
        {
            FirstName = createUserDto.FirstName,
            LastName = createUserDto.LastName,
            Email = createUserDto.Email,
            Password = BCrypt.HashPassword(createUserDto.Password)
        };

        user = await userDataAccess.Create(user);

        return new GetUserDTO
        {
            UserId = user.UserId,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email
        };
    }

    public async Task<GetUserDTO> Update(int userId, UpdateUserDTO updateUserDto)
    {
        var user = await userDataAccess.GetById(userId);
        if (user == null)
        {
            throw new Exception("Utilisateur non trouvé");
        }

        if (!string.IsNullOrEmpty(updateUserDto.FirstName))
            user.FirstName = updateUserDto.FirstName;

        if (!string.IsNullOrEmpty(updateUserDto.LastName))
            user.LastName = updateUserDto.LastName;

        if (!string.IsNullOrEmpty(updateUserDto.Email))
        {
            if (user.Email != updateUserDto.Email && await userDataAccess.EmailExists(updateUserDto.Email))
            {
                throw new Exception("Un utilisateur avec cet email existe déjà");
            }
            user.Email = updateUserDto.Email;
        }

        user = await userDataAccess.Update(user);

        return new GetUserDTO
        {
            UserId = user.UserId,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email
        };
    }

    public async Task<bool> Delete(int userId)
    {
        return await userDataAccess.Delete(userId);
    }
}
