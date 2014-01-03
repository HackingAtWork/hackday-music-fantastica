using Raven.Client;
using Raven.Client.Embedded;

namespace Fantastica.Api {
  public class DataStore {
    static DataStore instance;
    public SongRepository SongRepository { get; private set; }
    public UserRepository UserRepository { get; private set; }

    DataStore(IDocumentStore db) {
      SongRepository = new SongRepository(db);
      UserRepository = new UserRepository(db);
    }

    static DataStore Initialize() {
      var doc = new EmbeddableDocumentStore { DataDirectory = "RavenDB" };
      doc.Conventions.IdentityPartsSeparator = "-";
      doc.Initialize();
      return new DataStore(doc);
    }

    public static DataStore Instance {
      get { return instance ?? (instance = Initialize()); }
    }
  }
}
