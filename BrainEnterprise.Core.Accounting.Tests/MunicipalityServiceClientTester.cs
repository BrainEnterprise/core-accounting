using BrainEnterprise.Core.Accounting.Ws;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace BrainEnterprise.Core.Accounting.Tests
{
    [TestClass]
    public class MunicipalityServiceClientTester
    {

        [TestMethod]
        public void TestGetMuncipality()
        {
            MunicipalityServiceClient.UseCache = true;
            var client = new MunicipalityServiceClient();
            var mantova = client.GetMunicipality("E897");
            Assert.IsNotNull(mantova);
            Assert.IsTrue(mantova.Area.Name == "Nord-ovest");
            Assert.IsTrue(mantova.Area.Code == "1");
            Assert.IsTrue(mantova.Region.Name == "Lombardia");
            Assert.IsTrue(mantova.Region.Code == "03");
            Assert.IsTrue(mantova.Province.Name == "Mantova");
            Assert.IsTrue(mantova.Province.Abbreviation == "MN");

            var client2 = new MunicipalityServiceClient();
            var mantovaCached = client.GetMunicipality("E897");

            //var errorCatastalTest = client.GetMunicipality("E8888");
            //Assert.IsNotNull(errorCatastalTest);
        }

        [TestMethod]
        public void testGetMunicipalities()
        {
            var client = new MunicipalityServiceClient();

            var checkDoubles = client.GetMunicipalities();
            var doubles = checkDoubles.GroupBy(x => x.CadastralCode).Select(group => new { cadastralCode = group.Key, count = group.Count() }).Where(x => x.count > 1);
            Assert.IsTrue(doubles.Count() == 0);

            var checkDoubles2 = client.GetMunicipalities();

            var mantovaProvinces = client.GetMunicipalities(provinceFilter: "MN");
            Assert.IsNotNull(mantovaProvinces);
            Assert.IsTrue(mantovaProvinces.Count() == 64);

            var lombardiaProvinces = client.GetMunicipalities(regionFilter: "03");
            Assert.IsNotNull(lombardiaProvinces);
            Assert.IsTrue(lombardiaProvinces.Count() == 1506);

        }

        [TestMethod]
        public void TestGetAreas()
        {
            var client = new MunicipalityServiceClient();
            var list = client.GetAreas();
            Assert.IsTrue(list.Count() == 5);
        }

        [TestMethod]
        public void testGetRegions()
        {
            var client = new MunicipalityServiceClient();

            var _checkDoubles = client.GetRegions();
            //Assert.IsFalse(_checkDoubles.GroupBy(x => x.Code).Select(group => new { Code = group.Key, Count = group.Count() }).Where(x => x.Count > 1).Count() != 1);

            var _nordOvest = client.GetRegions("01");
            Assert.IsNotNull(_nordOvest);
            Assert.IsTrue(_nordOvest.First().Area.Name.Equals("Nord-ovest"));
            Assert.IsTrue(_nordOvest.Count() == 4);

            var _nordEst = client.GetRegions("02");
            Assert.IsNotNull(_nordEst);
            Assert.IsTrue(_nordEst.First().Area.Name.Equals("Nord-est"));
            Assert.IsTrue(_nordEst.Count() == 4);

            var _centro = client.GetRegions("03");
            Assert.IsNotNull(_centro);
            Assert.IsTrue(_centro.First().Area.Name.Equals("Centro"));
            Assert.IsTrue(_centro.Count() == 4);

            var _sud = client.GetRegions("04");
            Assert.IsNotNull(_sud);
            Assert.IsTrue(_sud.First().Area.Name.Equals("Sud"));
            Assert.IsTrue(_sud.Count() == 6);

            var _isole = client.GetRegions("05");
            Assert.IsNotNull(_isole);
            Assert.IsTrue(_isole.First().Area.Name.Equals("Isole"));
            Assert.IsTrue(_isole.Count() == 2);
        }

        [TestMethod]
        public void testGetProvinces()
        {
            var client = new MunicipalityServiceClient();

            var _checkDoubles = client.GetProvinces();
            //Assert.IsFalse(_checkDoubles.GroupBy(x => x.Abbreviation).Select(group => new { abbreviation = group.Key, Count = group.Count() }).Where(x => x.Count > 1).Count() != 1);

            var _Piemonte = client.GetProvinces(regionFilter: "01");
            Assert.IsNotNull(_Piemonte);
            Assert.IsTrue(_Piemonte.Count() == 8);

            var _Lombardia = client.GetProvinces("", "03");
            Assert.IsNotNull(_Lombardia);
            Assert.IsTrue(_Lombardia.Count() == 12);

            var _EmiliaRomagna = client.GetProvinces("", "08");
            Assert.IsNotNull(_EmiliaRomagna);
            Assert.IsTrue(_EmiliaRomagna.Count() == 10);

            var _Isole = client.GetProvinces("5", "");
            Assert.IsNotNull(_Isole);
            Assert.IsTrue(_Isole.Count() == 14);
        }

    }
}
