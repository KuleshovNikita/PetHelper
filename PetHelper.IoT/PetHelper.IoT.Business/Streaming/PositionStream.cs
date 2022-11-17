using Microsoft.Extensions.Configuration;
using PetHelper.IoT.Domain;
using System.Timers;
using Timer = System.Timers.Timer;

namespace PetHelper.IoT.Business.Streaming
{
    public class PositionStream : IPositionStream
    {
        private readonly string _mockClientPath;

        public PositionStream(IConfiguration config)
        {
            _mockClientPath = config.GetSection("StoragePath").Value!;
            _mockClientPath = $"{_mockClientPath}\\MockCoordinatesSentToClient.txt";
        }

        public void BeginPositionStreaming()
        {
            ThreadPool.QueueUserWorkItem((state) =>
            {
                var timer = new Timer
                {
                    Interval = 5000,
                    AutoReset = true,
                    Enabled = false
                };

                timer.Elapsed += ProducePosition;
                timer.Start();
            });
        }

        private void ProducePosition(object? sender, ElapsedEventArgs e)
        {
            //TODO change the logic to SignalR when developing the mobile app

            //mock implementation
            var position = new Position
            {
                Longitude = GenerateRandom(),
                Latitude = GenerateRandom()
            };

            using(var streamWriter = File.AppendText(_mockClientPath))
            {
                streamWriter.WriteLine($"{position.Longitude} {position.Latitude}");
            }
        }

        private decimal GenerateRandom()
        {
            var random = new Random();

            var integerPart = random.Next(0, 99);
            var floatPart = random.Next(100000, 999999);

            return Convert.ToDecimal($"{integerPart}.{floatPart}");
        }
    }
}
