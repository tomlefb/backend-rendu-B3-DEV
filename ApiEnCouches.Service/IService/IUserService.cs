namespace ApiEnCouches.Service.IService;

using ApiEnCouches.Service.DataTransferObject.User;

public interface IUserService
{
    Task<GetUserDTO> GetById(int userId);
    Task<List<GetUserDTO>> GetAll();
    Task<GetUserDTO> Create(CreateUserDTO createUserDto);
    Task<GetUserDTO> Update(int userId, UpdateUserDTO updateUserDto);
    Task<bool> Delete(int userId);
}
