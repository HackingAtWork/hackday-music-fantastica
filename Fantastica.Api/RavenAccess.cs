using System.Collections.Generic;
using System.Security.AccessControl;
using Raven.Client;
using Raven.Client.Embedded;

namespace Fantastica.Api {
  public class RavenAccess {
    static RavenAccess instance;
    readonly IDocumentStore dbInstance;

    RavenAccess(IDocumentStore db) {
      dbInstance = db;
    }
    static RavenAccess Initialize() {
      var doc = new EmbeddableDocumentStore { DataDirectory = "RavenDB" };
      doc.Conventions.IdentityPartsSeparator = "-";
      doc.Initialize();
      return new RavenAccess(doc);
    }

    public static RavenAccess Instance {
      get { return instance ?? (instance = Initialize()); }
    }

    public void Save(object data) {
      using (var session = dbInstance.OpenSession()) {
        session.Store(data);
        session.SaveChanges();
      }
    }

    public IEnumerable<T> Query<T>() {
      using (var session = dbInstance.OpenSession()) {
        return session.Query<T>();
      }
    }
  }
}

namespace Fantastica.Api.Entities {
  public class User {
    public string Id { get; set; }
    public string Name { get; set; }
  }

  public class Song {
    public string Id { get; set; }
    public string Title { get; set; }
    public string Artist { get; set; }
    public string Album { get; set; }
    public string AlbumArtist { get; set; }
    public string Path { get; set; }
  }
}
