using NetCoreJwtAuth.Data;
using NetCoreJwtAuth.Entities;
using System.Linq;

namespace NetCoreJwtAuth.Services {
  public interface IUserService {
    AppUser GetById(int Id);
    /* その他のCRUD処理は今回省略 */
  }

  public class UserService : IUserService {
    readonly MyDbContext myDbContext;

    public UserService(MyDbContext myDbContext) {
      this.myDbContext = myDbContext;
    }

    public AppUser GetById(int Id) =>
      myDbContext.Users.SingleOrDefault(user => user.ID == Id);
  }
}
