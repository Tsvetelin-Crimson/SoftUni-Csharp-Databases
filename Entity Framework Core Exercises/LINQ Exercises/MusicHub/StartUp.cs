namespace MusicHub
{
    using System;
    using System.Linq;
    using System.Text;
    using Data;
    using Initializer;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            MusicHubDbContext context = 
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);

            //Test your solutions here
            string result = ExportSongsAboveDuration(context, 4);
            Console.WriteLine(result);

        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            //Works
            //var albums = context.Producers
            //    .FirstOrDefault(x => x.Id == producerId)
            //    .Albums
            //    .Select(x => new
            //    {
            //        AlbumName = x.Name,
            //        x.ReleaseDate,
            //        Songs = x.Songs.Select(s => new
            //        {
            //            SongName = s.Name,
            //            SongPrice = s.Price,
            //            WriterName = s.Writer.Name
            //        })
            //        .ToList(),
            //        AlbumPrice = x.Price,
            //        ProducerName = x.Producer.Name
            //    })
            //    .OrderByDescending(x => x.AlbumPrice)
            //    .ToList();

            //Doesn't Work for some reason *Fixed*
            var producer = context.Producers
                .Where(x => x.Id == producerId)
                .Select(x => new
                {
                    ProducerName = x.Name,
                    Albums = x.Albums.Select(a => new
                    {
                        AlbumName = a.Name,
                        a.ReleaseDate,
                        Songs = a.Songs.Select(s => new
                        {
                            SongName = s.Name,
                            SongPrice = s.Price,
                            WriterName = s.Writer.Name
                        }),
                        AlbumPrice = a.Price // Do not use sum or w/e in here. Breaks Judge
                    })
                })
                .FirstOrDefault();

            var sb = new StringBuilder();

            foreach (var album in producer.Albums.OrderByDescending(x => x.AlbumPrice))
            {
                sb.AppendLine($"-AlbumName: {album.AlbumName}");
                sb.AppendLine($"-ReleaseDate: {album.ReleaseDate:MM/dd/yyyy}");
                sb.AppendLine($"-ProducerName: {producer.ProducerName}");
                sb.AppendLine($"-Songs:");
                int counter = 1;
                foreach (var song in album.Songs.OrderByDescending(x => x.SongName).ThenBy(x => x.WriterName))
                {
                    sb.AppendLine($"---#{counter++}");
                    sb.AppendLine($"---SongName: {song.SongName}");
                    sb.AppendLine($"---Price: {song.SongPrice:F2}");
                    sb.AppendLine($"---Writer: {song.WriterName}");
                }
                sb.AppendLine($"-AlbumPrice: {album.AlbumPrice:F2}");
            }
            return sb.ToString().TrimEnd();
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            var songs = context.Songs
                .ToList()
                .Where(x => x.Duration.TotalSeconds > duration)
                .Select(x => new
                {
                    SongName = x.Name,
                    PerformerFullName = x.SongPerformers.Select(x => x.Performer.FirstName + " " + x.Performer.LastName).FirstOrDefault(),
                    WriterName = x.Writer.Name,
                    ProducerName = x.Album.Producer.Name,
                    x.Duration,
                })
                .OrderBy(x => x.SongName)
                .ThenBy(x => x.WriterName)
                .ThenBy(x => x.PerformerFullName)
                .ToList();

            var sb = new StringBuilder();
            int counter = 1;
            foreach (var song in songs)
            {
                sb.AppendLine($"-Song #{counter++}")
                    .AppendLine($"---SongName: {song.SongName}")
                    .AppendLine($"---Writer: {song.WriterName}")
                    .AppendLine($"---Performer: {song.PerformerFullName}")
                    .AppendLine($"---AlbumProducer: {song.ProducerName}")
                    .AppendLine($"---Duration: {song.Duration:c}");
            }
            return sb.ToString().TrimEnd();
        }
    }
}
