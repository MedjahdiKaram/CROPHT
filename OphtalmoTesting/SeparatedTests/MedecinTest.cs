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
    public class MedecinTest:ICrud
    

{
        protected Repository<Medecin> Repo { get; set; }
    //protected Repository<Malade> MaladeRepo { get; set; }
    //protected Repository<Examen> ExamenRepo { get; set; }
    //protected Repository<CompteRendu> CompteRenduRepo { get; set; }


    public MedecinTest()
    {
        IUnitOfWork ophtalmocontext = new OphtalmoContext();
        Repo = new Repository<Medecin>(ophtalmocontext);
    }

        [TestMethod]
    public void Add()
    {
        var newmedecin = new Medecin {Nom = "Ahmed"};

        Repo.Add(newmedecin);
        var getnewmedfromrepo = Repo.Get(n => n.Nom == newmedecin.Nom).FirstOrDefault();
        Assert.AreNotEqual(getnewmedfromrepo, null);

    }

    [TestMethod]
    public void AddOrUpdate()
    {
        var newmedecin = new Medecin {Nom = "Sidahmed"};


        Repo.AddOrUpdate(newmedecin);
        var getnewmedfromrepo = Repo.Get(n => n.Nom == newmedecin.Nom);

        var getmedfreomrepo = Repo.Get(n => n.Nom == "Sidahmed").FirstOrDefault();
        if (getmedfreomrepo != null)
            getmedfreomrepo.Nom = "Hamid";
        else
        {
            Assert.IsTrue(false);
        }
        Repo.AddOrUpdate(getmedfreomrepo);
        getmedfreomrepo = null;
        getmedfreomrepo = Repo.Get(n => n.Nom == "Hamid").FirstOrDefault();

        Assert.AreNotEqual(getmedfreomrepo, null);

    }

    [TestMethod]
    public void Delete()
    {

        Repo.Add(new Medecin { Nom = "Medecin "+new Random().Next(1000) });
        Repo.Add(new Medecin { Nom = "Tabib "+new Random().Next(1000) });
        var medecin = Repo.Get().FirstOrDefault();
       
        Repo.Delete(medecin); Thread.Sleep(1000);
        var medecinnull = Repo.Get(n => n.Nom == medecin.Nom).FirstOrDefault() == null;
        medecin = Repo.Get().FirstOrDefault();
        if (medecin != null)
        {
            Repo.Delete(medecin.Id);
            Thread.Sleep(1000);
        }
        else Assert.IsTrue(false);
        var sendmedecinnull = Repo.Get(n => n.Nom == medecin.Nom).FirstOrDefault() == null;
        Assert.IsTrue(medecinnull & sendmedecinnull);
    }

    //    [TestMethod]
    //    public void AddOrUpdate()
    //    {

    //    }
}
}
