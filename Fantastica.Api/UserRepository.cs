using System.Linq;
using Fantastica.Api.Entities;
using Raven.Client;

namespace Fantastica.Api {
  public class UserRepository : Repository<User> {
    public UserRepository(IDocumentStore dbStore) {
      _dbStore = dbStore;
    }

    public string NextSongId(string userName, string playList) {
      using (var session = _dbStore.OpenSession()) {
        var user = session.Query<User>().FirstOrDefault(u => u.Name == userName);
        if (user == null)
          return "";

        var userList = user.Lists.FirstOrDefault(e => e.Name == playList);
        if (userList == null)
          return "";

        userList.MoveToNextSong();

        session.SaveChanges();
        return userList.CurrentSongId;
      }
    }
  }
}