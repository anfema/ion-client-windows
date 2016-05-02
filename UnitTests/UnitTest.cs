using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Anfema.Ion.DataModel;
using Anfema.Ion.Parsing;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Anfema.Ion.Authorization;
using System.Threading.Tasks;


namespace UnitTests
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void collectionStringEqualsCheck()
        {
            string collectionStringOriginal = "{\"collection\":[{\"identifier\":\"lookbook\",\"default_locale\":\"de_DE\",\"last_changed\":\"2016-04-18T16:56:52Z\",\"fts_db\":\"https://lookbook-dev.anfema.com/protected_media/fts/lookbook.sqlite3\",\"archive\":\"https://lookbook-dev.anfema.com/client/v1/de_DE/lookbook.tar?variation=default\",\"pages\":[{\"identifier\":\"barkers-department-store\",\"collection_identifier\":\"lookbook\",\"version_number\":1,\"last_changed\":\"2016-04-18T12:02:19Z\",\"position\":0,\"layout\":\"project\",\"locale\":\"de_DE\",\"parent\":\"commercial\",\"meta\":{\"materials\":\"//lookbook/material-thermatex-dummy\",\"description\":\"<p>BarkersDepartmentStore,<br>England,<br>THERMATEX®Alpha</p>\"}},{\"identifier\":\"harley-davidson-zentrale\",\"collection_identifier\":\"lookbook\",\"version_number\":1,\"last_changed\":\"2016-04-18T11:52:15Z\",\"position\":1,\"layout\":\"project\",\"locale\":\"de_DE\",\"parent\":\"commercial\",\"meta\":{\"description\":\"<p>HarleyDavidsonZentrale,<br>Neu-Isenburg,Deutschland,<br>HERADESIGN®xy</p>\"}},{\"identifier\":\"baudoin-wash-systems-bv\",\"collection_identifier\":\"lookbook\",\"version_number\":1,\"last_changed\":\"2016-04-18T11:52:24Z\",\"position\":2,\"layout\":\"project\",\"locale\":\"de_DE\",\"parent\":\"commercial\",\"meta\":{\"description\":\"<p>BaudoinWashSystemsB.V.,<br>Niederlande,<br>THERMATEX®Alpha</p>\"}},{\"identifier\":\"konsum-leipzig\",\"collection_identifier\":\"lookbook\",\"version_number\":1,\"last_changed\":\"2016-03-30T14:37:04Z\",\"position\":3,\"layout\":\"project\",\"locale\":\"de_DE\",\"parent\":\"commercial\",\"meta\":{\"description\":\"KonsumLeipzig,\\nLeipzig,Deutschland,\\nTHERMATEX®Alphacreme\"}},{\"identifier\":\"erika-cavallini\",\"collection_identifier\":\"lookbook\",\"version_number\":1,\"last_changed\":\"2016-03-31T12:35:45Z\",\"position\":4,\"layout\":\"project\",\"locale\":\"de_DE\",\"parent\":\"commercial\",\"meta\":{\"description\":\"<p>ErikaCavalliniMailand,<br>Italien,<br>HERADESIGN®xy</p>\"}},{\"identifier\":\"essa-primary-school\",\"collection_identifier\":\"lookbook\",\"version_number\":1,\"last_changed\":\"2016-04-18T16:51:37Z\",\"position\":0,\"layout\":\"project\",\"locale\":\"de_DE\",\"parent\":\"education\",\"meta\":{\"description\":\"<p>EssaPrimarySchool,<br>England,<br>THERMATEX®AntarisHERADESIGN®fine</p>\"}},{\"identifier\":\"grundschule-am-arnulfpark\",\"collection_identifier\":\"lookbook\",\"version_number\":1,\"last_changed\":\"2016-04-18T16:56:52Z\",\"position\":1,\"layout\":\"project\",\"locale\":\"de_DE\",\"parent\":\"education\",\"meta\":{\"description\":\"<p>GrundschuleamArnulfpark,<br>Mu¨nchen,Deutschland,<br>HERADESIGN®superfine</p>\"}},{\"identifier\":\"schule-fischerhude\",\"collection_identifier\":\"lookbook\",\"version_number\":1,\"last_changed\":\"2016-03-31T09:23:40Z\",\"position\":2,\"layout\":\"project\",\"locale\":\"de_DE\",\"parent\":\"education\",\"meta\":{\"description\":\"<p>SchuleFischerhude,<br>Deutschland,<br>THERMATEX®Baffel</p>\"}},{\"identifier\":\"kita-doberlug\",\"collection_identifier\":\"lookbook\",\"version_number\":1,\"last_changed\":\"2016-03-31T09:24:18Z\",\"position\":3,\"layout\":\"project\",\"locale\":\"de_DE\",\"parent\":\"education\",\"meta\":{\"description\":\"<p>KitaDoberlug,<br>Deutschland,<br>THERMATEX®VariolineMotiv</p>\"}},{\"identifier\":\"novopecherskaya-schule\",\"collection_identifier\":\"lookbook\",\"version_number\":1,\"last_changed\":\"2016-03-31T09:25:34Z\",\"position\":4,\"layout\":\"project\",\"locale\":\"de_DE\",\"parent\":\"education\",\"meta\":{\"description\":\"<p>NovopecherskayaSchule,<br>Kiew,Ukraine,<br>THERMATEX®Antaris<br>THERMATEX®Sonicelement<br>HERADESIGN®fineHERADESIGN®fine</p>\"}},{\"identifier\":\"universitat-leiden\",\"collection_identifier\":\"lookbook\",\"version_number\":1,\"last_changed\":\"2016-03-31T09:26:29Z\",\"position\":5,\"layout\":\"project\",\"locale\":\"de_DE\",\"parent\":\"education\",\"meta\":{\"description\":\"<p>Universita¨tLeiden,<br>Niederlande,<br>HERADESIGN®fine</p>\"}},{\"identifier\":\"incor-hospital\",\"collection_identifier\":\"lookbook\",\"version_number\":1,\"last_changed\":\"2016-03-31T09:35:01Z\",\"position\":0,\"layout\":\"project\",\"locale\":\"de_DE\",\"parent\":\"infrastructure\",\"meta\":{\"description\":\"<p>IncorHospital,<br>Sa~oPaulo,Brazil,<br>THERMATEX®Thermofon</p>\"}},{\"identifier\":\"multiplex-atmosphera-cinema\",\"collection_identifier\":\"lookbook\",\"version_number\":1,\"last_changed\":\"2016-03-31T09:30:04Z\",\"position\":0,\"layout\":\"project\",\"locale\":\"de_DE\",\"parent\":\"lifestyle\",\"meta\":{\"description\":\"<p>MULTIPLEX-AtmospheraCinema,<br>Kiew,Ukraine,<br>HERADESIGN®fine,<br>THERMATEX®Alpha,THERMATEX®AlphaBlack</p>\"}},{\"identifier\":\"bowling-bahn\",\"collection_identifier\":\"lookbook\",\"version_number\":1,\"last_changed\":\"2016-03-31T09:31:31Z\",\"position\":0,\"layout\":\"project\",\"locale\":\"de_DE\",\"parent\":\"sports-leisure\",\"meta\":{\"description\":\"<p>BowlingBahn,<br>Ukraine,<br>HERADESIGN®superfine<br>HERADESIGN®Deckensegel<br>THERMATEX®Alpha</p>\"}},{\"identifier\":\"mediacom-gmbh\",\"collection_identifier\":\"lookbook\",\"version_number\":1,\"last_changed\":\"2016-03-31T09:32:59Z\",\"position\":0,\"layout\":\"project\",\"locale\":\"de_DE\",\"parent\":\"office\",\"meta\":{\"description\":\"<p>MediacomGmbH,<br>Brilon,Deutschland,<br>THERMATEX®Sonicelement,<br>THERMATEX®LineWandabsorbermitMotiv</p>\"}},{\"identifier\":\"material-thermatex-dummy\",\"collection_identifier\":\"lookbook\",\"version_number\":1,\"last_changed\":\"2016-04-14T06:11:13Z\",\"position\":0,\"layout\":\"material\",\"locale\":\"de_DE\",\"parent\":\"settings\",\"meta\":{\"title\":\"Title\",\"subtitle\":\"Subtitle\"}},{\"identifier\":\"dummy-material-2\",\"collection_identifier\":\"lookbook\",\"version_number\":1,\"last_changed\":\"2016-04-14T15:44:00Z\",\"position\":1,\"layout\":\"material\",\"locale\":\"de_DE\",\"parent\":\"settings\",\"meta\":{\"title\":\"Title\",\"subtitle\":\"Subtitle\"}},{\"identifier\":\"commercial\",\"collection_identifier\":\"lookbook\",\"version_number\":1,\"last_changed\":\"2016-04-14T08:49:26Z\",\"position\":0,\"layout\":\"category\",\"locale\":\"de_DE\",\"parent\":null,\"meta\":{\"title\":\"Commercial\",\"menu-mask\":\"https://lookbook-dev.anfema.com/protected_media/images/58009e0c-953c-401a-825f-7594e9bfa2a4/01_commercial.png\"}},{\"identifier\":\"education\",\"collection_identifier\":\"lookbook\",\"version_number\":1,\"last_changed\":\"2016-03-24T14:30:38Z\",\"position\":1,\"layout\":\"category\",\"locale\":\"de_DE\",\"parent\":null,\"meta\":{\"title\":\"Education\",\"menu-mask\":\"https://lookbook-dev.anfema.com/protected_media/images/b5452444-7122-4a1d-b7a9-24889869939b/01_commercial.png\"}},{\"identifier\":\"infrastructure\",\"collection_identifier\":\"lookbook\",\"version_number\":1,\"last_changed\":\"2016-03-24T14:30:58Z\",\"position\":2,\"layout\":\"category\",\"locale\":\"de_DE\",\"parent\":null,\"meta\":{\"title\":\"Infrastructure\",\"menu-mask\":\"https://lookbook-dev.anfema.com/protected_media/images/fe5efb73-4ef4-4a0c-bbaa-f804ab91b024/01_commercial.png\"}},{\"identifier\":\"lifestyle\",\"collection_identifier\":\"lookbook\",\"version_number\":1,\"last_changed\":\"2016-03-24T14:31:16Z\",\"position\":3,\"layout\":\"category\",\"locale\":\"de_DE\",\"parent\":null,\"meta\":{\"title\":\"Lifestyle\",\"menu-mask\":\"https://lookbook-dev.anfema.com/protected_media/images/8d4ca813-211c-44d7-acf2-feb465b1e952/01_commercial.png\"}},{\"identifier\":\"sports-leisure\",\"collection_identifier\":\"lookbook\",\"version_number\":1,\"last_changed\":\"2016-03-24T14:31:35Z\",\"position\":4,\"layout\":\"category\",\"locale\":\"de_DE\",\"parent\":null,\"meta\":{\"title\":\"Sports&Leisure\",\"menu-mask\":\"https://lookbook-dev.anfema.com/protected_media/images/3f5e9f98-c050-4963-9556-79d7222d4270/01_commercial.png\"}},{\"identifier\":\"office\",\"collection_identifier\":\"lookbook\",\"version_number\":1,\"last_changed\":\"2016-03-24T14:31:53Z\",\"position\":5,\"layout\":\"category\",\"locale\":\"de_DE\",\"parent\":null,\"meta\":{\"title\":\"Office\",\"menu-mask\":\"https://lookbook-dev.anfema.com/protected_media/images/3d0a8578-b9ad-4d3c-86bc-7ca746170de8/01_commercial.png\"}},{\"identifier\":\"about\",\"collection_identifier\":\"lookbook\",\"version_number\":1,\"last_changed\":\"2016-04-14T16:32:43Z\",\"position\":6,\"layout\":\"about\",\"locale\":\"de_DE\",\"parent\":null,\"meta\":{\"title\":\"Legal\"}},{\"identifier\":\"settings\",\"collection_identifier\":\"lookbook\",\"version_number\":1,\"last_changed\":\"2016-04-11T14:55:35Z\",\"position\":7,\"layout\":\"settings\",\"locale\":\"de_DE\",\"parent\":null,\"meta\":{}}]}]}";
            
            IonCollection collection = DataParser.parseCollection( collectionStringOriginal );
            
            CollectionRoot root = new CollectionRoot();
            root.collection.Add( collection );

            string collectionStringResult = JsonConvert.SerializeObject( root );

            Assert.AreEqual( collectionStringOriginal, collectionStringResult );
        }


        [TestMethod]
        public void collectionEqualsCheck()
        {
            string collectionStringOriginal = "{\"collection\":[{\"identifier\":\"lookbook\",\"default_locale\":\"de_DE\",\"last_changed\":\"2016-04-18T16:56:52Z\",\"fts_db\":\"https://lookbook-dev.anfema.com/protected_media/fts/lookbook.sqlite3\",\"archive\":\"https://lookbook-dev.anfema.com/client/v1/de_DE/lookbook.tar?variation=default\",\"pages\":[{\"identifier\":\"barkers-department-store\",\"collection_identifier\":\"lookbook\",\"version_number\":1,\"last_changed\":\"2016-04-18T12:02:19Z\",\"position\":0,\"layout\":\"project\",\"locale\":\"de_DE\",\"parent\":\"commercial\",\"meta\":{\"materials\":\"//lookbook/material-thermatex-dummy\",\"description\":\"<p>BarkersDepartmentStore,<br>England,<br>THERMATEX®Alpha</p>\"}},{\"identifier\":\"harley-davidson-zentrale\",\"collection_identifier\":\"lookbook\",\"version_number\":1,\"last_changed\":\"2016-04-18T11:52:15Z\",\"position\":1,\"layout\":\"project\",\"locale\":\"de_DE\",\"parent\":\"commercial\",\"meta\":{\"description\":\"<p>HarleyDavidsonZentrale,<br>Neu-Isenburg,Deutschland,<br>HERADESIGN®xy</p>\"}},{\"identifier\":\"baudoin-wash-systems-bv\",\"collection_identifier\":\"lookbook\",\"version_number\":1,\"last_changed\":\"2016-04-18T11:52:24Z\",\"position\":2,\"layout\":\"project\",\"locale\":\"de_DE\",\"parent\":\"commercial\",\"meta\":{\"description\":\"<p>BaudoinWashSystemsB.V.,<br>Niederlande,<br>THERMATEX®Alpha</p>\"}},{\"identifier\":\"konsum-leipzig\",\"collection_identifier\":\"lookbook\",\"version_number\":1,\"last_changed\":\"2016-03-30T14:37:04Z\",\"position\":3,\"layout\":\"project\",\"locale\":\"de_DE\",\"parent\":\"commercial\",\"meta\":{\"description\":\"KonsumLeipzig,\\nLeipzig,Deutschland,\\nTHERMATEX®Alphacreme\"}},{\"identifier\":\"erika-cavallini\",\"collection_identifier\":\"lookbook\",\"version_number\":1,\"last_changed\":\"2016-03-31T12:35:45Z\",\"position\":4,\"layout\":\"project\",\"locale\":\"de_DE\",\"parent\":\"commercial\",\"meta\":{\"description\":\"<p>ErikaCavalliniMailand,<br>Italien,<br>HERADESIGN®xy</p>\"}},{\"identifier\":\"essa-primary-school\",\"collection_identifier\":\"lookbook\",\"version_number\":1,\"last_changed\":\"2016-04-18T16:51:37Z\",\"position\":0,\"layout\":\"project\",\"locale\":\"de_DE\",\"parent\":\"education\",\"meta\":{\"description\":\"<p>EssaPrimarySchool,<br>England,<br>THERMATEX®AntarisHERADESIGN®fine</p>\"}},{\"identifier\":\"grundschule-am-arnulfpark\",\"collection_identifier\":\"lookbook\",\"version_number\":1,\"last_changed\":\"2016-04-18T16:56:52Z\",\"position\":1,\"layout\":\"project\",\"locale\":\"de_DE\",\"parent\":\"education\",\"meta\":{\"description\":\"<p>GrundschuleamArnulfpark,<br>Mu¨nchen,Deutschland,<br>HERADESIGN®superfine</p>\"}},{\"identifier\":\"schule-fischerhude\",\"collection_identifier\":\"lookbook\",\"version_number\":1,\"last_changed\":\"2016-03-31T09:23:40Z\",\"position\":2,\"layout\":\"project\",\"locale\":\"de_DE\",\"parent\":\"education\",\"meta\":{\"description\":\"<p>SchuleFischerhude,<br>Deutschland,<br>THERMATEX®Baffel</p>\"}},{\"identifier\":\"kita-doberlug\",\"collection_identifier\":\"lookbook\",\"version_number\":1,\"last_changed\":\"2016-03-31T09:24:18Z\",\"position\":3,\"layout\":\"project\",\"locale\":\"de_DE\",\"parent\":\"education\",\"meta\":{\"description\":\"<p>KitaDoberlug,<br>Deutschland,<br>THERMATEX®VariolineMotiv</p>\"}},{\"identifier\":\"novopecherskaya-schule\",\"collection_identifier\":\"lookbook\",\"version_number\":1,\"last_changed\":\"2016-03-31T09:25:34Z\",\"position\":4,\"layout\":\"project\",\"locale\":\"de_DE\",\"parent\":\"education\",\"meta\":{\"description\":\"<p>NovopecherskayaSchule,<br>Kiew,Ukraine,<br>THERMATEX®Antaris<br>THERMATEX®Sonicelement<br>HERADESIGN®fineHERADESIGN®fine</p>\"}},{\"identifier\":\"universitat-leiden\",\"collection_identifier\":\"lookbook\",\"version_number\":1,\"last_changed\":\"2016-03-31T09:26:29Z\",\"position\":5,\"layout\":\"project\",\"locale\":\"de_DE\",\"parent\":\"education\",\"meta\":{\"description\":\"<p>Universita¨tLeiden,<br>Niederlande,<br>HERADESIGN®fine</p>\"}},{\"identifier\":\"incor-hospital\",\"collection_identifier\":\"lookbook\",\"version_number\":1,\"last_changed\":\"2016-03-31T09:35:01Z\",\"position\":0,\"layout\":\"project\",\"locale\":\"de_DE\",\"parent\":\"infrastructure\",\"meta\":{\"description\":\"<p>IncorHospital,<br>Sa~oPaulo,Brazil,<br>THERMATEX®Thermofon</p>\"}},{\"identifier\":\"multiplex-atmosphera-cinema\",\"collection_identifier\":\"lookbook\",\"version_number\":1,\"last_changed\":\"2016-03-31T09:30:04Z\",\"position\":0,\"layout\":\"project\",\"locale\":\"de_DE\",\"parent\":\"lifestyle\",\"meta\":{\"description\":\"<p>MULTIPLEX-AtmospheraCinema,<br>Kiew,Ukraine,<br>HERADESIGN®fine,<br>THERMATEX®Alpha,THERMATEX®AlphaBlack</p>\"}},{\"identifier\":\"bowling-bahn\",\"collection_identifier\":\"lookbook\",\"version_number\":1,\"last_changed\":\"2016-03-31T09:31:31Z\",\"position\":0,\"layout\":\"project\",\"locale\":\"de_DE\",\"parent\":\"sports-leisure\",\"meta\":{\"description\":\"<p>BowlingBahn,<br>Ukraine,<br>HERADESIGN®superfine<br>HERADESIGN®Deckensegel<br>THERMATEX®Alpha</p>\"}},{\"identifier\":\"mediacom-gmbh\",\"collection_identifier\":\"lookbook\",\"version_number\":1,\"last_changed\":\"2016-03-31T09:32:59Z\",\"position\":0,\"layout\":\"project\",\"locale\":\"de_DE\",\"parent\":\"office\",\"meta\":{\"description\":\"<p>MediacomGmbH,<br>Brilon,Deutschland,<br>THERMATEX®Sonicelement,<br>THERMATEX®LineWandabsorbermitMotiv</p>\"}},{\"identifier\":\"material-thermatex-dummy\",\"collection_identifier\":\"lookbook\",\"version_number\":1,\"last_changed\":\"2016-04-14T06:11:13Z\",\"position\":0,\"layout\":\"material\",\"locale\":\"de_DE\",\"parent\":\"settings\",\"meta\":{\"title\":\"Title\",\"subtitle\":\"Subtitle\"}},{\"identifier\":\"dummy-material-2\",\"collection_identifier\":\"lookbook\",\"version_number\":1,\"last_changed\":\"2016-04-14T15:44:00Z\",\"position\":1,\"layout\":\"material\",\"locale\":\"de_DE\",\"parent\":\"settings\",\"meta\":{\"title\":\"Title\",\"subtitle\":\"Subtitle\"}},{\"identifier\":\"commercial\",\"collection_identifier\":\"lookbook\",\"version_number\":1,\"last_changed\":\"2016-04-14T08:49:26Z\",\"position\":0,\"layout\":\"category\",\"locale\":\"de_DE\",\"parent\":null,\"meta\":{\"title\":\"Commercial\",\"menu-mask\":\"https://lookbook-dev.anfema.com/protected_media/images/58009e0c-953c-401a-825f-7594e9bfa2a4/01_commercial.png\"}},{\"identifier\":\"education\",\"collection_identifier\":\"lookbook\",\"version_number\":1,\"last_changed\":\"2016-03-24T14:30:38Z\",\"position\":1,\"layout\":\"category\",\"locale\":\"de_DE\",\"parent\":null,\"meta\":{\"title\":\"Education\",\"menu-mask\":\"https://lookbook-dev.anfema.com/protected_media/images/b5452444-7122-4a1d-b7a9-24889869939b/01_commercial.png\"}},{\"identifier\":\"infrastructure\",\"collection_identifier\":\"lookbook\",\"version_number\":1,\"last_changed\":\"2016-03-24T14:30:58Z\",\"position\":2,\"layout\":\"category\",\"locale\":\"de_DE\",\"parent\":null,\"meta\":{\"title\":\"Infrastructure\",\"menu-mask\":\"https://lookbook-dev.anfema.com/protected_media/images/fe5efb73-4ef4-4a0c-bbaa-f804ab91b024/01_commercial.png\"}},{\"identifier\":\"lifestyle\",\"collection_identifier\":\"lookbook\",\"version_number\":1,\"last_changed\":\"2016-03-24T14:31:16Z\",\"position\":3,\"layout\":\"category\",\"locale\":\"de_DE\",\"parent\":null,\"meta\":{\"title\":\"Lifestyle\",\"menu-mask\":\"https://lookbook-dev.anfema.com/protected_media/images/8d4ca813-211c-44d7-acf2-feb465b1e952/01_commercial.png\"}},{\"identifier\":\"sports-leisure\",\"collection_identifier\":\"lookbook\",\"version_number\":1,\"last_changed\":\"2016-03-24T14:31:35Z\",\"position\":4,\"layout\":\"category\",\"locale\":\"de_DE\",\"parent\":null,\"meta\":{\"title\":\"Sports&Leisure\",\"menu-mask\":\"https://lookbook-dev.anfema.com/protected_media/images/3f5e9f98-c050-4963-9556-79d7222d4270/01_commercial.png\"}},{\"identifier\":\"office\",\"collection_identifier\":\"lookbook\",\"version_number\":1,\"last_changed\":\"2016-03-24T14:31:53Z\",\"position\":5,\"layout\":\"category\",\"locale\":\"de_DE\",\"parent\":null,\"meta\":{\"title\":\"Office\",\"menu-mask\":\"https://lookbook-dev.anfema.com/protected_media/images/3d0a8578-b9ad-4d3c-86bc-7ca746170de8/01_commercial.png\"}},{\"identifier\":\"about\",\"collection_identifier\":\"lookbook\",\"version_number\":1,\"last_changed\":\"2016-04-14T16:32:43Z\",\"position\":6,\"layout\":\"about\",\"locale\":\"de_DE\",\"parent\":null,\"meta\":{\"title\":\"Legal\"}},{\"identifier\":\"settings\",\"collection_identifier\":\"lookbook\",\"version_number\":1,\"last_changed\":\"2016-04-11T14:55:35Z\",\"position\":7,\"layout\":\"settings\",\"locale\":\"de_DE\",\"parent\":null,\"meta\":{}}]}]}";

            IonCollection collection = DataParser.parseCollection( collectionStringOriginal );

            string collectionStringResult = JsonConvert.SerializeObject( collection );
            IonCollection collection2 = JsonConvert.DeserializeObject<IonCollection>( collectionStringResult );

            Assert.AreEqual( collection, collection2 );
        }


        [TestMethod]
        public void pageEqualsCheck()
        {
            string pageStringOriginal = "{\"page\":[{\"parent\":\"education\",\"identifier\":\"grundschule-am-arnulfpark\",\"collection\":\"lookbook\",\"last_changed\":\"2016-04-28T06:48:03Z\",\"archive\":\"https://lookbook-dev.anfema.com/client/v1/de_DE/lookbook/grundschule-am-arnulfpark.tar\",\"contents\":[{\"outlet\":\"project\",\"type\":\"containercontent\",\"children\":[{\"type\":\"filecontent\",\"is_searchable\":false,\"outlet\":\"project-pdf\",\"is_available\":false,\"variation\":\"default\"},{\"type\":\"connectioncontent\",\"is_searchable\":false,\"outlet\":\"materials\",\"is_available\":false,\"variation\":\"default\"},{\"type\":\"textcontent\",\"mime_type\":\"text/html\",\"is_multiline\":true,\"is_searchable\":false,\"outlet\":\"description\",\"text\":\"<p>PrimarySchoolArnulfpark,<br>Munich,Germany,<br>HERADESIGN®<i>superfine</i></p>\",\"variation\":\"default\"},{\"width\":2048,\"scale\":0,\"is_searchable\":false,\"height\":1536,\"type\":\"imagecontent\",\"mime_type\":\"image/jpeg\",\"checksum\":\"sha256:02f207f9a133241fd651a001ce79f780a63741873aed5615e0d88d14f05f71ae\",\"original_file_size\":243439,\"original_checksum\":\"sha256:02f207f9a133241fd651a001ce79f780a63741873aed5615e0d88d14f05f71ae\",\"file_size\":243439,\"original_image\":\"https://lookbook-dev.anfema.com/protected_media/images/e9fb32e5-2fa2-4628-b778-4dfbe7b682aa/02-02-1_TPOMWZN.jpg\",\"outlet\":\"images\",\"original_width\":2048,\"position\":0,\"variation\":\"default\",\"translation_y\":0,\"image\":\"https://lookbook-dev.anfema.com/protected_media/images/e9fb32e5-2fa2-4628-b778-4dfbe7b682aa/02-02-1_TPOMWZN.jpg\",\"original_height\":1536,\"translation_x\":0,\"original_mime_type\":\"image/jpeg\"},{\"width\":2048,\"scale\":0,\"is_searchable\":false,\"height\":1536,\"type\":\"imagecontent\",\"mime_type\":\"image/jpeg\",\"checksum\":\"sha256:f0b73615e96fadccca4fda81e8103b6d02b64894ef2f7356d4e9840d57cf60aa\",\"original_file_size\":466526,\"original_checksum\":\"sha256:f0b73615e96fadccca4fda81e8103b6d02b64894ef2f7356d4e9840d57cf60aa\",\"file_size\":466526,\"original_image\":\"https://lookbook-dev.anfema.com/protected_media/images/e9fb32e5-2fa2-4628-b778-4dfbe7b682aa/02-02-2_Y0LpxdS.jpg\",\"outlet\":\"images\",\"original_width\":2048,\"position\":1,\"variation\":\"default\",\"translation_y\":0,\"image\":\"https://lookbook-dev.anfema.com/protected_media/images/e9fb32e5-2fa2-4628-b778-4dfbe7b682aa/02-02-2_Y0LpxdS.jpg\",\"original_height\":1536,\"translation_x\":0,\"original_mime_type\":\"image/jpeg\"},{\"width\":2048,\"scale\":0,\"is_searchable\":false,\"height\":1536,\"type\":\"imagecontent\",\"mime_type\":\"image/jpeg\",\"checksum\":\"sha256:21c9d0da97ec753620d6bfbe61afd6d8a7ee27515b48b79ba1d3ed7da0ca9438\",\"original_file_size\":359975,\"original_checksum\":\"sha256:21c9d0da97ec753620d6bfbe61afd6d8a7ee27515b48b79ba1d3ed7da0ca9438\",\"file_size\":359975,\"original_image\":\"https://lookbook-dev.anfema.com/protected_media/images/e9fb32e5-2fa2-4628-b778-4dfbe7b682aa/02-02-3_GnnXUmT.jpg\",\"outlet\":\"images\",\"original_width\":2048,\"position\":2,\"variation\":\"default\",\"translation_y\":0,\"image\":\"https://lookbook-dev.anfema.com/protected_media/images/e9fb32e5-2fa2-4628-b778-4dfbe7b682aa/02-02-3_GnnXUmT.jpg\",\"original_height\":1536,\"translation_x\":0,\"original_mime_type\":\"image/jpeg\"}],\"variation\":\"default\"}],\"children\":[],\"locale\":\"de_DE\",\"layout\":\"project\",\"position\":1}]}";

            IonPage page = DataParser.parsePage( pageStringOriginal );
            string pageStringResult = JsonConvert.SerializeObject( page );
            IonPage page2 = JsonConvert.DeserializeObject<IonPage>( pageStringResult );

            Assert.AreEqual( page, page2 );
        }


        [TestMethod]
        // Config testing method requires internet and actually the knauf lookbook server
        public async Task configEqualsCheck()
        {
            string collectionIdentifier = "lookbook";
            AuthenticationHeaderValue authorizationHeader = await TokenAuthorization.GetAuthHeaderValue( "knauf-translator@anfe.ma", "Jdt9y9qHt3", "https://lookbook-dev.anfema.com/client/v1/login" );
            IonConfig config = new IonConfig( "https://lookbook-dev.anfema.com/client/v1/", "de_DE", collectionIdentifier, "default", authorizationHeader, 120, 100, false );

            string collectionIdentifier2 = "lookbook";
            AuthenticationHeaderValue authorizationHeader2 = await TokenAuthorization.GetAuthHeaderValue( "knauf-translator@anfe.ma", "Jdt9y9qHt3", "https://lookbook-dev.anfema.com/client/v1/login" );
            IonConfig config2 = new IonConfig( "https://lookbook-dev.anfema.com/client/v1/", "de_DE", collectionIdentifier2, "default", authorizationHeader2, 120, 100, false );

            Assert.IsTrue( config2.Equals( config ) );
        }


        [TestMethod]
        public void colorContentEqualsCheck()
        {
            string colorString = "{\"type\":\"colorcontent\",\"a\":255,\"is_searchable\":false,\"outlet\":\"information-color\",\"b\":39,\"r\":39,\"variation\":\"default\",\"g\":39}";

            IonColorContent color = JsonConvert.DeserializeObject<IonColorContent>( colorString );
            string colorString2 = JsonConvert.SerializeObject( color );
            IonColorContent color2 = JsonConvert.DeserializeObject<IonColorContent>( colorString2 );

            Assert.AreEqual( color, color2 );
        }

        
        [TestMethod]
        public void connectionContentEqualsCheck()
        {
            string connectionString = "{\"position\":0,\"type\":\"connectioncontent\",\"connection_string\":\"//lookbook/material-thermatex-dummy\",\"is_searchable\":false,\"outlet\":\"materials\",\"variation\":\"default\"}";

            IonConnectionContent connection = JsonConvert.DeserializeObject<IonConnectionContent>( connectionString );
            string connectionString2 = JsonConvert.SerializeObject( connection );
            IonConnectionContent connection2 = JsonConvert.DeserializeObject<IonConnectionContent>( connectionString2 );

            Assert.AreEqual( connection, connection2 );
        }


        [TestMethod]
        public void dateTimeContentEqualsCheck()
        {
            string dateTimeString = "{\"variation\":\"default\",\"is_searchable\":false,\"outlet\":\"end-date\",\"datetime\":\"2014 - 11 - 05T11: 05:29Z\",\"type\":\"datetimecontent\"}";

            IonDateTimeContent dateTime = JsonConvert.DeserializeObject<IonDateTimeContent>( dateTimeString );
            string dateTimeString2 = JsonConvert.SerializeObject( dateTime );
            IonDateTimeContent dateTime2 = JsonConvert.DeserializeObject<IonDateTimeContent>( dateTimeString2 );

            Assert.AreEqual( dateTime, dateTime2 );
        }


        [TestMethod]
        public void fileContentEqualsCheck()
        {
            string fileString = "{\"is_searchable\":false,\"outlet\":\"files\",\"file\":\"https://bireise-dev.anfema.com/protected_media/files/baaab979-37a5-4fc1-85aa-885ed2b96be9/testpdf_01.pdf\",\"position\":0,\"checksum\":\"sha256:d7415d339314185bcce7e8e829b12becdf36cad8788645c5de4bb1ebd8f6aa17\",\"type\":\"filecontent\",\"file_size\":467985,\"name\":\"testpdf_01.pdf\",\"variation\":\"default\",\"mime_type\":\"application/octet-stream\"}";

            IonFileContent file = JsonConvert.DeserializeObject<IonFileContent>( fileString );
            string fileString2 = JsonConvert.SerializeObject( file );
            IonFileContent file2 = JsonConvert.DeserializeObject<IonFileContent>( fileString2 );

            Assert.AreEqual( file, file2 );
        }


        [TestMethod]
        public void flagContentEqualsCheck()
        {
            string flagString = "{\"is_searchable\":false,\"is_enabled\":false,\"type\":\"flagcontent\",\"variation\":\"default\",\"outlet\":\"flag\"}";

            IonFlagContent flag = JsonConvert.DeserializeObject<IonFlagContent>( flagString );
            string flagString2 = JsonConvert.SerializeObject( flag );
            IonFlagContent flag2 = JsonConvert.DeserializeObject<IonFlagContent>( flagString2 );

            Assert.AreEqual( flag, flag2 );
        }


        [TestMethod]
        public void imageContentEqualsCheck()
        {
            string imageString = "{\"width\":624,\"scale\":0,\"is_searchable\":false,\"height\":349,\"type\":\"imagecontent\",\"mime_type\":\"image/jpeg\",\"checksum\":\"sha256:9cc5a94d656ea9a28712b91bf182681f742fea03f9371b6a4188c5526696f7bd\",\"original_file_size\":39234,\"original_checksum\":\"sha256:9cc5a94d656ea9a28712b91bf182681f742fea03f9371b6a4188c5526696f7bd\",\"file_size\":39234,\"original_image\":\"https://lookbook-dev.anfema.com/protected_media/images/e9fb32e5-2fa2-4628-b778-4dfbe7b682aa/AMF_Inspirationen_Lookbook_04-25.jpg\",\"outlet\":\"images\",\"original_width\":624,\"position\":2,\"variation\":\"default\",\"translation_y\":0,\"image\":\"https://lookbook-dev.anfema.com/protected_media/images/e9fb32e5-2fa2-4628-b778-4dfbe7b682aa/AMF_Inspirationen_Lookbook_04-25.jpg\",\"original_height\":349,\"translation_x\":0,\"original_mime_type\":\"image/jpeg\"}";

            IonImageContent image = JsonConvert.DeserializeObject<IonImageContent>( imageString );
            string imageString2 = JsonConvert.SerializeObject( image );
            IonImageContent image2 = JsonConvert.DeserializeObject<IonImageContent>( imageString2 );

            Assert.AreEqual( image, image2 );
        }


        [TestMethod]
        public void mediaContentEqualsCheck()
        {
            string mediaString = "{\"original_length\":16280,\"original_mime_type\":\"video/mp4\",\"original_file_size\":730851,\"height\":360,\"width\":234,\"is_searchable\":false,\"mime_type\":\"video/mp4\",\"outlet\":\"videos\",\"original_file\":\"https://bireise-dev.anfema.com/protected_media/files/baaab979-37a5-4fc1-85aa-885ed2b96be9/a8Mo3xd_460sv_kTQmsWb.mp4\",\"file\":\"https://bireise-dev.anfema.com/protected_media/files/baaab979-37a5-4fc1-85aa-885ed2b96be9/a8Mo3xd_460sv.mp4.converted.mp4\",\"position\":0,\"checksum\":\"sha256:6c9f9e6adce81888ef1ebb9e7e9cf808411100df7bed3bfdcad3d55195ee8cbd\",\"type\":\"mediacontent\",\"file_size\":478799,\"name\":\"a8Mo3xd_460sv.mp4\",\"variation\":\"default\",\"length\":16280,\"original_height\":360,\"original_checksum\":\"sha256:243a7054cc4d3abd8cc574db293b026a075ab24a703a2c67bad28dfdeca242bf\",\"original_width\":234}";

            IonMediaContent media = JsonConvert.DeserializeObject<IonMediaContent>( mediaString );
            string mediaString2 = JsonConvert.SerializeObject( media );
            IonMediaContent media2 = JsonConvert.DeserializeObject<IonMediaContent>( mediaString2 );

            Assert.AreEqual( media, media2 );
        }


        [TestMethod]
        public void numberContentEqualsCheck()
        {
            string numberString = "{\"is_searchable\":false,\"type\":\"numbercontent\",\"variation\":\"default\",\"value\":123456,\"outlet\":\"number\"}";

            IonNumberContent number = JsonConvert.DeserializeObject<IonNumberContent>( numberString );
            string numberString2 = JsonConvert.SerializeObject( number );
            IonNumberContent number2 = JsonConvert.DeserializeObject<IonNumberContent>( numberString2 );

            Assert.AreEqual( number, number2 );
        }


        [TestMethod]
        public void optionContentEqualsCheck()
        {
            string optionString = "{\"type\":\"optioncontent\",\"variation\":\"default\",\"value\":\"2\",\"outlet\":\"option\"}";

            IonOptionContent option = JsonConvert.DeserializeObject<IonOptionContent>( optionString );
            string optionString2 = JsonConvert.SerializeObject( option );
            IonOptionContent option2 = JsonConvert.DeserializeObject<IonOptionContent>( optionString2 );

            Assert.AreEqual( option, option2 );
        }


        [TestMethod]
        public void textContentEqualsCheck()
        {
            string textString = "{\"mime_type\":\"text/plain\",\"type\":\"textcontent\",\"variation\":\"default\",\"is_searchable\":true,\"outlet\":\"text\",\"is_multiline\":true,\"text\":\"Donecullamcorpernullanonmetusauctorfringilla.Duismollis,estnoncommodoluctus,nisierat\n\t\t\tporttitorligula,egetlaciniaodiosemnecelit.Vivamussagittislacusvelauguelaoreetrutrumfaucibus\n\t\t\tdolorauctor.Donecsedodiodui.\"}";

            IonTextContent text = JsonConvert.DeserializeObject<IonTextContent>( textString );
            string textString2 = JsonConvert.SerializeObject( text );
            IonTextContent text2 = JsonConvert.DeserializeObject<IonTextContent>( textString2 );

            Assert.AreEqual( text, text2 );
        }
    }
}
