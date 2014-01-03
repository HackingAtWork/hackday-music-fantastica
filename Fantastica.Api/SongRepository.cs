using System.Collections.Generic;
using System.Linq;
using Fantastica.Api.Entities;
using Raven.Client;

namespace Fantastica.Api {
  public class SongRepository : Repository<Song> {
    public SongRepository(IDocumentStore dbStore) {
      _dbStore = dbStore;
    }

    public void ExclusiveBulkSave(IEnumerable<Song> collection) {
      using (var session = _dbStore.OpenSession()) {
        var filePaths = from record in session.Query<Song>() select record.Path;

        foreach (var datum in collection.Where(song => !filePaths.Contains(song.Path))) {
          session.Store(datum);
        }

        session.SaveChanges();
      }
    }
  }
}