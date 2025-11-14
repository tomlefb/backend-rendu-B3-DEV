namespace ApiEnCouches.DataAccess.IDataAccess;

using ApiEnCouches.DataAccess.Models;

public interface IUsers
{
    Task<UsersModel> GetById(int userId);
    Task<UsersModel> GetByEmail(string email);
    Task<List<UsersModel>> GetAll();
    Task<UsersModel> Create(UsersModel user);
    Task<UsersModel> Update(UsersModel user);
    Task<bool> Delete(int userId);
    Task<bool> EmailExists(string email);
}
