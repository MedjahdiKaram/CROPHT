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
    public class ExamenTest : ICrud
    {
        protected Repository<Examen> Repo { get; set; }

        public ExamenTest()
        {
            IUnitOfWork ophtalmocontext = new OphtalmoContext();
            Repo = new Repository<Examen>(ophtalmocontext);
        }

        public void Add()
        {
            var newentity = new Examen {Nom = "Topo",Moment = DateTime.Now};

            Repo.Add(newentity);
            var getentityfromrepo = Repo.Get(n => n.Nom == newentity.Nom).FirstOrDefault();
            Assert.AreNotEqual(getentityfromrepo, null);
        }

        [TestMethod]
        public void AddOrUpdate()
        {
            var newentity = new Examen { Nom = "Angio", Moment = DateTime.Now };


            Repo.AddOrUpdate(newentity);
            //var getnewmedfromrepo = Repo.Get(n => n.Nom == newentity.Nom);

            var getentityfromrepo = Repo.Get(n => n.Nom == "Angio").FirstOrDefault();
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
            Repo.Add(new Examen { Nom = "L'Examen" + new Random().Next(1000), Moment = DateTime.Now });
            Repo.Add(new Examen { Nom = "Anéry" + new Random().Next(1000), Moment = DateTime.Now });

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
