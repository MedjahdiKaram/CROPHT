using System;
using System.Linq;
using System.Threading;
using DAL;
using InterfacesLib.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelsLib;

namespace OphtalmoTesting
{
    [TestClass]
    public class CompteRenduTest:ICrud
    {
         protected Repository<CompteRendu> Repo { get; set; }

        public CompteRenduTest()
        {
            IUnitOfWork ophtalmocontext = new OphtalmoContext();
            Repo = new Repository<CompteRendu>(ophtalmocontext);
        }

        public void Add()
        {
            var newentity = new CompteRendu {Contenu="un exemple de contenu",  Nom = "Compte Rendu 1"};

            Repo.Add(newentity);
            var getentityfromrepo = Repo.Get(n => n.Nom == newentity.Nom).FirstOrDefault();
            Assert.AreNotEqual(getentityfromrepo, null);
        }

        [TestMethod]
        public void AddOrUpdate()
        {
            var newentity = new CompteRendu {Contenu="un exemple de contenu",  Nom = "Compte Rendu 2"};


            Repo.AddOrUpdate(newentity);
            //var getnewmedfromrepo = Repo.Get(n => n.Nom == newentity.Nom);

            var getentityfromrepo = Repo.Get(n => n.Nom == "Compte Rendu 2").FirstOrDefault();
            if (getentityfromrepo != null)
                getentityfromrepo.Nom = "Compte Rendu";
            else
            {
                Assert.IsTrue(false);
            }
            Repo.AddOrUpdate(getentityfromrepo);
            getentityfromrepo = null;
            getentityfromrepo = Repo.Get(n => n.Nom == "Compte Rendu").FirstOrDefault();

            Assert.AreNotEqual(getentityfromrepo, null);
        }

        [TestMethod]
        public void Delete()
        {
            Repo.Add(new CompteRendu {Contenu="un exemple de contenu",  Nom = "C Rendu" + new Random().Next(1000)});
            Repo.Add(new CompteRendu {Contenu="un exemple de contenu",  Nom = "Compte Rnd" + new Random().Next(1000)});

            var firstone = Repo.Get().FirstOrDefault();
            Repo.Delete(firstone); Thread.Sleep(1000);
            var firstisnull = Repo.Get(n => n.Nom == firstone.Nom).FirstOrDefault() == null;
            firstone = Repo.Get().FirstOrDefault();
            if (firstone != null)
            {
                Repo.Delete(firstone.Id);
                Thread.Sleep(1000);
            }
            else Assert.IsTrue(false);
            var secondisnull = Repo.Get(n => n.Nom == firstone.Nom).FirstOrDefault() == null;
            Assert.IsTrue(firstisnull & secondisnull);
        }
    }
}
