using FestivalManager;

namespace FestivalManagerTests
{
    [TestClass]
    public class StageTests
    {
        [TestMethod]
        [TestCategory("OnStageCreate")]
        public void OnStageCreate_Valid()
        {
            Stage stage = new();
            Assert.IsTrue(stage is Stage);
        }

        [TestMethod]
        [TestCategory("OnStageCreate")]
        public void OnStageCreate_Invalid_StageIsNull()
        {
            Stage stage = null;
            Assert.IsFalse(stage is Stage);
        }

        [TestMethod]
        [TestCategory("AddPerformer")]
        public void AddPerformer_Valid()
        {
            Performer performer = new("Denis", "Biskup", 21);
            Stage stage = new();
            stage.AddPerformer(performer);

            Assert.IsTrue(stage.Performers.Contains(performer));
        }
        [TestMethod]
        [TestCategory("AddPerformer")]
        [ExpectedException(typeof(ArgumentException))]
        public void AddPerformer_Invalid_TooYoung()
        {
            Performer performer = new("Denis", "Biskup", 17);
            Stage stage = new();
            stage.AddPerformer(performer);
        }

        [TestMethod]
        [TestCategory("AddPerformer")]
        [ExpectedException(typeof(ArgumentException))]
        public void AddPerformer_Invalid_NegativeAge()
        {
            Performer performer = new("Denis", "Biskup", -5);
            Stage stage = new();
            stage.AddPerformer(performer);
        }

        [TestMethod]
        [TestCategory("AddPerformer")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddPerformer_Invalid_PerformerIsNull()
        {
            Performer performer = null;
            Stage stage = new();
            stage.AddPerformer(performer);
        }

        [TestMethod]
        [TestCategory("AddPerformer")]
        [ExpectedException(typeof(NullReferenceException))]
        public void AddPerformer_Invalid_StageIsNull()
        {
            Stage stage = null;
            Performer performer = new("Denis", "Biskup", 21);
            stage.AddPerformer(performer);
        }

        [TestMethod]
        [TestCategory("AddSong")]
        public void AddSong_Valid()
        {
            Song song = new("Dobra piosenka", TimeSpan.FromSeconds(61));
            Stage stage = new();
            stage.AddSong(song);
        }

        [TestMethod]
        [TestCategory("AddSong")]
        [ExpectedException(typeof(ArgumentException))]
        public void AddSong_Invalid_SongTooShort()
        {
            Song song = new("Dobra piosenka", TimeSpan.FromSeconds(59));
            Stage stage = new();
            stage.AddSong(song);
        }

        /*
            [TestMethod]
            [TestCategory("AddSong")]
            public void AddSong_Invalid_SongTooShort()
            {
                Song song = new("Dobra piosenka", TimeSpan.FromSeconds(60)); <- je¿eli jest tu 60 sekund to przechodzi, a w klasie jest, ¿e musi byæ d³u¿sza ni¿ 1 minuta "You can only add songs that are longer than 1 minute."
                Stage stage = new();
                stage.AddSong(song);
            }
         */

        [TestMethod]
        [TestCategory("AddSong")]
        [ExpectedException(typeof(ArgumentException))]
        public void AddSong_Invalid_NegativeSongDuration()
        {
            Song song = new("Dobra piosenka", TimeSpan.FromSeconds(-61));
            Stage stage = new();
            stage.AddSong(song);
        }

        [TestMethod]
        [TestCategory("AddSong")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddSong_Invalid_SongIsNull ()
        {
            Song song = null;
            Stage stage = new();
            stage.AddSong(song);
        }

        [TestMethod]
        [TestCategory("AddSong")]
        [ExpectedException(typeof(NullReferenceException))]
        public void AddSong_Invalid_StageIsNull()
        {
            Stage stage = null;
            Song song = new("Dobra piosenka", TimeSpan.FromSeconds(61));
            stage.AddSong(song);
        }

        [TestMethod]
        [TestCategory("AddSongToPerformer")]
        public void AddSongToPerformer_Valid()
        {
            Stage stage = new();
            Song song = new("Dobra piosenka", TimeSpan.FromSeconds(61));
            stage.AddSong(song);
            Performer performer = new("Denis", "Biskup", 21);
            stage.AddPerformer(performer);

            string returnMsg = stage.AddSongToPerformer("Dobra piosenka", "Denis Biskup");
            bool isReturnMsgValid = false;
            if(returnMsg == $"{song} will be performed by {performer}")
                isReturnMsgValid = true;
            Assert.IsTrue(isReturnMsgValid && performer.SongList.Contains(song));
        }

        [TestMethod]
        [TestCategory("AddSongToPerformer")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddSongToPerformer_Invalid_PerformerNameIsNull()
        {
            Stage stage = new();
            Song song = new("Dobra piosenka", TimeSpan.FromSeconds(61));
            stage.AddSong(song);
            Performer performer = new("Denis", "Biskup", 21);
            stage.AddPerformer(performer);

            string returnMsg = stage.AddSongToPerformer("Dobra piosenka", null);
            bool isReturnMsgValid = false;
            if (returnMsg == $"{song} will be performed by {performer}")
                isReturnMsgValid = true;
            Assert.IsTrue(isReturnMsgValid && performer.SongList.Contains(song));
        }

        [TestMethod]
        [TestCategory("AddSongToPerformer")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddSongToPerformer_Invalid_SongNameIsNull()
        {
            Stage stage = new();
            Song song = new("Dobra piosenka", TimeSpan.FromSeconds(61));
            stage.AddSong(song);
            Performer performer = new("Denis", "Biskup", 21);
            stage.AddPerformer(performer);

            string returnMsg = stage.AddSongToPerformer(null, "Denis Biskup");
            bool isReturnMsgValid = false;
            if (returnMsg == $"{song} will be performed by {performer}")
                isReturnMsgValid = true;
            Assert.IsTrue(isReturnMsgValid && performer.SongList.Contains(song));
        }

        [TestMethod]
        [TestCategory("AddSongToPerformer")]
        [ExpectedException(typeof(ArgumentException))]
        public void AddSongToPerformer_Invalid_PerformerNameIsEmpty()
        {
            Stage stage = new();
            Song song = new("Dobra piosenka", TimeSpan.FromSeconds(61));
            stage.AddSong(song);
            Performer performer = new("Denis", "Biskup", 21);
            stage.AddPerformer(performer);

            string returnMsg = stage.AddSongToPerformer("Dobra piosenka", "");
            bool isReturnMsgValid = false;
            if (returnMsg == $"{song} will be performed by {performer}")
                isReturnMsgValid = true;
            Assert.IsTrue(isReturnMsgValid && performer.SongList.Contains(song));
        }

        [TestMethod]
        [TestCategory("AddSongToPerformer")]
        [ExpectedException(typeof(ArgumentException))]
        public void AddSongToPerformer_Invalid_SongNameIsEmpty()
        {
            Stage stage = new();
            Song song = new("Dobra piosenka", TimeSpan.FromSeconds(61));
            stage.AddSong(song);
            Performer performer = new("Denis", "Biskup", 21);
            stage.AddPerformer(performer);

            string returnMsg = stage.AddSongToPerformer("", "Denis Biskup");
            bool isReturnMsgValid = false;
            if (returnMsg == $"{song} will be performed by {performer}")
                isReturnMsgValid = true;
            Assert.IsTrue(isReturnMsgValid && performer.SongList.Contains(song));
        }

        [TestMethod]
        [TestCategory("Play")]
        public void Play_Valid()
        {
            Stage stage = new();
            Song song = new("Dobra piosenka", TimeSpan.FromSeconds(61));
            Song song2 = new("Dobra piosenka2", TimeSpan.FromSeconds(61));
            Song song3 = new("Dobra piosenka3", TimeSpan.FromSeconds(61));
            stage.AddSong(song);
            stage.AddSong(song2);
            stage.AddSong(song3);
            Performer performer = new("Denis", "Biskup", 21);
            Performer performer2 = new("Test", "Tester", 19);
            stage.AddPerformer(performer);
            stage.AddPerformer(performer2);

            stage.AddSongToPerformer("Dobra piosenka", "Denis Biskup");
            stage.AddSongToPerformer("Dobra piosenka2", "Test Tester");
            stage.AddSongToPerformer("Dobra piosenka3", "Test Tester");
            
            string res = stage.Play();
            Assert.AreEqual(res, $"2 performers played 3 songs");
        }
    }

}