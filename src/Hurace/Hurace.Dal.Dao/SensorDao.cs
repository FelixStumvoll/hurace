using System.Collections.Generic;
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

        public Task<IEnumerable<Sensor>> FindAllSensorsForRace(int raceId) =>
            GeneratedQueryAsync(StatementFactory
                                .Select<Sensor>()
                                .Where<Sensor>((nameof(Sensor.RaceId), raceId))
                                .Build());

        public Task<bool> DeleteAllSensorsForRace(int raceId) =>
            ExecuteAsync($"Delete from {TableName} where raceId=@rid", ("@rid", raceId));
    }
}