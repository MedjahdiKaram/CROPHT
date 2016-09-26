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
    public class MaladeTest : ICrud
    {
        protected Repository<Malade> Repo { get; set; }

        public MaladeTest()
        {
            IUnitOfWork ophtalmocontext = new OphtalmoContext();
            Repo = new Repository<Malade>(ophtalmocontext);
        }

        public void Add()
        {
            var newentity = new Malade {Age = 69, Prenom = "Sassi", Nom = "Ahmed"};

            Repo.Add(newentity);
            var getentityfromrepo = Repo.Get(n => n.Nom == newentity.Nom).FirstOrDefault();
            Assert.AreNotEqual(getentityfromrepo, null);
        }

        [TestMethod]
        public void AddOrUpdate()
        {
            var newentity = new Malade {Age = 69, Prenom = "Sassi", Nom = "Sidahmed"};


            Repo.AddOrUpdate(newentity);
            //var getnewmedfromrepo = Repo.Get(n => n.Nom == newentity.Nom);

            var getentityfromrepo = Repo.Get(n => n.Nom == "Sidahmed").FirstOrDefault();
            if (getentityfromrepo != null)
                getentityfromrepo.Nom = "Hamid";
            else
            {
                Assert.IsTrue(false);
            }
            Repo.AddOrUpdate(getentityfromrepo);
            getentityfromrepo = null;
            getentityfromrepo = Repo.Get(n => n.Nom == "Hamid").FirstOrDefault();

            Assert.AreNotEqual(getentityfromrepo, null);
        }

        [TestMethod]
        public void Delete()
        {
            Repo.Add(new Malade {Age = 69, Prenom = "Sas si zoubir", Nom = "Zoubir" + new Random().Next(1000)});
            Repo.Add(new Malade {Age = 69, Prenom = "Sas si", Nom = "Mrid" + new Random().Next(1000)});

            var firstone = Repo.Get().FirstOrDefault();
            
            Repo.Delete(firstone);Thread.Sleep(1000);
            var firstisnull = Repo.Get(n => n.Nom == firstone.Nom).FirstOrDefault() == null;
            firstone = Repo.Get().FirstOrDefault();
            if (firstone != null)
            {
                Thread.Sleep(1000);
                Repo.Delete(firstone.Id);
            }
            else Assert.IsTrue(false);
            var secondisnull = Repo.Get(n => n.Nom == firstone.Nom).FirstOrDefault() == null;
            Assert.IsTrue(firstisnull & secondisnull);
        }
    }
}
