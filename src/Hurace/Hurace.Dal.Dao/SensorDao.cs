using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Dal.Common;
using Hurace.Dal.Common.StatementBuilder;
using Hurace.Dal.Dao.Base;
using Hurace.Dal.Domain;
using Hurace.Dal.Interface;

namespace Hurace.Dal.Dao
{
    public class SensorDao : DefaultCrudDao<Sensor>, ISensorDao
    {
        public SensorDao(IConnectionFactory connectionFactory, StatementFactory statementFactory) : base(
            connectionFactory, "hurace.sensor", statementFactory)
        {
        }

        public Task<IEnumerable<Sensor>?> FindAllSensorsForRace(int raceId) =>
            GeneratedQueryAsync(StatementFactory
                                .Select<Sensor>()
                                .Where<Sensor>((nameof(Sensor.RaceId), raceId))
                                .Build());

        public Task<bool> DeleteAllSensorsForRace(int raceId) =>
            ExecuteAsync($"Delete from {TableName} where raceId=@rid", ("@rid", raceId));

        public Task<int?> GetLastSensorNumber(int raceId) =>
            ExecuteScalarAsync($"select max(sensorNumber) from {TableName} where raceId = @rid", ("@rid", raceId));

        public async Task<Sensor> GetSensorForSensorNumber(int sensorNumber, int raceId) =>
            (await GeneratedQueryAsync(DefaultSelectQuery()
                                       .Where<Sensor>((nameof(Sensor.SensorNumber), sensorNumber),
                                                      (nameof(Sensor.RaceId), raceId)).Build()))
            .FirstOrDefault();
    }
}