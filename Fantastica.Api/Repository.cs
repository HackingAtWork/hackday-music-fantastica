using System;
using System.Collections.Generic;
using System.Linq;
using Raven.Client;

namespace Fantastica.Api {
  public abstract class Repository<T> {
    protected IDocumentStore _dbStore;

    public void Save(T data) {
      using (var session = _dbStore.OpenSession()) {
        session.Store(data);
        session.SaveChanges();
      }
    }

    public IEnumerable<T> FindAll() {
      using (var session = _dbStore.OpenSession()) {
        return session.Query<T>();
      }
    }

    public IEnumerable<T> Find<T>(Func<T, bool> pred) {
      using (var session = _dbStore.OpenSession()) {
        return session.Query<T>().Where(pred);
      }
    }
  }
}