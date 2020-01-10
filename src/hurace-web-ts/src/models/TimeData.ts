import { StartList } from './StartList';
import { Sensor } from './Sensor';

export interface TimeData {
    skierId: number;
    raceId: number;
    sensorId: number;
    sensor: Sensor;
    time: number;
    startList: StartList;
}
