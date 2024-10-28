// using PetManager.Core.Users.Enums;
//
// namespace PetManager.Core.Users.Entities;
//
// public class UserBuilder
// {
//     private readonly User _user = new();
//
//     public User Build()
//         => _user;
//
//     public UserBuilder SetFirstName(string firstName)
//     {
//         _user.FirstName = firstName;
//         return this;
//     }
//
//     public UserBuilder SetLastName(string lastName)
//     {
//         _user.LastName = lastName;
//         return this;
//     }
//
//     public UserBuilder SetEmail(string email)
//     {
//         _user.Email = email;
//         return this;
//     }
//
//     public UserBuilder SetPassword(string password)
//     {
//         _user.Password = password;
//         return this;
//     }
//
//     public UserBuilder SetLastChangePasswordDate(DateTimeOffset? lastChangePasswordDate)
//     {
//         _user.LastChangePasswordDate = lastChangePasswordDate;
//         return this;
//     }
//
//     public UserBuilder SetCreatedAt(DateTimeOffset createdAt)
//     {
//         _user.CreatedAt = createdAt;
//         return this;
//     }
//
//     public UserBuilder SetRole(UserRole role)
//     {
//         _user.Role = role;
//         return this;
//     }
// }