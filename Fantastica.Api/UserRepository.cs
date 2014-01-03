using Fantastica.Api.Entities;
using Raven.Client;

namespace Fantastica.Api {
  public class UserRepository : Repository<User> {
    public UserRepository(IDocumentStore dbStore) {
      _dbStore = dbStore;
    }
  }
}