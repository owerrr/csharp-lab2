using System;
using System.Collections.Generic;
using System.Linq;

namespace FestivalManager
{
    public class Stage
    {
        private const string CanNotBeNullMessage = "Can not be null!";

        private readonly List<Song> Songs;
        private readonly List<Performer> performers;

        public Stage()
        {
            this.Songs = new List<Song>();
            this.performers = new List<Performer>();
        }

        public IReadOnlyCollection<Performer> Performers => this.performers.AsReadOnly();

        public void AddPerformer(Performer performer)
        {
            this.ValidateNullValue(performer, nameof(performer), CanNotBeNullMessage);

            if (performer.Age < 18)
            {
                throw new ArgumentException("You can only add performers that are at least 18.");
            }

            this.performers.Add(performer);
        }

        public void AddSong(Song song)
        {
            this.ValidateNullValue(song, nameof(song), CanNotBeNullMessage);

            if (song.Duration.TotalMinutes < 1)
            {
                throw new ArgumentException("You can only add songs that are longer than 1 minute.");
            }

            this.Songs.Add(song);
        }

        public string AddSongToPerformer(string songName, string performerName)
        {
            this.ValidateNullValue(songName, nameof(songName), CanNotBeNullMessage);
            this.ValidateNullValue(performerName, nameof(performerName), CanNotBeNullMessage);

            var perfomer = this.GetPerformer(performerName);
            var song = this.GetSong(songName);

            perfomer.SongList.Add(song);

            return $"{song} will be performed by {perfomer}";
        }

        public string Play()
        {
            var songsCount = this.performers.Sum(p => p.SongList.Count());

            return $"{this.performers.Count} performers played {songsCount} songs";
        }

        private Performer GetPerformer(string performerName)
        {
            var performer = this.Performers.FirstOrDefault(p => p.FullName == performerName);

            if (performer == null)
            {
                throw new ArgumentException("There is no performer with this name.");
            }

            return performer;
        }

        private Song GetSong(string songName)
        {
            var song = this.Songs.FirstOrDefault(p => p.Name == songName);

            if (song == null)
            {
                throw new ArgumentException("There is no song with this name.");
            }

            return song;
        }

        private void ValidateNullValue(object variable, string variableName, string exceptionMessage)
        {
            if (variable == null)
            {
                throw new ArgumentNullException(variableName, exceptionMessage);
            }
        }
    }


    public class Performer
    {
        public Performer(string firstName, string lastName, int age)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Age = age;

            this.SongList = new List<Song>();
        }

        private string FirstName { get; }
        private string LastName { get; }

        public string FullName => FirstName + " " + LastName;

        public int Age { get; }
        public List<Song> SongList { get; }

        public override string ToString()
        {
            return this.FullName;
        }
    }

    public class Song
    {
        public Song(string name, TimeSpan duration)
        {
            this.Name = name;
            this.Duration = duration;
        }
        public string Name { get; }

        public TimeSpan Duration { get; }

        public override string ToString()
        {
            return $"{this.Name} ({this.Duration:mm\\:ss})";
        }
    }


}
