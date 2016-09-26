using System;
using System.Linq;
using DAL;
using InterfacesLib.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelsLib;

namespace OphtalmoTesting
{
    [TestClass]
    public class RelationShipTest
    {
        protected Repository<CompteRendu> CompteRendusRepo { get; set; }
        protected Repository<Malade> MaladeRepo { get; set; }
        protected Repository<Medecin> MedecinRepo { get; set; }
        protected Repository<Examen> ExamsRepo { get; set; }     
        public RelationShipTest()
        {
            IUnitOfWork ophtalmocontext = new OphtalmoContext();
            CompteRendusRepo = new Repository<CompteRendu>(ophtalmocontext);
            ExamsRepo= new Repository<Examen>(ophtalmocontext);
            MaladeRepo = new Repository<Malade>(ophtalmocontext);
            MedecinRepo=new Repository<Medecin>(ophtalmocontext);
        }

        [TestMethod]
        public void AddAllInOne()
        {
            var medecin = new Medecin {Nom = "Toubib"};
            var malade = new Malade {Nom = "Hmdaoui", Prenom = "Kalbou3", Age = 96};
            var exam = new Examen {Moment = DateTime.Now, Nom = "Examen Compliqué", Medecin = medecin, Malade = malade};
            var cr1 = new CompteRendu {Nom = "Un Compte Rendu au Hazard", Contenu = "Voilà le contenu", Examen = exam};           
            CompteRendusRepo.Add(cr1);
            var cr1Get = CompteRendusRepo.Get(n => n.Nom == "Un Compte Rendu au Hazard").FirstOrDefault();
            var virified = cr1Get.Examen.Moment== exam.Moment;
            
                virified = virified && cr1Get.Examen.Medecin.Nom == medecin.Nom && cr1Get.Examen.Malade.Nom == malade.Nom;
            Assert.IsTrue(virified);
        }
         
        [TestMethod]
        public void SeExamToCompteRenduAndAdd()
        {
            
            var exam = ExamsRepo.Get().FirstOrDefault();
            var cr = new CompteRendu { Nom = "CR " + new Random().Next(1000), Contenu = "le contenu expemplaire", Examen = exam };
            CompteRendusRepo.Add(cr);
            var crGet = CompteRendusRepo.Get(n => n.Nom == cr.Nom).FirstOrDefault();
            var wascreatedandlinked = crGet != null && (crGet.Nom == cr.Nom && crGet.Examen != null && crGet.Examen.Nom == exam.Nom);

            Assert.IsTrue(wascreatedandlinked);
        }

        [TestMethod]
        public void SimpleCreationScenario()
        {
            var malade = MaladeRepo.Get().FirstOrDefault();
            var medecin = MedecinRepo.Get().FirstOrDefault();
            var exam = new Examen
            {
                Nom = "L'examen"+new Random().Next(-1000,100),
                Medecin = medecin,
                Malade = malade,
                
            };
            exam.Moment =  DateTime.Now;
            var cr1 = new CompteRendu { Nom = "CR " + new Random().Next(1000,9000), Contenu = "le contenu expemplaire numéro "+ new Random().Next(1000,9000), Examen = exam };
            var cr2 = new CompteRendu { Nom = "CR " + new Random().Next(1000, 9000), Contenu = "le contenu expemplaire numéro " + new Random().Next(100, 900), Examen = exam };
            var cr3 = new CompteRendu { Nom = "CR " + new Random().Next(1000, 9000), Contenu = "le contenu expemplaire numéro " + new Random().Next(10, 90), Examen = exam };
            var cr4 = new CompteRendu { Nom = "CR " + new Random().Next(1000, 9000), Contenu = "le contenu expemplaire numéro " + new Random().Next(50, 99), Examen = exam };
            CompteRendusRepo.Add(cr1);
            var cr1Get = CompteRendusRepo.Get(n => n.Nom == cr1.Nom).FirstOrDefault();
            Assert.AreNotEqual(cr1Get,null);
            var cr2Get = CompteRendusRepo.Get(n => n.Nom == cr2.Nom).FirstOrDefault();
            Assert.AreNotEqual(cr2Get, null);
            var cr3Get = CompteRendusRepo.Get(n => n.Nom == cr3.Nom).FirstOrDefault();
            Assert.AreNotEqual(cr3Get, null);
            var cr4Get = CompteRendusRepo.Get(n => n.Nom == cr4.Nom).FirstOrDefault();
            Assert.AreNotEqual(cr4Get, null);
            

        }
    }
}
